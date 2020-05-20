namespace SendMeADrink_Official.Database
{
    /*Public class that stores all the information for a place*/
    public class Place
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }
        public string Postalcode { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Email { get; set; }
        public double Distance { get; set; }
    }
}
