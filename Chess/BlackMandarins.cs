using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lin.Chess
{
    public class BlackMandarins : Mandarins
    {
        internal BlackMandarins(int code) : base(code) {
            this.Chess = "士";
        }
    }
}
