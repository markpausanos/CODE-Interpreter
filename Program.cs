using Antlr4.Runtime;
using CODEInterpreter.Classes.ErrorHandling;
using CODEInterpreter.Classes.Visitor;
using CODEInterpreter.Content;

string baseDirectory;

while (true)
{
    Console.Write("Enter base directory: ");
    baseDirectory = Console.ReadLine();
    if (baseDirectory is not null && baseDirectory.Length != 0)
    {
        break;
    }
}

while (true)
{
    try
    {
        Console.Write("Enter file name: ");
        var fileName = Console.ReadLine();
        var testFileName = baseDirectory + fileName;
        var fileContents = File.ReadAllText(testFileName.TrimEnd());
        var fileLines = File.ReadAllLines(testFileName).Length;
        var inputStream = new AntlrInputStream(fileContents);

        var codeLexer = new CodeLexer(inputStream);
        var commonTokenStream = new CommonTokenStream(codeLexer);
        var codeParser = new CodeParser(commonTokenStream);
        codeParser.RemoveErrorListeners();
        codeParser.AddErrorListener(new CodeErrorListener());
        var codeContext = codeParser.program();
        var visitor = new CodeVisitor();
        visitor.Visit(codeContext);

        Console.Write("\nContinue? Y|N: ");
        string input = Console.ReadLine();
        if (input is object && input.Equals("N"))
        {
            break;
        };
        
    }
    catch (FileNotFoundException ex)
    {
        Console.WriteLine("File not found! Please make sure to give proper name and extension.");
    }
}
