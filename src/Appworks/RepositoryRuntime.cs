// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepositoryRuntime.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The repository runtime.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The repository runtime.
    /// </summary>
    public class RepositoryRuntime
    {
        #region Fields

        /// <summary>
        /// The factory.
        /// </summary>
        private readonly IRepositoryFactory factory;

        /// <summary>
        /// The lock obj.
        /// </summary>
        private readonly object lockObj = new object();

        /// <summary>
        /// The repositories.
        /// </summary>
        private readonly Dictionary<Type, object> repositories;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Prevents a default instance of the <see cref="RepositoryRuntime"/> class from being created.
        /// </summary>
        private RepositoryRuntime()
        {
            this.repositories = new Dictionary<Type, object>();
            this.factory = ServiceLocator.Instance.GetService<IRepositoryFactory>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static RepositoryRuntime Instance
        {
            get
            {
                return Nested.Inner;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get repository.
        /// </summary>
        /// <typeparam name="T">
        /// The entity type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="IRepository{T}"/>.
        /// </returns>
        public IRepository<T> GetRepository<T>() where T : class
        {
            lock (this.lockObj)
            {
                if (this.repositories.Keys.Contains(typeof(T)))
                {
                    return this.repositories[typeof(T)] as IRepository<T>;
                }

                IRepository<T> repository = this.factory.CreateRepository<T>();
                this.repositories.Add(typeof(T), repository);
                return repository;
            }
        }

        #endregion

        /// <summary>
        /// The nested.
        /// </summary>
        private static class Nested
        {
            #region Static Fields

            /// <summary>
            /// The inner.
            /// </summary>
            internal static readonly RepositoryRuntime Inner = new RepositoryRuntime();

            #endregion
        }
    }
}