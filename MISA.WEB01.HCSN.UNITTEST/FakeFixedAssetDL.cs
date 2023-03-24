using Misa.Web01.HCSN.BL;
using Misa.Web01.HCSN.DL;
using MISA.WEB01.HCSN.Common.entities;
using MISA.WEB01.HCSN.COMMON.Entities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WEB01.HCSN.UNITTEST
{
    internal class FakeFixedAssetDL : IFixedAssetBL
    {
        public FakeFixedAssetDL()
        {
        }

        public bool CheckDuplicateCode(FixedAsset record)
        {
            throw new NotImplementedException();
        }

        public int DeleteMultiple(List<Guid> listId)
        {
            throw new NotImplementedException();
        }

        public int DeleteRecordID(Guid id)
        {
            throw new NotImplementedException();
        }

        public PagingData<FixedAsset> FilterFixedAsset(string? keyword, int? pageSize, int? pageNumber, string? departmentName, string? fixedAssetCategoryName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<dynamic> GetAllRecords()
        {
            throw new NotImplementedException();
        }

        public string GetNewCode()
        {
            throw new NotImplementedException();
        }

      
        public FixedAsset GetRecordByID(Guid id)
        {
            throw new NotImplementedException();
        }

        public Guid InsertRecord(FixedAsset record)
        {
            return Guid.NewGuid();
        }

      
        public int UpdateRecord(FixedAsset entity, Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
