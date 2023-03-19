using Misa.Web01.HCSN.BL;
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
        private IBaseDL<T>  _baseDL;
        #endregion

        #region contructor
        public BaseBL(IBaseDL<T> baseDL)
        {
            _baseDL = baseDL;
        }
        #endregion
        protected List<string> listErrorMsgs = new List<string>();
        protected int devCode;
        /// <summary>
        /// API thêm mới bản ghi
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA(16/08/2022)
        public virtual Guid InsertRecord(T record)
        {

            return _baseDL.InsertRecord(record);

        }
        protected virtual bool ValidateCustom(T entity)
        {
            return true;
        }
        /// sửa 1  bản ghi
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA(20/08/2022)
        public int UpdateRecord(T entity, Guid id)
        {
            return _baseDL.UpdateRecord(entity, id);
        }
        /// <summary>
        /// lấy tất cả bản  ghi
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA(25/08/2022)
        public IEnumerable<dynamic> GetAllRecords()
        {
            return _baseDL.GetAllRecords();
        }
        /// <summary>
        /// xóa bản ghi
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA(16/08/2022)
        public int DeleteRecordID(Guid id)
        {
            return _baseDL.DeleteRecordID(id);
        }
        /// <summary>
        /// lấy bản ghi theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA(16/08/2022)
        public T GetRecordByID(Guid id)
        {
            return (T)(_baseDL.GetRecordByID(id));
        }


        /// <summary>
        ///  lấy mã nhân viên mới
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA(16/08/2022)
        public string GetNewCode()
        {
            return _baseDL.GetNewCode();
        }
    }
}
