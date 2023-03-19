﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Misa.Web01.HCSN.BL;
using Misa.Web01.HCSN.COMMON.entities;
using Misa.Web01.HCSN.DL;
using MISA.WEB01.HCSN.BaseControllers;
using MISA.WEB01.HCSN.Common.entities;
using MISA.WEB01.HCSN.COMMON.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace MISA.WEB01.HCSN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FixedAssetsController : BasesController<FixedAsset>
    {
        private IFixedAssetBL _fixedAssetBL;
        public FixedAssetsController(IFixedAssetBL fixedAssetBL) : base(fixedAssetBL)
        {
            _fixedAssetBL = fixedAssetBL;
        }
     

        /// <summary>
        ///  lấy dữ liệu phân trang
        /// </summary>
        /// <param name="keyword, pageSize, pageNumber"></param>
        /// <returns>về status500 hoặc status400 nếu lỗi ; return về status200 nếu thành công </returns>
        /// CreatedBy: HTTHOA(16/08/2022)
        [HttpGet("Filter")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(PagingData<FixedAsset>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult FilterEmployees([FromQuery] string? keyword, [FromQuery] int pageSize, [FromQuery] int pageNumber, [FromQuery] string? departmentName, [FromQuery] string? fixedAssetCategoryName )
        {
            
                var multipleResults = _fixedAssetBL.FilterFixedAsset(keyword, pageSize, pageNumber, departmentName, fixedAssetCategoryName);
                if (multipleResults != null)
                {
                    return StatusCode(StatusCodes.Status200OK, multipleResults);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
            }

        [SwaggerResponse(statusCode: StatusCodes.Status200OK)]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest)]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError)]
        [HttpDelete("delete-multiple")]
        public IActionResult DeleteMultiple(List<Guid> listId)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, _fixedAssetBL.DeleteMultiple(listId));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest,"lỗi");
            }
        }

    }
}