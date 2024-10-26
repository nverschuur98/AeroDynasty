using AeroDynasty.Models.AirlineModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeroDynasty.Models.Core
{
    public class UserData : BaseModel
    {
        private Airline _airline { get; set; }
        public Airline Airline { get
            {
                return _airline;
            }
            set
            {
                _airline = value;
                OnPropertyChanged(nameof(Airline));
            }
        }

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
