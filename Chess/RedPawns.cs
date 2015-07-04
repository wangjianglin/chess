using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lin.Chess
{
    public class RedPawns : Pawns
    {
        internal RedPawns(int code)
            : base(code)
        {
            this.Chess = "兵";
        }
    }
}
