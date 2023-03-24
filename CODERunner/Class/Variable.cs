using Antlr4.Runtime.Atn;
using CODEInterpreter.Classes.ErrorHandling;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CODEInterpreter.CODERunner.Class
{
    public class Variable
    {
        public string Name { get; set; }
        public object? Value { get; set; }
        public string DataType { get; set; }
        public int Line { get; set; }
        public Variable(string name, object? value, string dataType, int line)
        {
            Name = name;
            DataType = dataType;
            Line = line;

            if (value == null)
            {
                Value = null;
                return;
            }

            try
            {
                switch (DataType)
                {
                    case "INT":
                        Value = int.Parse(value.ToString()!);
                        break;
                    case "FLOAT":
                        Value = float.Parse(value.ToString()!);
                        break;
                    case "CHAR":
                        Value = char.Parse(value.ToString()!);
                        break;
                    case "BOOL":
                        Value = bool.Parse(value.ToString()!);
                        break;
                }
            }
            catch (Exception ex)
            {
                CodeErrorHandler.ThrowError(line, $"Cannot convert {value} to {dataType}");
            }
        }
        public void AssignVariable(object? value)
        {
            if (value == null)
            {
                Value = null;
                return;
            }

            try
            {
                switch (DataType)
                {
                    case "INT":
                        Value = int.Parse(value.ToString()!);
                        break;
                    case "FLOAT":
                        Value = float.Parse(value.ToString()!);
                        break;
                    case "CHAR":
                        Value = char.Parse(value.ToString()!);
                        break;
                    case "BOOL":
                        Value = bool.Parse(value.ToString()!);
                        break;
                }
            }
            catch (Exception ex)
            {
                CodeErrorHandler.ThrowError(Line, $"Cannot convert '{value}' to {DataType}");
            }
        }
    }
}
