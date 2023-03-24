using CODEInterpreter.Classes.ErrorHandling;

namespace CODEInterpreter.Classes.Runtime
{
    public class RuntimeFunction
    {
        public RuntimeData _runtimeData;
        public List<string> AvailableFunctions;

        public RuntimeFunction(RuntimeData runtimeData)
        {
            _runtimeData = runtimeData;
        }
        public void Display(object? args)
        {
            Console.Write(args);
        }
        public void Scan(List<string> args, int line)
        {
            string input = Console.ReadLine();
            string[] separatedInputs = input.Split(",");
            
            if (separatedInputs.Length != args.Count())
            {
                CodeErrorHandler.ThrowError(line, "Input values count do not match with SCAN args");
            }
            for (int i = 0; i < args.Count(); i++)
            {
                _runtimeData.AssignVariable(args[i], separatedInputs[i], line);
            }
        }
    }
}
