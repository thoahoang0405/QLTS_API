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
      
        public PagingData<FixedAsset> FilterFixedAsset(string? keyword, int? pageSize, string? departmentID, string? fixedAssetCategoryID, int? pageNumber)
        {
            return _fixedAssetDL.FilterFixedAsset(keyword, pageSize, departmentID, fixedAssetCategoryID, pageNumber);
           
        } 
        public int DeleteMultiple(List<Guid> listId)
            {
                return _fixedAssetDL.DeleteMultiple(listId);
            }
    }
}
