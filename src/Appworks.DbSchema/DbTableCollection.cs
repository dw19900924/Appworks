// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DbTableCollection.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The db table collection.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.DbSchema
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The db table collection.
    /// </summary>
    public class DbTableCollection : List<DbTable>
    {
        #region Public Methods and Operators

        /// <summary>
        /// The contains name.
        /// </summary>
        /// <param name="tableName">
        /// The table name.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool ContainsName(string tableName)
        {
            return this.Any(table => string.Equals(table.Name, tableName, StringComparison.CurrentCultureIgnoreCase));
        }

        #endregion
    }
}