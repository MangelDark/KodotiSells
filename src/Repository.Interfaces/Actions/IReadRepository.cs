using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces.Actions
{
    public interface IReadRepository<T,Y> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(Y id);
    }
}
