using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;


namespace Common
{
    public class EFGenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        DbContext _context;
        DbSet<TEntity> _dbSet;

        /// <summary>
        /// Конструктор репозитория получает ссылку на модель базы данных
        /// </summary>
        /// <param name="context"></param>
        public EFGenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        /// <summary>
        /// Добавить запись в БД
        /// </summary>
        /// <param name="item"></param>
        public void Add(TEntity item)
        {
            _dbSet.Add(item);
            _context.SaveChanges();
        }

        /// <summary>
        /// Поиск записи по первичному ключу
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TEntity FindByPrimaryKey(int key)
        {
            return _dbSet.Find(key);
        }

        /// <summary>
        /// Получить всю таблицу данных
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TEntity> Get()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        /// <summary>
        /// Получить выборку из таблицы по признаку
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> Get(Func<TEntity, bool> predicate)
        {
            return _dbSet.AsNoTracking().Where(predicate).ToList();
        }

        /// <summary>
        /// Удалить запись из таблицы
        /// </summary>
        /// <param name="item"></param>
        public void Remove(TEntity item)
        {
            _dbSet.Remove(item);
            _context.SaveChanges();
        }

        /// <summary>
        /// Обновить запись в таблице
        /// </summary>
        /// <param name="item"></param>
        public void Update(TEntity item)
        {
            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
