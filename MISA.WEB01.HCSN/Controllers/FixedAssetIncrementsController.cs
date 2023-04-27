using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Misa.Web01.HCSN.BL;
using Misa.Web01.HCSN.BL.BaseBL;
using Misa.Web01.HCSN.BL.FixedAssetIncrementBL;
using Misa.Web01.HCSN.COMMON.Entities;
using Misa.Web01.HCSN.COMMON.Entities.DTO;
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
        public FixedAssetIncrementsController(IFixedAssetIncrementBL iIncrement) : base(iIncrement)
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
        public IActionResult FilterIncrement([FromQuery] string? keyword, [FromQuery] int? pageSize, [FromQuery] int pageNumber = 1)
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
        /// <summary>
        /// sửa theo id
        /// </summary>
        /// <param name="employeeID"></param>
        ///<returns> status400 nếu lỗi ; return về status200 nếu thành công </returns>
        /// CreatedBy: HTTHOA(16/03/2023)

        [HttpPut]
        public IActionResult UpdateIncrement([FromBody] FixedAssetIncrement increment, [FromQuery] Guid incrementID)
        {

            try
            {

                int numberOfAffectedRows = _iIncrement.UpdateIncrement(increment, incrementID);

                if (numberOfAffectedRows > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, incrementID);
                }
                return StatusCode(StatusCodes.Status400BadRequest, ErrorResource.NotFound);
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
        /// <summary>
        /// API xóa 1 bản ghi
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns>về status500 hoặc status400 nếu lỗi ; return về status200 nếu thành công</returns>
        /// CreatedBy: HTTHOA(16/03/2023)
        [HttpDelete("Increment")]
        public IActionResult DeleteIncrementID([FromBody] List<Guid> listFixedAssetID, [FromQuery] Guid id)
        {
            try

            {
                int numberOfAffectedRows = _iIncrement.DeleteIncrementID(listFixedAssetID, id);

                // Xử lý kết quả trả về từ DB
                if (numberOfAffectedRows > 0)
                {
                    // Trả về dữ liệu cho client
                    return StatusCode(StatusCodes.Status200OK, id);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ErrorResource.NotFound);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorResource.ServerException);
            }
        }
        [SwaggerResponse(statusCode: StatusCodes.Status200OK)]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest)]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError)]
        [HttpDelete()]
        public IActionResult DeleteMultipleIncrement(DeleteMultipleIncrement listId)
        {
            try
            {
                int result = _iIncrement.DeleteMultipleIncrement(listId);
                if (result != 0)
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
