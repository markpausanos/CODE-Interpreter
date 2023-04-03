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

            var StringValue = value is bool ? value.ToString().ToUpper() : value.ToString();

            try
            {
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
                        Value = value.ToString()!.ToUpper();
                        break;
                }
            }
            catch (Exception ex)
            {
                CodeErrorHandler.ThrowError(line, $"Cannot convert {StringValue} to {dataType}");
            }
        }
        public void AssignVariable(object? value)
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            
            var StringValue = value is bool ? value.ToString().ToUpper() : value.ToString();
            try
            {
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
                        Value = value.ToString()!.ToUpper();
                        break;
                }
            }
            catch (Exception ex)
            {
                CodeErrorHandler.ThrowError(Line, $"Cannot convert '{StringValue}' to {DataType}");
            }
        }
    }
}
