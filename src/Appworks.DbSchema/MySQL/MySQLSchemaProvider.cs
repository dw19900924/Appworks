﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MySQLSchemaProvider.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The my sql schema provider.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.DbSchema.MySQL
{
    /// <summary>
    /// The my sql schema provider.
    /// </summary>
    public class MySQLSchemaProvider : MySQLSchemaProviderBase, IDbSchemaProvider
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
        public DatabaseCollection GetDatabaseCollection(
            string connectionString, 
            bool includeTable = false, 
            bool includeColumn = false)
        {
            return null;
        }

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
        public DbColumnCollection GetDbColumnCollection(string connectionString, string databaseName, string tableName)
        {
            return null;
        }

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
        public DbTableCollection GetDbTableCollection(
            string connectionString, 
            string databaseName, 
            bool includeColumn = false)
        {
            return null;
        }

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
        public DbViewCollection GetDbViewCollection(
            string connectionString, 
            string databaseName, 
            bool includeColumn = false)
        {
            return null;
        }

        #endregion
    }
}