using DataLayer.Data;
using DataLayer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using ModelLayer.Helper;
using System.Linq.Expressions;


namespace DataLayer.Services.Reposatories
{

    public class EntityRepository<T> : IEntity<T> where T : class
    {
        AppDbContext _db;
        public EntityRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task<T> AddAsync(T entity)
        {
           var model= await _db.AddAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<T>> AddManyAsync(List<T>? entities)
        {
             await _db.AddRangeAsync(entities);
            return entities;
        }

        public bool CheckEmailPattern(string email)
        {
          var check=  EmailValidation.CheckEmailRegex(email);
            return check;
        }

        public async Task<T> FindAnyAsync(Expression<Func<T,bool>> find)
        {
            var res=await _db.Set<T>().FirstOrDefaultAsync(find);
            return res;
        }

        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>>? where = null)
        {
            IQueryable<T> query = _db.Set<T>();
            if(where !=null)
                query=query.Where(where);
            return await query.ToListAsync();
        }
        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? where = null, int pageSize = 15, int pageNumber = 1)
        {
            IQueryable<T> query = _db.Set<T>();
            if (where != null)
                query = query.Where(where);
            //if (includes != null)
            //    foreach (var include in includes)
            //        query = query.Include(include);
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(Expression<Func<T,bool>>? find, string[]? includes=null)
        {
            IQueryable<T> query = _db.Set<T>();
            if(includes != null)
                foreach (var include in includes)
                    query = query.Include(include);
           
                    
            return query.FirstOrDefault(find);

        }
        public async Task<T> GetByIdAsyncAsNoTracking(Expression<Func<T, bool>>? find, string[]? includes = null)
        {
            IQueryable<T> query = _db.Set<T>().AsNoTracking<T>();
            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);


            return query.FirstOrDefault(find);

        }

        public async Task<IEnumerable<T>> SearchAsync(Expression<Func<T, bool>> find, Expression<Func<T, object>> order, 
            string orderBy, int pageSize=15, int pageNumber=1)
        {
            IQueryable<T> query = _db.Set<T>().Where(find);
            query=query.Skip((pageNumber-1)*pageSize).Take(pageSize);
            if (order != null)
            {
                if (orderBy == "desc")
                {
                    query = query.OrderByDescending(order);
                }
                else
                {
                    query = query.OrderBy(order);
                }
                   
            };
            return await query.ToListAsync();
        }
       public  T Edit ( T entity)
        {
            //_db.Entry(entity).State = EntityState.Modified;
            _db.Update(entity);
            return entity;
        }
        public void Delete (T entity)
        {
            _db.Set<T>().Remove(entity);

        }

       
    }
    
}
