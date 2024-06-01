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
        public async Task<Address> Create(Address address)
        {
            return await _repo.Create(address);
        }

        public async Task<Address> Edit(Address address)
        {
            return await _repo.Edit(address);
        }
    }
}
