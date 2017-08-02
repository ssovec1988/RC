using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Common
{
    public interface IGenericAdoModel
    {
        bool IsCloseConnection { get; set; }

        DataTable Get(string commandString, SqlConnection connection, SqlParameter[] parametrs = null, int? timeOut = null);
        void Execute(string commandString, SqlConnection connection, SqlParameter[] parametrs = null, int? timeOut = null);
    }
}
