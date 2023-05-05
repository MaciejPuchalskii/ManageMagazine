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

    public partial class SaleWindow : Window
    {
        List<Product> products = new List<Product>();
        Order order;
        List<Customer> customers = new List<Customer>();
        Database databaseObject;

        public SaleWindow()
        {
            InitializeComponent();
            databaseObject = new Database();
            GetProductsFromDB();
            GetCustomersFromDB();
            CustomerComboBox.ItemsSource = customers;
            ProductListView.ItemsSource = products;

        }

        private void GetProductsFromDB()
        {
            databaseObject.OpenConnection();
            string query = "Select * from products";
            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

            SQLiteDataReader result = myCommand.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    products.Add(new Product(Convert.ToInt32(result["id"]),
                                                                result["Name"].ToString(),
                                                                result["Manufacturer"].ToString(),
                                                                Convert.ToDouble(result["PurchasePrice"]),
                                                                Convert.ToDouble(result["SalePrice"]),
                                                                Convert.ToInt32(result["Quantity"])));
                }
            }

            databaseObject.CloseConnection();
        }
        private void GetCustomersFromDB()
        {
            databaseObject.OpenConnection();
            string query = "Select * from customers";
            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

            SQLiteDataReader result = myCommand.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    customers.Add(new Customer(Convert.ToInt32(result["id"]),
                                                                result["FirstName"].ToString(),
                                                                result["LastName"].ToString(),
                                                                result["PhoneNumber"].ToString(),
                                                                result["City"].ToString(),
                                                                result["Street"].ToString(),
                                                                Convert.ToInt32(result["HouseNumber"]),
                                                                result["PostalCode"].ToString()));
                }
            }

            databaseObject.CloseConnection();
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

        private void RefreshListView(ListView listView)
        {
            listView.ItemsSource = null;
        }


        private void QuantityTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsNumeric(e.Text);
        }

        private bool IsNumeric(string text)
        {
            return int.TryParse(text, out int result);
        }
        private void NewOrderButtonClick(object sender, RoutedEventArgs e)
        {
            SetEnabledState(true);
            order = new Order();

        }

        private void SetEnabledState(bool isEnabled)
        {
            MainGrid.IsEnabled = isEnabled;
        }
        



        private void AddToCart(object sender, RoutedEventArgs e)
        {
            if(QuantityTextBox.Text.Length > 0)
            {
                var selectedItem = (Product)ProductListView.SelectedItem;
                int quantity = Convert.ToInt32(QuantityTextBox.Text);
                order.OrderItems.Add(new OrderItem(Order.OrderId, quantity, selectedItem));


                RefreshListView(CartListView);
                CartListView.ItemsSource = order.OrderItems;

                
                
                Total.Text = Convert.ToString(order.Sum) + " $"; 

            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if(CartListView.SelectedIndex!=null)
            {
                int index = CartListView.SelectedIndex;
                order.OrderItems.RemoveAt(index);
                RefreshListView(CartListView);
                CartListView.ItemsSource= order.OrderItems;
                Total.Text = Convert.ToString(order.Sum) + " $";


            }
        }

        private void RefreshLists()
        {

            ProductListView.ItemsSource = null;
            SetEnabledState(false);
            CustomerComboBox.SelectedItem = null;
            ProductListView.SelectedItem = null;
            CartListView.ItemsSource = null;
            QuantityTextBox.Text=null;




            products.Clear();
            GetProductsFromDB();
            ProductListView.ItemsSource = products;
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            bool weHaveEveryting = true;
            List<int> productsId= new List<int>();

            foreach (OrderItem orderItem in order.OrderItems)
            {
                if (orderItem.Quantity > orderItem.Product.Quantity)
                {
                    weHaveEveryting = false;
                    productsId.Add(orderItem.ProductID);
                }
            }

            
                databaseObject.OpenConnection();
                int NextOrderId = databaseObject.GetLastId("Orders") + 1;
                if (order.OrderItems.Count > 0 && CustomerComboBox.SelectedItem != null)
                {

                    if (weHaveEveryting)
                    {
                        try
                        {
                            string query = "INSERT INTO Orders ('ID','CustomerID','TotalPrice') values (@ID,@CustomerID,@TotalPrice);";

                            SQLiteCommand orderCommand = new SQLiteCommand(query, databaseObject.myConnection);

                            var customer = (Customer)CustomerComboBox.SelectedItem;

                            int customerId = customer.Id;




                            orderCommand.Parameters.AddWithValue("@ID", NextOrderId);
                            orderCommand.Parameters.AddWithValue("@CustomerID", customerId);
                            orderCommand.Parameters.AddWithValue("@TotalPrice", order.Sum);

                            orderCommand.ExecuteNonQuery();

                            int recordId = 1;

                            foreach (OrderItem item in order.OrderItems)
                            {
                                string insertOrderItem = "INSERT INTO OrderItems ('OrderID','RecordId','ProductID','Quantity','Price') VALUES (@OrderID,@RecordId,@ProductID,@Quantity,@Price);";
                                string updateQuantity = "UPDATE Products SET Quantity = Quantity-@selling WHERE Id=@ProductID";
                                SQLiteCommand itemsCommand = new SQLiteCommand(insertOrderItem, databaseObject.myConnection);
                                SQLiteCommand updateComand = new SQLiteCommand(updateQuantity, databaseObject.myConnection);
                                itemsCommand.Parameters.AddWithValue("@OrderID", NextOrderId);
                                itemsCommand.Parameters.AddWithValue("@RecordId", recordId);
                                itemsCommand.Parameters.AddWithValue("@ProductID", item.ProductID);
                                itemsCommand.Parameters.AddWithValue("@Quantity", item.Quantity);
                                itemsCommand.Parameters.AddWithValue("@Price", item.Price);


                                updateComand.Parameters.AddWithValue("@selling", item.Quantity);
                                updateComand.Parameters.AddWithValue("@ProductID", item.ProductID);

                                itemsCommand.ExecuteNonQuery();

                                updateComand.ExecuteNonQuery();
                                recordId++;
                            }


                        InfoWindow window = new InfoWindow("Order confirmed");
                        window.ShowDialog();

                        RefreshLists();
                    }
                        catch (Exception)
                        {
                            InfoWindow window = new InfoWindow("Order wasn't added");
                            window.ShowDialog();
                        }
                        databaseObject.CloseConnection();
                    }
                    else
                    {
                        string message = "Pproducts with that IDs can't be send check stock: ";
                        foreach (int i in productsId)
                        {
                            message += i.ToString() + ",";
                        }

                        InfoWindow window = new InfoWindow(message);
                        window.ShowDialog();
                    }
                }
                else
                {
                    
                    InfoWindow window = new InfoWindow("Add more products or check if customer was chosen");
                    window.ShowDialog();
                }
            }
        



        

       
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchBox.Text;
            GetProductsByName(searchText);
        }
        private void GetProductsByName(string searchText)
        {
            // Przefiltruj listę produktów po nazwie
            List<Product> filteredProducts = products.Where(p => p.Name.ToLower().Contains(searchText.ToLower())).ToList();

            // Ustaw nową źródło danych dla ListView
            ProductListView.ItemsSource = filteredProducts;

        }
    }
}
