using SPPRC.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPPRC.Sample
{
    public class SampleRepository<T> : IRepository<T> where T:class,IEntity
    {

        private readonly List<T> _dataStorage;

        public SampleRepository(List<T> TestItems)
        {
            _dataStorage = TestItems;
        }

        public T GetById(int id)
        {
            return _dataStorage.Single((x) => x.Id == id);
        }

        public List<T> GetAll()
        {
            return _dataStorage;
        }

        public IQueryable<T> Find(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _dataStorage.AsQueryable().Where(predicate);
        }

        public void Add(T newEntity)
        {
           if (!_dataStorage.Contains(newEntity))
                _dataStorage.Add(newEntity);
        }

        public void Delete(T entity_to_remove)
        {
            if (_dataStorage.Contains(entity_to_remove))
                _dataStorage.Remove(entity_to_remove);
        } 

    }
}
