using AeroDynasty.Models.AirlineModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeroDynasty.ModelViews
{
    public class AirlineViewModel
    {
        public Airline currentAirline {  get; set; }

        public AirlineViewModel(Airline airline)
        {
            currentAirline = airline;
        }

        //Add actions for adding airliners, routes etc.
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
