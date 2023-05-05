using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ManageMagazine
{

    public partial class ProductsWindow : Window
    {
       
        List<Product> productList = new List<Product>(){ };
        Database databaseObject;
        bool isBeingEdited = false;

        public ProductsWindow()
        {
            InitializeComponent();
            databaseObject = new Database();
            GetDataFromDB();
            ProductsListView.ItemsSource = productList;
        }
        private void GetDataFromDB()
        {
            databaseObject.OpenConnection();
            string query = "Select * from products";
            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

            SQLiteDataReader result = myCommand.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    productList.Add(new Product(Convert.ToInt32(result["id"]),
                                                                result["Name"].ToString(),
                                                                result["Manufacturer"].ToString(),
                                                                Convert.ToDouble(result["PurchasePrice"]),
                                                                Convert.ToDouble(result["SalePrice"]),
                                                                Convert.ToInt32(result["Quantity"])));
                }
            }

            databaseObject.CloseConnection();
        }


        private void ClearTextInput()
        {
            ProductNameTxt.Text = null;
            ProductNameText.Visibility = Visibility.Visible;
            ProducerTxt.Text = null;
            ProducerText.Visibility = Visibility.Visible;
            SalePriceTxt.Text = null;
            SalePriceText.Visibility = Visibility.Visible;
            PurchasePriceTxt.Text = null;
            PurchasePriceText.Visibility = Visibility.Visible;
            QuantityTxt.Text = null;
            QuantityText.Visibility = Visibility.Visible;
        }

        private void Refresh()
        {
            ProductsListView.ItemsSource = null;
            productList.Clear();

            GetDataFromDB();
            ProductsListView.ItemsSource = productList;
        }
        private void AddNewProductButton_Click(object sender, RoutedEventArgs e)
        {

            if (!(string.IsNullOrEmpty(ProductNameTxt.Text) && string.IsNullOrEmpty(ProducerTxt.Text) && string.IsNullOrEmpty(SalePriceTxt.Text) && string.IsNullOrEmpty(PurchasePriceTxt.Text) && string.IsNullOrEmpty(QuantityTxt.Text)))
            {
                try
                {
                    string query = "INSERT INTO products ('Name','Manufacturer','PurchasePrice','SalePrice','Quantity') VALUES (@Name,@Manufacturer,@PurchasePrice,@SalePrice,@Quantity)";

                    SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

                    int lastProductId = productList.Count;

                    databaseObject.OpenConnection();

                        myCommand.Parameters.AddWithValue("@Name", ProductNameTxt.Text);
                        myCommand.Parameters.AddWithValue("@Manufacturer", ProducerTxt.Text);
                        myCommand.Parameters.AddWithValue("@PurchasePrice", Convert.ToDouble(SalePriceTxt.Text));
                        myCommand.Parameters.AddWithValue("@SalePrice", Convert.ToDouble(PurchasePriceTxt.Text));
                        myCommand.Parameters.AddWithValue("@Quantity", Convert.ToInt32(QuantityTxt.Text));
                        myCommand.ExecuteNonQuery();

                    databaseObject.CloseConnection();
                    productList.Add(new Product() {Id = lastProductId + 1, Name = ProductNameTxt.Text, Manufacturer = ProducerTxt.Text, SalePrice = Convert.ToDouble(SalePriceTxt.Text), PurchasePrice = Convert.ToDouble(PurchasePriceTxt.Text), Quantity = Convert.ToInt32(QuantityTxt.Text) });
                    Refresh();
                    ClearTextInput();

                }
                catch (Exception)
                {
                    InfoWindow window = new InfoWindow("Wrong Input");
                    window.ShowDialog();
                }
            }
        }
        private void Edit_Button_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsListView.SelectedIndex != -1) //sprawdzenie, czy wybrano element do edycji
            {
                int index = ProductsListView.SelectedIndex;
                int productId = productList[index].Id;
                if (!isBeingEdited)
                {
                    try
                    {
                        ProductNameTxt.Text = productList[index].Name;
                        ProducerTxt.Text = productList[index].Manufacturer;
                        SalePriceTxt.Text = productList[index].SalePrice.ToString();
                        PurchasePriceTxt.Text = productList[index].PurchasePrice.ToString();
                        QuantityTxt.Text = productList[index].Quantity.ToString();

                        EditSaveButton.Content = "SAVE";
                        isBeingEdited = true;

                    }
                    catch
                    {
                        InfoWindow window = new InfoWindow("Wrong Input");
                        window.ShowDialog();
                    }
                }
                else
                {
                    try
                    {
                        productList[index].Name = ProductNameTxt.Text;
                        productList[index].Manufacturer = ProducerTxt.Text;
                        productList[index].SalePrice = Convert.ToDouble(SalePriceTxt.Text);
                        productList[index].PurchasePrice = Convert.ToDouble(PurchasePriceTxt.Text);
                        productList[index].Quantity = Convert.ToInt32(QuantityTxt.Text);

                        EditSaveButton.Content = "EDIT";
                        isBeingEdited = false;

                        string query = "UPDATE products SET Name=@Name, Manufacturer=@Manufacturer, PurchasePrice=@PurchasePrice, SalePrice=@SalePrice, Quantity=@Quantity WHERE id=@id";

                        SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

                        databaseObject.OpenConnection();

                        myCommand.Parameters.AddWithValue("@Name", ProductNameTxt.Text);
                        myCommand.Parameters.AddWithValue("@Manufacturer", ProducerTxt.Text);
                        myCommand.Parameters.AddWithValue("@PurchasePrice", Convert.ToDouble(PurchasePriceTxt.Text));
                        myCommand.Parameters.AddWithValue("@SalePrice", Convert.ToDouble(SalePriceTxt.Text));
                        myCommand.Parameters.AddWithValue("@Quantity", Convert.ToInt32(QuantityTxt.Text));
                        myCommand.Parameters.AddWithValue("@id", productId); //zmiana indeksu na ID produktu

                        myCommand.ExecuteNonQuery();

                        databaseObject.CloseConnection();
                        Refresh();
                        ClearTextInput();

                    }
                    catch
                    {
                        InfoWindow window = new InfoWindow("Wrong Input");
                        window.ShowDialog();
                    }
                }
            }
            else //nie wybrano produktu do edycji
            {
                InfoWindow window = new InfoWindow("No element was chosen");
                window.ShowDialog();
            }
}
        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsListView.SelectedIndex != -1) //sprawdzenie, czy wybrano element do usunięcia
            {
                int index = ProductsListView.SelectedIndex;
                int productId = productList[index].Id;

                try
                {
                    string query = "DELETE FROM products WHERE id=@id";

                    SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

                    databaseObject.OpenConnection();

                    myCommand.Parameters.AddWithValue("@id", productId);

                    myCommand.ExecuteNonQuery();

                    databaseObject.CloseConnection();
                    productList.RemoveAt(index);

                    Refresh();
                    ClearTextInput();
                }
                catch (Exception)
                {
                    InfoWindow window = new InfoWindow("Error while deleting product");
                    window.ShowDialog();
                }
            }
            else //nie wybrano produktu do usunięcia
            {
                InfoWindow window = new InfoWindow("No element was chosen");
                window.ShowDialog();
            }
        }

        #region Focusing on TextBox



        private void ProductName_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ProductNameTxt.Focus();
        }

        private void ProductNameText_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ProductNameText.Text) && ProductNameText.Text.Length > 0)
            {
                ProductNameText.Visibility = Visibility.Collapsed;
            }
            else
            {
                ProductNameText.Visibility = Visibility.Visible;
            }
        }

        private void Producer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ProducerTxt.Focus();
        }        

        private void ProducerText_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(ProducerText.Text) && ProducerText.Text.Length > 0)
            {
                ProducerText.Visibility = Visibility.Collapsed;
            }
            else
            {
                ProducerText.Visibility = Visibility.Visible;
            }
        }

        private void PurchasePrice_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PurchasePriceTxt.Focus();
        }

        private void PurchasePriceText_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(PurchasePriceText.Text) && PurchasePriceText.Text.Length > 0)
            {
                PurchasePriceText.Visibility = Visibility.Collapsed;
            }
            else
            {
                PurchasePriceText.Visibility = Visibility.Visible;
            }
        }

        private void SalePrice_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SalePriceTxt.Focus();
        }

        private void SalePriceText_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(SalePriceText.Text) && SalePriceText.Text.Length > 0)
            {
                SalePriceText.Visibility = Visibility.Collapsed;
            }
            else
            {
                SalePriceText.Visibility = Visibility.Visible;
            }
        }

        private void Quantity_MouseDown(object sender, MouseButtonEventArgs e)
        {
            QuantityTxt.Focus();
        }

        private void Quantitys_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(QuantityText.Text) && QuantityText.Text.Length > 0)
            {
                QuantityText.Visibility = Visibility.Collapsed;
            }
            else
            {
                QuantityText.Visibility = Visibility.Visible;
            }
        }
        #endregion

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
