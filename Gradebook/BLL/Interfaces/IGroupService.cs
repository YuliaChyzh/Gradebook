using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DTO;

namespace BLL.Interfaces
{
    public interface IGroupService
    {
        GroupDTO GetGroup(int id);
        IEnumerable<GroupDTO> Get();
        void AddGroup(GroupDTO groupDTO);
        bool DeleteGroup(int id, int countStudents);
        void EditGroup(GroupDTO groupDTO);
        void Dispose();
    }
}
