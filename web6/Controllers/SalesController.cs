using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using web6.Data;
using web6.Helpers;
using web6.Models;
using X.PagedList;
using X.PagedList.Extensions;
using X.PagedList.Mvc;

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
        [Auth1]
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
        public IActionResult AddToCart(string tourId, int quantity) {
            if (quantity < 1) {
                ModelState.AddModelError("", "購入数は1以上の整数を入力してください。");
                return RedirectToAction("TourDetails", new {
                    id = tourId
                });
            }
            var tour = _db.Tours.FirstOrDefault(t => t.ID == tourId);
            if (tour == null)
                return NotFound();

            // セッションからカートを取得
            var cart = CartHelper.GetCart(HttpContext.Session);

            // 既に同じ商品がある場合は数量を加算
            var existing = cart.FirstOrDefault(c => c.Id == tourId);
            if (existing != null) {
                existing.Quantity += quantity;
            } else {
                cart.Add(new CartItem {
                    Id = tour.ID,
                    Name = tour.Name,
                    Date = tour.Date,
                    Price = tour.Price,
                    Quantity = quantity,
                });
            }

            // セッションに保存
            CartHelper.SaveCart(HttpContext.Session, cart);

            // カート画面に遷移
            return RedirectToAction("ViewCart");

        }

        public IActionResult ViewCart() {
            var cart = CartHelper.GetCart(HttpContext.Session);
            return View(cart); // ViewCart.cshtml
        }

        [HttpGet]
        public IActionResult SelectCustomer(string? searchName) {
            List<Customer> result = new List<Customer>();

            if (!string.IsNullOrEmpty(searchName)) {
                result = _db.Customers
                            .Where(c => c.Name.Contains(searchName))
                            .ToList();
            }

            var cart = CartHelper.GetCart(HttpContext.Session);

            var filteredCart = cart.Where(c => c.Quantity > 0).ToList();
            CartHelper.SaveCart(HttpContext.Session, filteredCart);

            var model = new SelectCustomerViewModel {
                SearchName = searchName,
                Customers = result,
                CartItems = filteredCart
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult RecalculateCart(List<CartItem> updatedCart) {
            var newCart = updatedCart
                .Where(item => item.Quantity > 0) // 数量0は除外
                .ToList();

            CartHelper.SaveCart(HttpContext.Session, newCart);
            return RedirectToAction("ViewCart");
        }

        [HttpPost]
        public IActionResult ClearCart() {
            CartHelper.SaveCart(HttpContext.Session, new List<CartItem>());
            return RedirectToAction("ViewCart");

        }

        [HttpPost]
        public IActionResult ConfirmOrder(string customerId) {
            var customer = _db.Customers.FirstOrDefault(c => c.ID == customerId);
            if (customer == null)
                return NotFound();

            var cart = CartHelper.GetCart(HttpContext.Session);

            var model = new ConfirmOrderViewModel {
                Customer = customer,
                CartItems = cart
            };

            return View(model); // ConfirmOrder.cshtml
        }

        [HttpPost]
        public IActionResult CompletePurchase(string customerId) {
            var customer = _db.Customers.FirstOrDefault(c => c.ID == customerId);
            if (customer == null)
                return NotFound();

            var cart = CartHelper.GetCart(HttpContext.Session);
            var failedItems = new List<CartItem>();
            var now = DateTime.Now;

            using (var transaction = _db.Database.BeginTransaction()) {
                try {
                    // 在庫チェック
                    foreach (var item in cart) {
                        var tour = _db.Tours.FirstOrDefault(t => t.ID.ToString() == item.Id);
                        if (tour == null || tour.Stock < item.Quantity) {
                            failedItems.Add(item);
                        }
                    }

                    if (failedItems.Any()) {
                        transaction.Rollback();
                        return View("PurchaseResult", new PurchaseResultViewModel {
                            Success = false,
                            FailedItems = failedItems
                        });
                    }

                    // OrderNoをユニークに発行
                    string orderNo = OrderHelper.GenerateUniqueOrderNo(_db);

                    // カート情報を保存
                    int num = 1;
                    foreach (var item in cart) {
                        var tour = _db.Tours.First(t => t.ID.ToString() == item.Id);
                        tour.Stock -= item.Quantity;

                        _db.Orders.Add(new Order {
                            OrderNo = orderNo,
                            num = num++,
                            Date = now,
                            CustomerId = customerId,
                            ItemId = item.Id,
                            Category = item.Id.StartsWith("TUR") ? "01" : "99",
                            UnitPrice = item.Price,
                            Quantity = item.Quantity,
                            Total = item.Price * item.Quantity
                        });
                    }

                    _db.SaveChanges();
                    transaction.Commit();

                    CartHelper.SaveCart(HttpContext.Session, new List<CartItem>());

                    return View("PurchaseResult", new PurchaseResultViewModel {
                        Success = true,
                        Customer = customer
                    });
                } catch (Exception) {
                    transaction.Rollback();
                    ModelState.AddModelError("", "注文処理中にエラーが発生しました。");
                    return RedirectToAction("ViewCart");
                }
            }
        }
    }
}
