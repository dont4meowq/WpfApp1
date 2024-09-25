using System;
using System.Collections.Generic;
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
            LoadApartments();
        }

        private void LoadApartments()
        {
            var apartments = dbHelper.GetAllApartments();

            // Здесь вы можете добавить код для загрузки адресов для каждой квартиры,
            // если они не загружаются автоматически
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
                // Считываем данные из текстовых полей
                Address newAddress = new Address
                {
                    Street = StreetTextBox.Text,
                    City = CityTextBox.Text,
                    PostalCode = PostalCodeTextBox.Text
                };

                // Добавляем адрес в базу данных
                dbHelper.AddAddress(newAddress);

                // Получаем AddressID для добавления квартиры
                int addressId = dbHelper.GetAddressId(newAddress.Street, newAddress.City, newAddress.PostalCode);

                // Создаем новую квартиру с введенными данными
                Apartment newApartment = new Apartment
                {
                    AddressID = addressId,
                    Area = decimal.Parse(AreaTextBox.Text),
                    Rooms = int.Parse(RoomsTextBox.Text),
                    Price = decimal.Parse(PriceTextBox.Text),
                    Description = DescriptionTextBox.Text
                };

                // Добавляем квартиру в базу данных
                dbHelper.AddApartment(newApartment);

                MessageBox.Show("Квартира успешно добавлена!");
                LoadApartments(); // Обновляем список квартир
            }
            catch (FormatException)
            {
                MessageBox.Show("Ошибка ввода данных. Проверьте, что площадь, количество комнат и цена введены корректно.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
        }

        private void EditApartmentButton_Click(object sender, RoutedEventArgs e)
        {
            if (ApartmentsDataGrid.SelectedItem is Apartment selectedApartment)
            {
                // Открываем окно редактирования с выбранной квартирой
                var editWindow = new EditApartmentWindow(selectedApartment);
                if (editWindow.ShowDialog() == true)
                {
                    LoadApartments(); // Обновляем список квартир
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите квартиру для редактирования.");
            }
        }

        private void DeleteApartmentButton_Click(object sender, RoutedEventArgs e)
        {
            if (ApartmentsDataGrid.SelectedItem is Apartment selectedApartment)
            {
                dbHelper.DeleteApartment(selectedApartment.ApartmentID);
                MessageBox.Show("Квартира успешно удалена!");
                LoadApartments(); // Обновляем список квартир
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите квартиру для удаления.");
            }
        }

        private void ApartmentsDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ApartmentsDataGrid.SelectedItem is Apartment selectedApartment)
            {
                // Заполняем текстовые поля данными выбранной квартиры
                StreetTextBox.Text = selectedApartment.Address.Street;
                CityTextBox.Text = selectedApartment.Address.City;
                PostalCodeTextBox.Text = selectedApartment.Address.PostalCode;
                AreaTextBox.Text = selectedApartment.Area.ToString();
                RoomsTextBox.Text = selectedApartment.Rooms.ToString();
                PriceTextBox.Text = selectedApartment.Price.ToString();
                DescriptionTextBox.Text = selectedApartment.Description;
            }
        }
    }
}
