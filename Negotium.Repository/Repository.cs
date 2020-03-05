using Microsoft.EntityFrameworkCore;
using Negotium.Common;
using Negotium.Common.Implements;
using Negotium.Common.Interface;
using Negotium.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;

namespace Negotium.Repository
{
    public class Repository<T> : BaseRepository<T, ApplicationContext> where T : class, new()
    {
        public Repository(IUnitOfWork unitOfWork, ApplicationContext context) : base(unitOfWork,context)
        {
        }
    }
}
