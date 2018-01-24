using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SPPRC.Model
{
    public interface IRepository<T> where T: class
    {
        T GetById(int id);

        List<T> GetAll();

        IQueryable<T> Find(Expression<Func<T, bool>> predicate);

        void Add(T newEntity);

        void Delete(T entity_to_remove);
    }
}
