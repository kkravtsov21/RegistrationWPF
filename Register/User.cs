using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Register
{
    public class User
    {
        public int id { get; set; }
        public string Login { get; set; }
        public string RealName { get; set; }
        public string PassHash { get; set; }
        public string ID_Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNum { get; set; }
        public string RegisterDT { get; set; }
        public DateTime LastVisitDT { get; set; }
        public DateTime RecoveryCode { get; set; }



    }
}
