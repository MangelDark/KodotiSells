using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces.Actions
{
    public interface IUpdateRepository<T> where T : class
    {
        void Update(T t);
    }
}
