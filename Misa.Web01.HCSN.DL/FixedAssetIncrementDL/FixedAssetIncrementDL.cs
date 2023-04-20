using Dapper;
using Google.Protobuf.Collections;
using Misa.Web01.HCSN.BL.BaseBL;
using Misa.Web01.HCSN.COMMON.Entities;
using MISA.WEB01.HCSN.Common.entities;
using MISA.WEB01.HCSN.COMMON.Entities;
using MISA.WEB01.HCSN.COMMON.Utilities;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web01.HCSN.DL.FixedAssetIncrementDL
{
    public class FixedAssetIncrementDL:BaseDL<FixedAssetIncrement>, IFixedAssetIncrementDL
    {
        #region Field
        readonly string _connectionDB = DatabaseContext.ConnectionString;

        #endregion
        public PagingData<FixedAssetIncrement> FilterFixedAssetIncrement(
           string? keyword,
           int? pageSize,
           int? pageNumber
           )
        {

            string storedProcedureName = "Proc_fixed_asset_increment_GetPaging";

            //var orConditions = new List<string>();
           

            string whereClause = "";
            if (keyword != null)
            {
                whereClause = $"voucher_code LIKE '%{keyword}%' OR description LIKE '%{keyword}%'";
                //orConditions.Add($"voucher_code LIKE '%{keyword}%'");
                //orConditions.Add($"description LIKE '%{keyword}%'");

            }
            
           
            else
            {
                whereClause = "";
            }

            var parameters = new DynamicParameters();
            parameters.Add("v_Sort", "modified_date DESC");

            parameters.Add("v_Limit", pageSize);
            parameters.Add("v_Offset", (pageNumber - 1) * pageSize);
            parameters.Add("v_Where", whereClause);
            using (var sqlConnection = new MySqlConnection(_connectionDB))
            {
                // Thực hiện gọi vào DB để chạy stored procedure với tham số đầu vào ở trên
                var multipleResults = sqlConnection.QueryMultiple(storedProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                if (multipleResults != null)
                {
                    var result = multipleResults.Read<FixedAssetIncrement>().ToList();
                    var TotalRecords = multipleResults.Read<long>().Single();               
                    var TotalCosts = multipleResults.Read<long>().Single();
                  


                    int TotalPagesAll = 1;

                    if (TotalRecords >= 0 && pageSize > 0)
                    {
                        TotalPagesAll = (int)(decimal)(TotalRecords / pageSize);
                        if (TotalRecords % pageSize != 0)
                        {
                            TotalPagesAll = TotalPagesAll + 1;
                        }
                        if (TotalRecords < pageSize)
                        {
                            pageNumber = 1;
                            TotalPagesAll = 1;
                        }
                    }


                    return new PagingData<FixedAssetIncrement>()
                    {
                        Data = result,
                        TotalRecords = TotalRecords,
                        TotalCost = TotalCosts,
                        TotalPages = TotalPagesAll

                    };
                }
            }
            return null;

        }

        public int InsertIncrement(FixedAssetIncrement paramsInsert)
        {
            var newId = Guid.NewGuid();
            var primary = typeof(FixedAssetIncrement).GetProperties().FirstOrDefault(prop => prop.GetCustomAttributes(typeof(KeyAttribute), true).Count() > 0);
            if (primary != null)
            {
                primary.SetValue(paramsInsert, newId);
            }
            // tên proc dùng để truy vấn
            string tableName = EntityUtilities.GetTableName<FixedAssetIncrement>();
            string insertRecordProcedureName = $"Proc_{tableName}_Insert";
            // chuẩn bị tham số đầu vào 
            var properties = typeof(FixedAssetIncrement).GetProperties();
            var parameters = new DynamicParameters();
            foreach (var property in properties)
            {
                var value = property.GetValue(paramsInsert); // lấy giá trị của property

                var propertyName = property.Name; // lấy tên của property

                parameters.Add($"@{propertyName}", value);
            }
            
            int numberOfAffetedRows = 0;
            using (var sqlConnection = new MySqlConnection(_connectionDB))

            {
                sqlConnection.Open();
                using (MySqlTransaction trans = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        numberOfAffetedRows = sqlConnection.Execute(insertRecordProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure, transaction: trans);
                        var voucher_id = paramsInsert.voucher_id;
                        InsertIncrementDetail(paramsInsert.listFixedAssetID, voucher_id, sqlConnection,trans);
                        UpdateFixedAsset(paramsInsert.listFixedAssetID, sqlConnection,trans);
                      
                        trans.Commit();

                        if (numberOfAffetedRows > 0)
                        {
                            return 1;
                        }


                        return 0;
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();

                        throw;
                    }
                }

            }
        }
        public void UpdateFixedAsset(List<Guid> listFixedAssetID, MySqlConnection sqlConnection, MySqlTransaction trans)
        {
            string updateFixedAssetProcedure = $"Proc_fixed_asset_UpdateActive";
            DynamicParameters parameters = new DynamicParameters();
            var listIdToString = "";

            if (listFixedAssetID != null || listFixedAssetID.Count > 0)
            {
                listIdToString = $"('{string.Join("','", listFixedAssetID)}')";
            }
           
            parameters.Add("v_listId", listIdToString);
            parameters.Add("v_active", 1);
            
            var result = sqlConnection.Execute(updateFixedAssetProcedure, parameters, commandType: System.Data.CommandType.StoredProcedure,transaction: trans);
        }
        public void InsertIncrementDetail(List<Guid> listFixedAssetID, Guid voucherID, MySqlConnection sqlConnection, MySqlTransaction trans)
        {
            string insertIncrementDetail = "INSERT INTO fixed_asset_increment_detail (fixed_asset_id, voucher_id) VALUES ";
            foreach (var item in listFixedAssetID)
            {
                if (item == listFixedAssetID[listFixedAssetID.Count - 1])
                {
                    insertIncrementDetail = insertIncrementDetail + $" ('{item}', '{voucherID}' );";
                }
                else
                {
                    insertIncrementDetail = insertIncrementDetail + $" ('{item}', '{voucherID}' ), ";
                }
            }
            var result = sqlConnection.Execute(insertIncrementDetail, commandType: System.Data.CommandType.Text, transaction:trans);

        }
        public override bool CheckDuplicateCode(FixedAssetIncrement record)
        {
            return base.CheckDuplicateCode(record);
        }
    }
}
