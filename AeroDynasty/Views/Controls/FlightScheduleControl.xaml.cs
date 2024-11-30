using AeroDynasty.Models.RouteModels;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace AeroDynasty.Views.Controls
{
    public partial class FlightScheduleControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty FlightScheduleProperty = DependencyProperty.Register(nameof(FlightSchedule), typeof(FlightSchedule), typeof(FlightScheduleControl), new PropertyMetadata(null, OnFlightScheduleChanged));

        public FlightSchedule FlightSchedule
        {
            get { return (FlightSchedule)GetValue(FlightScheduleProperty); }
            set { SetValue(FlightScheduleProperty, value); }
        }

        private double _outboundGridWidth;
        public double OutboundGridWidth
        {
            get => _outboundGridWidth;
            private set
            {
                _outboundGridWidth = value;
                OnPropertyChanged(nameof(OutboundGridWidth));
            }
        }

        private double _inboundGridWidth;
        public double InboundGridWidth
        {
            get => _inboundGridWidth;
            private set
            {
                _inboundGridWidth = value;
                OnPropertyChanged(nameof(InboundGridWidth));
            }
        }

        private double _turnAroundWidth;
        public double TurnAroundWidth
        {
            get => _turnAroundWidth;
            private set
            {
                _turnAroundWidth = value;
                OnPropertyChanged(nameof(TurnAroundWidth));
            }
        }

        private Thickness _outboundMargin;
        public Thickness OutboundMargin
        {
            get => _outboundMargin;
            private set
            {
                _outboundMargin = value;
                OnPropertyChanged(nameof(OutboundMargin));
            }
        }

        public FlightScheduleControl()
        {
            InitializeComponent();
            //DataContext = this;
            this.Loaded += (s, e) =>
            {
                Debug.WriteLine($"DataContext: {DataContext}");
                Debug.WriteLine($"FlightSchedule: {FlightSchedule}");
            };
        }

        private static void OnFlightScheduleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FlightScheduleControl control)
            {
                control.UpdateProperties();
            }
        }

        private void UpdateProperties()
        {
            OutboundGridWidth = FlightSchedule?.Outbound?.FlightDuration.TotalMinutes / 2 ?? 0;
            InboundGridWidth = FlightSchedule?.Inbound?.FlightDuration.TotalMinutes / 2 ?? 0;
            TurnAroundWidth = FlightSchedule?.TurnaroundTime.TotalMinutes / 2 ?? 0;

            // Calculate Margin
            var outboundStart = FlightSchedule?.Outbound?.DepartureTime.TotalMinutes / 2 ?? 0;
            OutboundMargin = new Thickness(outboundStart, 0, 0, 0);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
