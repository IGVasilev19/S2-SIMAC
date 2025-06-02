using BLL;
using DAL.Interfaces;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly INotificationRepository notificationRepository;
        private readonly IDeviceRepository deviceRepository;

        public AnalyticsService(INotificationRepository NotificationRepository, IDeviceRepository deviceRepository)
        {
            this.notificationRepository = NotificationRepository;
            this.deviceRepository = deviceRepository;
        }


        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Device> GetAll()
        {
            IEnumerable<Device> devices = deviceRepository.GetAll();
            return devices;
        }
        public IEnumerable<Notification> GetByDeviceID(int deviceID)
        {
            IEnumerable<Notification> notifications = notificationRepository.GetByDeviceID(deviceID);
            return notifications;
        }
    }
}
