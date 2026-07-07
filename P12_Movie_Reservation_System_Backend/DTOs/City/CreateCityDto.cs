namespace P12_Movie_Reservation_System_Backend.DTOs.City
{
    public class CreateCityDto
    {
        public string Name { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        public string Country { get; set; } = "India";
    }
}
