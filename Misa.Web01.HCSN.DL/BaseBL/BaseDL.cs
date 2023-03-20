using Dapper;
using Google.Protobuf.WellKnownTypes;
using Misa.Web01.HCSN.DL;
using MISA.WEB01.HCSN.COMMON.Utilities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web01.HCSN.BL.BaseBL
{
    public class BaseDL<T>: IBaseDL<T>
    {
        #region Field
        readonly string _connectionDB = "Server= localhost; Port=3306; Database=misa.hcsn.web01; User Id = root;Password=123456 ";
        ///<T> reneric
        #endregion

        #region method
        /// <summary>
        /// Thêm bản ghi
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA(16/03/2023)
        public virtual Guid InsertRecord(T record)
        {
            var newId = Guid.NewGuid();
            var primary=typeof(T).GetProperties().FirstOrDefault(prop => prop.GetCustomAttributes(typeof(KeyAttribute), true).Count() > 0);
           if(primary != null)
            {
                primary.SetValue(record, newId);
            }

            // tên proc dùng để truy vấn
                 string tableName = EntityUtilities.GetTableName<T>();
                string insertRecordProcedureName = $"Proc_{tableName}_Insert";



            // chuẩn bị tham số đầu vào 
            var properties = typeof(T).GetProperties();
                var parameters = new DynamicParameters();

                foreach (var property in properties)
                {
                    var value = property.GetValue(record); // lấy giá trị của property

                    var propertyName = property.Name; // lấy tên của property

                    parameters.Add($"@{propertyName}", value);
                }

                // thực hiện gọi vào DB
                int numberOfAffetedRows = 0;
                using (var sqlConnection = new MySqlConnection(_connectionDB))
                {
                    numberOfAffetedRows = sqlConnection.Execute(insertRecordProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                    if (numberOfAffetedRows > 0)
                    {
                        return newId;
                    }
                    return Guid.Empty;
               
            } 
           
        }

        

        /// <summary>
        /// Sửa bản ghi
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA(16/03/2023)
        public int UpdateRecord(T entity, Guid id)
        { 
            string tableName = EntityUtilities.GetTableName<T>();
            string storeProcedureName = $"Proc_{tableName}_Update";
            var properties = typeof(T).GetProperties();
            var parameters = new DynamicParameters();


            foreach (var property in properties)
            {
                string propertyName = $"@{property.Name}";
                var propertyValue = property.GetValue(entity);
                parameters.Add(propertyName, propertyValue);
            }
            using (var sqlConnection = new MySqlConnection(_connectionDB))
            {
                int numberOfAffectedRows = 0;
           

             numberOfAffectedRows = sqlConnection.Execute(storeProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
                
             return numberOfAffectedRows;
               
                

            }
        }

            /// <summary>
            /// Lấy tất cả bản ghi
            /// </summary>
            /// <param name=""></param>
            /// <returns></returns>
            /// CreatedBy: HTTHOA(16/03/2023)
            public IEnumerable<dynamic> GetAllRecords()
        {
            using (var sqlConnection = new MySqlConnection(_connectionDB))
            {
                string className = EntityUtilities.GetTableName<T>();
                var getAllRecords = $"SELECT * FROM {className}";
                var records = sqlConnection.Query(getAllRecords);

                return records;
            }


        }
        /// <summary>
        /// API xóa 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA(16/03/2023)

        public int DeleteRecordID(Guid id)
        {

            using (var sqlConnection = new MySqlConnection(_connectionDB))
            {

                var idName = typeof(T).GetProperties().First().Name;
                string className = EntityUtilities.GetTableName<T>();
                string sqlCommand = $"DELETE FROM {className} Where {idName}='{id}'";
                var parameters = new DynamicParameters();


                // Thực hiện gọi vào DB để chạy câu lệnh DELETE với tham số đầu vào ở trên
                int numberOfAffectedRows = sqlConnection.Execute(sqlCommand, parameters);

                return numberOfAffectedRows;
            }


        }
        /// <summary>
        /// Lấy theo ID
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA(16/03/2023)

        public T GetRecordByID(Guid id)
        {
            using (var sqlConnection = new MySqlConnection(_connectionDB))
            {
                var idName = typeof(T).GetProperties().First().Name;
                string className = EntityUtilities.GetTableName<T>();
                string sqlCommand = $"SELECT * FROM {className} Where {idName}='{id}'";
                var parameters = new DynamicParameters();
                return sqlConnection.QueryFirstOrDefault<T>(sqlCommand, parameters);


            }
        }

        /// <summary>
        /// Lấy mã mới
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        /// CreatedBy: HTTHOA(16/0333/2023)

        public string GetNewCode()
        {
            using (var sqlConnection = new MySqlConnection(_connectionDB))
            {
                string className = EntityUtilities.GetTableName<T>();
                string storedProcedureName = $"Proc_{className}_GetMaxCode";
                string maxCode = sqlConnection.QueryFirstOrDefault<string>(storedProcedureName, commandType: System.Data.CommandType.StoredProcedure);

                // Xử lý sinh mã mới tự động tăng
                // Tách chuỗi mã lớn nhất trong hệ thống để lấy phần số và chữ riêng
                string newCode = maxCode.Substring(0,2) + (Int64.Parse(maxCode.Substring(2)) + 1).ToString();

                // Trả về dữ liệu cho client
                return newCode;
            }
        }
        public bool CheckDuplicateCode(T record)
        {
           
            // lấy mã record
            var duplicateProperty = typeof(T).GetProperties().FirstOrDefault(prop => prop.GetCustomAttributes(typeof(DuplicateAttribute), true).Count() > 0);
            var code = duplicateProperty?.GetValue(record);

            //lấy tên procedure
            string tableName = EntityUtilities.GetTableName<T>();
            string procedureName = $"Proc_{tableName}_DuplicateCode";
           
            //tạo các param
            string className = typeof(T).Name;
            var parameters = new DynamicParameters();
            parameters.Add($"v_Code", code);
          

            using (var mySqlConnection = new MySqlConnection(_connectionDB))
            {
                // thực hiện chạy câu lệnh 
                var result = mySqlConnection.QueryFirstOrDefault<T>(procedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                if (result == null)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
