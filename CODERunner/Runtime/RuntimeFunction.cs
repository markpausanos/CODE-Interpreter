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
            AvailableFunctions = new List<string>() { "DISPLAY", "SCAN" };
        }
        public bool CallMethod(string identifier, List<object?> argsParams, object? argsSingle, int line)
        {
            if (!AvailableFunctions.Contains(identifier))
            {
                return false;
            }

            if (identifier.Equals("DISPLAY"))
            {
                Display(argsSingle);
            }

            if (identifier.Equals("SCAN"))
            {
                Scan(argsParams, line);
            }

            return true;
        }
        public void Display(object? args)
        {
            Console.Write(args);
        }
        public void Scan(List<object?> args, int line)
        {
            string input = Console.ReadLine();
            string[] separatedInputs = input.Split(",");
            
            if (separatedInputs.Length != args.Count())
            {
                CodeErrorHandler.ThrowError(line, "Input values count do not match with SCAN args");
            }
            for (int i = 0; i < args.Count(); i++)
            {
                _runtimeData.AssignVariable(args[i].ToString(), separatedInputs[i], line);
            }
        }
    }
}
