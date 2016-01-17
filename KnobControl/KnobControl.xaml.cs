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
            levelArc.Width = 190;
            levelArc.Height = 190;
            levelArc.IsHitTestVisible = false;
            levelArc.StrokeThickness = 10;

            knobGrid.Children.Add(levelArc);

            return true;
        }

        private bool UpdateArc()
        {
            double newAngle = (ARC_END_ANGLE - ARC_START_ANGLE) / (Maximum - Minimum) * (Value - Minimum) + ARC_START_ANGLE;

            levelArc.EndAngle = newAngle;

            return true;
        }


        private bool UpdateLabelAndArc()
        {
            UpdateValueLabel();
            UpdateArc();

            return true;
        }


        public KnobControl()
        {
            InitializeComponent();

            InitializeLevelArc();
        }


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

        private bool UpdateValueLabel()
        {
            if ( Title != null && Title.Length > 0 )
            {
                ValueLabel.Content = Title + ": ";
            }
            else
            {
                ValueLabel.Content = string.Empty;
            }

            ValueLabel.Content  += Value.ToString();

            if( Unit != null && Unit.Length > 0)
            {
                ValueLabel.Content  += " [" + Unit + "]";
            }


            return true;
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

        private void Ellipse_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double d = e.Delta / 120;

            Value += d * Step;
        }
    }


}
