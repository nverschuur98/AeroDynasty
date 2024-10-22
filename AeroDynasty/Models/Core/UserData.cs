using AeroDynasty.Models.AirlineModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeroDynasty.Models.Core
{
    public class UserData
    {
        public Airline Airline { get; private set; }

        /// <summary>
        /// Constructs a new user data class
        /// </summary>
        /// <param name="airline">The user airline</param>
        public UserData(Airline airline)
        {
            Airline = airline;
        }
    }
}
