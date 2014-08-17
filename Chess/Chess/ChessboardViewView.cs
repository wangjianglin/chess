//using Lin.Core;
//using Lin.Util;
//using System;
//using System.Activities.Presentation.Metadata;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Globalization;
//using System.Linq;
//using System.Text;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Markup;
//using System.Windows.Media;

using Lin.Core;
using Lin.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace Lin.Chess
{


    public class ChessboardView:Control
    {
        private class CheckerboardViewModel : ViewModel
        {
            [PropertyChanged("Size3333333djfgnflksnfdlksdjglksfjgjsdgklsfdmjgklfsdmnb")]
            private void SizeChange()
            {
                double size = self.Size;
                self.Margin = -size / 2.0;
                self.Diameter = size * 0.9;
                self.FontSize = size * 0.6;
            }

            private double width = 0;
            private double marginHeight = 0;
            private double marginWidth = 0;
            //private double Interval = 0;
            //private CheckerboardView view;
            public void setWidth(double width, double height)
            {
                double wInterval = width / 9.5;
                double hInterval = height / 10.5;
                self.Interval = wInterval < hInterval?wInterval:hInterval;
                //self.Interval = width / 9.5;
                self.CheckerboardWidth = self.Interval * 8;
                self.CheckerboardHeight = self.Interval * 9;
                self.FontSize = self.Interval * 0.75;
                self.FontWidth = self.Interval * 4.7;
                //marginHeight = marginWidth = self.Interval * 0.75;
                marginWidth = (width - self.CheckerboardWidth) / 2.0;
                marginHeight = (height - self.CheckerboardHeight) / 2.0;
                this.width = width;
                this.OnPropertyChanged("Positions");
                this.OnPropertyChanged("PositionMark");

                self.BorderTop = marginHeight - 3;
                self.BorderLeft = marginWidth - 3;
                self.BorderBottom = height - marginHeight + 3;
                self.BorderRight = width - marginWidth + 3;
                //self.PositionMark = CreatePositionMark();// "M 10,10 L 100,100";
            }
            public int GetPositionFromXY(double x, double y)
            {
                x -= marginWidth;
                y -= marginHeight;
                x = x / self.Interval + 3;
                y = y / self.Interval + 3;
                int px = (int)(x + 0.5);
                if (px < 3 || px > 11)
                {
                    return 256;
                }
                int py = (int)(y + 0.5);
                if (py < 3 || py > 12)
                {
                    return 256;
                }
                //if (Math.Abs(px - x) > 0.4 && Math.Abs(py - y) > 0.4)
                if ((px - x) * (px - x) + (py - y) * (py - y) > 0.4 * 0.4)
                {
                    return 256;
                }

                return py * 16 + px;
            }
            private Point[] CreatePositionMark(long pos, int type)//炮、兵位置标识
            {
                double w = self.Interval * 0.2;//标记的长度
                double m = self.Interval * 0.07;//标记离线的距离

                Point p = _positions[pos];
                Point[] result = new Point[3];// {new Point();new Point();new Point()};
                result[0] = new Point();
                result[1] = new Point();
                result[2] = new Point();

                switch (type)
                {
                    case 0://right-top
                        //result = " " + (p.X + w + m) + "," + (p.Y - m) + " " + (p.X + m) + "," + (p.Y - m) + " " + (p.X + m) + "," + (p.Y - w - m) + " ";
                        result[0].X = p.X + w + m;
                        result[0].Y = p.Y - m;
                        result[1].X = p.X + m;
                        result[1].Y = p.Y - m;
                        result[2].X = p.X + m;
                        result[2].Y = p.Y - w - m;
                        break;
                    case 1://left-top
                        //result = " " + (p.X - w - m) + "," + (p.Y + m) + " " + (p.X - m) + "," + (p.Y + m) + " " + (p.X - m) + "," + (p.Y + w + m) + " ";
                        result[0].X = p.X - w - m;
                        result[0].Y = p.Y - m;
                        result[1].X = p.X - m;
                        result[1].Y = p.Y - m;
                        result[2].X = p.X - m;
                        result[2].Y = p.Y - w - m;
                        break;
                    case 2://left-bottom
                        //result = " " + (p.X - w - m) + "," + (p.Y - m) + " " + (p.X - m) + "," + (p.Y - m) + " " + (p.X - m) + "," + (p.Y - w - m) + " ";
                        result[0].X = p.X - w - m;
                        result[0].Y = p.Y + m;
                        result[1].X = p.X - m;
                        result[1].Y = p.Y + m;
                        result[2].X = p.X - m;
                        result[2].Y = p.Y + w + m;
                        break;
                    case 3://right-bottom
                        //result = " " + (p.X + w + m) + "," + (p.Y + m) + " " + (p.X + m) + "," + (p.Y + m) + " " + (p.X + m) + "," + (p.Y + w + m) + " ";
                        result[0].X = p.X + w + m;
                        result[0].Y = p.Y + m;
                        result[1].X = p.X + m;
                        result[1].Y = p.Y + m;
                        result[2].X = p.X + m;
                        result[2].Y = p.Y + w + m;
                        break;
                }
                return result;
            }

            private ReadOnlyIndexProperty<long, Point> _positions = null;
            //private ReadOnlyIndexProperty<long[], String> _positionMark = null;
            public CheckerboardViewModel(ChessboardView view,double width,double height)
            {
                _positions = new Util.ReadOnlyIndexProperty<long, Point>(pos =>
                {
                    long x = pos % 16;
                    long y = pos / 16;
                    return new Point((x - 3) * self.Interval + this.marginWidth, (y - 3) * self.Interval + this.marginHeight);
                });
                self.PositionMark = new ReadOnlyIndexProperty<long, Point[][]>(pos =>
                {
                    return new Point[][] { CreatePositionMark(pos, 0) ,
                                                CreatePositionMark(pos,1),
                                                CreatePositionMark(pos,2),
                                                CreatePositionMark(pos,3)};
                });
                this.setWidth(width,height);
                self.StrokeThickness = 1.0;//线的大小
                self.Stroke = new SolidColorBrush(Colors.Black);//线的颜色
                self.BorderStrokeThickness =2.0;//外边框的大小
                self.BorderStroke = new SolidColorBrush(Colors.Black);//外边框的颜色
               
                //this.view = view;
                self.Positions = _positions;
            }

            //采用动态类对 属性 进行扩展
            //[PropertyChaned(nmae="")]
            private void PropertyChaned(object value,object old){

            }
        }


        public event SelectedEventHandler Selected;

        static ChessboardView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChessboardView), new FrameworkPropertyMetadata(typeof(ChessboardView)));
            //TypeDescriptor.AddProvider(new NumberTypeDescriptionProvider(typeof(long)), typeof(long));
            //TypeDescriptor.AddProvider(new NumberTypeDescriptionProvider(typeof(long)), typeof(long[]));

            //　使用AttributeTableBuilder给string添加一个新的属性
            //AttributeTableBuilder builder = new AttributeTableBuilder();
            //builder.AddCustomAttributes(typeof(int), new DesignerCategoryAttribute("Custom　category"));
            //MetadataStore.AddAttributeTable(builder.CreateTable());

            //Console.WriteLine("---------　包含自定义属性");
            //AttributeCollection attributeCollection = TypeDescriptor.GetAttributes(typeof(int));
            ////OutputAttributes(attributeCollection);
            //Console.WriteLine("---------　注册回调函数");

            ////使用AttributeCallback延时注册元数据（请求时）?
            //builder = new AttributeTableBuilder();
            //builder.AddCallback(typeof(string),
            //    new AttributeCallback(acb =>
            //    {
            //        Console.WriteLine("***　In　AttributeCallback,　增加一个新的属性");
            //        acb.AddCustomAttributes(new DesignTimeVisibleAttribute(false));
            //    }
            //    )
            //);
            //MetadataStore.AddAttributeTable(builder.CreateTable());

            //Console.WriteLine("---------　包含通过回调函数增加的自定义属性");
            //attributeCollection = TypeDescriptor.GetAttributes(typeof(string));
            ////OutputAttributes(attributeCollection);
            //Console.WriteLine("Press　Enter　to　Exit");
            //Console.ReadLine();
        }

       //private ChessPiece[] pieces = new ChessPiece[32];
        //private ChessPiece[] pieces = new ChessPiece[256];//记录棋盘中的棋子
        //private byte[] positions = new byte[32];//记录棋子位置
        public ChessPieceView[] items = new ChessPieceView[33];

        public Situation Situation { get; private set; }
        public ChessboardView()
        {
            Situation = new Situation();

            vm = new CheckerboardViewModel(this, this.ActualWidth, this.ActualHeight);
            this.SetDateContext((ViewModel)vm);
        }

        public ChessControl Control { get; set; }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            vm.setWidth(sizeInfo.NewSize.Width,sizeInfo.NewSize.Height);
            RefeshPos();
        }

        private dynamic vm = null;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            
            Canvas board = (Canvas)this.GetTemplateChild("PATH_Board");


            Func<ChessPiece, SelectedEventHandler> func = piece =>
            {
                SelectedEventHandler handler = (object sender, SelectedEventArgs args) =>
                {
                    if (this.Selected != null)
                    {
                        SelectedEventArgs args2 = null;
                        if (piece == null)
                        {
                            args2 = new SelectedEventArgs(this.chessMarkPos, args.Mouse, args.Count, args.X, args.Y, args.Chess);
                        }
                        else
                        {
                            args2 = new SelectedEventArgs(this.Situation.Positions[piece], args.Mouse, args.Count, args.X, args.Y, args.Chess);
                        }
                        this.Selected(this, args2);
                    }
                };
                return handler;
            };

            
            ChessPieceView item = null;
            //for (int n = 0; n < this.Situation.Positions.Length; n++)
            foreach(ChessPiece piece in this.Situation.Positions.Keys)
            {
                item = new ChessPieceView(this.Situation.Pieces[this.Situation.Positions[piece]]);
                item.Selected += func(piece);
                items[piece.Code] = item;
                board.Children.Add(item);
            }
            item = new ChessPieceView();
            item.Selected += func(null);
            items[32] = item;
            board.Children.Add(item);
            
        }

        public void Move(ChessPiece piece, int position)
        {
            this.Situation.Move(piece, position);
            this.RefeshPos();
            //piece.Position = (byte)positon;
            //Canvas.SetLeft(piece, vm.Positions[piece.Position].X);
            //Canvas.SetTop(piece, vm.Positions[piece.Position].Y);
        }

        private int chessMarkPos = 256;
        public void Mark(int position, bool isMark)
        {
            chessMarkPos = position;
            Canvas.SetLeft(items[32], vm.Positions[position].X);
            Canvas.SetTop(items[32], vm.Positions[position].Y);
            items[32].Size = vm.Interval;
            items[32].IsMark = true;
        }
        public bool CanMove(ChessPiece piece, int position)
        {
            return Situation.CanMove(piece, position);
        }
        private void RefeshPos()
        {
            ChessPieceView item = null;
            for (int n = 0; n < 32; n++)
            {
                item = this.items[n];
                if (item != null)
                {
                    Canvas.SetLeft(item, vm.Positions[this.Situation.Positions[item.Piece]].X);
                    Canvas.SetTop(item, vm.Positions[this.Situation.Positions[item.Piece]].Y);
                    item.Size = vm.Interval;
                }
                //Canvas.SetLeft(pieces[n], vm.Positions[pieces[n].Position].X);
                //Canvas.SetTop(pieces[n], vm.Positions[pieces[n].Position].Y);
                //pieces[n].Size = vm.Interval;
            }
        }

        public void Mark(ChessPiece piece, bool isMark)
        {
            this.items[piece.Code].IsMark = isMark;
        }
        #region 处理鼠标事件

        private void FireSelected(System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this.Selected != null)
            {
                int pos = vm.GetPositionFromXY(e.MouseDevice.GetPosition(this).X, e.MouseDevice.GetPosition(this).Y);
                if (pos < 256 && pos >= 0)
                {
                    if (this.Situation.Pieces[pos] != null)//如果此处有棋子，则不响应相关事件
                    {
                        return;
                    }
                }
                Mouse mouse = Mouse.LeftUp;
                if (e.ChangedButton == System.Windows.Input.MouseButton.Right && e.ButtonState == System.Windows.Input.MouseButtonState.Pressed)
                {
                    mouse = Mouse.RightDown;
                }
                else if (e.ChangedButton == System.Windows.Input.MouseButton.Right && e.ButtonState == System.Windows.Input.MouseButtonState.Released)
                {
                    mouse = Mouse.RightUp;
                }
                else if (e.ChangedButton == System.Windows.Input.MouseButton.Left && e.ButtonState == System.Windows.Input.MouseButtonState.Pressed)
                {
                    mouse = Mouse.LeftDown;
                }
                SelectedEventArgs args = new SelectedEventArgs(pos, mouse, e.ClickCount, e.MouseDevice.GetPosition(this).X, e.MouseDevice.GetPosition(this).Y, null);
                this.Selected(this, args);
            }
        }
        protected override void OnMouseDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            //base.OnMouseDown(e);
            FireSelected(e);
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            FireSelected(e);
            base.OnMouseUp(e);
        }
        #endregion

    }


    //[ValueConversion(typeof(string), typeof(int))]
    //public class NumberConverter : System.ComponentModel.TypeConverter
    //{
    //    public NumberConverter(Type targetType)
    //    {
    //    }
    //    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    //    {
    //        return base.CanConvertTo(context, destinationType);
    //    }

    //    public override object CreateInstance(ITypeDescriptorContext context, System.Collections.IDictionary propertyValues)
    //    {
    //        return base.CreateInstance(context, propertyValues);
    //    }


    //    public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
    //    {
    //        //return base.ConvertFrom(context, culture, value);
    //        //return 200L;
    //        int argInt;
    //        if (value == null || (typeof(string) != value.GetType() && typeof(String) != value.GetType()))
    //        {
    //            return 0L;
    //        }
    //        string numberString = (string)value;
    //        numberString = numberString.Substring(2);
    //        if (numberString.ToLower().StartsWith("0x"))
    //        {
    //            argInt = Int32.Parse(numberString,
    //                        NumberStyles.HexNumber,
    //                        CultureInfo.ReadOnly(new CultureInfo("en-us", false)).NumberFormat);
    //        }
    //        //else if (numberString.ToLower().StartsWith("0"))
    //        //{
    //        //    argInt = Int32.Parse(numberString,
    //        //                NumberStyles.,
    //        //                CultureInfo.ReadOnly(new CultureInfo("en-us", false)).NumberFormat);
    //        //}
    //        //else if (numberString.ToLower().StartsWith("0x"))
    //        //{
    //        //    argInt = Int32.Parse(numberString,
    //        //                NumberStyles.HexNumber,
    //        //                CultureInfo.ReadOnly(new CultureInfo("en-us", false)).NumberFormat);
    //        //}
    //        else
    //        {
    //            argInt = Int32.Parse(numberString,
    //                        NumberStyles.HexNumber,
    //                        CultureInfo.ReadOnly(new CultureInfo("en-us", false)).NumberFormat);
    //        }
    //        return argInt;
    //    }

    //    public override bool IsValid(ITypeDescriptorContext context, object value)
    //    {
    //        return base.IsValid(context, value);
    //    }
    //    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    //    {
    //        return true;
    //    }
    //    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}


    //public class NumberTypeDescriptionProvider : TypeDescriptionProvider
    //{
    //    private Type target;
    //    public NumberTypeDescriptionProvider(Type target)
    //    {
    //        this.target = target;
    //    }
    //    public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
    //    {
    //        return new NumberCustomTypeDescriptor(this.target);
    //    }
    //}

    //public class NumberCustomTypeDescriptor : ICustomTypeDescriptor
    //{
    //    private Type target;
    //    public NumberCustomTypeDescriptor(Type target)
    //    {
    //        this.target = target;
    //    }
    //    public AttributeCollection GetAttributes()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public string GetClassName()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public string GetComponentName()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public TypeConverter GetConverter()
    //    {
    //        return new NumberConverter(target);
    //    }

    //    public EventDescriptor GetDefaultEvent()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public PropertyDescriptor GetDefaultProperty()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public object GetEditor(Type editorBaseType)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public EventDescriptorCollection GetEvents(Attribute[] attributes)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public EventDescriptorCollection GetEvents()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public PropertyDescriptorCollection GetProperties()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public object GetPropertyOwner(PropertyDescriptor pd)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
