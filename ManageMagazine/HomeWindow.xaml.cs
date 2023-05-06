using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
using System.Windows.Shapes;

namespace ManageMagazine
{

    public partial class HomeWindow : Window
    {
        List<Product> lastProductList;
        Database databaseObject;
        public HomeWindow()
        {
            InitializeComponent();
            databaseObject = new Database();
            lastProductList = new List<Product>() { };
            CheckingLastProducts();
        }
        private void GetDataFromDB()
        {
            databaseObject.OpenConnection();
            string query = "Select * from products Where Quantity<5 Order BY Quantity ASC";
            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

            SQLiteDataReader result = myCommand.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    lastProductList.Add(new Product(Convert.ToInt32(result["id"]),
                                                                result["Name"].ToString(),
                                                                result["Manufacturer"].ToString(),
                                                                Convert.ToDouble(result["PurchasePrice"]),
                                                                Convert.ToDouble(result["SalePrice"]),
                                                                Convert.ToInt32(result["Quantity"])));
                }
            }

            databaseObject.CloseConnection();
        }
        private void CheckingLastProducts()
        {
            GetDataFromDB(); 
            LastProductsListView.ItemsSource = lastProductList;

        }

        #region ClosingMinimalizingApp
        /* Closing, Minimalizing, Login Navigation buttons functionality*/

        // Closing button function
        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
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

        

       
        #endregion

    }
}
