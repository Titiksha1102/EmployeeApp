using EmployeeApp.Data.Interfaces.AddressRepo;
using EmployeeApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeApp.Services.AddressServiceFolder
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _repo;
        public AddressService(IAddressRepository repo)
        {
            _repo = repo;
        }
        public Address Create(Address address)
        {
            return _repo.Create(address);
        }

        public Address Edit(Address address)
        {
            return _repo.Edit(address);
        }
    }
}
