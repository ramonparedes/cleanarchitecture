
using System;

namespace Negotium.Common.Interface
{
    public interface IUnitOfWork: IDisposable
    {
        void CreateTransaction();
        void Commit();
        void Rollback();
        void Save();
    }

}
