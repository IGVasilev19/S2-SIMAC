using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IDeviceService : IService<Device>
    {
        Device GetById(int deviceID);
        Status GetDeviceStatus(Device device);
        void Update(Device device);
        IEnumerable<Device> GetByOrganization(int organizationID);
        public IEnumerable<Device> SearchDevices(string filter, int organizationId);
        Device ChangeStatus(int deviceId, int status);
    }
}
