using System.Text;
using System.Security.Cryptography;
//using System.Web.Security;

namespace web6.Helpers {
    public static class PasswordUtil {
        public static string ComputeSha256Hash(string rawData) {
            using (SHA256 sha256 = SHA256.Create()) {
                byte[] bytes = Encoding.UTF8.GetBytes(rawData);
                byte[] hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        public static string SimpleHash(string input) {
            int hash = 17;
            foreach (char c in input) {
                hash = hash * 31 + c;
            }

            return Math.Abs(hash).ToString("X"); 
        }

        public static class PasswordHelper {
            //public static string HashPasswordSHA1(string password) {
            //    return FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");
            //}

            public static string PseudoHash(string password) {
                string salt = "MyFixedSalt123";  // ソルト
                string combined = salt + password;
                byte[] bytes = Encoding.UTF8.GetBytes(combined);
                return Convert.ToBase64String(bytes);
            }

        }
    }
}
