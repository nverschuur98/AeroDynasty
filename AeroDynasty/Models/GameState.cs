using AeroDynasty.Models.AirlineModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeroDynasty.Models
{
    public class GameState
    {
        public List<Airline> Airlines { get; set; }
        public DateTime CurrentDate { get; set; }
    }
}
