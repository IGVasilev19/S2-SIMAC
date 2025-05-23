using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;

namespace DAL.Interfaces
{
    public interface IDeviceRepository : IRepository<Device>
    {
        IEnumerable<Device> GetByOrganization(int organizationID);
        Status GetStatus(Device device);

    }
}
