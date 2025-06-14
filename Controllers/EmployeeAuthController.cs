using Microsoft.AspNetCore.Mvc;
using web6.Data;
using web6.Models;

public class EmployeeAuthController : Controller {
    private readonly ApplicationDbContext _db;
    public EmployeeAuthController(ApplicationDbContext db) => _db = db;

    public ActionResult Index(string name) {
        var employees = _db.Employees
            .Where(e => name == null || e.Name.Contains(name))
            .Select(e => new EmployeeAuthViewModel {
                Id = e.Id,
                Name = e.Name,
                Auth1 = e.auth1,
                Auth2 = e.auth2,
                Auth3 = e.auth3
            }).ToList();

        return View(employees);
    }

    [HttpPost]
    public ActionResult Update(List<EmployeeAuthViewModel> updatedEmployees) {
        foreach (var item in updatedEmployees) {
            if (item == null) {
                System.Diagnostics.Debug.WriteLine("item が null");
                continue;
            }

            if (string.IsNullOrEmpty(item.Id)) {
                System.Diagnostics.Debug.WriteLine("item.Id が null または空");
                continue;
            }

            System.Diagnostics.Debug.WriteLine("ID: " + item.Id);
        }
        foreach (var item in updatedEmployees) {
            var emp = _db.Employees.Find(item.Id);
            if (emp != null) {
                emp.auth1 = item.Auth1;
                emp.auth2 = item.Auth2;
                emp.auth3 = item.Auth3;
            }
        }
        _db.SaveChanges();
        TempData["Message"] = "更新しました";
        return RedirectToAction("Index");
    }
}