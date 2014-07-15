using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lin.Chess
{
    public class RedRooks:Rooks
    {
        private byte code;
        private byte? position;

        internal RedRooks(int code, byte? pos = null):base(code, pos) {}

    }
}
