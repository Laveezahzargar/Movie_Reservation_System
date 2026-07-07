namespace P12_Movie_Reservation_System_Backend.Models.DomainModels
{
    public class City
    {
        public int CityId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        public string Country { get; set; } = "India";

        public ICollection<Theater> Theaters { get; set; } = new List<Theater>();
    }
}
