using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IAnalyticsService : IService<Device>
    {
        IEnumerable<Notification> GetByDeviceID(int deviceID);
    }
}
