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

        public void DeleteApartment(int apartmentId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM Apartments WHERE ApartmentID = @ApartmentID", connection);
                command.Parameters.AddWithValue("@ApartmentID", apartmentId);
                command.ExecuteNonQuery();
            }
        }

        public void UpdateApartment(Apartment apartment)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("UPDATE Apartments SET Area = @Area, Rooms = @Rooms, Price = @Price, Description = @Description WHERE ApartmentID = @ApartmentID", connection);
                command.Parameters.AddWithValue("@Area", apartment.Area);
                command.Parameters.AddWithValue("@Rooms", apartment.Rooms);
                command.Parameters.AddWithValue("@Price", apartment.Price);
                command.Parameters.AddWithValue("@Description", apartment.Description);
                command.Parameters.AddWithValue("@ApartmentID", apartment.ApartmentID);
                command.ExecuteNonQuery();
            }
        }

        public List<Apartment> GetAllApartments(string sortBy = "ApartmentID", bool ascending = true)
        {
            var apartments = new List<Apartment>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sortOrder = ascending ? "ASC" : "DESC";

                var query = $"SELECT * FROM Apartments ORDER BY {sortBy} {sortOrder}";

                var command = new SqlCommand(query, connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var apartment = new Apartment
                        {
                            ApartmentID = reader.GetInt32(0),
                            AddressID = reader.GetInt32(1),
                            Area = reader.GetDecimal(2),
                            Rooms = reader.GetInt32(3),
                            Price = reader.GetDecimal(4),
                            Description = reader.GetString(5)
                        };
                        apartments.Add(apartment);
                    }
                }
            }
            return apartments;
        }

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

        public List<Apartment> GetAvailableApartments()
        {
            List<Apartment> apartments = new List<Apartment>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                SELECT a.ApartmentID, a.Area, a.Rooms, a.Price, a.Description,
                       ad.Street, ad.City, ad.PostalCode 
                FROM Apartments a 
                JOIN ApartmentStatus s ON a.ApartmentID = s.ApartmentID 
                JOIN Addresses ad ON a.AddressID = ad.AddressID
                WHERE s.Status = 'Available'";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Apartment apartment = new Apartment
                        {
                            ApartmentID = (int)reader["ApartmentID"],
                            Area = (decimal)reader["Area"],
                            Rooms = (int)reader["Rooms"],
                            Price = (decimal)reader["Price"],
                            Description = reader["Description"].ToString(),
                            Address = new Address
                            {
                                Street = reader["Street"].ToString(),
                                City = reader["City"].ToString(),
                                PostalCode = reader["PostalCode"].ToString()
                            }
                        };
                        apartments.Add(apartment);
                    }
                }
            }

            return apartments;
        }

        public void MarkApartmentAsSold(int apartmentId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Обновляем статус квартиры в базе данных
                string query = "UPDATE ApartmentStatus SET Status = 'Sold' WHERE ApartmentID = @ApartmentID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApartmentID", apartmentId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public Address GetAddressById(int addressId)
        {
            Address address = null;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Addresses WHERE AddressID = @AddressID", connection);
                command.Parameters.AddWithValue("@AddressID", addressId);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        address = new Address
                        {
                            AddressID = reader.GetInt32(0),
                            Street = reader.GetString(1),
                            City = reader.GetString(2),
                            PostalCode = reader.GetString(3)
                        };
                    }
                }
            }
            return address;
        }
    }
}
