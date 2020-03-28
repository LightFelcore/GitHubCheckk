using System;
using System.Collections.Generic;
using System.Text;

namespace SendMeADrink_Official.Database
{
    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email  { get; set; }
        public string Passwd { get; set; }
        public string Age { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
