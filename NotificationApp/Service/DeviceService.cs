using BLL;
using DAL;
using DAL.Interfaces;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository _deviceRepository;

        public DeviceService(IDeviceRepository deviceRepository)
        {
            this._deviceRepository = deviceRepository;
        }

        public IEnumerable<Device> GetAll()
        {
            return _deviceRepository.GetAll();
        }

        public Status GetDeviceStatus(Device device)
        {
            return _deviceRepository.GetStatus(device); 
        }
        public Device GetById(int ID) 
        {
            return _deviceRepository.GetById(ID);
        }
        public void Update(Device device)
        {
            _deviceRepository.Update(device);
        }

    }
}
