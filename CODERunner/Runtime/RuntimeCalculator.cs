using CODEInterpreter.Classes.ErrorHandling;

namespace CODEInterpreter.Classes.ValidKeywords
{
    public class RuntimeCalculator
    {
        private string GetTypeObject(object? obj)
        {
            if (obj is int)
            {
                return "INT";
            }
            else if (obj is float)
            {
                return "FLOAT";
            }
            else if (obj is char)
            {
                return "CHAR";
            }
            else if (obj is bool)
            {
                return "BOOL";
            }
            else
            {
                return "NULL";
            }
        }
        private void ThrowErrorInCalculator(object? left, object? right, string symbol, int line)
        {
            string leftType = GetTypeObject(left);
            string rightType = GetTypeObject(right);

            CodeErrorHandler.ThrowError
            (line, $"Unsupported operand '{symbol}' for types {leftType} and {rightType}");
        }
        public object? Add(object? left, object? right, int line)
        {
            if (left is int l && right is int r)
            {
                return l + r;
            }

            if (left is float lf && right is float rf)
            {
                return lf + rf;
            }

            if (left is int lInt && right is int rFloat)
            {
                return lInt + rFloat;
            }

            if (left is float lFloat && right is int rInt)
            {
                return lFloat + rInt;
            }

            ThrowErrorInCalculator(left, right, "+", line);

            return null;
        }
        public object? Subtract(object? left, object? right, int line)
        {
            if (left is int l && right is int r)
            {
                return l - r;
            }

            if (left is float lf && right is float rf)
            {
                return lf - rf;
            }

            if (left is int lInt && right is int rFloat)
            {
                return lInt - rFloat;
            }

            if (left is float lFloat && right is int rInt)
            {
                return lFloat - rInt;
            }

            ThrowErrorInCalculator(left, right, "-", line);

            return null;
        }
        public object? Modulo(object? left, object? right, int line)
        {
            if (left is int l && right is int r)
            {
                return l % r;
            }

            if (left is float lf && right is float rf)
            {
                return lf % rf;
            }

            if (left is int lInt && right is int rFloat)
            {
                return lInt % rFloat;
            }

            if (left is float lFloat && right is int rInt)
            {
                return lFloat % rInt;
            }

            ThrowErrorInCalculator(left, right, "%", line);

            return null;
        }
        public object? Concatenation(object? left, object? right, int line)
        {
            if (left is object && right is object)
            {
                return left.ToString() + right.ToString();
            }

            ThrowErrorInCalculator(left, right, "&", line);

            return null;
        }
    }
}
