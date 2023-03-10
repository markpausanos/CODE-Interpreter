using Antlr4.Runtime.Misc;
using CODEInterpreter.Classes.ErrorHandling;
using CODEInterpreter.Classes.Runtime;
using CODEInterpreter.Content;

namespace CODEInterpreter.Classes
{
    public class CodeVisitor : CodeBaseVisitor<object?>
    {
        RuntimeData _runtimeData;
        private bool _canExecute = false;
        private bool _canAssign = true;
        public CodeVisitor(CodeLexer codeLexer, int fileLength)
        {
            _runtimeData = new RuntimeData(fileLength);
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
            _canAssign = true;
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

        public override object? VisitAssignment([NotNull] CodeParser.AssignmentContext context)
        {
            var variableType = context.DATA_TYPE();

            if (!_canExecute)
            {
                ErrorHandler.ThrowError(context.Start.Line, "BEGIN CODE not found.");
            }
            if (!_canAssign)
            {
                ErrorHandler.ThrowError
                (context.Start.Line, "Cannot assign variables after execution.");
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
                if (variable.INT() != null)
                {
                    ErrorHandler.ThrowError
                    (context.Start.Line, $"Unxpected symbol \"{variable.INT().GetText()}\".");
                }
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
    }
}
