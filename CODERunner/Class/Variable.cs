using CODEInterpreter.Classes.ErrorHandling;

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
                var StringValue = value.ToString()!;

                switch (DataType)
                {
                    case "INT":
                        
                        Value = int.Parse(StringValue);
                        break;
                    case "FLOAT":
                        Value = float.Parse(StringValue);
                        break;
                    case "CHAR":
                        Value = char.Parse(StringValue);
                        break;
                    case "BOOL":
                        Value = bool.Parse(StringValue);
                        Value = Value.ToString()!.ToUpper();
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
                var StringValue = value.ToString()!;

                switch (DataType)
                {
                    case "INT":

                        Value = int.Parse(StringValue);
                        break;
                    case "FLOAT":
                        Value = float.Parse(StringValue);
                        break;
                    case "CHAR":
                        Value = char.Parse(StringValue);
                        break;
                    case "BOOL":
                        Value = bool.Parse(StringValue);
                        Value = Value.ToString()!.ToUpper();
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
