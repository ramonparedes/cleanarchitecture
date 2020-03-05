using System;
using System.Data.Entity.Validation;
using Microsoft.EntityFrameworkCore.Storage;
using Negotium.Common.Interface;

namespace Negotium.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext Context;
        private bool _disposed;
        private string _errorMessage = string.Empty;
        private IDbContextTransaction _objTran;

        public UnitOfWork(ApplicationContext context)
        {
            Context = context;
        }
       
        public void CreateTransaction()
        {
            _objTran = Context.Database.BeginTransaction();
        }
        public void Commit()
        {
            _objTran.Commit();
        }
        public void Rollback()
        {
            _objTran.Rollback();
            _objTran.Dispose();
        }
        public void Save()
        {
            try
            {
                Context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        _errorMessage += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
                throw new Exception(_errorMessage, dbEx);
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    Context.Dispose();
            _disposed = true;
        }
    }
}
