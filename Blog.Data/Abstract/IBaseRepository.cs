using System;
using System.Linq.Expressions;

namespace Blog.Data.Abstract
{
    public interface IBaseRepository<T> where T: class
    {
        void Add(T entity);
        void Delete(T entity);
        void DeleteWhere(Expression<Func<T, bool>> predicate);
        void Commit();
    }
}