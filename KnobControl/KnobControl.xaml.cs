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
                UpdateValueLabel();
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
                UpdateValueLabel();
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
                UpdateValueLabel();
            }
        }

        private bool UpdateValueLabel()
        {

            this.ValueLabel.Content = Title + ": " + Value.ToString() + " [" + Unit + "]";

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

    }


}
