using Misa.Web01.HCSN.BL;
using Misa.Web01.HCSN.COMMON;
using Misa.Web01.HCSN.COMMON.Resource;
using MISA.WEB01.HCSN.Common.entities;
using MISA.WEB01.HCSN.COMMON;
using MISA.WEB01.HCSN.COMMON.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web01.HCSN.BL
{
    public class BaseBL<T> : IBaseBL<T>
    {
        #region Field
        private IBaseDL<T> _baseDL;
        #endregion

        #region contructor

        public BaseBL(IBaseDL<T> baseDL)
        {
            _baseDL = baseDL;
        }
        #endregion
        #region method
        /// <summary>
        /// API thêm mới bản ghi
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA(16/03/2023)
        public virtual Guid InsertRecord(T record)
        {
            Validate(record);

            return _baseDL.InsertRecord(record);

        }


        /// sửa 1  bản ghi
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA(17/03/2023)
        public virtual int UpdateRecord(T entity, Guid id)
        {
            Validate(entity);
            return _baseDL.UpdateRecord(entity, id);
        }
        /// <summary>
        /// lấy tất cả bản  ghi
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA(15/03/2023)
        public IEnumerable<dynamic> GetAllRecords()
        {
            return _baseDL.GetAllRecords();
        }
        /// <summary>
        /// xóa bản ghi
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA(16/03/2023)
        public int DeleteRecordID(Guid id)
        {
            return _baseDL.DeleteRecordID(id);
        }
        /// <summary>
        /// lấy bản ghi theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA(16/03/2023)
        public T GetRecordByID(Guid id)
        {
            return (T)(_baseDL.GetRecordByID(id));
        }


        /// <summary>
        ///  lấy mã mới
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA(16/03/2023)
        public string GetNewCode()
        {
            return _baseDL.GetNewCode();
        }
        /// <summary>
        /// hàm validate
        /// </summary>
        /// <param name="record"></param>
        /// <exception cref="ExceptionService"></exception>
        public virtual void Validate(T record)
        {

            // lấy danh sách property
            string className = typeof(T).Name;
            var properties = typeof(T).GetProperties();

            // tạo dictionerry để lưu lại lỗi

            var errorsData = new Dictionary<string, object>();

            // duyệt để validate theo từng property
            foreach (var property in properties)
            {
                var errorMsg = new List<string>(); // tạo biến để lưu lại lỗi theo từng property

                var value = (property.GetValue(record, null) ?? string.Empty).ToString(); // lấy giá trị property

                var attrs = property.GetCustomAttributes(true); // lấy dánh sách attribute

                foreach (object attr in attrs)
                {
                    var attrName = attr.GetType().Name; // lấy tên attr 

                    // check null
                    if (attrName == "RequiredAttribute")
                    {
                        if (string.IsNullOrEmpty(value))
                        {
                            errorMsg.Add($"{MISAResource.GetResource(property.Name)} {ErrorResource.Required}");

                        }
                    }
                    // check độ dài
                    if (attrName == "StringLengthAttribute" && !string.IsNullOrEmpty(value))
                    {
                        int max = Int32.Parse(attr.GetType().GetProperty("MaximumLength").GetValue(attr, null).ToString());
                        int min = Int32.Parse(attr.GetType().GetProperty("MinimumLength").GetValue(attr, null).ToString());

                        if (value.Length > max || value.Length < min)
                        {
                            errorMsg.Add($"{MISAResource.GetResource(property.Name)} {ErrorResource.MaxLength} {min} đến {max}!");
                        }

                    }
                    // check giới hạn lớn nhất hoặc nhỏ nhất
                    if (attrName == "RangeAttribute" && !string.IsNullOrEmpty(value))
                    {
                        var max = float.Parse(attr.GetType().GetProperty("Maximum").GetValue(attr, null).ToString());
                        var min = float.Parse(attr.GetType().GetProperty("Minimum").GetValue(attr, null).ToString());
                        var valueToInt = float.Parse(value);

                        if (valueToInt > max || valueToInt <= min)
                        {
                            errorMsg.Add($"{MISAResource.GetResource(property.Name)} {ErrorResource.Range} {min} đến {max}!");
                        }

                    }
                    // check trùng mã 
                    if (attrName == "DuplicateAttribute" && !string.IsNullOrEmpty(value))
                    {

                        if (!_baseDL.CheckDuplicateCode(record))
                        {
                            errorMsg.Add($"{MISAResource.GetResource(property.Name)} {ErrorResource.DuplicateKey}");
                            errorsData.Add(property.Name,errorMsg);
                            throw new ErrorService(ErrorResource.ValidateFail, errorsData, MISAErrorCode.DuplicateCode);
                        }
                    }

                }


                if (errorMsg.Count > 0)
                {
                    errorsData.Add(property.Name, errorMsg);
                }
            }

            if (errorsData.Count > 0)
            {
                throw new ErrorService(ErrorResource.ValidateFail, errorsData, MISAErrorCode.Validate);
            }

        }
       
        #endregion
    }
}
