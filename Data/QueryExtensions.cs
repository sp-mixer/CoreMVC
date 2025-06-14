using web6.Models;
using System.Linq;
using web6.Data;

namespace web6.Data {
    public static class QueryExtensions {
        public static IQueryable<TourViewModel> BuildTourQuery(this ApplicationDbContext db) {
            return from t in db.Tours
                   join a in db.Areas on t.AreaId equals a.ID
                   join f in db.Flights on t.FlightId equals f.ID
                   join h in db.Hotels on t.HotelId equals h.ID
                   select new TourViewModel {
                       ID = t.ID,
                       Name = t.Name,
                       AreaName = a.Name,
                       FlightName = f.Name,
                       HotelName = h.Name,
                       Date = t.Date,
                       Days = t.Days,
                       Price = t.Price,
                       Stock = t.Stock
                   };
        }

        public static IQueryable<HotelViewModel> BuildHotelQuery(this ApplicationDbContext db) {
            return from h in db.Hotels
                   join c in db.Cities on h.CityId equals c.ID
                   select new HotelViewModel {
                       ID = h.ID,
                       Name = h.Name,
                       CityName = c.Name,
                       Date = h.Date,
                       Price = h.Price,
                       Stock = h.Stock
                   };
        }

        public static IQueryable<FlightViewModel> BuildFlightQuery(this ApplicationDbContext db) {
            return from f in db.Flights
                   join da in db.Airports on f.DepartureAirportId equals da.ID
                   join aa in db.Airports on f.ArrivalAirportId equals aa.ID
                   select new FlightViewModel {
                       ID = f.ID,
                       Name = f.Name,
                       Date = f.Date,
                       DepartureTime = f.DepartureTime,
                       ArrivalTime = f.ArrivalTime,
                       DepartureAirportName = da.Name,
                       ArrivalAirportName = aa.Name,
                       Price = f.Price,
                       Stock = f.Stock
                   };
        }
    }


}
