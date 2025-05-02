using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IRoleService
    {
        void AssignPermission(int roleId, string permission);
        //Not complete, need to implement
    }
}
