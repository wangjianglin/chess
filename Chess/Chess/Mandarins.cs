﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lin.Chess
{
    public abstract class Mandarins : ChessPiece
    {
        internal Mandarins(int code, byte? pos = null) : base(code, pos) { }
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