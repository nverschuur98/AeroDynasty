using AeroDynasty.Models.AirlineModels;
using AeroDynasty.Models.AirlinerModels;
using AeroDynasty.Models.Core;
using AeroDynasty.ModelViews;
using AeroDynasty.Services;
using System.Windows.Input;

namespace AeroDynasty.Models.AircraftModels
{
    public class AircraftBuy
    {
        public AircraftModel AircraftModel { get; private set; }
        public int Amount { get; set; }
        public Airline Buyer { get; set; }

        public ICommand BuyUserAircraftCommand { get; }

        public Price totalPrice
        {
            get
            {
                return new Price(AircraftModel.Price.Amount * Amount);
            }
        }

        public AircraftBuy(AircraftModel model, int amt)
        {
             AircraftModel = model;
            Amount = amt;

            BuyUserAircraftCommand = new RelayCommand(BuyUserAircraft);
        }

        private void BuyUserAircraft()
        {
            Airline buyer = GameData.Instance.UserData.Airline;

            if(Amount <= 0)
            {
                return;
            }

            for (int i = 0; i < Amount; i++)
            {
                Airliner air = new Airliner();
                air.Model = AircraftModel;
                air.Registration = "PH-XXX";
                air.Owner = buyer;

                //Check if sufficient money available
                if (buyer.SufficientCash(AircraftModel.Price))
                {
                    buyer.SubtractCash(AircraftModel.Price);

                    GameData.Instance.Airliners.Add(air);
                }
                else
                {
                    //Not enough money available
                    return;
                }

            }
        }
    }
}
