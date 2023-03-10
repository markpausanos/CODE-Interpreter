using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CODEInterpreter.Classes
{
    public class ValidTokensV1
    {
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
                    .Concat(ValidBeginnables)
                    .Concat(ValidProcedures)
                    .ToList()
                    ;
            }
        }

    }
}
