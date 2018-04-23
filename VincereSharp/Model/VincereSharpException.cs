using System;
using System.Collections.Generic;
using System.Text;

namespace VincereSharp
{
    class VincereSharpException : Exception
    {
        public VincereSharpException(string message) : base(message)
        {
        }

        public VincereSharpException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
