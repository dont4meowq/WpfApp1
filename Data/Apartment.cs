public class Apartment
{
    public int ApartmentID { get; set; }
    public int AddressID { get; set; }
    public decimal Area { get; set; }
    public int Rooms { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }

    // Добавьте свойство Address для связи с адресом
    public Address Address { get; set; }
}
