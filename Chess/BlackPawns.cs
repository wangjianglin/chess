using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lin.Chess
{
    public class BlackPawns : Pawns
    {
        internal BlackPawns(int code)
            : base(code)
        {
            this.Chess = "卒";
        }
    }
}
