using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace web6.Data {
    // EF Core のマイグレーションツールが呼び出すファクトリ
    public class ApplicationDbContextFactory
        : IDesignTimeDbContextFactory<ApplicationDbContext> {
        public ApplicationDbContext CreateDbContext(string[] args) {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(
                "Server=localhost\\SQLEXPRESS;"
              + "Initial Catalog=TravelDb;"
              + "Trusted_Connection=True;"
              + "TrustServerCertificate=True;"    // ← これを必ず追加
            );
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
