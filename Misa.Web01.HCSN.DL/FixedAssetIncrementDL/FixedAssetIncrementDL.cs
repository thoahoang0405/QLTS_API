using Dapper;
using Google.Protobuf.Collections;
using Misa.Web01.HCSN.BL.BaseBL;
using Misa.Web01.HCSN.COMMON.Entities;
using Misa.Web01.HCSN.COMMON.Entities.DTO;
using MISA.WEB01.HCSN.Common.entities;
using MISA.WEB01.HCSN.COMMON.Entities;
using MISA.WEB01.HCSN.COMMON.Utilities;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using Org.BouncyCastle.Asn1.Esf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web01.HCSN.DL.FixedAssetIncrementDL
{
    public class FixedAssetIncrementDL : BaseDL<FixedAssetIncrement>, IFixedAssetIncrementDL
    {
        #region Field
        readonly string _connectionDB = DatabaseContext.ConnectionString;

        #endregion
        /// <summary>
        /// lấy phân trong chứng tứ
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns>  </returns>
        /// CREATED BY: HTTHOA(25/04/2023)
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
        /// <summary>
        /// insert chứng từ
        /// </summary>
        /// <param name="paramsInsert"></param>
        /// <returns>thành công => 1 ; thất bại =>0</returns>
        /// CREATED BY: HTTHOA(25/04/2023)
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
                        if (numberOfAffetedRows > 0)
                        {
                            var voucher_id = paramsInsert.voucher_id;
                            int detail = InsertIncrementDetail(paramsInsert.listFixedAssetID, voucher_id, sqlConnection, trans);
                            int updatefixed = UpdateFixedAsset(paramsInsert.listFixedAssetID, 1, sqlConnection, trans);

                            if (updatefixed == 0 || detail == 0)
                            {
                                trans.Rollback();
                                return 0;


                            }
                            else
                            {
                                trans.Commit();

                                return 1;
                            }
                        }
                        else
                        {
                            trans.Rollback();
                            return 0;

                        }
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();

                        throw;
                    }

                }
            }

        }
        /// <summary>
        /// cập nhật tài sản
        /// </summary>
        /// <param name="listFixedAssetID"></param>
        /// <param name="active"></param>
        /// <param name="sqlConnection"></param>
        /// <param name="trans"></param>
        /// <returns>thành công => 1 ; thất bại =>0</returns>
        /// CREATED BY: HTTHOA(25/04/2023)
        public int UpdateFixedAsset(List<Guid> listFixedAssetID, int? active, MySqlConnection sqlConnection, MySqlTransaction trans)
        {
            string updateFixedAssetProcedure = $"Proc_fixed_asset_UpdateActive";
            DynamicParameters parameters = new DynamicParameters();
            var listIdToString = "";

            if (listFixedAssetID != null || listFixedAssetID.Count > 0)
            {
                listIdToString = $"('{string.Join("','", listFixedAssetID)}')";
            }

            parameters.Add("v_listId", listIdToString);
            parameters.Add("v_active", active);
            int result = 0;
            result = sqlConnection.Execute(updateFixedAssetProcedure, parameters, commandType: System.Data.CommandType.StoredProcedure, transaction: trans);
            if (result > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// thêm chi tiết chứng từ
        /// </summary>
        /// <param name="listFixedAssetID"></param>
        /// <param name="voucherID"></param>
        /// <param name="sqlConnection"></param>
        /// <param name="trans"></param>
        /// <returns>thành công => 1 ; thất bại =>0</returns>
        /// CREATED BY: HTTHOA(25/04/2023)
        public int InsertIncrementDetail(List<Guid> listFixedAssetID, Guid voucherID, MySqlConnection sqlConnection, MySqlTransaction trans)
        {
            string insertIncrementProcedure = "insert INTO fixed_asset_increment_detail(fixed_asset_id, voucher_id, modified_date) VALUES ";
            //DynamicParameters parameters = new DynamicParameters();
            //var insertIncrementDetail = "";
            foreach (var item in listFixedAssetID)
            {
                if (item == listFixedAssetID[listFixedAssetID.Count - 1])
                {
                    insertIncrementProcedure = insertIncrementProcedure + $" ('{item}', '{voucherID}', CURRENT_TIMESTAMP );";
                }
                else
                {
                    insertIncrementProcedure = insertIncrementProcedure + $" ('{item}', '{voucherID}',CURRENT_TIMESTAMP ), ";
                }
            }
            //parameters.Add("v_data", insertIncrementDetail);
            int result = 0;
            result = sqlConnection.Execute(insertIncrementProcedure, commandType: System.Data.CommandType.Text, transaction: trans);
            if (result > 0)
            {
                return 1;
            }
            else { return 0; }

        }
        /// <summary>
        /// sửa chứng từ
        /// </summary>
        /// <param name="increment"></param>
        /// <param name="incrementID"></param>
        /// <returns>thành công => 1 ; thất bại =>0</returns>
        /// CREATED BY: HTTHOA(25/04/2023)
        public int UpdateIncrement(FixedAssetIncrement increment, Guid incrementID)
        {
            string tableName = EntityUtilities.GetTableName<FixedAssetIncrement>();
            string storeProcedureName = $"Proc_{tableName}_Update";
            var properties = typeof(FixedAssetIncrement).GetProperties();
            var parameters = new DynamicParameters();
            foreach (var property in properties)
            {
                string propertyName = $"@{property.Name}";
                var propertyValue = property.GetValue(increment);
                parameters.Add(propertyName, propertyValue);
            }
            using (var sqlConnection = new MySqlConnection(_connectionDB))
            {
                sqlConnection.Open();
                using (MySqlTransaction trans = sqlConnection.BeginTransaction())
                {
                    int numberOfAffectedRows = 0;
                    numberOfAffectedRows = sqlConnection.Execute(storeProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure, transaction: trans);
                    if (numberOfAffectedRows > 0)
                    {

                        if (increment.listFixedAssetID.Count > 0)
                        {
                            int detail = InsertIncrementDetail(increment.listFixedAssetID, increment.voucher_id, sqlConnection, trans);
                            int updatefixed = UpdateFixedAsset(increment.listFixedAssetID, 1, sqlConnection, trans);
                            if (increment.listFixedAssetIdDelete.Count > 0)
                            {
                                int deleteDetail = DeleteMultipleIncrementDetail(increment.listFixedAssetIdDelete, sqlConnection, trans);

                                int updatefixedDelete = UpdateFixedAsset(increment.listFixedAssetIdDelete, 0, sqlConnection, trans);
                                if (updatefixed == 0 || deleteDetail == 0 || updatefixedDelete == 0 || detail == 0)
                                {
                                    trans.Rollback();
                                    return 0;
                                }
                                else
                                {
                                    trans.Commit();

                                    return 1;
                                }
                            }
                            if (updatefixed == 0 || detail == 0)
                            {
                                trans.Rollback();
                                return 0;
                            }
                            else
                            {
                                trans.Commit();

                                return 1;
                            }
                           
                        }
                        else
                        {
                            if (increment.listFixedAssetIdDelete.Count > 0)
                            {
                                int deleteDetail = DeleteMultipleIncrementDetail(increment.listFixedAssetIdDelete, sqlConnection, trans);

                                int updatefixedDelete = UpdateFixedAsset(increment.listFixedAssetIdDelete, 0, sqlConnection, trans);
                                if ( deleteDetail == 0 || updatefixedDelete == 0 )
                                {
                                    trans.Rollback();
                                    return 0;
                                }
                                else
                                {
                                    trans.Commit();

                                    return 1;
                                }
                            }
                            else
                            { 
                            trans.Commit();
                            return numberOfAffectedRows;
                            }
                        }
                       
                    }
                    else
                    {
                        trans.Rollback();
                        return 0;
                    }

                }
            }
        }
        /// <summary>
        /// xóa chứng từ
        /// </summary>
        /// <param name="listFixedAssetID"></param>
        /// <param name="id"></param>
        /// <returns> thành công => 1 ; thất bại =>0</returns>
        /// CREATED BY: HTTHOA(25/04/2023)
        public int DeleteIncrementID(List<Guid>? listFixedAssetID, Guid id)
        {
            var idName = typeof(FixedAssetIncrement).GetProperties().First().Name;
            string className = EntityUtilities.GetTableName<FixedAssetIncrement>();
            string sqlCommand = $"DELETE FROM {className} Where {idName}='{id}'";
            var parameters = new DynamicParameters();


            using (var sqlConnection = new MySqlConnection(_connectionDB))
            {
                sqlConnection.Open();
                using (MySqlTransaction trans = sqlConnection.BeginTransaction())
                {
                    // Thực hiện gọi vào DB để chạy câu lệnh DELETE với tham số đầu vào ở trên
                    int numberOfAffectedRows = sqlConnection.Execute(sqlCommand, parameters, transaction: trans);

                    if (numberOfAffectedRows > 0)
                    {
                        UpdateFixedAsset(listFixedAssetID, 0, sqlConnection, trans);
                        trans.Commit();
                        return numberOfAffectedRows;


                    }
                    else
                    {
                        trans.Rollback();
                        return 0;

                    }

                }
            }

        }
        public int DeleteMultipleIncrement(DeleteMultipleIncrement listId)
        {
            string procedureNameCommand = "Proc_fixed_asset_increment_DeleteMultiple";

            // tạo param
            var parameters = new DynamicParameters();

            var listIdToString = "";

            if (listId.listIncrementDeleted == null || listId.listIncrementDeleted.Count == 0)
            {
                return 0;
            }
            listIdToString = $"('{string.Join("','", listId.listIncrementDeleted)}')";

            parameters.Add("@v_ListId", listIdToString);


            // chạy câu lệnh 

            using (var sqlConnection = new MySqlConnection(_connectionDB))
            {
                sqlConnection.Open();
                using (MySqlTransaction trans = sqlConnection.BeginTransaction())
                {

                    int result = sqlConnection.Execute(procedureNameCommand, parameters, commandType: System.Data.CommandType.StoredProcedure, transaction:trans);

                    if (result == listId.listIncrementDeleted.Count)
                    {
                        UpdateFixedAsset(listId.listFixedAssetUpdate, 0, sqlConnection, trans);
                        trans.Commit();
                        return result;
                    }
                    else
                    {
                        trans.Rollback();
                        return 0;
                    }

                }
            }
        }
        public int DeleteMultipleIncrementDetail(List<Guid> listId, MySqlConnection sqlConnection, MySqlTransaction trans)
        {
            
            // lấy procedure name
            string procedureNameCommand = "Proc_fixed_asset_increment_detail_DeleteMultiple";


            // tạo param
            var parameters = new DynamicParameters();

            var listIdToString = "";

            if (listId == null || listId.Count == 0)
            {
                return 0;
            }
            listIdToString = $"('{string.Join("','", listId)}')";

            parameters.Add("@v_ListId", listIdToString);
            int result = 0;
            result = sqlConnection.Execute(procedureNameCommand, parameters, commandType: System.Data.CommandType.StoredProcedure, transaction: trans);
            if (result > 0)
            {
                return 1;
            }
            else { return 0; }

        }
        public override bool CheckDuplicateCode(FixedAssetIncrement record)
        {
            return base.CheckDuplicateCode(record);
        }
    }
}
