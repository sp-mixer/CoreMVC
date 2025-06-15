using Microsoft.AspNetCore.Mvc;
using web6.Data;
using web6.Helpers;

namespace web6.Controllers {
    public class ToolsController : Controller {
        private readonly ApplicationDbContext _context;

        public ToolsController(ApplicationDbContext context) {
            _context = context;
        }

        public IActionResult HashAllPasswords() {
            var users = _context.Employees.ToList();

            foreach (var user in users) {
                // すでにハッシュ済みかをチェック（64桁のhexで判断）
                if (string.IsNullOrEmpty(user.Password_hash) || user.Password_hash.Length != 64) {
                    user.Password_hash = PasswordUtil.ComputeSha256Hash(user.Password_hash);
                }
            }

            _context.SaveChanges();
            return Content("全パスワードをハッシュ化しました。");
        }
    }

}
