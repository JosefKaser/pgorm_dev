using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using System.Data.Common;
using System.Data;

namespace MY_NAMESPACE
{
    #region ObjectRelationMapper
    public delegate T ObjectRelationMapper<T>(IDataReader reader);
    #endregion

    #region DataAccess
    public class DataAccess
    {
        #region Props
        public static NpgsqlConnection Connection { get; set; }
        private static bool IsDatabaseInitialized { get; set; }
        #endregion

        #region CheckDatabase
        protected static void CheckDatabase()
        {
            if (!IsDatabaseInitialized)
            {
                throw new Exception("Database system is not yet initialized");
            }
        }
        #endregion

        #region InitializeDatabase
        public static void InitializeDatabase(string host, string database, string username, string password)
        {
            string conn = string.Format("host={0};database={1};user={2};password={3};",
                host, database, username, password);

            Connection = new NpgsqlConnection(conn);

            try
            {
                Connection.Open();
                IsDatabaseInitialized = true;
            }
            catch (Exception e)
            {
                IsDatabaseInitialized = false;
                throw e;
            }
        }
        #endregion

        #region Transaction
        public static NpgsqlTransaction BeginTransaction()
        {
            CheckDatabase();
            return Connection.BeginTransaction();
        }

        public static void CreateSavePoint(string savespoint, NpgsqlTransaction transation)
        {
            NpgsqlCommand command = Connection.CreateCommand();
            command.CommandText = string.Format("SAVEPOINT {0}", savespoint);
            command.ExecuteNonQuery();
        }

        public static void RollbackToSavePoint(string savespoint, NpgsqlTransaction transation)
        {
            NpgsqlCommand command = Connection.CreateCommand();
            command.CommandText = string.Format("ROLLBACK TO SAVEPOINT  {0}", savespoint);
            command.ExecuteNonQuery();
        }

        public static void ReleaseSavePoint(string savespoint, NpgsqlTransaction transation)
        {
            NpgsqlCommand command = Connection.CreateCommand();
            command.CommandText = string.Format("RELEASE SAVEPOINT {0}", savespoint);
            command.ExecuteNonQuery();
        }

        public static void RollbackTransaction(NpgsqlTransaction transaction)
        {
            transaction.Rollback();
        }

        public static void CommitTransaction(NpgsqlTransaction transaction)
        {
            transaction.Commit();
        }
        #endregion

        #region ExecuteObjectQuery
        public static List<T> ExecuteObjectQuery<T>(string sqlStatement, ObjectRelationMapper<T> ormFunction, params DbParameter[] parameters)
        {
            List<T> result = new List<T>();

            NpgsqlCommand command = new NpgsqlCommand(sqlStatement, DataAccess.Connection);
            SetupParameters(command, parameters);

            NpgsqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    result.Add(ormFunction(reader));
                }
                return result;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region ExecuteNoneQuery
        public static void ExecuteNoneQuery(string sqlStatement, params DbParameter[] parameters)
        {
            NpgsqlCommand command = new NpgsqlCommand(sqlStatement, DataAccess.Connection);
            SetupParameters(command, parameters);
            command.ExecuteNonQuery();
        } 
        #endregion

        #region ExecuteScalarQuery
        public static T ExecuteScalarQuery<T>(string sqlStatement, params DbParameter[] parameters)
        {
            return (T)ExecuteScalarQuery(sqlStatement, parameters);
        }

        public static object ExecuteScalarQuery(string sqlStatement, params DbParameter[] parameters)
        {
            NpgsqlCommand command = new NpgsqlCommand(sqlStatement, DataAccess.Connection);
            SetupParameters(command, parameters);
            return command.ExecuteScalar();
        } 
        #endregion

        #region SetupParameters
        private static void SetupParameters(DbCommand command, DbParameter[] parameters)
        {
            foreach (DbParameter param in parameters)
                command.Parameters.Add(param);
        } 
        #endregion

        #region Convert
        public static T Convert<T>(object data, object default_if_null)
        {
            if (data == null || data == DBNull.Value)
                return (T)default_if_null;
            else
                return (T)data;
        } 
        #endregion

        #region NewParameter
        public static DbParameter NewParameter(string name, object value)
        {
            return new NpgsqlParameter(name, value);
        } 
        #endregion

    } 
    #endregion
}
