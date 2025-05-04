using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewGameProject.ServerLogic.Parsing.Exceptions
{
    public class UnknownCommandException : Exception
    {
        public UnknownCommandException(string message) : base(message) { }
    }
}
