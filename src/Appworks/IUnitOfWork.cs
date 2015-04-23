// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUnitOfWork.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The UnitOfWork interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks
{
    using System.Data;

    using Appworks.Repository;

    /// <summary>
    /// The UnitOfWork interface.
    /// </summary>
    public interface IUnitOfWork
    {
        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether committed.
        /// </summary>
        bool Committed { get; }

        /// <summary>
        /// Gets the context.
        /// </summary>
        IRepositoryContext Context { get; }

        /// <summary>
        /// Gets the transaction.
        /// </summary>
        IDbTransaction Transaction { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The begin transaction.
        /// </summary>
        /// <param name="isolationLevel">
        /// The isolation level.
        /// </param>
        void BeginTransaction(IsolationLevel isolationLevel);

        /// <summary>
        /// The commit.
        /// </summary>
        void Commit();

        #endregion
    }
}