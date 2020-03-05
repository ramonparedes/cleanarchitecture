using Microsoft.EntityFrameworkCore;
using Negotium.Common.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;

namespace Negotium.Common.Implements
{
    public class BaseRepository<T,C> : IRepository<T> where T : class, new() where C : DbContext
    {
        private string _errorMessage = string.Empty;
        private bool _isDisposed;
        private readonly IUnitOfWork _uow;
        private DbSet<T> _entity;
        private readonly C _Context;
        public BaseRepository(IUnitOfWork uow,C context)
        {
            _uow = uow;
            _Context = context;
            _entity = _Context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return _entity.AsEnumerable<T>();
        }

        public IEnumerable<T> GetPredicate(Expression<Func<T, bool>> predicate)
        {
            return _entity.Where(predicate).AsEnumerable<T>();
        }

        public T GetById(long id)
        {
            return _entity.Find(id);
        }

        public IPagedList<T> GetPaged(int pageIndex, int pageSize, string sortExpression = null)
        {
            var query = _entity.AsNoTracking();

            return new PagedList<T>(query, (pageIndex * pageSize) - pageSize, pageSize);
        }

        public IPagedList<T> GetPaged(Func<IQueryable<T>, IQueryable<T>> transform, Expression<Func<T, bool>> filter = null, int pageIndex = -1, int pageSize = -1, string sortExpression = null)
        {
            var query = filter == null ? _entity.AsNoTracking() : _entity.AsNoTracking().Where(filter);
            var notSortedResults = transform(query);

            return new PagedList<T>(notSortedResults, (pageIndex * pageSize) - pageSize, pageSize);
        }

        public IPagedList<TResult> GetPaged<TResult>(Func<IQueryable<T>, IQueryable<TResult>> transform, Expression<Func<T, bool>> filter = null, int pageIndex = -1, int pageSize = -1, string sortExpression = null)
        {
            var query = filter == null ? _entity.AsNoTracking() : _entity.AsNoTracking().Where(filter);
            var notSortedResults = transform(query);

            return new PagedList<TResult>(notSortedResults, (pageIndex * pageSize) - pageSize, pageSize);
        }


        public void Insert(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
           
                _entity.Add(entity);
                _uow.Commit();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        _errorMessage += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
                _uow.Rollback();
                throw new Exception(_errorMessage, dbEx);
            }
        }

        public void Update(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                _entity.Attach(entity);
                SetEntryModified(entity);
                _uow.Commit();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        _errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                _uow.Rollback();
                throw new Exception(_errorMessage, dbEx);
            }
        }

        public void Delete(long id)
        {
            try
            {
                T existing = _entity.Find(id);

                if (existing != null)
                    _entity.Remove(existing);
                _uow.Commit();

            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        _errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                _uow.Rollback();
                throw new Exception(_errorMessage, dbEx);

            }
        }

        public virtual void SetEntryModified(T entity)
        {
            _Context.Entry(entity).State = EntityState.Modified;
        }

        public void Dispose()
        {
            if (_Context != null)
                _Context.Dispose();
            _isDisposed = true;
        }

        public T1 IPagedList<T1>(Expression<Func<T1, bool>> predicate, int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
