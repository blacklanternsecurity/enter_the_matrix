using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Enter_The_Matrix.Models
{
    public class Account
    {
        public string userName { get; set; }
        [DataType(DataType.Password)]
        public string password { get; set; }
        public string returnUrl { get; set; }

    }
}
