using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces.Actions
{
    public interface IRemoveRepository<T> 
    {
        void Remove(T id);
    }
}
