using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lin.Chess
{
    public class RedKing : King
    {
        internal RedKing(int code, byte? pos = null) : base(code, pos) {
            this.Chess = "帅";
        }
    }
}
