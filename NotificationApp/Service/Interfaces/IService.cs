using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IService<T>
    {
        IEnumerable<T> GetAll();
        void DeleteById(int id);
    }
}
