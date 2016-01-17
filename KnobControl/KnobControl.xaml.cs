using Microsoft.Expression.Shapes;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class KnobControl : UserControl
    {
        public KnobControl()
        {
            InitializeComponent();

            InitializeLevelArc();
        }

        private bool UpdateLabelAndArc()
        {
            UpdateValueLabel();
            UpdateArc();

            return true;
        }

        #region Arc Control

        public Color ColorForMinimum
        {
            get; set;
        }

        public Color ColorForMaximum
        {
            get; set;
        }

        private const double ARC_START_ANGLE = -180;
        private const double ARC_END_ANGLE = 180;

        private Arc levelArc
        {
            get;
            set;
        }

        private bool InitializeLevelArc()
        {
            levelArc = new Arc();
            levelArc.Stretch = Stretch.None;
            levelArc.StartAngle = ARC_START_ANGLE;
            levelArc.EndAngle = ARC_END_ANGLE;
            levelArc.Stroke = Brushes.Red;
            levelArc.Width = 200;
            levelArc.Height = 200;
            levelArc.IsHitTestVisible = false;
            levelArc.StrokeThickness = 12;

            knobGrid.Children.Add(levelArc);


            ColorForMaximum = Colors.Red;   // default color
            ColorForMinimum = Colors.Green; // default color

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
            double newAngle = (ARC_END_ANGLE - ARC_START_ANGLE) / (Maximum - Minimum) * (Value - Minimum) + ARC_START_ANGLE;

            levelArc.EndAngle = newAngle;


            double newColorAlpha = 1.0 / (Maximum - Minimum) * (Value - Minimum);
            Color newColor = ColorBlend(ColorForMinimum, ColorForMaximum, newColorAlpha);
            levelArc.Stroke = new SolidColorBrush(newColor);

            return true;
        }

        #endregion

        #region Public Knob Control Properties
        public string _Title;
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
            }
        }

        private double _Minimum;
        public double Minimum
        {
            get
            {
                return _Minimum;
            }
            set
            {
                _Minimum = value;
                _Minimum = Math.Min(_Minimum, Maximum);
            }
        }

        private double _Maximum;
        public double Maximum
        {
            get
            {
                return _Maximum;
            }
            set
            {
                _Maximum = value;
                _Maximum = Math.Max(_Maximum, _Minimum);
            }
        }

        private double _Step;
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
        private bool UpdateValueLabel()
        {
            if (Title != null && Title.Length > 0)
            {
                ValueLabel.Content = Title + ": ";
            }
            else
            {
                ValueLabel.Content = string.Empty;
            }

            ValueLabel.Content += Value.ToString();

            if (Unit != null && Unit.Length > 0)
            {
                ValueLabel.Content += " [" + Unit + "]";
            }


            return true;
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
