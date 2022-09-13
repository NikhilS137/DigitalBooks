using AuthServer.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Models
{
    public class UserValidationModel
    {
        private readonly DBDigitalBooksContext _context = new DBDigitalBooksContext();
        public string userName { get; set; }
        public string password { get; set; }
      
        public UserMaster ValidateCredentials(string UserName, string Password)
        {
            UserMaster userMaster1 = null;

            if (_context.UserMasters == null)
            {
                return userMaster1;
               // return false;
            }

            //var userMaster = (from x in _context.UserMasters
            //                   where x.UserName == UserName && x.Password == EncryptionDecryption.EncodePasswordToBase64(Password)
            //                   select x.UserId).SingleOrDefault();

            userMaster1 = (from x in _context.UserMasters
                              where x.UserName == UserName && x.Password == EncryptionDecryption.EncodePasswordToBase64(Password)
                              select x).SingleOrDefault();

            //if (userMaster > 0)
            //{
            //    return true;
            //}

            //return false;
            return userMaster1;

        }
    }
}
