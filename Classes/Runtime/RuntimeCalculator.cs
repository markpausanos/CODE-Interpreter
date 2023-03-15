using CODEInterpreter.Classes.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CODEInterpreter.Classes.ValidKeywords
{
    public class RuntimeCalculator
    {
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

            string leftErrorText = left is null ? "null" : left.GetType().ToString();
            string rightErrorText = right is null ? "null" : right.GetType().ToString();

            ErrorHandler.ThrowError
            (line, $"Unsupported operand '+' for types {leftErrorText} and {rightErrorText}");

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

            string leftErrorText = left is null ? "null" : left.GetType().ToString();
            string rightErrorText = right is null ? "null" : right.GetType().ToString();

            ErrorHandler.ThrowError
            (line, $"Unsupported operand '-' for types {leftErrorText} and {rightErrorText}");

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

            string leftErrorText = left is null ? "null" : left.GetType().ToString();
            string rightErrorText = right is null ? "null" : right.GetType().ToString();

            ErrorHandler.ThrowError
            (line, $"Unsupported operand '%' for types {leftErrorText} and {rightErrorText}");

            return null;
        }
    }
}
