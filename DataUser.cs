using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.Net_baseUsers
{
    //класс хранения данных пользователя
    public class DataUser
    {
        public string log { get; set; }
        public string address { get; set; }
        public int phone { get; set; }
        public int password { get; set; }
        public bool admin { get; set; }
        public DataUser() { }
        public DataUser(string log, string address, int phone, int password, bool admin)
        {
            this.log = log;
            this.address = address;
            this.phone = phone;
            this.password = password;
            this.admin = admin;
        }
        public DataUser(DataUser user)
        {
            this.log = user.log;
            this.address = user.address;
            this.phone = user.phone;
            this.password = user.password;
            this.admin = user.admin;
        }
        public static bool operator ==(DataUser u1, DataUser u2)
        {
            if (u1.address == u2.address && u1.admin == u2.admin && u1.log == u2.log && u1.password == u2.password && u1.phone == u2.phone)
            {
                return true;
            }
            return false;
        }
        public static bool operator !=(DataUser u1, DataUser u2)
        {
            if (u1.address != u2.address || u1.admin != u2.admin || u1.log != u2.log || u1.password != u2.password || u1.phone != u2.phone)
            {
                return true;
            }
            return false;
        }
    }
}
