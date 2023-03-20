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
using System.Windows.Shapes;

namespace ManageMagazine
{

    public partial class CustomersWindow : Window
    {
        public CustomersWindow()
        {
            InitializeComponent();
            UsersListView.ItemsSource = custemerList;
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

        List<Customer> custemerList = new List<Customer>()
        {
            new Customer() { Id = 1, FirstName = "Anna", LastName = "Kowalska", PhoneNumber = "111 111 111", City = "Warszawa", Street = "Marszałkowska", HouseNumber = "1", PostalCode = "00-001" },
            new Customer() { Id = 2, FirstName = "Jan", LastName = "Nowak", PhoneNumber = "222 222 222", City = "Kraków", Street = "Krakowska", HouseNumber = "2", PostalCode = "30-001" },
            new Customer() { Id = 3, FirstName = "Maria", LastName = "Nowacka", PhoneNumber = "333 333 333", City = "Gdańsk", Street = "Gdańska", HouseNumber = "3", PostalCode = "80-001" },
            new Customer() { Id = 4, FirstName = "Adam", LastName = "Kowalski", PhoneNumber = "444 444 444", City = "Poznań", Street = "Poznańska", HouseNumber = "4", PostalCode = "60-001" },
            new Customer() { Id = 5, FirstName = "Karolina", LastName = "Kowalczyk", PhoneNumber = "555 555 555", City = "Wrocław", Street = "Wrocławska", HouseNumber = "5", PostalCode = "50-001" },
            new Customer() { Id = 6, FirstName = "Tomasz", LastName = "Kowalski", PhoneNumber = "666 666 666", City = "Szczecin", Street = "Szczecińska", HouseNumber = "6", PostalCode = "70-001" },
            new Customer() { Id = 7, FirstName = "Magdalena", LastName = "Nowakowska", PhoneNumber = "777 777 777", City = "Gdynia", Street = "Gdyńska", HouseNumber = "7", PostalCode = "81-001" },
            new Customer() { Id = 8, FirstName = "Kamil", LastName = "Nowak", PhoneNumber = "888 888 888", City = "Katowice", Street = "Katowicka", HouseNumber = "8", PostalCode = "40-001" },
            new Customer() { Id = 9, FirstName = "Natalia", LastName = "Kowalska", PhoneNumber = "999 999 999", City = "Łódź", Street = "Łódzka", HouseNumber = "9", PostalCode = "90-001" },
            new Customer() { Id = 10, FirstName = "Piotr", LastName = "Kowalczyk", PhoneNumber = "101 101 101", City = "Gliwice", Street = "Gliwicka", HouseNumber = "10", PostalCode = "44-100" }
        };



        #endregion

    }
}
