using Microsoft.Expression.Shapes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KnobControl
{
    /// <summary>
    /// Interaction logic for KnobControl.xaml
    /// </summary>
    [DefaultEvent("ValueChanged"), DefaultProperty("Value")]
    public partial class KnobControl : UserControl
    {
        private RoutedPropertyChangedEventHandler<double> onValueChanged;
        public event RoutedPropertyChangedEventHandler<double> ValueChanged
        {
            add
            {
                onValueChanged += value;
            }
            remove
            {
                onValueChanged -= value;
            }
        }

        public KnobControl()
        {
            InitializeComponent();

            InitializeLevelArc();
            InitializeLabel();


            Title = "Time";
            Minimum = 0;
            Maximum = 10;
            Value = 7;
            Step = 1;
            Unit = "h";
        }

        private bool UpdateLabelAndArc()
        {
            UpdateValueLabel();
            UpdateArc();

            return true;
        }

        #region Arc Control

        [Description("Gets or sets a color for the knob control's minimum value."), Category("Knob Control")]
        public Color ColorForMinimum
        {
            get;
            set;
        }

        [Description("Gets or sets a color for the knob control's maximum value."), Category("Knob Control")]
        public Color ColorForMaximum
        {
            get;
            set;
        }

        private double _arcStartAngle;
        [Description("Gets or sets start angle for the level indicating arc. Unit is in degree."), Category("Knob Control")]
        public double arcStartAngle
        {
            get
            {
                return _arcStartAngle;
            }
            set
            {
                _arcStartAngle = value;

                if (levelIndicatingArc != null)
                {
                    UpdateArc();
                }
                
            }
        }

        private double _arcEndAngle;
        [Description("Gets or sets end angle for the level indicating arc. Unit is in degree."), Category("Knob Control")]
        public double arcEndAngle
        {
            get
            {
                return _arcEndAngle;
            }
            set
            {
                _arcEndAngle = value;

                if(levelIndicatingArc !=null)
                {
                    UpdateArc();
                }
            }
        }

        private Arc levelIndicatingArc
        {
            get;
            set;
        }

        private bool InitializeLevelArc()
        {
            arcStartAngle = -180;   // default start angle
            arcEndAngle = 180;      // default end angle

            levelIndicatingArc = new Arc();
            levelIndicatingArc.Stretch = Stretch.None;
            levelIndicatingArc.StartAngle = arcStartAngle;
            levelIndicatingArc.EndAngle = arcEndAngle;
            levelIndicatingArc.Stroke = Brushes.Red;
            levelIndicatingArc.Width = 200;
            levelIndicatingArc.Height = 200;
            levelIndicatingArc.IsHitTestVisible = false;
            levelIndicatingArc.StrokeThickness = 20;

            knobGrid.Children.Add(levelIndicatingArc);
            
            ColorForMaximum = Colors.Red;   // default color
            ColorForMinimum = Colors.Blue; // default color

            return true;
        }

        private Color ColorBlend(Color color1, Color color2, double alpha)
        {
            byte r = (byte)((color1.R * (1-alpha)) + color2.R * alpha);
            byte g = (byte)((color1.G * (1-alpha)) + color2.G * alpha);
            byte b = (byte)((color1.B * (1-alpha)) + color2.B * alpha);

            return Color.FromRgb(r, g, b);
        }

        private bool UpdateArc()
        {
            levelIndicatingArc.StartAngle = arcStartAngle;

            double newAngle = (arcEndAngle - arcStartAngle) / (Maximum - Minimum) * (Value - Minimum) + arcStartAngle;

            levelIndicatingArc.EndAngle = newAngle;


            double newColorAlpha = 1.0 / (Maximum - Minimum) * (Value - Minimum);
            Color newColor = ColorBlend(ColorForMinimum, ColorForMaximum, newColorAlpha);
            levelIndicatingArc.Stroke = new SolidColorBrush(newColor);

            return true;
        }

        #endregion

        #region Public Knob Control Properties
        public string _Title;
        [Description("Gets or sets title for the knob control."), Category("Knob Control")]
        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                _Title = value;
                UpdateLabelAndArc();
            }
        }

        private string _Unit;
        [Description("Gets or sets unit text for the knob control."), Category("Knob Control")]
        public string Unit
        {
            get
            {
                return _Unit;
            }
            set
            {
                _Unit = value;
                UpdateLabelAndArc();
            }
        }

        private double _Value;
        [Description("Gets or sets value for the knob control."), Category("Knob Control")]
        public double Value
        {
            get
            {
                return _Value;
            }
            set
            {
                double oldValue = _Value;
                _Value = value;
                _Value = Math.Max(Math.Min(_Value, Maximum), Minimum);
                UpdateLabelAndArc();

                if( onValueChanged!=null && oldValue != _Value)
                {
                    RoutedPropertyChangedEventArgs<double> e = new RoutedPropertyChangedEventArgs<double>(oldValue, _Value);

                    onValueChanged(this, e);
                }

            }
        }

        private double _Minimum;
        [Description("Gets or sets the minimum value for the knob control. It can not be more than the maximum."), Category("Knob Control")]
        public double Minimum
        {
            get
            {
                return _Minimum;
            }
            set
            {
                _Minimum = value;
                //_Minimum = Math.Min(_Minimum, Maximum);
            }
        }

        private double _Maximum;
        [Description("Gets or sets the maximum value for the knob control. It can not be less than the maximum."), Category("Knob Control")]
        public double Maximum
        {
            get
            {
                return _Maximum;
            }
            set
            {
                _Maximum = value;
                //_Maximum = Math.Max(_Maximum, Minimum);
            }
        }

        private double _Step;
        [Description("Gets or sets a step for the knob control."), Category("Knob Control")]
        public double Step
        {
            get
            {
                return _Step;
            }
            set
            {
                _Step = value;
            }
        }
        #endregion


        #region Label Control
        private bool InitializeLabel()
        {

            LabelFontSize = 22;
            LabelFont = new FontFamily("Consolas");
            
            return true;
        }

        private bool UpdateValueLabel()
        {
            // for Title Label
            if (Title != null && Title.Length > 0)
            {
                displayTextBlock.Text = Title;
            }
            else
            {
                displayTextBlock.Text = string.Empty;
            }

            if (LabelFont != null)
            {
                displayTextBlock.FontFamily = LabelFont;
            }
            displayTextBlock.FontSize = LabelFontSize;


            // for Value Label
            displayTextBlock.Text += "\n" + Value.ToString();

            if (Unit != null && Unit.Length > 0)
            {
                displayTextBlock.Text += "[" + Unit + "]";
            }
            


            return true;
        }

        private FontFamily _LabelFont;
        [Description("Gets or sets a font for the knob control."), Category("Knob Control")]
        public FontFamily LabelFont
        {
            get
            {
                return _LabelFont;
            }
            set
            {
                _LabelFont = value;

                if(displayTextBlock != null)
                {
                    UpdateValueLabel();
                }
            }
        }

        private double _LabelFontSize;
        [Description("Gets or sets a font size for the knob control."), Category("Knob Control")]
        public double LabelFontSize
        {
            get
            {
                return _LabelFontSize;
            }
            set
            {
                _LabelFontSize = value;

                if(displayTextBlock != null)
                {
                    UpdateValueLabel();
                }
            }
        }

        #endregion


        #region Mouse Event Controllers

        private bool isMouseDown = false;
        private Point previousMousePosition;
        private double mouseMoveThreshold = 1.0;
        private void Ellipse_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double d = e.Delta / 120; // Mouse wheel 1 click (120 delta) = 1 step

            Value += d * Step;
        }

        private void Ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = true;
            (sender as Ellipse).CaptureMouse();
            previousMousePosition = e.GetPosition((Ellipse)sender);
        }

        private void Ellipse_MouseMove(object sender, MouseEventArgs e)
        {
            if( isMouseDown )
            {
                Point newMousePosition = e.GetPosition((Ellipse)sender);

                double dY = (previousMousePosition.Y - newMousePosition.Y);

                if( Math.Abs(dY) > mouseMoveThreshold)
                {
                    Value += Math.Sign(dY) * Step;
                }

                previousMousePosition = newMousePosition;
                
            }
        }

        private void Ellipse_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = false;
            (sender as Ellipse).ReleaseMouseCapture();
        }
        #endregion


    }


}
