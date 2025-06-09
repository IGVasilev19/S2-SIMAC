using BLL;
using DAL;
using DAL.Interfaces;
using Microsoft.IdentityModel.Tokens;
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

        public void DeleteById(int id)
        {
            var device = _deviceRepository.GetById(id);
            if (device == null)
                throw new KeyNotFoundException("Device not found.");

            _deviceRepository.Delete(id);
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
            //_deviceRepository.Update(device);
        }
        public IEnumerable<Device> GetByOrganization(int organizationID) 
        {
            return _deviceRepository.GetByOrganization(organizationID);
        }

        public IEnumerable<Device> SearchDevices(string filter, int organizationId)
        {
            IEnumerable<Device> filteredDevices = _deviceRepository.GetByOrganization(organizationId);
            if (!string.IsNullOrEmpty(filter))
            {
                filteredDevices = filteredDevices.Where(s => s.Name.ToUpper().Contains(filter.ToUpper()));
            }
            return filteredDevices;
        }

        public Device ChangeStatus(int deviceId, int status) //0 = Online, 1 = Offline  
        {
            if (deviceId <= 0 || !Enum.IsDefined(typeof(Status), status))
            {
                throw new ArgumentException("Invalid device ID or status value.");
            }

            Device device = _deviceRepository.GetById(deviceId);

            if (device == null)
            {
                throw new KeyNotFoundException("Device not found.");
            }

            device.DeviceStatus = (Status)status;
            _deviceRepository.Update(device);
            return device;
        }
    }
}
