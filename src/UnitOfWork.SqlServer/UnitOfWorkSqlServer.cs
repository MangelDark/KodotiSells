using Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitOfWork.Interfaces;

namespace UnitOfWork.SqlServer
{
    public class UnitOfWorkSqlServer : IUnitOfWork
    {
        private readonly IConfiguration _configuration;
        public UnitOfWorkSqlServer(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IUnitOfWorkAdapter Create()
        {
            var connectionString = _configuration == null
                ? Parameters.ConnectionString
                : _configuration.GetConnectionString("myconn");

            return new UnitOfWorkSqlServerAdapter(connectionString);
        }

    }
}
