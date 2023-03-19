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
       
        public PagingData<FixedAsset> FilterFixedAsset(string? keyword, int? pageSize, int? pageNumber, string? departmentName, string? fixedAssetCategoryName);
        public int DeleteMultiple(List<Guid> listId);
    }
}
