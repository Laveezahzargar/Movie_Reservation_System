namespace P12_Movie_Reservation_System_Backend.Models.DomainModels
{
    public class ShowSeat
    {
        public int ShowSeatId { get; set; }

        public int ShowId { get; set; }
        public Show Show { get; set; }

        public int SeatId { get; set; }
        public Seat Seat { get; set; }

        public string Status { get; set; }
        // Available, Reserved, Booked

        public ICollection<BookingSeat> BookingSeats { get; set; }
    }
}
