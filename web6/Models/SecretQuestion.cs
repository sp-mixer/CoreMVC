using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace web6.Models {
    [Table("SecretQuestion")]
    public class Secretquestion {
        [Key]
        public string Id { get; set; }
        public string Question { get; set; }
    }
}
