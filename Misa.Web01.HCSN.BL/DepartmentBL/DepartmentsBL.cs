using Misa.Web01.HCSN.COMMON.entities;
using Misa.Web01.HCSN.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web01.HCSN.BL
{
    public class DepartmentsBL: BaseBL<Departments>, IDepartmentsBL
    {
        private IDepartmentsDL _departmentDL;
        public DepartmentsBL(IDepartmentsDL departmentsDL) : base(departmentsDL)
        {
            _departmentDL = departmentsDL;
        }
    }
}
