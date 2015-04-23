// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRepositoryFactory.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The RepositoryFactory interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks
{
    /// <summary>
    /// The RepositoryFactory interface.
    /// </summary>
    public interface IRepositoryFactory
    {
        #region Public Methods and Operators

        /// <summary>
        /// The create repository.
        /// </summary>
        /// <typeparam name="T">
        /// The entity type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="IRepository{T}"/>.
        /// </returns>
        IRepository<T> CreateRepository<T>() where T : class;

        #endregion
    }
}