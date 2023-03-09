using Antlr4.Runtime;
using CODEInterpreter.Content;

namespace CODEInterpreter.Classes
{
    public class CodeVisitor : CodeBaseVisitor<object?>
    {
        ValidTokens _validTokens;
        Stack<KeyValuePair<string, int>> _runtimeStack;
        CodeLexer _lexer;
        int _fileLength;
        public CodeVisitor(CodeLexer codeLexer, int fileLength)
        {
            _validTokens = new();
            _runtimeStack = new();
            _lexer = codeLexer;
            _fileLength = fileLength;
        }
        private void ThrowError(int line, string message)
        {
            Console.WriteLine($"Error at line {line}.");
            Console.WriteLine("Details: " + message);
        }
        private void PushToken(string token, int line)
        {
            _runtimeStack.Push(new KeyValuePair<string, int>(token, line));
        }   
        public override object? VisitBegin(CodeParser.BeginContext context)
        {
            CheckIfEnd();
            var begin = context.BEGINNABLE().GetText();
          
            if (!_validTokens.ValidBeginnables.Contains(begin))
            {
                ThrowError(
                    context.start.Line, 
                    $"Invalid token \"{begin}\". Expected CODE, IF, WHILE."
                    );
                Environment.Exit(400);
            }
            if (_runtimeStack.Count == 0 && !begin.Equals("CODE"))
            {
                ThrowError(context.start.Line, "Code program must begin with BEGIN CODE.");
                Environment.Exit(400);
            }
           
            PushToken(begin, context.start.Line);
            return null;
        }
        public override object? VisitEnd(CodeParser.EndContext context)
        {
            var end = context.BEGINNABLE().GetText();

            if (!_validTokens.ValidBeginnables.Contains(end))
            {
                ThrowError(
                    context.start.Line,
                    $"Invalid token \"{end}\". Expected CODE, IF, WHILE."
                    );
                Environment.Exit(400);
            }
            if (_runtimeStack.Count == 0 ||
                _runtimeStack.Peek().Key == null ||
                !_runtimeStack.Peek().Key!.Equals(end))
            {
                ThrowError(context.start.Line, $"END {end} must match BEGIN {end}.");
                Environment.Exit(400);
            }
            _runtimeStack.Pop();
            CheckIfEnd();
            return null;
        }
        public void CheckIfEnd()
        {
            if (_lexer.Line == _fileLength && _runtimeStack.Count != 0)
            {
                while (_runtimeStack.Count != 0)
                {
                    var kv = _runtimeStack.Peek();
                    var end = kv.Key;
                    var line = kv.Value;
                    ThrowError(line, $"END {end} must match BEGIN {end}.");
                    _runtimeStack.Pop();
                }
                Environment.Exit(400);
            }
        }
    }
}
