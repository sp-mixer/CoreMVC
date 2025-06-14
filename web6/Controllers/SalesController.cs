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
        public IActionResult TourList(string? searchName, string? searchDate, string? sortColumn, string? sortDirection,
                                        int page = 1, int pageSize = 10) {
            var query = _db.BuildTourQuery();

            // 検索
            if (!string.IsNullOrEmpty(searchName))
                query = query.Where(t => t.Name.Contains(searchName));

            if (!string.IsNullOrEmpty(searchDate) && DateTime.TryParse(searchDate, out var date))
                query = query.Where(t => t.Date.Date == date.Date);

            // ソート
            bool asc = sortDirection == "asc";
            query = sortColumn switch {
                "Name" => asc ? query.OrderBy(t => t.Name) : query.OrderByDescending(t => t.Name),
                "Date" => asc ? query.OrderBy(t => t.Date) : query.OrderByDescending(t => t.Date),
                "Price" => asc ? query.OrderBy(t => t.Price) : query.OrderByDescending(t => t.Price),
                _ => query.OrderBy(t => t.ID)
            };

            var pagedTours = query.ToPagedList(page, pageSize);

            var model = new TourListViewModel {
                SearchName = searchName,
                SearchDate = searchDate,
                SortColumn = sortColumn,
                SortDirection = sortDirection,
                Page = page,
                PageSize = pageSize,
                Tours = pagedTours
            };

            return View(model);
        }

        public IActionResult TourDetails(string id) {
            var tour = _db.Tours.FirstOrDefault(e => e.ID == id);
            if (tour == null)
                return NotFound();

            return View(tour);
        }

        [HttpPost]
        public IActionResult AddToCart(int tourId, int quantity) {
            if (quantity < 1) {
                ModelState.AddModelError("", "購入数は1以上の整数を入力してください。");
                return RedirectToAction("TourDetails", new {
                    id = tourId
                }); // エラー時は詳細画面へ戻す等
            }

            // 通常のカート処理...
            return View();
        }
    }
}
