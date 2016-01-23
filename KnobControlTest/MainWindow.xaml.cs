/// Simple WPF/C# Knob Control 
/// Author: n37jan (n37jan@gmail.com)
/// License: BSD License

using System;
using System.Windows;


namespace KnobControlTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void KnobControl_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Console.WriteLine("Control Value Changed!");
        }
    }
}
