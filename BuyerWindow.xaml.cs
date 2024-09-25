using System.Windows;
using WpfApp1.Data;

namespace WpfApp1
{
    public partial class BuyerWindow : Window
    {
        private DatabaseHelper dbHelper;

        public BuyerWindow()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            LoadAvailableApartments();
        }

        private void LoadAvailableApartments()
        {
            var apartments = dbHelper.GetAvailableApartments();
            AvailableApartmentsDataGrid.ItemsSource = apartments; 
        }

        private void BuyApartment_Click(object sender, RoutedEventArgs e)
        {
            var selectedApartment = (Apartment)AvailableApartmentsDataGrid.SelectedItem;
            if (selectedApartment != null)
            {
                dbHelper.MarkApartmentAsSold(selectedApartment.ApartmentID);
                MessageBox.Show("Квартира куплена!");
                LoadAvailableApartments(); 
            }
            else
            {
                MessageBox.Show("Будь ласка, виберіть квартиру для купівлі.");
            }
        }

        private void AvailableApartmentsDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        }
    }
}
