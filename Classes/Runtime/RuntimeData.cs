using CODEInterpreter.Classes.ErrorHandling;
using CODEInterpreter.Classes.ValidKeywords;

namespace CODEInterpreter.Classes.Runtime
{
    public class RuntimeData
    {
        private Stack<KeyValuePair<string, int>> _runtimeStack;
        private Dictionary<string, object?> _runtimeVariables;
        private ValidTokensV1 _validTokensV1;
        private int _fileLength;
        public RuntimeData(int fileLength)
        {
            _runtimeStack = new Stack<KeyValuePair<string, int>>();
            _runtimeVariables = new Dictionary<string, object?>();
            _validTokensV1 = new ValidTokensV1();
            _fileLength = fileLength;
        }
        public void PushToken(string token, int line)
        {
            CheckIfEnd(line);

            if (!_validTokensV1.ValidBeginnables.Contains(token))
            {
                ErrorHandler.ThrowError
                (line, $"Invalid token BEGIN \"{token}\".");
            }
            if (_runtimeStack.Count == 0 && !token.Equals("CODE"))
            {
                ErrorHandler.ThrowError
                (line, "Expected BEGIN CODE, found none.");
            }

            _runtimeStack.Push(new KeyValuePair<string, int>(token, line));
        }

        public void PopToken(string token, int line)
        {
            if (!_validTokensV1.ValidBeginnables.Contains(token))
            {
                ErrorHandler.ThrowError
                (line, $"Invalid token END \"{token}\".");
            }
            if (_runtimeStack.Count == 0  || 
                _runtimeStack.Peek().Key == null ||
                !_runtimeStack.Peek().Key!.Equals(token)
                )
            {
                ErrorHandler.ThrowError(line, $"BEGIN {token} not found.");
            }

            CheckIfEnd(line);
            _runtimeStack.Pop();
        }
        public void AddVariable(string dataType, string name, object? value, int line)
        {
            if (_validTokensV1.ValidReservedKeywords.Contains(name))
            {
                ErrorHandler.ThrowError
                (line, "Cannot use KEYWORD as IDENTIFIER.");
            }
            if (_runtimeVariables.ContainsKey(name))
            {
                ErrorHandler.ThrowError
                (line, $"Variable {name} already defined.");
            }
            if (!_validTokensV1.ValidDataTypes.Contains(dataType.Trim()))
            {
                ErrorHandler.ThrowError(line, $"Invalid data type \"{dataType}\".");
            }

            _runtimeVariables.Add(name, value);
        }
        public void CheckIfEnd(int line)
        {
            if (line == _fileLength && _runtimeStack.Count != 0)
            {
                while (_runtimeStack.Count != 0)
                {
                    var kv = _runtimeStack.Peek();
                    var token = kv.Key;
                    var errorLine = kv.Value;
                    ErrorHandler.ThrowError
                    (errorLine, $"BEGIN {token} must match END {token} or vice versa.");
                }
            }
        }
    }
}
