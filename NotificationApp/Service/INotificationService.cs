using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;

namespace Service
{
    public interface INotificationService : IService<Notification>
    {
        Notification GetById(int id);
    }
}
