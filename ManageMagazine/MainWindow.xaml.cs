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

namespace ManageMagazine
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

        #region ClosingMinimalizingApp
        /* Closing, Minimalizing, Login Navigation buttons functionality*/

        // Closing button function

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Minimalize button function

        private void MinimalizeButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                this.WindowState = WindowState.Minimized;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

      
        
        private void SalesNavigation(object sender, RoutedEventArgs e)
        {
            try
            {
                SaleWindow saleWindow = new();
                saleWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ProductsNavigation(object sender, RoutedEventArgs e)
        {
            try
            {
                ProductsWindow productsWindow = new();
                productsWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void CustomersNavigation(object sender, RoutedEventArgs e)
        {
            try
            {
                CustomersWindow customerWindow = new();
                customerWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void UserNavigation(object sender, RoutedEventArgs e)
        {
            try
            {
                UserWindow userWindow = new();
                userWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void HomeNavigation(object sender, RoutedEventArgs e)
        {

            try
            {
                HomeWindow homeWindow = new();
                homeWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        #endregion

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
