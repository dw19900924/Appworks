// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitOfWork.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The unit of work.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks
{
    using System;
    using System.Data;
    
    /// <summary>
    /// The unit of work.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        #region Fields

        /// <summary>
        /// The transaction.
        /// </summary>
        private IDbTransaction transaction;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        public UnitOfWork(IRepositoryContext context)
        {
            this.Context = context;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the context.
        /// </summary>
        public IRepositoryContext Context { get; private set; }

        /// <summary>
        /// Gets the transaction.
        /// </summary>
        public IDbTransaction Transaction
        {
            get
            {
                return this.transaction;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The begin transaction.
        /// </summary>
        /// <param name="isolationLevel">
        /// The isolation level.
        /// </param>
        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            if (this.Transaction != null && this.Transaction.Connection != null)
            {
                throw new Exception("Not finished previous transaction.");
            }

            this.transaction = this.Context.Connection.BeginTransaction(isolationLevel);
        }

        /// <summary>
        /// The commit.
        /// </summary>
        public void Commit()
        {
            this.transaction.Commit();
        }

        #endregion
    }
}