using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using System.Data.Common;
using System.Data;

namespace MY_NAMESPACE.Core
{
    #region ObjectRelationMapper
    public delegate T ObjectRelationMapper<T>(IDataReader reader);
    #endregion

    #region DataAccess
    public class DataAccess
    {
        #region Props
        public static NpgsqlConnection Connection { get; set; }
        public static bool ThrowMapperException { get; set; }
        private static bool IsDatabaseInitialized { get; set; }
        #endregion

        #region DataAccess
        static DataAccess()
        {
            ThrowMapperException = true;
        } 
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

        #region ExecuteSingleObjectQuery
        public static T ExecuteSingleObjectQuery<T>(string sqlStatement, ObjectRelationMapper<T> ormFunction, NpgsqlTransaction trans, params DbParameter[] parameters)
        {
            List<T> result = ExecuteObjectQuery<T>(sqlStatement, ormFunction, trans, parameters);
            if (result != null)
                return result[0];
            else
            {
                object nullobj = null;
                return (T)nullobj;
            }
        } 
        #endregion

        #region ExecuteObjectQuery
        public static R ExecuteObjectQuery<T,R>(string sqlStatement, ObjectRelationMapper<T> ormFunction, NpgsqlTransaction trans, params DbParameter[] parameters) where R : List<T>,new()
        {
            R result = new R();

            NpgsqlCommand command = new NpgsqlCommand(sqlStatement, DataAccess.Connection, trans);
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

        public static List<T> ExecuteObjectQuery<T>(string sqlStatement, ObjectRelationMapper<T> ormFunction,NpgsqlTransaction trans, params DbParameter[] parameters)
        {
            List<T> result = new List<T>();

            NpgsqlCommand command = new NpgsqlCommand(sqlStatement, DataAccess.Connection,trans);
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
        public static int ExecuteNoneQuery(string sqlStatement,NpgsqlTransaction trans, params DbParameter[] parameters)
        {
            NpgsqlCommand command = new NpgsqlCommand(sqlStatement, DataAccess.Connection,trans);
            SetupParameters(command, parameters);
            return command.ExecuteNonQuery();
        } 
        #endregion

        #region ExecuteScalarQuery
        public static T ExecuteScalarQuery<T>(string sqlStatement,NpgsqlTransaction trans, params DbParameter[] parameters)
        {
            return (T)ExecuteScalarQuery(sqlStatement,trans, parameters);
        }

        public static object ExecuteScalarQuery(string sqlStatement,NpgsqlTransaction trans, params DbParameter[] parameters)
        {
            NpgsqlCommand command = new NpgsqlCommand(sqlStatement, DataAccess.Connection,trans);
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