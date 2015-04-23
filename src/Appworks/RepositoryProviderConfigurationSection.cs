// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepositoryProviderConfigurationSection.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The repository provider configuration section.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks
{
    using System.Configuration;

    /// <summary>
    /// The repository provider configuration section.
    /// </summary>
    public class RepositoryProviderConfigurationSection : ConfigurationSection
    {
        #region Public Properties

        /// <summary>
        /// Gets the repository.
        /// </summary>
        [ConfigurationProperty("Repository")]
        public string Repository
        {
            get
            {
                return (string)base["Repository"];
            }
        }

        /// <summary>
        /// Gets the repository context.
        /// </summary>
        [ConfigurationProperty("RepositoryContext")]
        public string RepositoryContext
        {
            get
            {
                return (string)base["RepositoryContext"];
            }
        }

        #endregion
    }
}