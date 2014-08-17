using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lin.Chess
{
    public class BlackElephants:Elephants
    {
        internal BlackElephants(int code) : base(code) {
            this.Chess = "象";
        }
    }
}
