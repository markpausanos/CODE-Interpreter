using Antlr4.Runtime.Misc;
using CODEInterpreter.Classes.ErrorHandling;
using CODEInterpreter.Classes.Runtime;
using CODEInterpreter.Classes.ValidKeywords;
using CODEInterpreter.Content;

namespace CODEInterpreter.Classes
{
    public class CodeVisitor : CodeBaseVisitor<object?>
    {
        RuntimeData _runtimeData;
        RuntimeCalculator _valueCalculator;
        RuntimeFunction _runtimeFunction;
        private bool _canExecute = false;
        private bool _canDeclare = false;
        public CodeVisitor(CodeLexer codeLexer, int fileLength)
        {
            _runtimeData = new RuntimeData(fileLength);
            _valueCalculator = new RuntimeCalculator();
            _runtimeFunction = new RuntimeFunction();
        }
        public override object? VisitBegin_code([NotNull] CodeParser.Begin_codeContext context)
        {
            if (_canExecute)
            {
                CodeErrorHandler.ThrowError
                (context.Start.Line, $"BEGIN CODE already declared.");
            }
            var begin = context.CODE();

            if (begin == null)
            {
                CodeErrorHandler.ThrowError
                (context.Start.Line, $"Expected CODE, found none." );
            }

            var beginText = begin.GetText();

            _canExecute = true;
            _canDeclare = true;
            return null;
        }
        public override object? VisitEnd_code([NotNull] CodeParser.End_codeContext context)
        {
            var end = context.CODE();

            if (!_canExecute)
            {
                CodeErrorHandler.ThrowError(context.Start.Line, "BEGIN CODE not found.");
            }
            if (end == null)
            {
                CodeErrorHandler.ThrowError
                (context.Start.Line, $"Expected CODE, found none.");
            }

            var endText = end.GetText();

            _canExecute = false;
            _canDeclare = false;

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
            var variableType = context.DATA_TYPE();

            if (!_canExecute)
            {
                CodeErrorHandler.ThrowError(context.Start.Line, "BEGIN CODE not found.");
            }
            if (!_canDeclare)
            {
                CodeErrorHandler.ThrowError
                (context.Start.Line, "Cannot declare variables after execution.");
            }
            if (variableType == null)
            {
                CodeErrorHandler.ThrowError
                (context.Start.Line, "Expected DATA TYPE, found none.");
            }

            var variableDataType = variableType.GetText();
            var variables = context.variable();

            foreach (var variable in variables)
            {
                //if (variable.IDENTIFIER() == null)
                //{
                //    ErrorHandler.ThrowError
                //    (context.Start.Line, $"Unxpected symbol \"{variable.DATA_TYPE().GetText()}\".");
                //}
                if (variable.IDENTIFIER() == null)
                {
                    CodeErrorHandler.ThrowError
                    (context.Start.Line, "Expected IDENTIFIER, found none.");
                }

                var variableName = variable.IDENTIFIER().GetText();
                var variableValue = variable.expression() == null ? null : variable.expression().GetText();

                _runtimeData.AddVariable(variableDataType, variableName, variableValue, context.Start.Line);
            }
   
            return null;
        }
        public override object? VisitAssignment([NotNull] CodeParser.AssignmentContext context)
        {
            if (!_canExecute)
            {
                CodeErrorHandler.ThrowError(context.Start.Line, "BEGIN CODE not found.");
            }
            if (context.expression() == null)
            {
                CodeErrorHandler.ThrowError(context.Start.Line, "Value not found.");
            }

            var identifiers = context.IDENTIFIER();
            var value = Visit(context.expression());
            
            foreach (var identifier in identifiers)
            {
                if (identifier == null)
                {
                    CodeErrorHandler.ThrowError(context.Start.Line, "Identifier not found.");
                }
                _runtimeData.AssignVariable(identifier.GetText(), value, context.Start.Line);
            }
            
            return base.VisitAssignment(context);
        }
        public override object? VisitFunction_call([NotNull] CodeParser.Function_callContext context)
        {
            _runtimeFunction.CallMethod(context.IDENTIFIER().GetText());
            return base.VisitFunction_call(context);
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
                return context.BOOL().GetText() == "TRUE";
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

            return _runtimeData.GetValue(variableName);
;       }
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
                // TODO: Concatenate
                _ => CodeErrorHandler.ThrowError(context.Start.Line, $"Unexpected token '{op}'.")
            };
        }
    }
}
