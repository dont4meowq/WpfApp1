using System.Windows;
using WpfApp1.Data;

namespace WpfApp1
{
    public partial class AgentWindow : Window
    {
        private DatabaseHelper dbHelper;

        public AgentWindow()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
        }

        // Обработчик события нажатия кнопки для добавления квартиры
        private void AddApartmentButton_Click(object sender, RoutedEventArgs e)
        {
            // Пример добавления адреса
            Address newAddress = new Address
            {
                Street = "Example St.",
                City = "City",
                PostalCode = "12345"
            };
            dbHelper.AddAddress(newAddress);

            // Получаем AddressID
            int addressId = dbHelper.GetAddressId(newAddress.Street, newAddress.City, newAddress.PostalCode);

            // Пример добавления квартиры
            Apartment newApartment = new Apartment
            {
                AddressID = addressId,
                Area = 100,
                Rooms = 3,
                Price = 500000,
                Description = "Nice apartment"
            };
            dbHelper.AddApartment(newApartment);

            MessageBox.Show("Квартира успешно добавлена!");
        }
    }
}
