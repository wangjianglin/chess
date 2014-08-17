using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lin.Chess
{
    public class BlackKing : King
    {
        internal BlackKing(int code) : base(code) {
            this.Chess = "将";
        }
    }
}
