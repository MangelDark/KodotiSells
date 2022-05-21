using Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.SqlServer
{
    public class ClientRepository : Repository, IClientRepository
    {
        public ClientRepository(SqlConnection context,SqlTransaction transaction)
        {
            this._context = context;
            this._transaction = transaction;
        }
        public Client Get(int id)
        {
            var command = CreateCommand("SELECT * FROM clients WITH(NOLOCK) WHERE id = @clientId");
            command.Parameters.AddWithValue("@clientId", id);

            using (var reader = command.ExecuteReader())
            {
                reader.Read();

                return new Client
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Name = reader["name"].ToString()
                };
            }
        }

        public IEnumerable<Client> GetAll()
        {
            throw new System.NotImplementedException();
        }
    }
}
