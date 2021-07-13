using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enter_The_Matrix.Models
{
    public class LocalAuthSettings : ILocalAuthSettings
    {
        public string AdminName { get; set; }
        public string AdminPassword { get; set; }
    }

    public interface ILocalAuthSettings
    {
        string AdminName { get; set; }
        string AdminPassword { get; set; }
    }
}
