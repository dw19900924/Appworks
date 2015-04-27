// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DbKey.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The db key type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.DbSchema
{
    /// <summary>
    /// The db key type.
    /// </summary>
    public enum DbKeyType
    {
        /// <summary>
        /// The primary key.
        /// </summary>
        PrimaryKey = 1, 

        /// <summary>
        /// The foreign key.
        /// </summary>
        ForeignKey = 2
    }

    /// <summary>
    /// The db key.
    /// </summary>
    public class DbKey : DbObject
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the key type.
        /// </summary>
        public DbKeyType KeyType { get; set; }

        #endregion
    }
}