using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Lin.Chess
{
    public class BlackCannons : Cannons
    {
        static BlackCannons()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(BlackCannons), new FrameworkPropertyMetadata(typeof(BlackCannons)));
        }
        internal BlackCannons(int code) : base(code) { }
    }
}
