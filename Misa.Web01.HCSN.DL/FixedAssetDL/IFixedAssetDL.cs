using Misa.Web01.HCSN.BL;
using Misa.Web01.HCSN.COMMON.Entities.DTO;
using MISA.WEB01.HCSN.Common.entities;
using MISA.WEB01.HCSN.COMMON.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web01.HCSN.DL
{
    public interface IFixedAssetDL:IBaseDL<FixedAsset>
    {

        #region Method
        /// <summary>
        /// Hàm lấy dữ liệu phân trang 
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageSize"></param>
        /// <param name="departmentID"></param>
        /// <param name="fixedAssetCategoryID"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA(10/03/2022)
        public PagingData<FixedAsset> FilterFixedAsset(string? keyword,
            int? pageSize,
            Guid? departmentID,
            Guid? fixedAssetCategoryID,
            int? pageNumber,
            int? active,
            List<Guid> listId);
        /// <summary>
        /// xóa nhiều bản ghi
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA(30/03/2023)
        public int DeleteMultiple(List<Guid> listId);
        /// <summary>
        /// Hàm lấy dữ liệu xuaát file excel
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageSize"></param>
        /// <param name="departmentID"></param>
        /// <param name="fixedAssetCategoryID"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA(31/03/2022)
        public PagingData<FixedAsset> FilterFixedAssetExcel(
            string? keyword,

            Guid? departmentID,
            Guid? fixedAssetCategoryID

            );
        public int UpdateFixedAsset(List<Guid> listId, int active);
        #endregion
    }
}
