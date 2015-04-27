// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDbSchemaProvider.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The DbSchemaProvider interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.DbSchema
{
    /// <summary>
    /// The DbSchemaProvider interface.
    /// </summary>
    public interface IDbSchemaProvider
    {
        #region Public Methods and Operators

        /// <summary>
        /// The get database collection.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <param name="includeTable">
        /// The include table.
        /// </param>
        /// <param name="includeColumn">
        /// The include column.
        /// </param>
        /// <returns>
        /// The <see cref="DatabaseCollection"/>.
        /// </returns>
        DatabaseCollection GetDatabaseCollection(
            string connectionString, 
            bool includeTable = false, 
            bool includeColumn = false);

        /// <summary>
        /// The get db column collection.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <param name="databaseName">
        /// The database name.
        /// </param>
        /// <param name="tableName">
        /// The table name.
        /// </param>
        /// <returns>
        /// The <see cref="DbColumnCollection"/>.
        /// </returns>
        DbColumnCollection GetDbColumnCollection(string connectionString, string databaseName, string tableName);

        /// <summary>
        /// The get db table collection.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <param name="databaseName">
        /// The database name.
        /// </param>
        /// <param name="includeColumn">
        /// The include column.
        /// </param>
        /// <returns>
        /// The <see cref="DbTableCollection"/>.
        /// </returns>
        DbTableCollection GetDbTableCollection(string connectionString, string databaseName, bool includeColumn = false);

        /// <summary>
        /// The get db view collection.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <param name="databaseName">
        /// The database name.
        /// </param>
        /// <param name="includeColumn">
        /// The include column.
        /// </param>
        /// <returns>
        /// The <see cref="DbViewCollection"/>.
        /// </returns>
        DbViewCollection GetDbViewCollection(string connectionString, string databaseName, bool includeColumn = false);

        #endregion
    }
}