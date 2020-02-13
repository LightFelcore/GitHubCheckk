using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SendMeADrink_Official.Database
{
    public class Inloggegevens
    {
        [PrimaryKey]
        public int id { get; set; }
        public string email { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int age { get; set; }
    }
}
