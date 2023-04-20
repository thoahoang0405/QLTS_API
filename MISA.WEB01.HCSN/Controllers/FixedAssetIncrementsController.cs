using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Misa.Web01.HCSN.BL;
using Misa.Web01.HCSN.BL.BaseBL;
using Misa.Web01.HCSN.BL.FixedAssetIncrementBL;
using Misa.Web01.HCSN.COMMON.Entities;
using Misa.Web01.HCSN.COMMON.Resource;
using MISA.WEB01.HCSN.BaseControllers;
using MISA.WEB01.HCSN.Common.entities;
using MISA.WEB01.HCSN.COMMON;
using MISA.WEB01.HCSN.COMMON.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace MISA.WEB01.HCSN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FixedAssetIncrementsController : BasesController<FixedAssetIncrement>
    {
        private IFixedAssetIncrementBL _iIncrement;
        public FixedAssetIncrementsController(IFixedAssetIncrementBL iIncrement):base(iIncrement) 
        {
            _iIncrement = iIncrement;
        }

        /// <summary>
        /// phan trang ghi tang
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [HttpGet("Filter")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(PagingData<FixedAssetIncrement>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public  IActionResult FilterIncrement([FromQuery] string? keyword, [FromQuery] int? pageSize,[FromQuery] int pageNumber = 1)
        {


            var multipleResults = _iIncrement.FilterFixedAssetIncrement(keyword, pageSize, pageNumber);
            if (multipleResults != null)
            {
                return StatusCode(StatusCodes.Status200OK, multipleResults);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, ErrorResource.NotFound);
            }
        }

        
        
        [HttpPost("IncrementDetail")]
        public IActionResult InsertIncrement(FixedAssetIncrement paramsInsert)
        {
     
            try
            {
                int result = _iIncrement.InsertIncrement(paramsInsert);


                if (result != 0)
                {
                    return StatusCode(StatusCodes.Status201Created, result);

                }
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.GenerateServerErrorResult());

            }


            catch (ErrorService ex)
            {
                if (ex.ErrorCode == MISAErrorCode.DuplicateCode)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, HandleError.GenerateDuplicateErrorResult(ex));
                }
                return StatusCode(StatusCodes.Status400BadRequest, HandleError.GenerateValidateErrorResult(ex));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorResource.ServerException);
            }

        }


    }
}
