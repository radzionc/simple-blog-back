using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Blog.Model;

namespace Blog.Data.Abstract
{
    public interface IEntityBaseRepository<T>  where T : class
    {
        void Add(T entity);
        void Delete(T entity);
        void DeleteWhere(Expression<Func<T, bool>> predicate);
        void Commit();
        IEnumerable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
        T GetSingle(Expression<Func<T, bool>> predicate);
        T GetSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        void Update(T entity);
    }
}