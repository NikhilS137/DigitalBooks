using AuthServer.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Models
{
    public class UserValidationModel
    {
        private readonly DBDigitalBooksContext _context = new DBDigitalBooksContext();
        public string userName { get; set; }
        public string password { get; set; }
      
        public bool ValidateCredentials(string UserName, string Password)
        {

            if (_context.UserMasters == null)
            {
                return false;
            }
            
            var userMaster = (from x in _context.UserMasters
                               where x.UserName == UserName && x.Password == EncryptionDecryption.EncodePasswordToBase64(Password)
                               select x.UserId).SingleOrDefault();

            if (userMaster > 0)
            {
                return true;
            }

            return false;

        }
    }
}
