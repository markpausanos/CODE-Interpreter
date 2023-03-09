using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CODEInterpreter.Classes
{
    public class ValidTokens
    {
        public List<string> ValidBeginnables 
        { 
            get {
                return new List<string>
                {
                    "CODE",
                    "IF",
                    "WHILE"
                };
            } 
         }
        

    }
}
