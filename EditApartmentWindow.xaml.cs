using System.Windows;
using WpfApp1.Data;

namespace WpfApp1
{
    public partial class EditApartmentWindow : Window
    {
        private Apartment apartment;

        public EditApartmentWindow(Apartment apartment)
        {
            InitializeComponent();
            this.apartment = apartment;
            LoadApartmentData();
        }

        private void LoadApartmentData()
        {
            AreaTextBox.Text = apartment.Area.ToString();
            RoomsTextBox.Text = apartment.Rooms.ToString();
            PriceTextBox.Text = apartment.Price.ToString();
            DescriptionTextBox.Text = apartment.Description;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Обновляем данные квартиры
            apartment.Area = decimal.Parse(AreaTextBox.Text);
            apartment.Rooms = int.Parse(RoomsTextBox.Text);
            apartment.Price = decimal.Parse(PriceTextBox.Text);
            apartment.Description = DescriptionTextBox.Text;

            // Сохраняем изменения в базе данных
            DatabaseHelper dbHelper = new DatabaseHelper();
            dbHelper.UpdateApartment(apartment);

            MessageBox.Show("Квартира успешно обновлена!");
            DialogResult = true; // Закрываем окно с результатом
            Close();
        }
    }
}
