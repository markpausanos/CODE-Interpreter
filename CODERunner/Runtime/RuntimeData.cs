using CODEInterpreter.Classes.ErrorHandling;
using CODEInterpreter.Classes.ValidKeywords;
using CODEInterpreter.CODERunner.Class;

namespace CODEInterpreter.Classes.Runtime
{
    public class RuntimeData
    {
        private Stack<KeyValuePair<string, int>> _runtimeStack;
        private Dictionary<string, Variable> _runtimeVariables;
        private ValidTokensV1 _validTokensV1;

        public RuntimeData()
        {
            _runtimeStack = new Stack<KeyValuePair<string, int>>();
            _runtimeVariables = new Dictionary<string, Variable>();
            _validTokensV1 = new ValidTokensV1();
        }
        public void PushToken(string token, int line)
        {
            if (!_validTokensV1.ValidBeginnables.Contains(token))
            {
                CodeErrorHandler.ThrowError
                (line, $"Invalid token BEGIN \"{token}\".");
            }

            _runtimeStack.Push(new KeyValuePair<string, int>(token, line));
        }
        public void PopToken(string token, int line)
        {
            if (!_validTokensV1.ValidBeginnables.Contains(token))
            {
                CodeErrorHandler.ThrowError
                (line, $"Invalid token END \"{token}\".");
            }
            if (_runtimeStack.Count == 0  || 
                _runtimeStack.Peek().Key == null ||
                !_runtimeStack.Peek().Key!.Equals(token)
                )
            {
                CodeErrorHandler.ThrowError(line, $"BEGIN {token} not found.");
            }

            _runtimeStack.Pop();
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
        public object? GetValue(string identifier)
        {
            return _runtimeVariables[identifier].Value;
        }
    }
}
