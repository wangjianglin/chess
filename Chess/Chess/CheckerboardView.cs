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


namespace Lin.Chess
{

    [ValueConversion(typeof(string), typeof(int))]
    public class NumberConverter : System.ComponentModel.TypeConverter
    {
        public NumberConverter(Type targetType)
        {
        }
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return base.CanConvertTo(context, destinationType);
        }

        public override object CreateInstance(ITypeDescriptorContext context, System.Collections.IDictionary propertyValues)
        {
            return base.CreateInstance(context, propertyValues);
        }


        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            //return base.ConvertFrom(context, culture, value);
            //return 200L;
            int argInt;
            string numberString = (string)value;
            numberString = numberString.Substring(2);
            if (Int32.TryParse(numberString,
                        NumberStyles.HexNumber,
                        CultureInfo.ReadOnly(new CultureInfo("en-us", false)).NumberFormat,
                        out argInt))
            {
                //arg = argInt;
            }
            return argInt;
        }

        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            return base.IsValid(context, value);
        }
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return true;
        }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    

    public class NumberTypeDescriptionProvider : TypeDescriptionProvider
    {
        private Type target;
        public NumberTypeDescriptionProvider(Type target)
        {
            this.target = target;
        }
        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            return new NumberCustomTypeDescriptor(this.target);
        }
    }

    public class NumberCustomTypeDescriptor : ICustomTypeDescriptor
    {
        private Type target;
        public NumberCustomTypeDescriptor(Type target)
        {
            this.target = target;
        }
        public AttributeCollection GetAttributes()
        {
            throw new NotImplementedException();
        }

        public string GetClassName()
        {
            throw new NotImplementedException();
        }

        public string GetComponentName()
        {
            throw new NotImplementedException();
        }

        public TypeConverter GetConverter()
        {
            return new NumberConverter(target);
        }

        public EventDescriptor GetDefaultEvent()
        {
            throw new NotImplementedException();
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            throw new NotImplementedException();
        }

        public object GetEditor(Type editorBaseType)
        {
            throw new NotImplementedException();
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            throw new NotImplementedException();
        }

        public EventDescriptorCollection GetEvents()
        {
            throw new NotImplementedException();
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            throw new NotImplementedException();
        }

        public PropertyDescriptorCollection GetProperties()
        {
            throw new NotImplementedException();
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            throw new NotImplementedException();
        }
    }

    public class CheckerboardView:Control
    {
        //在VM中确定各个棋子与屏幕上显示的位置，
        private class CheckerboardViewModel : Lin.Core.ViewModel.ViewModel
        {
            private double width = 0;
            private double marginHeight = 0;
            private double marginWidth = 0;
            private double interval = 0;
            CheckerboardView view;
            public void setWidth(double width)
            {
                interval = width / 9.5;
                marginHeight = marginWidth = interval * 0.75;
                this.width = width;
                this.OnPropertyChanged("Positions");
            }

            private Lin.Util.ReadOnlyIndexProperty<long, Point> positions = null;
            public CheckerboardViewModel(CheckerboardView view,double width)
            {
                this.setWidth(width);
                this.view = view;
                positions = new Util.ReadOnlyIndexProperty<long, Point>(pos =>
                {
                    long x = pos % 16;
                    long y = pos / 16;
                    return new Point((x-3) * this.interval +this. marginWidth,(y-3)*this.interval+this.marginHeight);
                });

                self.Positions = positions;
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
            TypeDescriptor.AddProvider(new NumberTypeDescriptionProvider(typeof(long)), typeof(long));

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


        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            vm.setWidth(sizeInfo.NewSize.Width);
        }

        private CheckerboardViewModel vm = null;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            vm = new CheckerboardViewModel(this,this.ActualWidth);
            this.DataContext = vm;
            DependencyObject board = this.GetTemplateChild("PATH_Board");
            for (byte n = 0; n < 32; n++)
            {
                pieces[n] = BuildChessPices(n);
                //board.Children.Add(pieces[n]);
            }
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
}
