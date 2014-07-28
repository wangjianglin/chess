using Lin.Util;
using System;
using System.Activities.Presentation.Metadata;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;


namespace Lin.Chess
{


    public class CheckerboardView:Control
    {
        public event SelectedEventHandler Selected;
        //在VM中确定各个棋子与屏幕上显示的位置，
        private class CheckerboardViewModel : Lin.Core.ViewModel.ViewModel
        {
            private double width = 0;
            private double marginHeight = 0;
            private double marginWidth = 0;
            //private double Interval = 0;
            CheckerboardView view;
            public void setWidth(double width,double height)
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
            private Point[] CreatePositionMark(long pos, int type)
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
            public CheckerboardViewModel(CheckerboardView view,double width,double height)
            {
                _positions = new Util.ReadOnlyIndexProperty<long, Point>(pos =>
                {
                    long x = pos % 16;
                    long y = pos / 16;
                    return new Point((x - 3) * self.Interval + this.marginWidth, (y - 3) * self.Interval + this.marginHeight);
                });
                self.PositionMark = new ReadOnlyIndexProperty<long, Point[][]>(pos => {
                    return new Point[][] { CreatePositionMark(pos, 0) ,
                                                CreatePositionMark(pos,1),
                                                CreatePositionMark(pos,2),
                                                CreatePositionMark(pos,3)};
                });
                this.setWidth(width,height);
                self.StrokeThickness = 1.0;
                self.Stroke = new SolidColorBrush(Colors.Black);
                self.BorderStrokeThickness =2.0;
                self.BorderStroke = new SolidColorBrush(Colors.Black);
               
                this.view = view;
                self.Positions = _positions;
            }

            //[Command(nmae="")]
            private void Command(object param)
            {

            }

            //采用动态类对 属性 进行扩展
            //[PropertyChaned(nmae="")]
            private void PropertyChaned(object value,object old){

            }
        }
        static CheckerboardView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CheckerboardView), new FrameworkPropertyMetadata(typeof(CheckerboardView)));
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

        private ChessPiece[] pieces = new ChessPiece[32];

        public CheckerboardView()
        {
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
            vm = new CheckerboardViewModel(this,this.ActualWidth,this.ActualHeight);
            this.DataContext = vm;
            Canvas board = (Canvas)this.GetTemplateChild("PATH_Board");
            for (byte n = 0; n < 32; n++)
            {
                pieces[n] = BuildChessPices(n);
                pieces[n].Selected += (object sender, SelectedEventArgs args) =>
                {
                    if (this.Selected != null)
                    {
                        this.Selected(this, args);
                    }
                };
                board.Children.Add(pieces[n]);
            }
        }

        protected override void OnMouseDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            //base.OnMouseDown(e);
            if (this.Selected != null)
            {
                Mouse mouse = Mouse.Left;
                if (e.ChangedButton == System.Windows.Input.MouseButton.Right)
                {
                    mouse = Mouse.Right;
                }
                SelectedEventArgs args = new SelectedEventArgs(256, mouse, e.ClickCount, e.MouseDevice.GetPosition(this).X, e.MouseDevice.GetPosition(this).Y, null);
                this.Selected(this, args);
            }
        }

        private void RefeshPos()
        {
            for (byte n = 0; n < 32; n++)
            {
                Canvas.SetLeft(pieces[n], vm.Positions[pieces[n].Position].X);
                Canvas.SetTop(pieces[n], vm.Positions[pieces[n].Position].Y);
                pieces[n].Size = vm.Interval;
            }
        }


        protected override void OnMouseLeftButtonUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
        }

        private static ChessPiece BuildChessPices(byte code, byte? position = null)
        {
            ChessPiece piece = null;
            switch (code)
            {
                case 0:
                case 1:
                    piece = new RedRooks(code, position);
                    break;
                case 2:
                case 3:
                    piece = new RedKnights(code, position);
                    break;
                case 4:
                case 5:
                    piece = new RedElephants(code, position);
                    break;
                case 6:
                case 7:
                    piece = new RedMandarins(code, position);
                    break;
                case 8:
                    piece = new RedKing(code, position);
                    break;
                case 9:
                case 10:
                    piece = new RedCannons(code, position);
                    break;
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                    piece = new RedPawns(code, position);
                    break;

                case 0 + 16:
                case 1 + 16:
                    piece = new BlackRooks(code, position);
                    break;
                case 2 + 16:
                case 3 + 16:
                    piece = new BlackKnights(code, position);
                    break;
                case 4 + 16:
                case 5 + 16:
                    piece = new BlackElephants(code, position);
                    break;
                case 6 + 16:
                case 7 + 16:
                    piece = new BlackMandarins(code, position);
                    break;
                case 8 + 16:
                    piece = new BlackKing(code, position);
                    break;
                case 9 + 16:
                case 10 + 16:
                    piece = new BlackCannons(code, position);
                    break;
                case 11 + 16:
                case 12 + 16:
                case 13 + 16:
                case 14 + 16:
                case 15 + 16:
                    piece = new BlackPawns(code, position);
                    break;
            }
            return piece;
        }
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
