using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Misa.Web01.HCSN.BL;
using Misa.Web01.HCSN.COMMON.entities;
using Misa.Web01.HCSN.COMMON.Resource;
using Misa.Web01.HCSN.DL;
using MISA.WEB01.HCSN.BaseControllers;
using MISA.WEB01.HCSN.Common.entities;
using MISA.WEB01.HCSN.COMMON;
using MISA.WEB01.HCSN.COMMON.Entities;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Globalization;
using static Org.BouncyCastle.Bcpg.Attr.ImageAttrib;

namespace MISA.WEB01.HCSN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FixedAssetsController : BasesController<FixedAsset>
    {
        #region Field
        private IFixedAssetBL _fixedAssetBL;
        #endregion

        #region Contructor
        public FixedAssetsController(IFixedAssetBL fixedAssetBL) : base(fixedAssetBL)
        {
            _fixedAssetBL = fixedAssetBL;
        }
        #endregion




        #region Method
        /// <summary>
        ///  lấy dữ liệu phân trang
        /// </summary>
        /// <param name="keyword, pageSize, pageNumber"></param>
        /// <returns>về status500 hoặc status400 nếu lỗi ; return về status200 nếu thành công </returns>
        /// CreatedBy: HTTHOA(16/03/2023)
        [HttpPost("Filter")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(PagingData<FixedAsset>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult FilterFixedAsset(
            [FromQuery] string? keyword,
            [FromQuery] int? pageSize,
            [FromQuery] Guid? departmentID, 
            [FromQuery] Guid? fixedAssetCategoryID, 
            [FromQuery] int pageNumber, 
            [FromQuery] int? active,
            [FromBody] List<Guid> listId
            )
        {


            var multipleResults = _fixedAssetBL.FilterFixedAsset(keyword, pageSize, departmentID, fixedAssetCategoryID, pageNumber, active, listId);
            if (multipleResults != null)
            {
                return StatusCode(StatusCodes.Status200OK, multipleResults);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, ErrorResource.NotFound);
            }
        }
        /// <summary>
        /// xóa nhiều bản ghi
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        [SwaggerResponse(statusCode: StatusCodes.Status200OK)]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest)]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError)]
        [HttpDelete()]
        public IActionResult DeleteMultiple(List<Guid> listId)
        {
            try
            {
                int result = _fixedAssetBL.DeleteMultiple(listId);
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


        /// <summary>
        /// xuất khẩu ra file excel
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="departmentID"></param>
        /// <param name="fixedAssetCategoryID"></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA(1/04/2023)
        [HttpGet("ExportExcel")]
        public IActionResult ExportExcel([FromQuery] string? keyword, [FromQuery] Guid? departmentID, [FromQuery] Guid? fixedAssetCategoryID)
        {
            try
            {
                var stream = new MemoryStream();
                stream = _fixedAssetBL.ExportExcel(keyword, departmentID, fixedAssetCategoryID);
                string excelName = "Danh_tai_san.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ErrorResource.ExceptionMsg);
            }
        }
        [HttpPut("ListID")]
        public IActionResult UpdateFixedAsset([FromBody] List<Guid> listId, [FromQuery] int active)
        {
            int result = _fixedAssetBL.UpdateFixedAsset(listId, active);

            if (result > 0)
            {
                return StatusCode(StatusCodes.Status200OK, listId);
            }
            return StatusCode(StatusCodes.Status400BadRequest, ErrorResource.NotFound);
        }
        [HttpPost("FilterFixedAsetChoose")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(PagingData<FixedAsset>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult FilterChoose(
           [FromQuery] string? keyword,
           [FromQuery] int? pageSize,
           [FromQuery] Guid? voucherId,
           [FromQuery] int pageNumber,
           [FromQuery] int? active,
           [FromBody] List<Guid> listId
           )
        {


            var multipleResults = _fixedAssetBL.FilterChoose(keyword, pageSize, voucherId, pageNumber, active, listId);
            if (multipleResults != null)
            {
                return StatusCode(StatusCodes.Status200OK, multipleResults);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, ErrorResource.NotFound);
            }
        }
        #endregion
    }
}
