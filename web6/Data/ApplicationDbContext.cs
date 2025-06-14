using Microsoft.EntityFrameworkCore;
using web6.Models;

namespace web6.Data {
    public class ApplicationDbContext :DbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Customer> Customers {
            get; set;
        }
        public DbSet<Order> Orders {
            get; set;
        }
        public DbSet<Employee> Employees {
            get; set;
        }
        public DbSet<Tour> Tours {
            get; set;
        }
        public DbSet<Area> Areas {
            get; set;
        }
        public DbSet<City> Cities {
            get; set;
        }
        public DbSet<Hotel> Hotels {
            get; set;
        }
        public DbSet<Flight> Flights {
            get; set;
        }
        public DbSet<Airport> Airports {
            get; set;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            // Order の複合主キー定義
            modelBuilder.Entity<Order>()
                .HasKey(o => new { o.OrderNo, o.num });
        }
    }
}
