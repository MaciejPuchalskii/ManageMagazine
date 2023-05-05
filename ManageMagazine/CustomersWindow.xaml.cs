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

    public partial class CustomersWindow : Window
    {
       
        List<Customer> customersList = new List<Customer>() { };
        Database databaseObject;
        bool isBeingEdited = false;

        public CustomersWindow()
        {
            InitializeComponent();
            databaseObject = new Database();
            GetDataFromDB();
            CustomersListView.ItemsSource = customersList;
        }
        private void GetDataFromDB()
        {
            databaseObject.OpenConnection();
            string query = "Select * from customers";
            SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

            SQLiteDataReader result = myCommand.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    customersList.Add(new Customer(Convert.ToInt32(result["id"]),
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


        private void AddCustomerButtonCLick(object sender, RoutedEventArgs e)
        {
            if(!(string.IsNullOrEmpty(CustomerNameTxt.Text) && 
                string.IsNullOrEmpty(LastNameTxt.Text) && 
                string.IsNullOrEmpty(PhoneNumberTxt.Text) && 
                string.IsNullOrEmpty(CityTxt.Text) && 
                string.IsNullOrEmpty(StreetTxt.Text) &&
                string.IsNullOrEmpty(HouseNumberTxt.Text) && 
                string.IsNullOrEmpty(PostalCodeTxt.Text)))
            {
                try
                {
                    string query = "INSERT INTO customers ('FirstName','LastName','PhoneNumber','City','Street','HouseNumber','PostalCode') VALUES (@FirstName,@LastName,@PhoneNumber,@City,@Street,@HouseNumber,@PostalCode)";

                    SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

                    int lastCustomerId = customersList.Count;

                    databaseObject.OpenConnection();

                    myCommand.Parameters.AddWithValue("@FirstName", CustomerNameTxt.Text);
                    myCommand.Parameters.AddWithValue("@LastName",LastNameTxt.Text);
                    myCommand.Parameters.AddWithValue("@PhoneNumber",PhoneNumberTxt.Text);
                    myCommand.Parameters.AddWithValue("@City", CityTxt.Text);
                    myCommand.Parameters.AddWithValue("@Street", StreetTxt.Text);
                    myCommand.Parameters.AddWithValue("@HouseNumber", Convert.ToInt32(HouseNumberTxt.Text));
                    myCommand.Parameters.AddWithValue("@PostalCode", PostalCodeTxt.Text);
                    myCommand.ExecuteNonQuery();

                    databaseObject.CloseConnection();
                    customersList.Add(
                        new Customer(
                            lastCustomerId + 1,
                            CustomerNameTxt.Text,
                            LastNameTxt.Text,
                            PhoneNumberTxt.Text,
                            CityTxt.Text,
                            StreetTxt.Text,
                            Convert.ToInt32(HouseNumberTxt.Text),
                            PostalCodeTxt.Text
                        ));
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
        private void EditCustomerButtonCLick(object sender, RoutedEventArgs e)
        {
            if (CustomersListView.SelectedIndex != -1) //sprawdzenie, czy wybrano element do edycji
            {
                int index = CustomersListView.SelectedIndex;
                int customerId = customersList[index].Id;
                if (!isBeingEdited)
                {
                    try
                    {
                        CustomerNameTxt.Text = customersList[index].FirstName;
                        LastNameTxt.Text = customersList[index].LastName;
                        PhoneNumberTxt.Text = customersList[index].PhoneNumber;
                        CityTxt.Text = customersList[index].City;
                        StreetTxt.Text = customersList[index].Street;
                        HouseNumberTxt.Text = customersList[index].HouseNumber.ToString();
                        PostalCodeTxt.Text = customersList[index].PostalCode;

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
                    //try
                    //{
                        customersList[index].FirstName = CustomerNameTxt.Text;
                        customersList[index].LastName = LastNameTxt.Text;
                        customersList[index].PhoneNumber = PhoneNumberTxt.Text;
                        customersList[index].City = CityTxt.Text;
                        customersList[index].Street = StreetTxt.Text;
                        customersList[index].HouseNumber = Convert.ToInt32(HouseNumberTxt.Text);
                        customersList[index].PostalCode = PostalCodeTxt.Text;

                        EditSaveButton.Content = "EDIT";
                        isBeingEdited = false;
                       

                        string query = "UPDATE customers SET FirstName=@FirstName, LastName=@LastName, PhoneNumber=@PhoneNumber, City=@City, Street=@Street, HouseNumber = @HouseNumber, PostalCode = @PostalCode WHERE id=@id";

                        SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

                        databaseObject.OpenConnection();

                       
                        myCommand.Parameters.AddWithValue("@FirstName", CustomerNameTxt.Text);
                        myCommand.Parameters.AddWithValue("@LastName", LastNameTxt.Text);
                        myCommand.Parameters.AddWithValue("@PhoneNumber", PhoneNumberTxt.Text);
                        myCommand.Parameters.AddWithValue("@City", CityTxt.Text);
                        myCommand.Parameters.AddWithValue("@Street", StreetTxt.Text);
                        myCommand.Parameters.AddWithValue("@HouseNumber", Convert.ToInt32(HouseNumberTxt.Text));
                        myCommand.Parameters.AddWithValue("@PostalCode", PostalCodeTxt.Text);
                        myCommand.Parameters.AddWithValue("@id", customerId); //zmiana indeksu na ID produktu


                        myCommand.ExecuteNonQuery();

                        databaseObject.CloseConnection();
                        Refresh();
                        ClearTextInput();

                    //}
                    //catch
                    //{
                    //    InfoWindow window = new InfoWindow("Wrong Input");
                    //    window.ShowDialog();
                    //}
                }
            }
            else //nie wybrano produktu do edycji
            {
                InfoWindow window = new InfoWindow("No element was chosen");
                window.ShowDialog();
            }
        }
        private void DeleteCustomerButtonCLick(object sender, RoutedEventArgs e)
        {
            if (CustomersListView.SelectedIndex != -1) //sprawdzenie, czy wybrano element do usunięcia
            {
                int index = CustomersListView.SelectedIndex;
                int customerId = customersList[index].Id;

                try
                {
                    string query = "DELETE FROM customers WHERE id=@id";

                    SQLiteCommand myCommand = new SQLiteCommand(query, databaseObject.myConnection);

                    databaseObject.OpenConnection();

                    myCommand.Parameters.AddWithValue("@id", customerId);

                    myCommand.ExecuteNonQuery();

                    databaseObject.CloseConnection();
                    customersList.RemoveAt(index);

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

        private void ClearTextInput()
        {
            CustomerNameTxt.Text = null;
            CustomerNameText.Visibility = Visibility.Visible;
            LastNameTxt.Text = null;
            LastNameText.Visibility = Visibility.Visible;
            PhoneNumberTxt.Text = null;
            PhoneNumberText.Visibility = Visibility.Visible;
            CityTxt.Text = null;
            CityText.Visibility = Visibility.Visible;
            StreetTxt.Text = null;
            StreetText.Visibility = Visibility.Visible;
            HouseNumberTxt.Text = null;
            HouseNumberText.Visibility = Visibility.Visible;
            PostalCodeTxt.Text = null;
            PostalCodeText.Visibility = Visibility.Visible;
        }
        private void Refresh()
        {
            CustomersListView.ItemsSource = null;
            customersList.Clear();

            GetDataFromDB();
            CustomersListView.ItemsSource = customersList;
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

        #region MouseDown Focusing Text etc

        private void CustomerNameText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CustomerNameTxt.Focus();
        }

        private void CustomerNameTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(CustomerNameText.Text) && CustomerNameText.Text.Length > 0)
            {
                CustomerNameText.Visibility = Visibility.Collapsed;
            }
            else
            {
                CustomerNameText.Visibility = Visibility.Visible;
            }
        }


        private void LastNameText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            LastNameTxt.Focus();
        }
        private void LastNameTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(LastNameText.Text) && LastNameText.Text.Length > 0)
            {
                LastNameText.Visibility = Visibility.Collapsed;
            }
            else
            {
                LastNameText.Visibility = Visibility.Visible;
            }
        }

        private void PhoneNumberText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PhoneNumberTxt.Focus();
        }

        private void PhoneNumberTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(PhoneNumberText.Text) && PhoneNumberText.Text.Length > 0)
            {
                PhoneNumberText.Visibility = Visibility.Collapsed;
            }
            else
            {
                PhoneNumberText.Visibility = Visibility.Visible;
            }
        }

        private void CityText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CityTxt.Focus();
        }

        private void CityTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(CityText.Text) && CityText.Text.Length > 0)
            {
                CityText.Visibility = Visibility.Collapsed;
            }
            else
            {
                CityText.Visibility = Visibility.Visible;
            }
        }

        private void StreetText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            StreetTxt.Focus();
        }

        private void StreetTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(StreetText.Text) && StreetText.Text.Length > 0)
            {
                StreetText.Visibility = Visibility.Collapsed;
            }
            else
            {
                StreetText.Visibility = Visibility.Visible;
            }
        }

        private void HouseNumberText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HouseNumberTxt.Focus();
        }

        private void HouseNumberTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(HouseNumberText.Text) && HouseNumberText.Text.Length > 0)
            {
                HouseNumberText.Visibility = Visibility.Collapsed;
            }
            else
            {
                HouseNumberText.Visibility = Visibility.Visible;
            }
        }

        private void PostalCodeText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PostalCodeTxt.Focus();
        }

        private void PostalCodeTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(PostalCodeText.Text) && PostalCodeText.Text.Length > 0)
            {
                PostalCodeText.Visibility = Visibility.Collapsed;
            }
            else
            {
                PostalCodeText.Visibility = Visibility.Visible;
            }
        }

        #endregion
    }
}
