using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Negotium.Common
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T>GetAll();
        IEnumerable<T> GetPredicate(Expression<Func<T, bool>> predicate);
        T GetById(long id);
        T IPagedList<T>(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize);
        void Insert(T entity);
        void Update(T entity);
        void Delete(long id);

    }
}
