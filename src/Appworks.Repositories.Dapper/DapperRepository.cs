// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DapperRepository.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The dapper repository.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Repositories.Dapper
{
    using System.Collections.Generic;
    using System.Data;

    using Appworks.Specifications;

    /// <summary>
    /// The dapper repository.
    /// </summary>
    /// <typeparam name="T">
    /// The entity type.
    /// </typeparam>
    public class DapperRepository<T> : IRepository<T>
        where T : class
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DapperRepository{T}"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public DapperRepository(IRepositoryContext context)
        {
            this.Context = context;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the context.
        /// </summary>
        public IRepositoryContext Context { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <param name="transaction">
        /// The transaction.
        /// </param>
        /// <param name="commandTimeout">
        /// The command timeout.
        /// </param>
        public void Add(T entity, IDbTransaction transaction = null, int? commandTimeout = null)
        {
        }

        /// <summary>
        /// The exists.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="transaction">
        /// The transaction.
        /// </param>
        /// <param name="commandTimeout">
        /// The command timeout.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Exists(object key, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return false;
        }

        /// <summary>
        /// The exists.
        /// </summary>
        /// <param name="specification">
        /// The specification.
        /// </param>
        /// <param name="transaction">
        /// The transaction.
        /// </param>
        /// <param name="commandTimeout">
        /// The command timeout.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Exists(
            ISpecification<T> specification, 
            IDbTransaction transaction = null, 
            int? commandTimeout = null)
        {
            return false;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <param name="orderBy">
        /// The order by.
        /// </param>
        /// <param name="transaction">
        /// The transaction.
        /// </param>
        /// <param name="commandTimeout">
        /// The command timeout.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        public IEnumerable<T> GetAll(
            Dictionary<string, SortAs> orderBy = null, 
            IDbTransaction transaction = null, 
            int? commandTimeout = null)
        {
            return null;
        }

        /// <summary>
        /// The get by key.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="transaction">
        /// The transaction.
        /// </param>
        /// <param name="commandTimeout">
        /// The command timeout.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public T GetByKey(object key, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return null;
        }

        /// <summary>
        /// The get list.
        /// </summary>
        /// <param name="specification">
        /// The specification.
        /// </param>
        /// <param name="transaction">
        /// The transaction.
        /// </param>
        /// <param name="commandTimeout">
        /// The command timeout.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        public IEnumerable<T> GetList(
            ISpecification<T> specification, 
            IDbTransaction transaction = null, 
            int? commandTimeout = null)
        {
            return null;
        }

        /// <summary>
        /// The get list.
        /// </summary>
        /// <param name="specification">
        /// The specification.
        /// </param>
        /// <param name="orderBy">
        /// The order by.
        /// </param>
        /// <param name="transaction">
        /// The transaction.
        /// </param>
        /// <param name="commandTimeout">
        /// The command timeout.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        public IEnumerable<T> GetList(
            ISpecification<T> specification, 
            Dictionary<string, SortAs> orderBy = null, 
            IDbTransaction transaction = null, 
            int? commandTimeout = null)
        {
            return null;
        }

        /// <summary>
        /// The get paged result.
        /// </summary>
        /// <param name="pageNumber">
        /// The page number.
        /// </param>
        /// <param name="pageSize">
        /// The page size.
        /// </param>
        /// <param name="orderBy">
        /// The order by.
        /// </param>
        /// <returns>
        /// The <see cref="PagedList{T}"/>.
        /// </returns>
        public PagedList<T> GetPagedResult(int pageNumber, int pageSize, Dictionary<string, SortAs> orderBy = null)
        {
            return null;
        }

        /// <summary>
        /// The get paged result.
        /// </summary>
        /// <param name="pageNumber">
        /// The page number.
        /// </param>
        /// <param name="pageSize">
        /// The page size.
        /// </param>
        /// <param name="specification">
        /// The specification.
        /// </param>
        /// <param name="orderBy">
        /// The order by.
        /// </param>
        /// <returns>
        /// The <see cref="PagedList{T}"/>.
        /// </returns>
        public PagedList<T> GetPagedResult(
            int pageNumber, 
            int pageSize, 
            ISpecification<T> specification, 
            Dictionary<string, SortAs> orderBy = null)
        {
            return null;
        }

        /// <summary>
        /// The remove.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="transaction">
        /// The transaction.
        /// </param>
        /// <param name="commandTimeout">
        /// The command timeout.
        /// </param>
        public void Remove(object key, IDbTransaction transaction = null, int? commandTimeout = null)
        {
        }

        /// <summary>
        /// The remove.
        /// </summary>
        /// <param name="specification">
        /// The specification.
        /// </param>
        /// <param name="transaction">
        /// The transaction.
        /// </param>
        /// <param name="commandTimeout">
        /// The command timeout.
        /// </param>
        public void Remove(
            ISpecification<T> specification, 
            IDbTransaction transaction = null, 
            int? commandTimeout = null)
        {
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <param name="transaction">
        /// The transaction.
        /// </param>
        /// <param name="commandTimeout">
        /// The command timeout.
        /// </param>
        public void Update(T entity, IDbTransaction transaction = null, int? commandTimeout = null)
        {
        }

        #endregion
    }
}