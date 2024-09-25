using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace WpfApp1.Data
{
    public class DatabaseHelper
    {
        private string connectionString;

        public DatabaseHelper()
        {
            connectionString = ConfigurationManager.ConnectionStrings["MaclerDB"].ConnectionString;
        }

        // Метод для добавления адреса
        public void AddAddress(Address address)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Addresses (Street, City, PostalCode) VALUES (@Street, @City, @PostalCode);";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Street", address.Street);
                command.Parameters.AddWithValue("@City", address.City);
                command.Parameters.AddWithValue("@PostalCode", address.PostalCode);
                command.ExecuteNonQuery();
            }
        }

        // Метод для получения AddressID
        public int GetAddressId(string street, string city, string postalCode)
        {
            int addressId = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT AddressID FROM Addresses WHERE Street = @Street AND City = @City AND PostalCode = @PostalCode";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Street", street);
                command.Parameters.AddWithValue("@City", city);
                command.Parameters.AddWithValue("@PostalCode", postalCode);

                var result = command.ExecuteScalar();
                if (result != null)
                {
                    addressId = (int)result;
                }
            }
            return addressId;
        }

        // Метод для добавления квартиры
        public void AddApartment(Apartment apartment)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Apartments (AddressID, Area, Rooms, Price, Description) VALUES (@AddressID, @Area, @Rooms, @Price, @Description)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@AddressID", apartment.AddressID);
                command.Parameters.AddWithValue("@Area", apartment.Area);
                command.Parameters.AddWithValue("@Rooms", apartment.Rooms);
                command.Parameters.AddWithValue("@Price", apartment.Price);
                command.Parameters.AddWithValue("@Description", apartment.Description);
                command.ExecuteNonQuery();
            }
        }

        // Метод для получения списка квартир
        public List<Apartment> GetApartments()
        {
            List<Apartment> apartments = new List<Apartment>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Apartments";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Apartment apartment = new Apartment
                    {
                        ApartmentID = (int)reader["ApartmentID"],
                        Area = (decimal)reader["Area"],
                        Rooms = (int)reader["Rooms"],
                        Price = (decimal)reader["Price"],
                        Description = (string)reader["Description"],
                        AddressID = (int)reader["AddressID"]
                    };
                    apartments.Add(apartment);
                }
            }
            return apartments;
        }
    }
}
