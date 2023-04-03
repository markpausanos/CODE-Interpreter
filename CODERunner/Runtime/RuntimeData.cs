using CODEInterpreter.Classes.ErrorHandling;
using CODEInterpreter.Classes.ValidKeywords;
using CODEInterpreter.CODERunner.Class;

namespace CODEInterpreter.Classes.Runtime
{
    public class RuntimeData
    {
        private Dictionary<string, Variable> _runtimeVariables;
        private ValidTokensV1 _validTokensV1;
        public RuntimeData()
        {
            _runtimeVariables = new Dictionary<string, Variable>();
            _validTokensV1 = new ValidTokensV1();
        }
        public void AddVariable(string dataType, string name, object? value, int line)
        {
            if (_validTokensV1.ValidReservedKeywords.Contains(name))
            {
                CodeErrorHandler.ThrowError
                (line, "Cannot use KEYWORD as IDENTIFIER.");
            }
            if (_runtimeVariables.ContainsKey(name))
            {
                CodeErrorHandler.ThrowError
                (line, $"Variable {name} already defined.");
            }
            if (!_validTokensV1.ValidDataTypes.Contains(dataType.Trim()))
            {
                CodeErrorHandler.ThrowError(line, $"Invalid data type \"{dataType}\".");
            }

            _runtimeVariables.Add(name, new Variable(name, value, dataType, line));
        }
        public bool CheckVariableExists(string identifier)
        {
            return _runtimeVariables.ContainsKey(identifier);
        }
        public void AssignVariable(string identifier, object? value, int line)
        {
            if (!_runtimeVariables.ContainsKey(identifier))
            {
                CodeErrorHandler.ThrowError(line, $"Variable {identifier} not found.");
            }

            _runtimeVariables[identifier].AssignVariable(value);
        }
        public object? GetValue(string variableName, int line)
        {
            if (!CheckVariableExists(variableName))
            {
                CodeErrorHandler.ThrowError
                (line, $"Unexpected token {variableName}. Variable {variableName} is not defined.");
            }

            return _runtimeVariables[variableName].Value;
        }
    }
}
