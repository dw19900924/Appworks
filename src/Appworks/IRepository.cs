// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRepository.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The Repository interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks
{
    using System.Collections.Generic;
    using System.Data;

    using Appworks.Specifications;

    /// <summary>
    /// The Repository interface.
    /// </summary>
    /// <typeparam name="T">
    /// The entity type.
    /// </typeparam>
    public interface IRepository<T>
        where T : class
    {
        #region Public Properties

        /// <summary>
        /// Gets the context.
        /// </summary>
        IRepositoryContext Context { get; }

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
        void Add(T entity, IDbTransaction transaction = null, int? commandTimeout = null);

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
        bool Exists(object key, IDbTransaction transaction = null, int? commandTimeout = null);

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
        bool Exists(ISpecification<T> specification, IDbTransaction transaction = null, int? commandTimeout = null);

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
        IEnumerable<T> GetAll(
            Dictionary<string, SortAs> orderBy = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null);

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
        T GetByKey(object key, IDbTransaction transaction = null, int? commandTimeout = null);

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
        IEnumerable<T> GetList(
            ISpecification<T> specification,
            IDbTransaction transaction = null,
            int? commandTimeout = null);

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
        IEnumerable<T> GetList(
            ISpecification<T> specification,
            Dictionary<string, SortAs> orderBy = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null);

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
        PagedList<T> GetPagedResult(int pageNumber, int pageSize, Dictionary<string, SortAs> orderBy = null);

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
        PagedList<T> GetPagedResult(
            int pageNumber,
            int pageSize,
            ISpecification<T> specification,
            Dictionary<string, SortAs> orderBy = null);

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
        void Remove(object key, IDbTransaction transaction = null, int? commandTimeout = null);

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
        void Remove(ISpecification<T> specification, IDbTransaction transaction = null, int? commandTimeout = null);

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
        void Update(T entity, IDbTransaction transaction = null, int? commandTimeout = null);

        #endregion
    }
}