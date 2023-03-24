using Antlr4.Runtime;
using CODEInterpreter.Classes;
using CODEInterpreter.Classes.ErrorHandling;
using CODEInterpreter.Classes.Visitor;
using CODEInterpreter.Content;

var testFileName = "D:\\College\\BSCS\\CS Year 3\\CS SECOND SEM\\CS322 - Programming Languages\\CODEInterpreter\\Content\\test.code";

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