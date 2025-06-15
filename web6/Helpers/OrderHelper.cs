using Microsoft.EntityFrameworkCore;
using web6.Data;

namespace web6.Helpers {
    public static class OrderHelper {
        public static string GenerateUniqueOrderNo(ApplicationDbContext db) {
            string prefix = "ORD" + DateTime.Now.ToString("yyyyMMdd");

            var todayOrders = db.Orders
                .Where(o => o.OrderNo.StartsWith(prefix))
                .ToList();

            int maxSerial = todayOrders
                .Select(o => int.Parse(o.OrderNo.Substring(11)))
                .DefaultIfEmpty(0)
                .Max();

            return $"{prefix}{(maxSerial + 1).ToString("D4")}";
        }
    }
}
