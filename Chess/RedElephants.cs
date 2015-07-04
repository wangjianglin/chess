using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lin.Chess
{
    public class RedElephants : Elephants
    {
        internal RedElephants(int code)
            : base(code)
        {
            this.Chess = "相";
        }
    }
}
