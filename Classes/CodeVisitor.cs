using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using CODEInterpreter.Content;

namespace CODEInterpreter.Classes
{
    public class CodeVisitor : CodeBaseVisitor<object?>
    {
        bool _canExecute = false;
        int _fileLength;

        ValidTokensV1 _validTokens;

        Stack<KeyValuePair<string, int>> _runtimeStack;
        Dictionary<string, object?> _runtimeVariables;
        public CodeVisitor(CodeLexer codeLexer, int fileLength)
        {
            _validTokens = new();
            _runtimeStack = new();
            _runtimeVariables = new();
            _fileLength = fileLength;
        }

        // Helper Methods
        private void ThrowError(int line, string message)
        {
            Console.WriteLine($"Error: Line {line}.");
            Console.WriteLine("Details: " + message);
            Environment.Exit(400);
        }
        private void PushToken(string token, int line)
        {
            _runtimeStack.Push(new KeyValuePair<string, int>(token, line));
        }

        // Visitor Methods

        public override object? VisitBegin_code([NotNull] CodeParser.Begin_codeContext context)
        {
            CheckIfEnd(context.Start.Line);
            var begin = context.CODE();

            if (begin == null)
            {
                ThrowError
                    (
                        context.Start.Line,
                        $"Missing Token after BEGIN. Expected CODE, IF, WHILE."
                    );
            }
            var contextLine = context.Start.Line;
            var beginText = begin.GetText();

            if (!_validTokens.ValidBeginnables.Contains(beginText))
            {
                ThrowError
                    (
                        context.start.Line,
                        $"Invalid token \"{begin.GetText()}\". Expected CODE, IF, WHILE."
                    );
            }
            if (_runtimeStack.Count == 0 && !beginText.Equals("CODE"))
            {
                ThrowError
                    (
                        context.start.Line,
                        "Code program must begin with BEGIN CODE."
                    );
            }

            PushToken(beginText, context.Start.Line);
            _canExecute = true;

            return null;
        }
        public override object? VisitEnd_code([NotNull] CodeParser.End_codeContext context)
        {
            var end = context.CODE();

            if (end == null)
            {
                ThrowError
                    (
                        context.Start.Line,
                        $"Missing Token after END. Expected CODE."
                    );
            }

            var endText = end.GetText();

            if (!_validTokens.ValidBeginnables.Contains(endText))
            {
                ThrowError
                    (
                        context.Start.Line,
                        $"Invalid token \"{endText}\". Expected CODE."
                    );
            }

            if (
                !_canExecute ||
                _runtimeStack.Count == 0 ||
                _runtimeStack.Peek().Key == null ||
                !_runtimeStack.Peek().Key!.Equals(endText)
                )
            {
                ThrowError(context.Start.Line, $"END {endText} must match BEGIN {endText}.");
            }

            _runtimeStack.Pop();
            CheckIfEnd(context.Start.Line);

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
            var variableDataType = context.DATA_TYPE().GetText();

            var variables = context.variable();
            foreach (var variable in variables)
            {
                Console.Write(variable.IDENTIFIER().GetText());
                if (variable.expression() == null)
                {
                    continue;
                }
                Console.WriteLine(" " + variable.expression().GetText());
            }
            foreach (var variable in variables)
            {
                if (_validTokens.ValidReservedKeywords.Contains(variable.IDENTIFIER().GetText()))
                {
                    ThrowError
                        (
                            context.Start.Line, 
                            $"{variable.GetText()} is a KEYWORD. Cannot use KEYWORDS for variable names."
                        );
                }
                Console.Write(variable.GetText());
            }
            return null;
        }
        public void CheckIfEnd(int line)
        {
            if (line == _fileLength && _runtimeStack.Count != 0)
            {
                while (_runtimeStack.Count != 0)
                {
                    var kv = _runtimeStack.Peek();
                    var end = kv.Key;
                    var errorLine = kv.Value;
                    ThrowError(errorLine, $"END {end} must match BEGIN {end}.");
                    _runtimeStack.Pop();
                }
            }
        }
    }
}
