using Microsoft.Data.SqlClient;
using System.Data;

namespace PFCRepository.Repositories.Common.Interfaces
{
    public interface ISqlHelper : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        DataTable ExecuteTable(CommandType cmdType, string cmdText, SqlParameter[] cmdParms = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        DataTable ExecuteTableV2(string cmdText, CommandType cmdType = CommandType.Text, SqlParameter[] cmdParms = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        DataSet ExecuteDataSet(CommandType cmdType, string cmdText, SqlParameter[] cmdParms = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>       
        DataTable ExecuteTable(string connectionString, CommandType cmdType, string cmdText, SqlParameter[] cmdParms = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        int ExecuteNonQuery(CommandType cmdType, string cmdText, SqlParameter[] cmdParms = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        object ExecuteScalarQuery(CommandType cmdType, string cmdText, SqlParameter[] cmdParms = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        SqlDataReader ExecuteReaderQuery(CommandType cmdType, string cmdText, SqlParameter[] cmdParms = null);
    }
}
