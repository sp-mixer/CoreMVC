using Microsoft.AspNetCore.Mvc;
using web6.Data;
using web6.Models;
using X.PagedList.Mvc;
using X.PagedList;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using X.PagedList.Extensions;

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
            if (!string.IsNullOrEmpty(searchDateInput) && DateTime.TryParse(searchDateInput, out var searchDate))
                query = query.Where(x => x.Date.Date == searchDate.Date);
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

        [HttpGet]
        public IActionResult TourList(int page = 1, int pageSize = 10) {
            var query = _db.BuildTourQuery().OrderBy(t => t.ID);
            IPagedList<TourViewModel> pagedTours = query.ToPagedList(page, pageSize);
            return View(pagedTours);
        }
    }
}
