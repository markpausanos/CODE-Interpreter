using Antlr4.Runtime.Misc;
using CODEInterpreter.Classes.ErrorHandling;
using CODEInterpreter.Classes.Runtime;
using CODEInterpreter.Classes.ValidKeywords;
using CODEInterpreter.Content;

namespace CODEInterpreter.Classes.Visitor
{
    public class CodeVisitor : CodeBaseVisitor<object?>
    {
        RuntimeData _runtimeData;
        RuntimeCalculator _runtimeCalculator;
        RuntimeFunction _runtimeFunction;
        public CodeVisitor()
        {
            _runtimeData = new RuntimeData();
            _runtimeFunction = new RuntimeFunction(_runtimeData);
            _runtimeCalculator = new RuntimeCalculator();
        }
        public override object? VisitDeclaration([NotNull] CodeParser.DeclarationContext context)
        {
            var variableType = context.data_type();
            var variableDataType = variableType.GetText();
            var variables = context.variable();

            foreach (var variable in variables)
            {
                var variableName = variable.IDENTIFIER().GetText();
                var variableValue = variable.expression() == null ? null : Visit(variable.expression());

                _runtimeData.AddVariable(variableDataType, variableName, variableValue, context.Start.Line);
            }

            return null;
        }
        public override object? VisitAssignment([NotNull] CodeParser.AssignmentContext context)
        {
            var identifiers = context.IDENTIFIER();
            var value = Visit(context.expression());

            foreach (var identifier in identifiers)
            {
                _runtimeData.AssignVariable(identifier.GetText(), value, context.Start.Line);
            }

            return null;
        }
        public override object? VisitDisplay([NotNull] CodeParser.DisplayContext context)
        {
            _runtimeFunction.Display(Visit(context.expression()));
            return null;
        }
        public override object? VisitScan([NotNull] CodeParser.ScanContext context)
        {
            List<string> args = new List<string>();

            foreach (var arg in context.IDENTIFIER())
            {
                if (_runtimeData.CheckVariableExists(arg.GetText()))
                {
                    args.Add(arg.GetText());
                }
                else
                {
                    CodeErrorHandler.ThrowError(context.Start.Line, $"Variable {arg.GetText()} not found.");
                }
            }

            _runtimeFunction.Scan(args, context.Start.Line);
            return null;
        }
        public override object? VisitIf_block([NotNull] CodeParser.If_blockContext context)
        {
            object? result = Visit(context.expression());

            if (result is not object || result is not bool)
            {
                CodeErrorHandler.ThrowError(context.Start.Line, $"Cannot use non-BOOL expression \"{result}\" as condition.");
            }

            if ((bool)result == true)
            {
                Visit(context.else_block());
                return true;
            }
            else
            {
                var elseifBlocks = context.else_if_block();

                foreach (var elseIfBlock in elseifBlocks)
                {
                    if ((bool)Visit(elseIfBlock)! == true)
                    {
                        return false;
                    }
                }

                if (context.else_block() is object)
                {
                    Visit(context.else_block());
                }

                return false;
            }
        }
        public override object? VisitElse_if_block([NotNull] CodeParser.Else_if_blockContext context)
        {
            object? result = Visit(context.expression());

            if (result is not object || result is not bool)
            {
                CodeErrorHandler.ThrowError(context.Start.Line, $"Cannot use non-BOOL expression \"{result}\" as condition.");
            }

            if ((bool)result == true)
            {
                Visit(context.if_code_block());
                return true;
            }

            return false;
        }
        public override object? VisitWhile_block([NotNull] CodeParser.While_blockContext context)
        {
            object? result = Visit(context.expression());

            if (result is not object || result is not bool)
            {
                CodeErrorHandler.ThrowError(context.Start.Line, $"Cannot use non-BOOL expression \"{result}\" as condition.");
            }

            while ((bool)result == true)
            {
                if (Visit(context.while_code_block()) is object m && (bool)m == false)
                {
                    return false;
                }
                result = Visit(context.expression());
            }

            return null;
        }
        public override object? VisitWhile_code_block([NotNull] CodeParser.While_code_blockContext context)
        {
            foreach (var whileLine in context.while_line())
            {
                if (whileLine.@break() is object)
                {
                    return false;
                }
                if (whileLine.@continue() is object)
                {
                    return true;
                }
                Visit(whileLine);
            }
            return null;
        }
        public override object? VisitConstant([NotNull] CodeParser.ConstantContext context)
        {
            if (context.INT() != null)
            {
                return int.Parse(context.INT().GetText());
            }

            if (context.CHAR() != null)
            {
                return char.Parse(context.CHAR().GetText()[1..^1]);
            }

            if (context.FLOAT() != null)
            {
                return float.Parse(context.FLOAT().GetText());
            }

            if (context.BOOL() != null)
            {
                return context.BOOL().GetText()[1..^1].ToUpper().Trim().Equals("TRUE");
            }

            if (context.STRING() != null)
            {
                return context.STRING().GetText()[1..^1];
            }

            return null;
        }
        public override object? VisitEscapeCharExpression([NotNull] CodeParser.EscapeCharExpressionContext context)
        {
            return char.Parse(context.ESCAPE_CHAR().GetText()[1..^1]);
        }
        public override object? VisitIdentifierExpression([NotNull] CodeParser.IdentifierExpressionContext context)
        {
            var variableName = context.IDENTIFIER().GetText();

            if (!_runtimeData.CheckVariableExists(variableName))
            {
                CodeErrorHandler.ThrowError
                (context.Start.Line, $"Unexpected token {variableName}. Variable {variableName} is not defined.");
            }

            return _runtimeData.GetValue(variableName) ?? "NULL";
        }
        public override object? VisitParenthesizedExpression([NotNull] CodeParser.ParenthesizedExpressionContext context)
        {
            return Visit(context.expression());
        }
        public override object? VisitNotExpression([NotNull] CodeParser.NotExpressionContext context)
        {
            return _runtimeCalculator.Not(Visit(context.expression()), context.Start.Line);
        }
        public override object? VisitUnaryExpression([NotNull] CodeParser.UnaryExpressionContext context)
        {
            return _runtimeCalculator.Unary(Visit(context.expression()), context.unary().GetText(), context.Start.Line);
        }
        public override object? VisitMultiplyExpression([NotNull] CodeParser.MultiplyExpressionContext context)
        {
            var left = Visit(context.expression(0));
            var right = Visit(context.expression(1));
            var op = context.multiply_op().GetText();

            return op switch
            {
                "*" => _runtimeCalculator.Multiply(left, right, context.Start.Line),
                "/" => _runtimeCalculator.Divide(left, right, context.Start.Line),
                "%" => _runtimeCalculator.Modulo(left, right, context.Start.Line),
                _ => throw new NotImplementedException()
            };
        }
        public override object? VisitAddExpression([NotNull] CodeParser.AddExpressionContext context)
        {
            var left = Visit(context.expression(0));
            var right = Visit(context.expression(1));
            var op = context.add_op().GetText();

            return op switch
            {
                "+" => _runtimeCalculator.Add(left, right, context.Start.Line),
                "-" => _runtimeCalculator.Subtract(left, right, context.Start.Line),
                _ => throw new NotImplementedException()
            };
        }
        public override object? VisitCompareExpression([NotNull] CodeParser.CompareExpressionContext context)
        {
            var left = Visit(context.expression(0));
            var right = Visit(context.expression(1));
            var op = context.compare_op().GetText();

            return op switch
            {
                ">" => _runtimeCalculator.GreaterThan(left, right, context.Start.Line),
                "<" => _runtimeCalculator.LessThan(left, right, context.Start.Line),
                ">=" => _runtimeCalculator.GreaterThanOrEqualTo(left, right, context.Start.Line),
                "<=" => _runtimeCalculator.LessThanOrEqualTo(left, right, context.Start.Line),
                "==" => _runtimeCalculator.Equal(left, right, context.Start.Line),
                "<>" => _runtimeCalculator.NotEqual(left, right, context.Start.Line),
                _ => throw new NotImplementedException()
            }; ;
        }
        public override object? VisitConcatExpression([NotNull] CodeParser.ConcatExpressionContext context)
        {
            var left = Visit(context.expression(0));
            var right = Visit(context.expression(1));

            return _runtimeCalculator.Concatenation(left, right, context.Start.Line);
        }
        public override object? VisitNewlineExpression([NotNull] CodeParser.NewlineExpressionContext context)
        {
            return "\n";
        }
    }
}
