using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lin.Chess
{
    public class RedElephants : Elephants
    {
        internal RedElephants(int code, byte? pos = null)
            : base(code, pos)
        {
            this.Chess = "相";
        }
    }
}
