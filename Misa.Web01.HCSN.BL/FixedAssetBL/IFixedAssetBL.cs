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
    public interface IFixedAssetBL : IBaseBL<FixedAsset>
    {
        public PagingData<FixedAsset> FilterFixedAsset(string? keyword, int? pageSize, string? departmentID, string? fixedAssetCategoryID, int? pageNumber);
        
        public int DeleteMultiple(List<Guid> listId);

    }
}
