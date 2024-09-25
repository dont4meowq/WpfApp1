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
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            apartment.Area = decimal.Parse(AreaTextBox.Text);
            apartment.Rooms = int.Parse(RoomsTextBox.Text);
            apartment.Price = decimal.Parse(PriceTextBox.Text);
            apartment.Description = DescriptionTextBox.Text;

            DatabaseHelper dbHelper = new DatabaseHelper();
            dbHelper.UpdateApartment(apartment);

            MessageBox.Show("Квартира успішно змінена!");
            DialogResult = true; 
            Close();
        }
    }
}
