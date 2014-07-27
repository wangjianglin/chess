using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lin.Chess
{
    public abstract class Knights : ChessPiece
    {
        internal Knights(int code, byte? pos = null)
            : base(code, pos)
        {
            this.Chess = "马";
        }
        public override int[] Setps
        {
            get { throw new NotImplementedException(); }
        }

        public override bool CanMove(int position)
        {
            throw new NotImplementedException();
        }
    }
}
