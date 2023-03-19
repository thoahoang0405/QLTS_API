using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.WEB01.HCSN.BaseControllers;
using Misa.Web01.HCSN.COMMON.entities;
using Misa.Web01.HCSN.BL;

namespace MISA.WEB01.HCSN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : BasesController<Departments>
    {
        private IDepartmentsBL _departmentsBL;
        public DepartmentsController(IDepartmentsBL departmentsBL) : base(departmentsBL)
        {
            _departmentsBL = departmentsBL;
        }


    }
}
