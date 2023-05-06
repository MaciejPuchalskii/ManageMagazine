using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ManageMagazine
{

    public partial class ShipmentWindow : Window
    {
        List<Order> orderList;
        Database databaseObject;
        public ShipmentWindow() 
        {
            InitializeComponent();
            databaseObject = new Database();

            orderList = GetOrdersDB();
            OrdersListView.ItemsSource = orderList;
        }

        private List<Order> GetOrdersDB()
        {
            List<Order> orders = new List<Order>();
            databaseObject.OpenConnection();
            string query = "Select * from Orders";
            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

            SQLiteDataReader result = myCommand.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    orders.Add(new Order(Convert.ToInt32(result["ID"]),
                                                                Convert.ToInt32(result["CustomerID"]),
                                                                Convert.ToDouble(result["TotalPrice"])));

                }
            }


           foreach(Order order in orders)
            {
                string selectQuery = "Select oi.OrderId, oi.RecordID,oi.ProductID, oi.Quantity, oi.Price, p.ID, p.Name, p.Manufacturer, p.PurchasePrice, p.SalePrice, p.Quantity as QuantityP from OrderItems oi JOIN Products p ON p.ID=oi.ProductID where oi.OrderID=@id;";
                SQLiteCommand my2Command = new SQLiteCommand(selectQuery, databaseObject.myConnection);

                my2Command.Parameters.AddWithValue("@id",order.OrderId);


               SQLiteDataReader result2 = my2Command.ExecuteReader();

                if (result2.HasRows)
                {
                
                    while (result2.Read())
                    {
                       // order.OrderItems.Add(new OrderItem(Convert.ToInt32(result2["oi.OrderID"]), Convert.ToInt32(result2["oi.Quantity"]), new Product(Convert.ToInt32(result2["p.ID"]), Convert.ToString(result2["p.Name"]), Convert.ToString(result2["p.Manufacturer"]), Convert.ToDouble(result2["p.PurchasePrice"]), Convert.ToDouble(result2["p.SalePrice"]), Convert.ToInt32(result2["p.Quantity"]))));
                        order.OrderItems.Add(new OrderItem(Convert.ToInt32(result2["OrderID"]), Convert.ToInt32(result2["Quantity"]), new Product(Convert.ToInt32(result2["ID"]), Convert.ToString(result2["Name"]), Convert.ToString(result2["Manufacturer"]), Convert.ToDouble(result2["PurchasePrice"]), Convert.ToDouble(result2["SalePrice"]), Convert.ToInt32(result2["QuantityP"]))));
                    }
                }

            }


            databaseObject.CloseConnection();
            return orders;
        }


        private void OrdersListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedOrder = (Order)OrdersListView.SelectedItem;

            databaseObject.OpenConnection();
            string selectedQuery = "SELECT * FROM Customers Where ID = @id";
            SQLiteCommand myCommand = new SQLiteCommand(selectedQuery, databaseObject.myConnection);

            myCommand.Parameters.AddWithValue("@id", selectedOrder.CustomerId);

            SQLiteDataReader result = myCommand.ExecuteReader();

            if(result.Read())
            {
                NameText.Text = Convert.ToString(result["FirstName"]) + " " + Convert.ToString(result["LastName"]);
                CityAndPostText.Text = Convert.ToString(result["City"]) + " " + Convert.ToString(result["PostalCode"]);
                StreeTHouseNumberText.Text = Convert.ToString(result["Street"]) +" "+ Convert.ToString(result["HouseNumber"]);
                PhoneNumberText.Text = Convert.ToString(result["PhoneNumber"]);
                TotalSumText.Text ="Sum: " + selectedOrder.Sum.ToString();
                databaseObject.CloseConnection();

            }

            OrderItemsListView.ItemsSource = null;
            OrderItemsListView.ItemsSource = selectedOrder.OrderItems;


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
