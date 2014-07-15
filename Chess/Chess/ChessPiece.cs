using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Lin.Chess
{
    public abstract class ChessPiece:Control
    {
        private static readonly byte[] POSITIONS = new byte[] { 0xcb, 0xc3, 0xca, 0xc4, 0xc9, 0xc5, 0xc8, 0xc6, 0xc7, 0xaa, 0xa4, 0x9b, 0x99, 0x97, 0x95, 0x93, 0x33, 0x3b, 0x34, 0x3a, 0x35, 0x39, 0x36, 0x38, 0xc7, 0x54, 0x5a, 0x63, 0x65, 0x67, 0x69, 0x6a };

        //private byte[] positions = null;//new byte[] {0xcb,0xc3,0xca,0xc4,0xc9,0xc5,0xc8,0xc6,0xc7,0xaa,0xa4,0x9b,0x99,0x97,0x95,0x93,0x33,0x3b,0x34,0x3a,0x35,0x39,0x36,0x38,0xc7,0x54,0x5a,0x63,0x65,0x67,0x69,0x6a};
        private byte _pos = 0;
        private int code;
        private byte? pos;
        internal ChessPiece(int code, byte? pos = null)
        {
            if (pos == null)
            {
                _pos = POSITIONS[code];
            }
            else
            {
                _pos = (byte)pos;
            }
        }

        public byte Position{
            get { return _pos; }
            protected set{_pos = value;}
        }

        public bool IsMark { get; set; }

        public double Size { get; set; }

        public abstract int[] Setps { get; }

        public void Move(int positon)
        {

        }

        public abstract bool CanMove(int position);
    }
}
