﻿
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Misa.Web01.HCSN.BL;
using Misa.Web01.HCSN.COMMON;
using Misa.Web01.HCSN.COMMON.Resource;
using MISA.WEB01.HCSN.COMMON;
using MySql.Data.MySqlClient;
using System.Reflection;

namespace MISA.WEB01.HCSN.BaseControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasesController<T> : ControllerBase
    {


        #region Field

        private IBaseBL<T> _baseBL;

        #endregion

        #region Constructor

        public BasesController(IBaseBL<T> baseBL)
        {
            _baseBL = baseBL;
        }


        #endregion

        #region Method
        ///  <summary>
        /// thêm mới bản ghi
        /// </summary>
        /// <param name = "record" ></ param >
        /// < returns > trả về validate nếu lỗi validate, trả về HttpContext nếu bị trùng mã và exception, trả về data nếu thành công</returns>
        /// CreatedBy: HTTHOA(16/03/2023)
        [HttpPost]
        public virtual IActionResult InsertRecord([FromBody] T record)
        {

            try
            {
                var numberOfAffectedRows = _baseBL.InsertRecord(record);


                if (numberOfAffectedRows != Guid.Empty)
                {
                    return StatusCode(StatusCodes.Status201Created, numberOfAffectedRows);

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

        [HttpPut("{id}")]
        public IActionResult UpdateRecord([FromBody] T entity, [FromRoute] Guid id)
        {

            try
            {

                int numberOfAffectedRows = _baseBL.UpdateRecord(entity, id);

                if (numberOfAffectedRows > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, id);
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
        /// Lấy tất cả bản ghi
        /// </summary>
        /// <param name=""></param>
        /// <returns>về status500 hoặc status400 nếu lỗi ; return về status200 nếu thành công</returns>
        /// CreatedBy: HTTHOA(15/03/2023)
        [HttpGet]
        public IActionResult GetAllRecords()
        {
            try

            {
                var records = _baseBL.GetAllRecords();

                if (records != null)
                {
                    return StatusCode(StatusCodes.Status200OK, records);
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, ErrorResource.NotFound);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorResource.ServerException);
            }
        }

        /// <summary>
        /// Lấy bản ghi theo ID
        /// </summary>
        /// <param name="GetRecordID"></param>
        ///<returns>về status500 hoặc status400 nếu lỗi ; return về status200 nếu thành công </returns>
        /// CreatedBy: HTTHOA(16/03/2023)
        [HttpGet("{id}")]
        public IActionResult GetRecordID([FromRoute] Guid id)
        {
            try

            {
                var result = _baseBL.GetRecordByID(id);

                if (result != null)
                {
                    return StatusCode(StatusCodes.Status200OK, result);
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound);
                }
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ErrorResource.ServerException);
            }



        }

        /// <summary>
        /// Lấy mã  mới
        /// </summary>
        /// <param name=""></param>
        /// ><returns>về status500 hoặc status400 nếu lỗi ; return về status200 nếu thành công </returns>
        /// CreatedBy: HTTHOA(16/03/2023)
        [HttpGet("NewCode")]
        public IActionResult GetNewCode()
        {
            try

            {
                string newCode = _baseBL.GetNewCode();
                return StatusCode(StatusCodes.Status200OK, newCode);

            }
            catch (Exception ex)
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
        [HttpDelete("{id}")]
        public IActionResult DeleteEmployeeByID([FromRoute] Guid id)
        {
            try

            {
                int numberOfAffectedRows = _baseBL.DeleteRecordID(id);

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

        #endregion
    }
}
