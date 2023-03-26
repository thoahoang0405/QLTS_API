using Misa.Web01.HCSN.COMMON.Entities.DTO;
using Misa.Web01.HCSN.DL;
using MISA.WEB01.HCSN.Common.entities;
using MISA.WEB01.HCSN.COMMON.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web01.HCSN.BL
{
    public class FixedAssetBL:BaseBL<FixedAsset>, IFixedAssetBL
    {
        private IFixedAssetDL _fixedAssetDL;
        public FixedAssetBL(IFixedAssetDL fixedAssetDL) : base(fixedAssetDL)
        {
            _fixedAssetDL = fixedAssetDL;
        }

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
        public PagingData<FixedAsset> FilterFixedAsset(string? keyword, int? pageSize, Guid? departmentID, Guid? fixedAssetCategoryID, int? pageNumber)
        {
            return _fixedAssetDL.FilterFixedAsset(keyword, pageSize, departmentID, fixedAssetCategoryID, pageNumber);
           
        }
        /// <summary>
        /// xóa nhiều bản ghi
        /// </summary>
        /// <param name="listId"></param>
        /// <returns></returns>
        public int DeleteMultiple(List<Guid> listId)
            {
                return _fixedAssetDL.DeleteMultiple(listId);
            }
    }
}
