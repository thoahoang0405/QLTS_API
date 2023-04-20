using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.WEB01.HCSN.BaseControllers;
using Misa.Web01.HCSN.COMMON.entities;
using Misa.Web01.HCSN.COMMON.Entities;
using Misa.Web01.HCSN.BL;
using Misa.Web01.HCSN.BL.BaseBL;

namespace MISA.WEB01.HCSN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetsController : BasesController<Budget>
    {
        private IBaseBL<Budget> _ibase;
      public BudgetsController(Misa.Web01.HCSN.BL.IBaseBL<Budget> ibudget) :base(ibudget) { 
            _ibase= ibudget;
        
        }
    }
}
