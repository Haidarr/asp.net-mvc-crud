using System;
using System.Text;
using System.Security.Cryptography;

namespace MyProject.Web.Services
{
    public class usersControl
    {
        public static string HashPassword(string phrase)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            SHA256Managed hasher = new SHA256Managed();
            byte[] hashedDataBytes = hasher.ComputeHash(encoder.GetBytes(phrase));
            return Convert.ToBase64String(hashedDataBytes);
        }
    }
}
