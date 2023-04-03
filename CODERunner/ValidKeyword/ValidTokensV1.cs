namespace CODEInterpreter.Classes.ValidKeywords
{
    public class ValidTokensV1
    {
        private List<string> _validDataTypes = new List<string>()
        {
            "INT",
            "CHAR",
            "BOOL",
            "FLOAT"
        };
        private List<string> _validBeginnables = new List<string>()
        {
            "IF",
            "WHILE"
        };
        private List<string> _validProcedures = new List<string>()
        {
            "DISPLAY",
            "SCAN"
        };
        private List<string> _validReservedKeywords = new List<string>()
        { 
            "BEGIN",
            "END"
        };
    
        public List<string> ValidDataTypes
        {
            get
            {
                return _validDataTypes;
            }
        }
        public List<string> ValidBeginnables
        {
            get
            {
                return _validBeginnables;
            }
        }
        public List<string> ValidProcedures
        {
            get
            {
                return _validProcedures;
            }
        }
        public List<string> ValidReservedKeywords
        {
            get
            {
                return _validReservedKeywords
                    .Concat(ValidDataTypes)
                    .Concat(ValidBeginnables)
                    .Concat(ValidProcedures)
                    .ToList()
                    ;
            }
        }
        public void AddProcedure(string name)
        {
            throw new NotImplementedException();
        }
    }
}
