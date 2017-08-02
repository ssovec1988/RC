using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        TEntity FindByPrimaryKey(int key);
        IEnumerable<TEntity> Get();
        IEnumerable<TEntity> Get(Func<TEntity, bool> predicate);
        void Add(TEntity item);
        void Update(TEntity item);
        void Remove(TEntity item);
    }
}
