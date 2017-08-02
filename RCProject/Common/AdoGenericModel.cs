using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class AdoGenericModel : IGenericAdoModel
    {
        private bool _isCloseConnection;

        /// <summary>
        /// Флаг требуется ли закрывать соединение с БД после выполнения операции
        /// </summary>
        public bool IsCloseConnection
        {
            get { return _isCloseConnection; }
            set { _isCloseConnection = value; }
        }

        /// <summary>
        /// Выполнить команду или хранимую процедуру
        /// </summary>
        /// <param name="commandString"></param>
        /// <param name="connection"></param>
        /// <param name="parametrs"></param>
        /// <param name="timeOut"></param>
        public void Execute(string commandString, SqlConnection connection, SqlParameter[] parametrs = null, int? timeOut = default(int?))
        {
            SqlCommand command = new SqlCommand(commandString, connection);

            if (timeOut != null)
                command.CommandTimeout = (int)timeOut;

            if (parametrs != null)
            {
                command.CommandType = CommandType.StoredProcedure;
                foreach (SqlParameter p in parametrs)
                    command.Parameters.Add(p);
            }

            try
            {
                command.ExecuteNonQuery();
                if (_isCloseConnection)
                    connection.Close();
            }
            catch
            {
                if (_isCloseConnection)
                    connection.Close();
            }
        }

        /// <summary>
        /// Получения набора данных из БД
        /// </summary>
        /// <param name="commandString"></param>
        /// <param name="connect"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public DataTable Get(string commandString, SqlConnection connection, SqlParameter[] parametrs = null, int? timeOut = default(int?))
        {
            SqlCommand command = new SqlCommand(commandString, connection);

            if (parametrs != null)
            {
                command.CommandType = CommandType.StoredProcedure;
                foreach (SqlParameter p in parametrs)
                    command.Parameters.Add(p);
            }

            if (timeOut != null)
                command.CommandTimeout = (int)timeOut;

            DataTable returnedTable = null;

            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet set = new DataSet();
                adapter.Fill(set);

                returnedTable = set.Tables[0];

                if (_isCloseConnection)
                    connection.Close();
            }
            catch
            {
                if (_isCloseConnection)
                    connection.Close();
            }

            return returnedTable;

        }

        /// <summary>
        /// Открытие подключения к БД
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public SqlConnection OpenConnections(string connectionString)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
            }
            catch
            {
                return null;
            }
            return connection;
        }
    }
}
