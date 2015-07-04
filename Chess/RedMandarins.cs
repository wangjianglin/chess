using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lin.Chess
{
    public class RedMandarins : Mandarins
    {
        internal RedMandarins(int code)
            : base(code)
        {
            this.Chess = "仕";
        }
    }
}
