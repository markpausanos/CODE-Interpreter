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
        RuntimeCalculator _valueCalculator;
        RuntimeFunction _runtimeFunction;
        private bool _canDeclare = false;
        public CodeVisitor()
        {
            _runtimeData = new RuntimeData();
            _runtimeFunction = new RuntimeFunction(_runtimeData);
            _valueCalculator = new RuntimeCalculator();
        }
        public override object? VisitBegin_code([NotNull] CodeParser.Begin_codeContext context)
        {
            _canDeclare = true;

            return null;
        }
        public override object? VisitIf_block([NotNull] CodeParser.If_blockContext context)
        {
            return base.VisitIf_block(context);
        }

        public override object? VisitElse_block([NotNull] CodeParser.Else_blockContext context)
        {
            return base.VisitElse_block(context);
        }

        public override object? VisitDeclaration([NotNull] CodeParser.DeclarationContext context)
        {
            var variableType = context.IDENTIFIER();

            if (!_canDeclare)
            {
                CodeErrorHandler.ThrowError
                (context.Start.Line, "Cannot declare variables after other operations.");
            }

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
            _canDeclare = false;

            var identifiers = context.IDENTIFIER();
            var value = Visit(context.expression());

            foreach (var identifier in identifiers)
            {
                _runtimeData.AssignVariable(identifier.GetText(), value, context.Start.Line);
            }

            return base.VisitAssignment(context);
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
                args.Add(arg.GetText());
            }

            _runtimeFunction.Scan(args, context.Start.Line);
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
            return _valueCalculator.Not(Visit(context.expression()), context.Start.Line);
        }
        public override object? VisitUnaryExpression([NotNull] CodeParser.UnaryExpressionContext context)
        {
            return _valueCalculator.Unary(Visit(context.expression()), context.unary().GetText(), context.Start.Line);
        }
        public override object? VisitMultiplyExpression([NotNull] CodeParser.MultiplyExpressionContext context)
        {
            var left = Visit(context.expression(0));
            var right = Visit(context.expression(1));
            var op = context.multiply_op().GetText();

            //TODO: Add calcs for multiply
            return op switch
            {
                "+" => _valueCalculator.Add(left, right, context.Start.Line),
                "-" => _valueCalculator.Subtract(left, right, context.Start.Line),
                "%" => _valueCalculator.Modulo(left, right, context.Start.Line),
                _ => CodeErrorHandler.ThrowError(context.Start.Line, $"Unexpected token '{op}'.")
            };
        }
        public override object? VisitAddExpression([NotNull] CodeParser.AddExpressionContext context)
        {
            var left = Visit(context.expression(0));
            var right = Visit(context.expression(1));
            var op = context.add_op().GetText();

            return op switch
            {
                "+" => _valueCalculator.Add(left, right, context.Start.Line),
                "-" => _valueCalculator.Subtract(left, right, context.Start.Line),
                "%" => _valueCalculator.Modulo(left, right, context.Start.Line),
                _ => CodeErrorHandler.ThrowError(context.Start.Line, $"Unexpected token '{op}'.")
            };
        }
        public override object? VisitConcatExpression([NotNull] CodeParser.ConcatExpressionContext context)
        {
            var left = Visit(context.expression(0));
            var right = Visit(context.expression(1));

            return _valueCalculator.Concatenation(left, right, context.Start.Line);
        }
        public override object? VisitNewlineExpression([NotNull] CodeParser.NewlineExpressionContext context)
        {
            return "\n";
        }
    }
}
