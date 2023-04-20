using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Misa.Web01.HCSN.BL;
using Misa.Web01.HCSN.BL.FixedAssetIncrementDetailBL;
using Misa.Web01.HCSN.COMMON.Entities;
using Misa.Web01.HCSN.COMMON.Resource;
using MISA.WEB01.HCSN.BaseControllers;
using Swashbuckle.AspNetCore.Annotations;

namespace MISA.WEB01.HCSN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FixedAssetIncrementDetailsController : BasesController<FixedAssetIncrementDetail>
    {
        private IFixedAssetIncrementDetailBL _iBaseBL;
        public FixedAssetIncrementDetailsController(IFixedAssetIncrementDetailBL iBaseBL):base(iBaseBL) 
        {
            _iBaseBL = iBaseBL;
        }

        [HttpGet("Detail")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK)]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest)]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError)]
      
        public IActionResult SelectByVoucher([FromQuery] Guid listId)
        {
            try
            {
                var result = _iBaseBL.SelectByVoucher(listId);
                if (result != null)
                {
                    return StatusCode(StatusCodes.Status200OK, result);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ErrorResource.NotFound);
                }

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ErrorResource.ExceptionMsg);
            }
        }
    }
}
