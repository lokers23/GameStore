using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DAL.Intefaces
{
    public interface IActionDB<T>
    {
        Task<bool> Create(T entity);
        Task<bool> Delete(T entity);
        //bool Update(T entity);
        Task<T> Get(int id);
        Task<List<T>> Select();
    }
}
