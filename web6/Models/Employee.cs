namespace web6.Models {
    public class Employee {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string Password_hash { get; set;}
        public string Role { get; set; }
        public bool ban { get; set; }
        public bool auth1 { get;set; }
        public bool auth2 { get; set; }
        public bool auth3 { get; set; }
    }
}
