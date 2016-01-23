using Microsoft.Expression.Shapes;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace KnobControlNamespace
{
    /// <summary>
    /// Interaction logic for KnobControl.xaml
    /// </summary>
    [DefaultEvent("ValueChanged"), DefaultProperty("Value")]
    public partial class KnobControl : UserControl
    {
        public event RoutedPropertyChangedEventHandler<double> ValueChanged = null;

        private void onChanged(RoutedPropertyChangedEventArgs<double> e)
        {
            if (ValueChanged != null)
            {
                ValueChanged(this, e);
            }

            Update();
        }

        #region Private Fields

        private Color _ColorForMinimum = Colors.Blue;
        private Color _ColorForMaximum = Colors.Red;
        private double _arcStartAngle = -180;
        private double _arcEndAngle = 180;
        public string _Title = "Missing";
        private string _Unit = "hours";
        private double _Value = 7;
        private double _Minimum = 0;
        private double _Maximum = 10;
        private double _Step = 1;
        private double _LabelFontSize = 22;
        private FontFamily _LabelFont = new FontFamily("Consolas");
        private Arc levelIndicatingArc = new Arc();

        private bool isMouseDown = false;
        private Point previousMousePosition;
        private double mouseMoveThreshold = 20;

        #endregion Private Fields

        public KnobControl()
        {
            InitializeComponent();

            Initialize();
        }

        private void Initialize()
        {
            levelIndicatingArc.Stretch = Stretch.None;
            levelIndicatingArc.StartAngle = arcStartAngle;
            levelIndicatingArc.EndAngle = arcEndAngle;
            levelIndicatingArc.Stroke = Brushes.Red;
            levelIndicatingArc.IsHitTestVisible = false;
            levelIndicatingArc.StrokeThickness = 20;

            knobGrid.Children.Add(levelIndicatingArc);
        }

        [Description("Gets or sets a color for the knob control's minimum value."), Category("Knob Control")]
        public Color ColorForMinimum
        {
            get
            {
                return _ColorForMinimum;
            }
            set
            {
                _ColorForMinimum = value;

                Update();
            }
        }

        [Description("Gets or sets a color for the knob control's maximum value."), Category("Knob Control")]
        public Color ColorForMaximum
        {
            get
            {
                return _ColorForMaximum;
            }
            set
            {
                _ColorForMaximum = value;

                Update();
            }
        }

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

                Update();
            }
        }

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

                Update();
            }
        }

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

                Update();
            }
        }

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

                Update();
            }
        }

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

                if (this.IsLoaded)
                {
                    _Value = Math.Max(Math.Min(_Value, Maximum), Minimum);
                    onChanged(new RoutedPropertyChangedEventArgs<double>(oldValue, _Value));
                    Update();
                }
            }
        }

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

                Update();
            }
        }

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

                Update();
            }
        }

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

                Update();
            }
        }

        private void Update()
        {
            // for Title Label
            displayTextBlock.Text = string.Empty;
            if (Title.Length > 0)
            {
                displayTextBlock.Text = Title;
            }

            displayTextBlock.FontFamily = LabelFont;
            displayTextBlock.FontSize = LabelFontSize;

            // for Value Label
            displayTextBlock.Text += "\n" + Value.ToString();

            if (Unit.Length > 0)
            {
                displayTextBlock.Text += "[" + Unit + "]";
            }

            // Update Arc
            levelIndicatingArc.StartAngle = arcStartAngle;

            double newAngle = (arcEndAngle - arcStartAngle) / (Maximum - Minimum) * (Value - Minimum) + arcStartAngle;
            levelIndicatingArc.EndAngle = newAngle;

            double newColorAlpha = 1.0 / (Maximum - Minimum) * (Value - Minimum);
            Color newColor = ColorBlend(ColorForMinimum, ColorForMaximum, newColorAlpha);
            levelIndicatingArc.Stroke = new SolidColorBrush(newColor);
        }

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

                Update();
            }
        }

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

                Update();
            }
        }

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
            if (isMouseDown)
            {
                Point newMousePosition = e.GetPosition((Ellipse)sender);

                double dY = (previousMousePosition.Y - newMousePosition.Y);

                if (Math.Abs(dY) > mouseMoveThreshold)
                {
                    Value += Math.Sign(dY) * Step;
                    previousMousePosition = newMousePosition;
                }
            }
        }

        private void Ellipse_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = false;
            (sender as Ellipse).ReleaseMouseCapture();
        }

        private Color ColorBlend(Color color1, Color color2, double alpha)
        {
            byte r = (byte)((color1.R * (1 - alpha)) + color2.R * alpha);
            byte g = (byte)((color1.G * (1 - alpha)) + color2.G * alpha);
            byte b = (byte)((color1.B * (1 - alpha)) + color2.B * alpha);

            return Color.FromRgb(r, g, b);
        }

        private void knobUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Update();
        }
    }
}