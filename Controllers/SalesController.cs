using Microsoft.AspNetCore.Mvc;
using web6.Data;
using web6.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace web6.Controllers {
    public class SalesController : Controller {
        private readonly ApplicationDbContext _db;
        public SalesController(ApplicationDbContext db) => _db = db;

        [HttpGet]
        public IActionResult Index() {
            // 初回表示はツアー検索アクションへリダイレクト
            return RedirectToAction(nameof(SearchTours));
        }

        [HttpGet]
        public async Task<IActionResult> SearchTours(string? searchName = null, string? searchDateInput = null, int? minPrice = null,
                                                        int? maxPrice = null, string? sortColumn = null, string? sortDirection = null,
                                                        int page = 1, int pageSize = 15) {
            var query = _db.BuildTourQuery();
            // フィルタ
            if (!string.IsNullOrEmpty(searchName))
                query = query.Where(x => x.Name.Contains(searchName));
            if (minPrice.HasValue)
                query = query.Where(x => x.Price >= minPrice.Value);
            if (maxPrice.HasValue)
                query = query.Where(x => x.Price <= maxPrice.Value);
            // ソート
            bool asc = sortDirection == "asc";
            query = sortColumn switch {
                "Name" => asc ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name),
                "Date" => asc ? query.OrderBy(x => x.Date) : query.OrderByDescending(x => x.Date),
                "Price" => asc ? query.OrderBy(x => x.Price) : query.OrderByDescending(x => x.Price),
                _ => query.OrderBy(x => x.ID)
            };
            // ページング
            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var hotels = await QueryExtensions.BuildHotelQuery(_db).ToListAsync();
            var flights = await QueryExtensions.BuildFlightQuery(_db).ToListAsync();
            var vm = new SalesIndexViewModel {
                Tours = items,
                Flights = flights,
                Hotels = hotels,
                SearchName = searchName,
                SearchDateInput = searchDateInput,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                SortColumn = sortColumn,
                SortDirection = sortDirection,
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = total
            };
            return View("Index", vm);
        }
    }
}
