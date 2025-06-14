namespace web6.Models {
    public class CartItem {
        public int Id {
            get; set;
        }             // 商品ID（ツアー、ホテル、航空便）
        public string Name {
            get; set;
        }
        public DateTime Date {
            get; set;
        }
        public int Price {
            get; set;
        }
        public int Quantity { get; set; } = 1;
    }
}
