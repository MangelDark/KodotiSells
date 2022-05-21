using Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitOfWork.Interfaces;

namespace UnitOfWork.SqlServer
{
    internal class UnitOfWorkSqlServerAdapter : IUnitOfWorkAdapter
    {
        private SqlConnection _context { get; set; }
        private SqlTransaction _transaction { get; set; }
        public IUnitOfWorkRepository Repositories { get; set; }

        public UnitOfWorkSqlServerAdapter()
        {
            _context =  new SqlConnection(Parameters.ConnectionString);
            _context.Open();
            _transaction = _context.BeginTransaction();
            Repositories = new UnitOfWorkSqlServerRepository(_context, _transaction);
        }
        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
            }
            if (_context != null) 
            {
                _context.Close();
                _context.Dispose();

            }
            Repositories = null;
        }

        public void SaveChanges()
        {
            _transaction.Commit();
        }
    }
}
