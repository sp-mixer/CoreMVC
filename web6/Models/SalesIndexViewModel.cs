using X.PagedList;

namespace web6.Models {

    public class TourViewModel{
        public string ID { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string AreaName { get; set; } = string.Empty;
        public string FlightName { get; set; } = string.Empty;
        public string HotelName { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int Days { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
    }

    public class HotelViewModel{
        public string ID { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
    }

    public class FlightViewModel{
        public string ID { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public string DepartureAirportName { get; set; } = string.Empty;
        public string ArrivalAirportName { get; set; } = string.Empty;
        public int Price { get; set; }
        public int Stock { get; set; }
    }

    public class SalesIndexViewModel {
        public List<TourViewModel> Tours { get; set; } = new List<TourViewModel>();
        public List<HotelViewModel> Hotels { get; set; } = new List<HotelViewModel>();
        public List<FlightViewModel> Flights { get; set; } = new List<FlightViewModel>();

        // 検索条件
        public string? SearchName { get; set;}
        public string? SearchDateInput { get; set;}
        public int? MinPrice {get; set;}
        public int? MaxPrice { get; set;}

        // ソート情報
        public string? SortColumn { get; set;}
        public string? SortDirection { get; set;}

        // ページング情報
        public int PageNumber {get; set;}
        public int PageSize {get; set;}
        public int TotalItems {get; set;}
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
    }
    public class TourListViewModel {
        // 検索条件
        public string? SearchName {
            get; set;
        }
        public string? SearchDate {
            get; set;
        }

        // ソート条件
        public string? SortColumn {
            get; set;
        }
        public string? SortDirection {
            get; set;
        }

        // ページング
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        // 結果リスト（PagedList）
        public IPagedList<TourViewModel> Tours {
            get; set;
        }
    }


}
