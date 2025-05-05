using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;

namespace DAL.Interfaces
{
    public interface INotificationRepository : IRepository<Notification>
    {
        public List<Notification> GetByPermission(int permissionId);
    }

}
