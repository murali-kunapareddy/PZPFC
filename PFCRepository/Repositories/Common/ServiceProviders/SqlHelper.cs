using Microsoft.Data.SqlClient;
using PFCRepository.Repositories.Common.Interfaces;
using PFCRepository.Utilities;
using System.Data;

namespace PFCRepository.Repositories.Common.ServiceProviders
{
    public class SqlHelper : ISqlHelper
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly string _connectionString;

        public SqlHelper()
        {
            _connectionString = AppConfig.ConnectionString;
        }

        public SqlHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>       
        /// <returns></returns>
        public DataTable ExecuteTable(CommandType cmdType, string cmdText, SqlParameter[] cmdParms = null)
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                SqlDataAdapter oDataAdapter = new SqlDataAdapter();
                DataTable oDataTable = new DataTable();
                try
                {

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        PrepareCommand(cmd, connection, cmdType, cmdText, cmdParms);
                        oDataAdapter.SelectCommand = cmd;
                        connection.Open();
                        oDataAdapter.Fill(oDataTable);
                        connection.Close();
                        cmd.Parameters.Clear();
                        return oDataTable;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("ExecuteTable", ex);
                }
                finally
                {
                    connection.Close();
                    oDataAdapter.Dispose();
                    oDataTable.Dispose();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>       
        /// <returns></returns>
        public DataTable ExecuteTableV2(string cmdText, CommandType cmdType = CommandType.Text, SqlParameter[] cmdParms = null)
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                SqlDataAdapter oDataAdapter = new SqlDataAdapter();
                DataTable oDataTable = new DataTable();
                try
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        PrepareCommand(cmd, connection, cmdType, cmdText, cmdParms);
                        oDataAdapter.SelectCommand = cmd;
                        connection.Open();
                        oDataAdapter.Fill(oDataTable);
                        connection.Close();
                        cmd.Parameters.Clear();
                        return oDataTable;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("ExecuteTable", ex);
                }
                finally
                {
                    connection.Close();
                    oDataAdapter.Dispose();
                    oDataTable.Dispose();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>       
        /// <returns></returns>
        public DataSet ExecuteDataSet(CommandType cmdType, string cmdText, SqlParameter[] cmdParms = null)
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                SqlDataAdapter oDataAdapter = new SqlDataAdapter();
                DataSet oDataTable = new DataSet();
                try
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        PrepareCommand(cmd, connection, cmdType, cmdText, cmdParms);
                        oDataAdapter.SelectCommand = cmd;
                        connection.Open();
                        oDataAdapter.Fill(oDataTable);
                        connection.Close();
                        cmd.Parameters.Clear();
                        return oDataTable;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("ExecuteTable", ex);
                }
                finally
                {
                    connection.Close();
                    oDataAdapter.Dispose();
                    oDataTable.Dispose();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>       
        /// <returns></returns>
        public DataTable ExecuteTable(string connectionString, CommandType cmdType, string cmdText, SqlParameter[] cmdParms = null)
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                SqlDataAdapter oDataAdapter = new SqlDataAdapter();
                DataTable oDataTable = new DataTable();

                try
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        PrepareCommand(cmd, connection, cmdType, cmdText, cmdParms);
                        oDataAdapter.SelectCommand = cmd;
                        connection.Open();
                        oDataAdapter.Fill(oDataTable);
                        connection.Close();
                        cmd.Parameters.Clear();
                        return oDataTable;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("ExecuteTable", ex);
                }
                finally
                {
                    connection.Close();
                    oDataAdapter.Dispose();
                    oDataTable.Dispose();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(CommandType cmdType, string cmdText, SqlParameter[] cmdParms = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {

                        PrepareCommand(cmd, connection, cmdType, cmdText, cmdParms);
                        connection.Open();
                        int val = cmd.ExecuteNonQuery();
                        connection.Close();
                        cmd.Parameters.Clear();
                        return val;
                    }

                }
                catch (SqlException ex)
                {
                    throw new Exception("SQL Exception ", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public object ExecuteScalarQuery(CommandType cmdType, string cmdText, SqlParameter[] cmdParms = null)
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {

                    using (var cmd = new SqlCommand())
                    {
                        PrepareCommand(cmd, connection, cmdType, cmdText, cmdParms);
                        connection.Open();
                        object val = cmd.ExecuteScalar();
                        connection.Close();
                        cmd.Parameters.Clear();
                        return val;
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("SQL Exception ", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public SqlDataReader ExecuteReaderQuery(CommandType cmdType, string cmdText, SqlParameter[] cmdParms = null)
        {

            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    SqlDataReader val;
                    using (var cmd = new SqlCommand())
                    {
                        PrepareCommand(cmd, connection, cmdType, cmdText, cmdParms);
                        connection.Open();
                        val = cmd.ExecuteReader();
                        //connection.Close();
                        cmd.Parameters.Clear();
                        return val;
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("SQL Exception ", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="conn"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public bool PrepareCommand(SqlCommand cmd, SqlConnection conn, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            //if (!(conn.State == ConnectionState.Open))
            //{
            //    conn.Open();
            //}
            try
            {
                cmd.Connection = conn;
                cmd.CommandText = cmdText;
                cmd.CommandTimeout = 600;
                cmd.Parameters.Clear();
                // cmd.ParameterCheck = True
                cmd.CommandType = cmdType;
                if (cmdParms != null)
                {
                    //SqlParameter parm;
                    foreach (SqlParameter parm in cmdParms)
                    {
                        cmd.Parameters.Add(parm);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("SQL Exception ", ex);
            }
            catch (Exception exx)
            {
                throw new Exception("PrepareCommand : ", exx);
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
