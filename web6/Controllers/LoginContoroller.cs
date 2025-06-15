using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using web6.Data; // DbContextの場所に応じて変更してください
using web6.Helpers;
using web6.Models;

namespace web6.Controllers {
    public class LoginController : Controller {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context) {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index() {
            return View(); // Login画面表示
        }

        [HttpPost]
        public IActionResult Index(string id, string password) {
            // IDだけで検索（DBクエリに通る）
            var emp = _context.Employees.FirstOrDefault(e => e.ID == id);

            // ID一致しない or BANされてる
            if (emp is null || emp.Password_hash != password) {
                ViewBag.Message = "IDまたはパスワードが違うか、利用が制限されています。";
                return View();
            }

            string hashedInput = PasswordUtil.ComputeSha256Hash(password);

            // セッションにログイン情報を保存
            HttpContext.Session.SetString("UserId", emp.ID);
            HttpContext.Session.SetString("UserName", emp.Name);
            HttpContext.Session.SetString("Role", emp.Role ?? "");
            HttpContext.Session.SetString("Auth1", emp.auth1.ToString());
            HttpContext.Session.SetString("Auth2", emp.auth2.ToString());
            HttpContext.Session.SetString("Auth3", emp.auth3.ToString());

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout() {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}