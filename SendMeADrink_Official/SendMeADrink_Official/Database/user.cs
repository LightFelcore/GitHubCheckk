namespace SendMeADrink_Official.Database
{
    /*Public class that stores all the information for a user*/
    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email  { get; set; }
        public string Age { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
