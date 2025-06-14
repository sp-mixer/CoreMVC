using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace web6.Models {
    [Table("SecretQuestion")]
    public class SecretQuestion {
        [Key]
        public string Id {
            get; set;
        }
        public string Question {
            get; set;
        }
    }

    [Table("Employees")]
    public class Employee {
        [Key]
        public string Id {
            get; set;
        }
        public string Name {
            get; set;
        }
        public string Mail {
            get; set;
        }
        public string Password_hash {
            get; set;
        }

        [NotMapped]
        public string Password {
            get; set;
        }

        public string Role {
            get; set;
        }
        public bool ban {
            get; set;
        }
        public bool auth1 {
            get; set;
        }
        public bool auth2 {
            get; set;
        }
        public bool auth3 {
            get; set;
        }

        public string Q1_id {
            get; set;
        }
        public string Q1_answer {
            get; set;
        }

        public string Q2_id {
            get; set;
        }
        public string Q2_answer {
            get; set;
        }

        public string Q3_id {
            get; set;
        }
        public string Q3_answer {
            get; set;
        }
    }
}