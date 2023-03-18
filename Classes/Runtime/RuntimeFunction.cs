using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CODEInterpreter.Classes.Runtime
{
    public class RuntimeFunction
    {
        public List<string> AvailableFunctions;

        public RuntimeFunction()
        {
            AvailableFunctions = new List<string>() { "DISPLAY", "SCAN" };
        }
        public bool CallMethod(string identifier)
        {
            if (!AvailableFunctions.Contains(identifier))
            {
                return false;
            }
            return true;
        }
        public void Display()
        {

        }
    }
}
