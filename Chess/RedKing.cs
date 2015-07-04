using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lin.Chess
{
    public class RedKing : King
    {
        internal RedKing(int code) : base(code) {
            this.Chess = "帅";
        }
    }
}
