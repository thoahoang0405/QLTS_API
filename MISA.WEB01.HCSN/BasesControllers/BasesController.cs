
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Misa.Web01.HCSN.BL;
using Misa.Web01.HCSN.COMMON;
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
        ///  <summary>
        /// thêm mới bản ghi
        /// </summary>
        /// <param name = "record" ></ param >
        /// < returns > trả về validate nếu lỗi validate, trả về HttpContext nếu bị trùng mã và exception, trả về data nếu thành công</returns>
        /// CreatedBy: HTTHOA(16/08/2022)
        [HttpPost]
        public virtual IActionResult InsertRecord([FromBody] T record)
        {

            try
            {
                ///var validate = "lỗi";
                //if (validate != null)
                //{
                //    return StatusCode(StatusCodes.Status400BadRequest, validate);
                //}

                var numberOfAffectedRows = _baseBL.InsertRecord(record);


            if (numberOfAffectedRows != Guid.Empty)
            {
                return StatusCode(StatusCodes.Status201Created, record);

            }
            else
            {

                return StatusCode(StatusCodes.Status400BadRequest, Resource.Error_BadRequest);
            }
            }
            

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
        /// <summary>
        /// sửa theo id
        /// </summary>
        /// <param name="employeeID"></param>
        ///<returns> status400 nếu lỗi ; return về status200 nếu thành công </returns>
        /// CreatedBy: HTTHOA(16/08/2022)

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
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, Resource.Error_BadRequest);
                }
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
            

        }


        /// <summary>
        /// Lấy tất cả bản ghi
        /// </summary>
        /// <param name=""></param>
        /// <returns>về status500 hoặc status400 nếu lỗi ; return về status200 nếu thành công</returns>
        /// CreatedBy: HTTHOA(30/08/2022)

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
                    return StatusCode(StatusCodes.Status404NotFound);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "lỗi");
            }
        }


        /// <summary>
        /// Lấy nhân viên theo ID
        /// </summary>
        /// <param name="employeeID"></param>
        ///<returns>về status500 hoặc status400 nếu lỗi ; return về status200 nếu thành công </returns>
        /// CreatedBy: HTTHOA(16/08/2022)

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

                return StatusCode(StatusCodes.Status500InternalServerError, "lỗi");
            }



        }
        /// <summary>
        /// Lấy mã nhân viên mới
        /// </summary>
        /// <param name=""></param>
        /// ><returns>về status500 hoặc status400 nếu lỗi ; return về status200 nếu thành công </returns>
        /// CreatedBy: HTTHOA(16/08/2022)

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

                return StatusCode(StatusCodes.Status500InternalServerError, "lỗi");
            }
        }
        /// <summary>
        /// API xóa nhân viên
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns>về status500 hoặc status400 nếu lỗi ; return về status200 nếu thành công</returns>
        /// CreatedBy: HTTHOA(16/08/2022)

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
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "lỗi");
            }
        }

    }
}
