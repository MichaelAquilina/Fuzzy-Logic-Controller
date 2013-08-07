using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuzzyLogicController
{
    class InvalidRangeException : Exception
    {
        public InvalidRangeException( String message )
            :base( message )
        {
        }
    }
}
