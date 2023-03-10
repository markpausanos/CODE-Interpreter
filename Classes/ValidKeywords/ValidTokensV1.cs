namespace CODEInterpreter.Classes.ValidKeywords
{
    public class ValidTokensV1
    {
        public List<string> ValidDataTypes
        {
            get
            {
                return new List<string>
                {
                    "INT",
                    "CHAR",
                    "BOOL",
                    "FLOAT"
                };
            }
        }
        public List<string> ValidBeginnables
        {
            get
            {
                return new List<string>
                {
                    "CODE",
                    "IF",
                    "WHILE"
                };
            }
        }
        public List<string> ValidProcedures
        {
            get
            {
                return new List<string>
                {
                    "DISPLAY",
                    "SCAN"
                };
            }
        }
        public List<string> ValidReservedKeywords
        {
            get
            {
                return new List<string>()
                    .Concat(ValidDataTypes)
                    .Concat(ValidBeginnables)
                    .Concat(ValidProcedures)
                    .ToList()
                    ;
            }
        }

    }
}
