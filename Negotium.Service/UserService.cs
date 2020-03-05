using Negotium.Common;
using Negotium.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Negotium.Service
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _repository;

        public UserService(IRepository<User> repository)
        {
            _repository = repository;
        }
        public IEnumerable<User> Get()
        {
            return _repository.GetAll();
        }
        public User Get(long id)
        {
            return _repository.GetById(id);
        }
        public IEnumerable<User> Get(Expression<Func<User, bool>> predicate)
        {
            return _repository.GetPredicate(predicate);
        }
        public void Insert(User user)
        {
             _repository.Insert(user);
        }

        public void Update(User user)
        {
            _repository.Update(user);
        }

        public void Delete(long id)
        {
            _repository.Delete(id);
        }

        //forstest
        private List<User> _users = new List<User>
        {
            new User { Id = 1, UserName = "", Password = ""}
        };

        public User Autheticate(string userName, string password)
        {
            var user = _repository.GetById(1);

            return user ?? null;
        }
    }
}
