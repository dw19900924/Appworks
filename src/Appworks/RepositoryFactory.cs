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
    using System;
    using System.Configuration;

    /// <summary>
    ///     The repository factory.
    /// </summary>
    public class RepositoryFactory : IRepositoryFactory
    {
        #region Static Fields

        /// <summary>
        /// The provider section.
        /// </summary>
        private static readonly RepositoryProviderConfigurationSection ProviderSection =
            ConfigurationManager.GetSection("ProviderSection") as RepositoryProviderConfigurationSection;

        #endregion

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
        public IRepository<T> CreateRepository<T>() where T : class
        {
            Type repositoryType = Type.GetType(ProviderSection.Repository);
            if (repositoryType != null)
            {
                Type[] args = { typeof(T) };
                repositoryType = repositoryType.MakeGenericType(args);

                Type contextType = Type.GetType(ProviderSection.RepositoryContext);
                if (contextType != null)
                {
                    object repositoryContext = Activator.CreateInstance(contextType);

                    return (IRepository<T>)Activator.CreateInstance(repositoryType, new[] { repositoryContext });
                }
            }

            return null;
        }

        #endregion
    }
}