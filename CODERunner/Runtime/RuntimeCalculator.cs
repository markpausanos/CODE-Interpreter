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
            else if (obj is string)
            {
                return "STRING";
            }
            else
            {
                return "NULL";
            }
        }
        public object? Unary(object? expression, string symbol, int line)
        {
            int multiplier = symbol.Equals("+") ? 1 : -1;

            if (expression is int && expression is object)
            {
                expression = int.Parse(expression.ToString());

                return multiplier * (int)expression;
            }

            if (expression is float && expression is object)
            {
                expression = float.Parse(expression.ToString());

                return multiplier * (float)expression;
            }

            CodeErrorHandler.ThrowError
            (line, $"Unsupported operand 'UNARY' for type {GetTypeObject(expression)}");

            return null;
        }
        private void ThrowErrorInCalculator(object? left, object? right, string symbol, int line)
        {
            CodeErrorHandler.ThrowError
            (line, $"Unsupported operand '{symbol}' for types {GetTypeObject(left)} and {GetTypeObject(right)}");
        }
        public object? Multiply(object? left, object? right, int line)
        {
            if (left is int l && right is int r)
            {
                return l * r;
            }

            if (left is float lf && right is float rf)
            {
                return lf * rf;
            }

            if (left is int lInt && right is int rFloat)
            {
                return lInt * rFloat;
            }

            if (left is float lFloat && right is int rInt)
            {
                return lFloat * rInt;
            }

            ThrowErrorInCalculator(left, right, "*", line);

            return null;
        }
        public object? Divide(object? left, object? right, int line)
        {
            if (left is int l && right is int r)
            {
                return l / r;
            }

            if (left is float lf && right is float rf)
            {
                return lf / rf;
            }

            if (left is int lInt && right is int rFloat)
            {
                return lInt / rFloat;
            }

            if (left is float lFloat && right is int rInt)
            {
                return lFloat / rInt;
            }

            ThrowErrorInCalculator(left, right, "/", line);

            return null;
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
        public bool? GreaterThan(object? left, object? right, int line)
        {
            if (left is int l && right is int r)
            {
                if(l > r)
                {
                    return true;
                }
                return false;
            }

            if (left is float lf && right is float rf)
            {
                if (lf > rf)
                {
                    return true;
                }
                return false;
            }

            if (left is int lInt && right is int rFloat)
            {
                if (lInt > rFloat)
                {
                    return true;
                }
                return false;
            }

            if (left is float lFloat && right is int rInt)
            {
                if (lFloat > rInt)
                {
                    return true;
                }
                return false;
            }

            ThrowErrorInCalculator(left, right, ">", line);

            return null;
        }
        public bool? LessThan(object? left, object? right, int line)
        {
            if (left is int l && right is int r)
            {
                if (l < r)
                {
                    return true;
                }
                return false;
            }

            if (left is float lf && right is float rf)
            {
                if (lf < rf)
                {
                    return true;
                }
                return false;
            }

            if (left is int lInt && right is int rFloat)
            {
                if (lInt < rFloat)
                {
                    return true;
                }
                return false;
            }

            if (left is float lFloat && right is int rInt)
            {
                if (lFloat < rInt)
                {
                    return true;
                }
                return false;
            }

            ThrowErrorInCalculator(left, right, "<", line);

            return null;
        }
        public bool? GreaterThanOrEqualTo(object? left, object? right, int line)
        {
            if (left is int l && right is int r)
            {
                if (l >= r)
                {
                    return true;
                }
                return false;
            }

            if (left is float lf && right is float rf)
            {
                if (lf >= rf)
                {
                    return true;
                }
                return false;
            }

            if (left is int lInt && right is int rFloat)
            {
                if (lInt >= rFloat)
                {
                    return true;
                }
                return false;
            }

            if (left is float lFloat && right is int rInt)
            {
                if (lFloat >= rInt)
                {
                    return true;
                }
                return false;
            }

            ThrowErrorInCalculator(left, right, ">=", line);

            return null;
        }
        public bool? LessThanOrEqualTo(object? left, object? right, int line)
        {
            if (left is int l && right is int r)
            {
                if (l <= r)
                {
                    return true;
                }
                return false;
            }

            if (left is float lf && right is float rf)
            {
                if (lf <= rf)
                {
                    return true;
                }
                return false;
            }

            if (left is int lInt && right is int rFloat)
            {
                if (lInt <= rFloat)
                {
                    return true;
                }
                return false;
            }

            if (left is float lFloat && right is int rInt)
            {
                if (lFloat <= rInt)
                {
                    return true;
                }
                return false;
            }

            ThrowErrorInCalculator(left, right, "<=", line);

            return null;
        }
        public bool? Equal(object? left, object? right, int line)
        {
            if (left is int l && right is int r)
            {
                if (l == r)
                {
                    return true;
                }
                return false;
            }

            if (left is float lf && right is float rf)
            {
                if (lf == rf)
                {
                    return true;
                }
                return false;
            }

            if (left is int lInt && right is int rFloat)
            {
                if (lInt == rFloat)
                {
                    return true;
                }
                return false;
            }

            if (left is float lFloat && right is int rInt)
            {
                if (lFloat == rInt)
                {
                    return true;
                }
                return false;
            }

            ThrowErrorInCalculator(left, right, "==", line);

            return null;
        }
        public bool? NotEqual(object? left, object? right, int line)
        {
            if (left is int l && right is int r)
            {
                if (l != r)
                {
                    return true;
                }
                return false;
            }

            if (left is float lf && right is float rf)
            {
                if (lf != rf)
                {
                    return true;
                }
                return false;
            }

            if (left is int lInt && right is int rFloat)
            {
                if (lInt != rFloat)
                {
                    return true;
                }
                return false;
            }

            if (left is float lFloat && right is int rInt)
            {
                if (lFloat != rInt)
                {
                    return true;
                }
                return false;
            }

            ThrowErrorInCalculator(left, right, "<>", line);

            return null;
        }
        public bool? Not(object? expression, int line)
        {
            if (expression is object && expression is bool)
            {
                return !((bool)expression);
            }

            CodeErrorHandler.ThrowError
            (line, $"Unsupported operand 'NOT' for type {GetTypeObject(expression)}");

            return null;
        }
        public bool? And(object? left, object? right, int line)
        {
            if (left is bool l && right is bool r)
            {
                return l && r;
            }

            ThrowErrorInCalculator(left, right, "AND", line);
            return null;
        }

        public bool? Or(object? left, object? right, int line)
        {
            if (left is bool l && right is bool r)
            {
                return l || r;
            }

            ThrowErrorInCalculator(left, right, "OR", line);
            return null;
        }
        public object? Concatenation(object? left, object? right, int line)
        {
            if (left is object && right is object)
            {
                if (left is bool)
                {
                    left = left.ToString()!.ToUpper();
                }

                if (right is bool)
                {
                    right = right.ToString()!.ToUpper();
                }

                return left.ToString() + right.ToString();
            }

            ThrowErrorInCalculator(left, right, "&", line);

            return null;
        }
    }
}
