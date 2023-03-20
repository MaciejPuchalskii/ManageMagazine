using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace ManageMagazine
{

    public partial class ProductsWindow : Window
    {
        public ProductsWindow()
        {
            InitializeComponent();
            ProductsListView.ItemsSource = productList;

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


         List<Product> productList = new List<Product>()
        {
            new Product() { Id = 1, Name = "Blue Shark T-Shirt", Manufacturer = "Adidas",  PurchasePrice = 12.1 , SalePrice = 50.2 , Quantity = 10},
            new Product() { Id = 2, Name = "Red Crab T-Shirt", Manufacturer = "Nike",  PurchasePrice = 11.1 , SalePrice = 30.2 , Quantity = 10},
            new Product() { Id = 3, Name = "Green Lizard T-Shirt", Manufacturer = "Hias",  PurchasePrice = 5.1 , SalePrice = 20.2 , Quantity = 10},
            new Product() { Id = 4, Name = "Black Hoodie", Manufacturer = "Adidas",  PurchasePrice = 16.1 , SalePrice = 78.2 , Quantity = 15},
            new Product() { Id = 5, Name = "Green Lizard T-Shirt", Manufacturer = "Hias",  PurchasePrice = 5.1 , SalePrice = 20.2 , Quantity = 15},
            new Product() { Id = 6, Name = "Leather Belt", Manufacturer = "Lancerto",  PurchasePrice = 15.1 , SalePrice = 55.2 , Quantity = 4},

        };

        private void AddNewProductButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddEditProductWindow addEdit = new();
                addEdit.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
