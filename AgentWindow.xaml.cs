using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Data;

namespace WpfApp1
{
    public partial class AgentWindow : Window
    {
        private DatabaseHelper dbHelper;
        private List<Apartment> apartments;

        public AgentWindow()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            LoadApartments();
        }
        private void ChangeStatusButton_Click(object sender, RoutedEventArgs e)
        {
            if (ApartmentsDataGrid.SelectedItem is Apartment selectedApartment)
            {
                string selectedStatus = ((ComboBoxItem)StatusComboBox.SelectedItem).Content.ToString();
                DatabaseHelper dbHelper = new DatabaseHelper();
                dbHelper.UpdateApartmentStatus(selectedApartment.ApartmentID, selectedStatus);
                MessageBox.Show("Статус квартири змінився на: " + selectedStatus);
                LoadApartments(); 
            }
            else
            {
                MessageBox.Show("Будь ласка, виберіть квартиру.");
            }
        }

        private void LoadApartments()
        {
            apartments = dbHelper.GetAllApartments();
            foreach (var apartment in apartments)
            {
                apartment.Address = dbHelper.GetAddressById(apartment.AddressID);
            }
            ApartmentsDataGrid.ItemsSource = apartments;
        }

        private void AddApartmentButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Address newAddress = new Address
                {
                    Street = StreetTextBox.Text,
                    City = CityTextBox.Text,
                    PostalCode = PostalCodeTextBox.Text
                };

                dbHelper.AddAddress(newAddress);
                int addressId = dbHelper.GetAddressId(newAddress.Street, newAddress.City, newAddress.PostalCode);

                Apartment newApartment = new Apartment
                {
                    AddressID = addressId,
                    Area = decimal.Parse(AreaTextBox.Text),
                    Rooms = int.Parse(RoomsTextBox.Text),
                    Price = decimal.Parse(PriceTextBox.Text),
                    Description = DescriptionTextBox.Text
                };

                dbHelper.AddApartment(newApartment);
                MessageBox.Show("Квартира успішно додана!");
                LoadApartments();
            }
            catch (FormatException)
            {
                MessageBox.Show("Помилка при вводі даних. Перевірте, що площа, кількість кімнат і ціна введені коректно.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Відбулася помилка: " + ex.Message);
            }
        }

        private void EditApartmentButton_Click(object sender, RoutedEventArgs e)
        {
            if (ApartmentsDataGrid.SelectedItem is Apartment selectedApartment)
            {
                var editWindow = new EditApartmentWindow(selectedApartment);
                if (editWindow.ShowDialog() == true)
                {
                    LoadApartments();
                }
            }
            else
            {
                MessageBox.Show("Будь ласка, виберіть квартиру для редагування.");
            }
        }

        private void DeleteApartmentButton_Click(object sender, RoutedEventArgs e)
        {
            if (ApartmentsDataGrid.SelectedItem is Apartment selectedApartment)
            {
                dbHelper.DeleteApartment(selectedApartment.ApartmentID);
                MessageBox.Show("Квартира успішно видалена!");
                LoadApartments();
            }
            else
            {
                MessageBox.Show("Будь ласка, виберіть квартиру для видалення.");
            }
        }

        private void ApartmentsDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ApartmentsDataGrid.SelectedItem is Apartment selectedApartment)
            {
                StreetTextBox.Text = selectedApartment.Address.Street;
                CityTextBox.Text = selectedApartment.Address.City;
                PostalCodeTextBox.Text = selectedApartment.Address.PostalCode;
                AreaTextBox.Text = selectedApartment.Area.ToString();
                RoomsTextBox.Text = selectedApartment.Rooms.ToString();
                PriceTextBox.Text = selectedApartment.Price.ToString();
                DescriptionTextBox.Text = selectedApartment.Description;
            }
        }

        private void SortByComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        }

        private void SortAscending_Click(object sender, RoutedEventArgs e)
        {
            var selectedSortOption = (SortByComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (selectedSortOption != null)
            {
                switch (selectedSortOption)
                {
                    case "ApartmentID":
                        apartments = apartments.OrderBy(a => a.ApartmentID).ToList();
                        break;
                    case "Area":
                        apartments = apartments.OrderBy(a => a.Area).ToList();
                        break;
                    case "Rooms":
                        apartments = apartments.OrderBy(a => a.Rooms).ToList();
                        break;
                    case "Price":
                        apartments = apartments.OrderBy(a => a.Price).ToList();
                        break;
                }
                ApartmentsDataGrid.ItemsSource = apartments;
            }
        }

        private void SortDescending_Click(object sender, RoutedEventArgs e)
        {
            var selectedSortOption = (SortByComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (selectedSortOption != null)
            {
                switch (selectedSortOption)
                {
                    case "ApartmentID":
                        apartments = apartments.OrderByDescending(a => a.ApartmentID).ToList();
                        break;
                    case "Area":
                        apartments = apartments.OrderByDescending(a => a.Area).ToList();
                        break;
                    case "Rooms":
                        apartments = apartments.OrderByDescending(a => a.Rooms).ToList();
                        break;
                    case "Price":
                        apartments = apartments.OrderByDescending(a => a.Price).ToList();
                        break;
                }
                ApartmentsDataGrid.ItemsSource = apartments;
            }
        }
    }
}
