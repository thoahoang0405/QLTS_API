
using Misa.Web01.HCSN.BL;
using Misa.Web01.HCSN.COMMON.Entities;
using Misa.Web01.HCSN.COMMON.Entities.DTO;
using MISA.WEB01.HCSN.COMMON.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web01.HCSN.DL.FixedAssetIncrementDL
{
    public interface IFixedAssetIncrementDL:IBaseDL<FixedAssetIncrement>
    {
        /// <summary>
        /// phân trang chứng từ
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA((9/5/2023)
        public PagingData<FixedAssetIncrement> FilterFixedAssetIncrement(
        string? keyword,
        int? pageSize,
        int? pageNumber
        );
        /// <summary>
        /// thêm chứng  từ
        /// </summary>
        /// <param name="paramsInsert"></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA((9/5/2023)
        public int InsertIncrement(FixedAssetIncrement paramsInsert);
        /// <summary>
        /// cập nhật chứng từ
        /// </summary>
        /// <param name="increment"></param>
        /// <param name="incrementID"></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA((9/5/2023)
        public int UpdateIncrement(FixedAssetIncrement increment, Guid incrementID);
        /// <summary>
        /// xóa 1 chứng từ
        /// </summary>
        /// <param name="listFixedAssetID"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA((9/5/2023)
        public int DeleteIncrementID(List<Guid>? listFixedAssetID, Guid id);
        /// <summary>
        /// xóa nhiều chứng từ
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA((9/5/2023)
        public int DeleteMultipleIncrement(DeleteMultipleIncrement listId);
    }
}
