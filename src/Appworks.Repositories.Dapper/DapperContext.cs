// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DapperContext.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The dapper context.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Repositories.Dapper
{
    using System.Data;
    using System.Data.Common;

    /// <summary>
    /// The dapper context.
    /// </summary>
    public class DapperContext : IRepositoryContext
    {
        #region Fields

        /// <summary>
        /// The connection string.
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// The connection.
        /// </summary>
        private IDbConnection connection;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DapperContext"/> class.
        /// </summary>
        public DapperContext()
        {
            this.connectionString = Allotment.Instance.ConnectionString;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the connection.
        /// </summary>
        public IDbConnection Connection
        {
            get
            {
                if (this.connection == null)
                {
                    DbProviderFactory providerFactory = Allotment.Instance.GetProviderFactory();

                    this.connection = providerFactory.CreateConnection();
                    if (this.connection != null)
                    {
                        this.connection.ConnectionString = this.connectionString;
                    }
                }

                if (null != this.connection)
                {
                    if (this.connection.State != ConnectionState.Open)
                    {
                        this.connection.Open();
                    }
                }

                return this.connection;
            }
        }

        #endregion
    }
}