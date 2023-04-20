using Dapper;
using Misa.Web01.HCSN.BL.BaseBL;
using Misa.Web01.HCSN.COMMON.Entities;
using MISA.WEB01.HCSN.Common.entities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web01.HCSN.DL.FixedAssetIncrementDetailDL
{
    public class FixedAssetIncrementDetailDL:BaseDL<FixedAssetIncrementDetail>, IFixedAssetIncrementDetailDL
    {
        #region Field
        readonly string _connectionDB = DatabaseContext.ConnectionString;

        #endregion
        public IEnumerable<dynamic> SelectByVoucher(Guid listId)
        {
            // lấy procedure name
            string procedureNameCommand = "Proc_fixed_asset_increment_GetByVoucher";

            DynamicParameters parameters = new DynamicParameters();
               
            parameters.Add("@v_id", listId);


            // chạy câu lệnh 

            using (var sqlConnection = new MySqlConnection(_connectionDB))
            {
                var result = sqlConnection.Query(procedureNameCommand, parameters,  commandType: System.Data.CommandType.StoredProcedure);
                return result;

            }
        }
        


    }
}
