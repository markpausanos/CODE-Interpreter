using Antlr4.Runtime;
using Antlr4.Runtime.Misc;

namespace CODEInterpreter.Classes.ErrorHandling
{
    public class CodeErrorListener : BaseErrorListener
    {
        public override void SyntaxError
        ([NotNull] IRecognizer recognizer, [Nullable] IToken offendingSymbol,
        int line, int charPositionInLine, [NotNull] string msg,
        [Nullable] RecognitionException e)
        {
            Console.Error.WriteLine($"Syntax error: Unexpected symbol {offendingSymbol.Text} at line {line}, column {charPositionInLine + 1}");
            Console.Error.WriteLine($"Details: {msg}");
            Environment.Exit(400);
            base.SyntaxError(recognizer, offendingSymbol, line, charPositionInLine, msg, e);
        }
    }
}
