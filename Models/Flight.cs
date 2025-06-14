namespace web6.Models {
    public class Flight {
        public string ID {
            get; set;
        }
        public string Name {
            get; set;
        }
        public DateTime Date {
            get; set;
        }
        public TimeSpan DepartureTime {
            get; set;
        }
        public TimeSpan ArrivalTime {
            get; set;
        }
        public string DepartureAirportId {
            get; set;
        }
        public string ArrivalAirportId {
            get; set;
        }
        public int Price {
            get; set;
        }
        public int Stock {
            get; set;
        }
    }
}
