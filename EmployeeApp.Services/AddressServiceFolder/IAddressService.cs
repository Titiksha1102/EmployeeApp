using EmployeeApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Services.AddressServiceFolder
{
    public interface IAddressService
    {
        public Address Create(Address address);
        public Address Edit(Address address);
    }
}
