// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Allotment.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The allotment.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks
{
    using System;
    using System.Configuration;
    using System.Data.Common;

    /// <summary>
    /// The allotment.
    /// </summary>
    public class Allotment
    {
        #region Fields

        /// <summary>
        /// The connection string.
        /// </summary>
        private string connectionString;

        /// <summary>
        /// The connection string settings.
        /// </summary>
        private ConnectionStringSettings connectionStringSettings;

        /// <summary>
        /// The provider name.
        /// </summary>
        private string providerName;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Prevents a default instance of the <see cref="Allotment" /> class from being created.
        /// </summary>
        private Allotment()
        {
            this.Prepare();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static Allotment Instance
        {
            get
            {
                return Nested.Inner;
            }
        }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(this.connectionString))
                {
                    this.connectionString = this.connectionStringSettings.ConnectionString;
                }

                return this.connectionString;
            }

            set
            {
                this.connectionString = value;
            }
        }

        /// <summary>
        /// Gets or sets the provider name.
        /// </summary>
        public string ProviderName
        {
            get
            {
                if (string.IsNullOrEmpty(this.providerName))
                {
                    if (string.IsNullOrEmpty(this.connectionStringSettings.ProviderName))
                    {
                        throw new Exception("Can't find the 'providerName' attribute in the config of the connection string.");
                    }

                    this.providerName = this.connectionStringSettings.ProviderName;
                }

                return this.providerName;
            }

            set
            {
                this.providerName = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get provider factory.
        /// </summary>
        /// <returns>
        /// The <see cref="DbProviderFactory" />.
        /// </returns>
        public DbProviderFactory GetProviderFactory()
        {
            return DbProviderFactories.GetFactory(this.ProviderName);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The prepare.
        /// </summary>
        private void Prepare()
        {
            this.connectionStringSettings = ConfigurationManager.ConnectionStrings["ConnectionString"];
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
            internal static readonly Allotment Inner = new Allotment();

            #endregion
        }
    }
}