using Repository.Interfaces;
using Repository.SqlServer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitOfWork.Interfaces;

namespace UnitOfWork.SqlServer
{
    public class UnitOfWorkSqlServerRepository : IUnitOfWorkRepository
    {
        public UnitOfWorkSqlServerRepository(SqlConnection context, SqlTransaction transaction)
        {
            InvoiceRepository = new InvoiceRepository(context,transaction);
            InvoiceDetailRepository = new InvoiceDetailRepository(context,transaction);
            ClientRepository = new ClientRepository(context, transaction);
            ProductRepository = new ProductRepository(context, transaction);
        }

        public IInvoiceRepository InvoiceRepository { get; }

        public IInvoiceDetailRepository InvoiceDetailRepository { get; }

        public IClientRepository ClientRepository { get; }
        public IProductRepository ProductRepository { get; }
    }
}
