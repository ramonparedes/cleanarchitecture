using Negotium.Data;
using Negotium.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Negotium.Service
{
    public interface IUserService 
    {
        User Autheticate(string userName, string password);
        IEnumerable<User> Get();
        User Get(long id);
        IEnumerable<User> Get(Expression<Func<User, bool>> predocate);
        void Insert(User user);
        void Update(User user);
        void Delete(long id);
    }
}
