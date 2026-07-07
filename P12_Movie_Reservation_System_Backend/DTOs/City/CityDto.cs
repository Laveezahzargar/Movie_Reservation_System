namespace P12_Movie_Reservation_System_Backend.DTOs.City
{
    public class CityDto
    {
        public int CityId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;
    }
}
