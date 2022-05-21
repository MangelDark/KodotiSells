using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.SqlServer
{
    public abstract class Repository
    {
        protected SqlConnection _context;
        protected SqlTransaction _transaction;

        protected SqlCommand CreateCommand(string query)
        {
            return new SqlCommand(query,_context,_transaction);
        } 
    }

   
}
