using AeroDynasty.Models.RouteModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AeroDynasty.Views.Controls
{
    /// <summary>
    /// Interaction logic for FlightWeekScheduleControl.xaml
    /// </summary>
    public partial class FlightWeekScheduleControl : UserControl, INotifyPropertyChanged
    {
        // DependencyProperty for FlightSchedule to bind to ObservableCollection<FlightSchedule>
        public static readonly DependencyProperty FlightScheduleProperty =
            DependencyProperty.Register(nameof(FlightSchedule), typeof(ObservableCollection<FlightSchedule>), typeof(FlightWeekScheduleControl), new PropertyMetadata(null, OnFlightScheduleChanged));

        // Public property for accessing the FlightSchedule collection
        public ObservableCollection<FlightSchedule> FlightSchedule
        {
            get { return (ObservableCollection<FlightSchedule>)GetValue(FlightScheduleProperty); }
            set
            {
                SetValue(FlightScheduleProperty, value);
                Debug.WriteLine($"ScheduledFlights updated: {FlightSchedule?.ToString()}");
                OnPropertyChanged(nameof(FlightSchedule)); // Notify that the FlightSchedule property has changed
            }
        }

        // Method to filter the FlightSchedule collection based on the day of the week
        public ObservableCollection<FlightSchedule> GetFlightScheduleForDay(DayOfWeek dayOfWeek)
        {
            // Filter the FlightSchedule collection to return those whose Outbound flight's DepartureDay matches the specified day
            var filteredSchedules = FlightSchedule?.Where(schedule => schedule.Outbound?.DepartureDay == dayOfWeek).ToList();
            return filteredSchedules != null ? new ObservableCollection<FlightSchedule>(filteredSchedules) : new ObservableCollection<FlightSchedule>();
        }

        // Properties for each day of the week that return the filtered FlightSchedule
        public ObservableCollection<FlightSchedule> MondaySchedule => GetFlightScheduleForDay(DayOfWeek.Monday);
        public ObservableCollection<FlightSchedule> TuesdaySchedule => GetFlightScheduleForDay(DayOfWeek.Tuesday);
        public ObservableCollection<FlightSchedule> WednesdaySchedule => GetFlightScheduleForDay(DayOfWeek.Wednesday);
        public ObservableCollection<FlightSchedule> ThursdaySchedule => GetFlightScheduleForDay(DayOfWeek.Thursday);
        public ObservableCollection<FlightSchedule> FridaySchedule => GetFlightScheduleForDay(DayOfWeek.Friday);
        public ObservableCollection<FlightSchedule> SaturdaySchedule => GetFlightScheduleForDay(DayOfWeek.Saturday);
        public ObservableCollection<FlightSchedule> SundaySchedule => GetFlightScheduleForDay(DayOfWeek.Sunday);

        // Dependency property changed callback
        private static void OnFlightScheduleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Any additional logic needed when FlightSchedule is changed can go here
            if (d is FlightWeekScheduleControl control)
            {
                Debug.WriteLine($"FlightSchedule changed. New value: {e.NewValue}");
            }
        }

        // PropertyChanged event for INotifyPropertyChanged interface
        public event PropertyChangedEventHandler PropertyChanged;

        // Method to notify property changes for binding
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public FlightWeekScheduleControl()
        {
            InitializeComponent();

            // Debug output for checking the DataContext and FlightSchedule property when the control is loaded
            this.Loaded += (s, e) =>
            {
                Debug.WriteLine($"DataContext: {DataContext}");
                Debug.WriteLine($"FlightSchedule: {FlightSchedule?.Count ?? 0} flights loaded.");
            };
        }
    }
}
