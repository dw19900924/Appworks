// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRepositoryContext.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The RepositoryContext interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks
{
    using System.Data;

    /// <summary>
    /// The RepositoryContext interface.
    /// </summary>
    public interface IRepositoryContext
    {
        #region Public Properties

        /// <summary>
        /// Gets the connection.
        /// </summary>
        IDbConnection Connection { get; }

        #endregion
    }
}