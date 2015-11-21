using CurrencyCalculator.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CurrencyCalculator.Wpf.Code
{
    public class CurrencyCalculatorWindow : Window
    {

        [STAThread]
        public static void Main(string[] args)
        {
            Application app = new Application();
            CurrencyCalculatorWindow window = new CurrencyCalculatorWindow();
            window.Show();
            window.Title = "MyCurrencyCalculator";

            app.Run();
        }

        private TextBox txtLeftValue;
        private TextBox txtRightValue;
        private ComboBox cmbLeftCurrency;
        private ComboBox cmbRightCurrency;

        private ICurrencyCalculator currencyCalculator;

        public CurrencyCalculatorWindow()
        {
            //this.Content = "Hallo ";
            txtLeftValue = new TextBox() { Width = 80 };
            txtRightValue = new TextBox() { Width = 80 };
            cmbLeftCurrency = new ComboBox() { Margin = new Thickness(5,0,5,0)};
            cmbRightCurrency = new ComboBox() { Margin = new Thickness(5, 0, 0, 0) };


            currencyCalculator = CurrencyCalculatorFactory.GetCalculator();
            foreach(var item in currencyCalculator.GetCurrencyData())
            {
                cmbLeftCurrency.Items.Add(item);
                cmbRightCurrency.Items.Add(item);
            }

            cmbLeftCurrency.SelectionChanged += onSelectionChanged;
            cmbRightCurrency.SelectionChanged += onSelectionChanged;

            StackPanel panel = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(10, 10, 10, 10)
            };
            panel.Children.Add(txtLeftValue);
            panel.Children.Add(cmbLeftCurrency);
            panel.Children.Add(txtRightValue);
            panel.Children.Add(cmbRightCurrency);

            this.SizeToContent = SizeToContent.WidthAndHeight;
            this.ResizeMode = ResizeMode.NoResize;
            this.Content = panel;

        }

        private void onSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(sender == cmbLeftCurrency)
            {
                Convert(Conversion.LeftToRight);
            }
            else
            {
                Convert(Conversion.RightToLeft);
            }
        }

        private void Convert(Conversion conversion)
        {
            if (cmbLeftCurrency.SelectedItem == null || cmbRightCurrency.SelectedItem == null)
                return;

            string leftCurrency = ((CurrencyData)cmbLeftCurrency.SelectedItem).Symbol;
            string rightCurrency = ((CurrencyData)cmbRightCurrency.SelectedItem).Symbol;

            double input;
            if (conversion == Conversion.LeftToRight)
            {
                if (double.TryParse(txtLeftValue.Text, out input))
                    txtRightValue.Text = currencyCalculator.Convert(input, leftCurrency, rightCurrency).ToString("F2");
            }
            else
            {
                if (double.TryParse(txtRightValue.Text, out input))
                    txtLeftValue.Text = currencyCalculator.Convert(input, rightCurrency, leftCurrency).ToString("F2");
            }
        }
    }

 
}
