namespace web6.Models {
    public class Order {
        public string OrderNo {
            get; set;
        }
        public int num {
            get;
            set;
        }
        public DateTime Date {
            get; set;
        }
        public string CustomerId {
            get; set;
        }
        public string ItemId {
            get; set;
        }
        public string Category {
            get; set;
        }
        public int UnitPrice {
            get; set;
        }
        public int Quantity {
            get; set;
        }
        public int Total { get; set; }
    }
}
