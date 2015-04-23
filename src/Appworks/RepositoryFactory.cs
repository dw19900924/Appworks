// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepositoryFactory.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The repository factory.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks
{
    /// <summary>
    /// The repository factory.
    /// </summary>
    public class RepositoryFactory : IRepositoryFactory
    {
        #region Public Methods and Operators

        /// <summary>
        /// The create repository.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <typeparam name="T">
        /// The entity type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="IRepository{T}"/>.
        /// </returns>
        public IRepository<T> CreateRepository<T>(IRepositoryContext context) where T : class
        {
            return null;
        }

        #endregion
    }
}