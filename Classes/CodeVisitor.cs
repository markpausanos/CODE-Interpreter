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
        ValueCalculator _valueCalculator;
        private bool _canExecute = false;
        private bool _canDeclare = true;
        public CodeVisitor(CodeLexer codeLexer, int fileLength)
        {
            _runtimeData = new RuntimeData(fileLength);
            _valueCalculator = new ValueCalculator();
        }
        public override object? VisitBegin_code([NotNull] CodeParser.Begin_codeContext context)
        {
            var begin = context.CODE();

            if (begin == null)
            {
                ErrorHandler.ThrowError
                (context.Start.Line, $"Expected CODE, found none." );
            }

            var beginText = begin.GetText();

            _runtimeData.PushToken(beginText, context.Start.Line);
            _canExecute = true;
            return null;
        }
        public override object? VisitEnd_code([NotNull] CodeParser.End_codeContext context)
        {
            var end = context.CODE();

            if (!_canExecute)
            {
                ErrorHandler.ThrowError(context.Start.Line, "BEGIN CODE not found.");
            }
            if (end == null)
            {
                ErrorHandler.ThrowError
                (context.Start.Line, $"Expected CODE, found none.");
            }

            var endText = end.GetText();

            _runtimeData.PopToken(endText, context.Start.Line);
            _canExecute = false;
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
                ErrorHandler.ThrowError(context.Start.Line, "BEGIN CODE not found.");
            }
            if (!_canDeclare)
            {
                ErrorHandler.ThrowError
                (context.Start.Line, "Cannot declare variables after execution.");
            }
            if (variableType == null)
            {
                ErrorHandler.ThrowError
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
                    ErrorHandler.ThrowError
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
                ErrorHandler.ThrowError(context.Start.Line, "BEGIN CODE not found.");
            }

            var variables = context.variable();
            var expression = context.expression();
            
            
            return base.VisitAssignment(context);
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
                ErrorHandler.ThrowError
                (context.Start.Line, $"Unexpected token {variableName}. Variable {variableName} is not defined.");
            }

            return _runtimeData.GetValue(variableName);
;       }
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
                _ => ErrorHandler.ThrowError(context.Start.Line, $"Unexpected token '{op}'.")
            };
        }
    }
}
