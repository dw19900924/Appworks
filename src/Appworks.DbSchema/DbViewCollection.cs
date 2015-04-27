// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DbViewCollection.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The db view collection.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.DbSchema
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The db view collection.
    /// </summary>
    public class DbViewCollection : List<DbView>
    {
        #region Public Methods and Operators

        /// <summary>
        /// The contains name.
        /// </summary>
        /// <param name="viewName">
        /// The view name.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool ContainsName(string viewName)
        {
            return this.Any(view => string.Equals(view.Name, viewName, StringComparison.CurrentCultureIgnoreCase));
        }

        #endregion
    }
}