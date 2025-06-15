using System.ComponentModel.DataAnnotations;

namespace web6.Models {
    public class CartItem {
        public string Id {
            get; set;
        }             // 商品ID（ツアー、ホテル、航空便）
        public string Name {
            get; set;
        }
        public DateTime Date {
            get; set;
        }
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int Price {
            get; set;
        }
        public int Quantity { get; set; } = 1;
    }
}
