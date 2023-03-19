using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Misa.Web01.HCSN.BL;
using Misa.Web01.HCSN.COMMON.Entities;
using Misa.Web01.HCSN.DL;
using MISA.WEB01.HCSN.BaseControllers;

namespace MISA.WEB01.HCSN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FixedAssetCategoriesController : BasesController<FixedAssetCategory>
    {
        private IFixedAssetCategoryBL _fixedAssetCategoryBL;
        public FixedAssetCategoriesController(IFixedAssetCategoryBL fixedAssetCategoryBL) : base(fixedAssetCategoryBL)
        {
            _fixedAssetCategoryBL = fixedAssetCategoryBL;
        }
    }
}
