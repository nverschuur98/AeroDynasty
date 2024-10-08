using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Linq;
using System.Collections.ObjectModel;
using AeroDynasty.Models;
using AeroDynasty.ModelViews;

namespace AeroDynasty.Services
{
    public class GameStateManager
    {
        // The GameState model to hold data for saving/loading
        public class GameState
        {
            public DateTime CurrentDate { get; set; }
            public List<Airline> Airlines { get; set; }
            public List<Airport> Airports { get; set; }
            public List<Route> Routes { get; set; }
        }

        // Save the current game state to a JSON file
        public static async Task SaveGame(GameViewModel gameViewModel, string filePath)
        {
            await Task.CompletedTask;

            throw new NotImplementedException();
        }

        // Load the game state from a JSON file and apply it to the GameViewModel
        public static void LoadGame(GameViewModel gameViewModel, string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
