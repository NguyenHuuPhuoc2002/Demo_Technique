﻿namespace Unit_Of_Work.Repository.Interface
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetById(string id);
        Task<IEnumerable<TEntity>> GetAll();
        Task Add(TEntity entity);
        Task Delete(TEntity entity);
        Task Update(TEntity entity);
    }
}
