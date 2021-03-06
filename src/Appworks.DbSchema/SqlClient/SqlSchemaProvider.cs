﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlSchemaProvider.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The sql schema provider.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.DbSchema.SqlClient
{
    using System.Data.SqlClient;
    using System.Linq;

    using Microsoft.SqlServer.Management.Common;
    using Microsoft.SqlServer.Management.Smo;

    using DatabaseCollection = Appworks.DbSchema.DatabaseCollection;

    /// <summary>
    /// The sql schema provider.
    /// </summary>
    public class SqlSchemaProvider : SqlSchemaProviderBase, IDbSchemaProvider
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
            var databaseCollection = new DatabaseCollection();

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                var serverConnection = new ServerConnection(sqlConnection);
                var server = new Server(serverConnection);

                foreach (
                    var database in
                        server.Databases.Cast<Database>().AsQueryable().Where(d => d.IsSystemObject == false))
                {
                    var dbItem = new DbSchema.Database { Name = database.Name };
                    if (includeTable && database.Tables.Count > 0)
                    {
                        foreach (var table in database.Tables.Cast<Table>().AsQueryable().Where(t => t.IsSystemObject == false))
                        {
                            var dbTableItem = new DbTable { Name = table.Name };
                            if (includeColumn && table.Columns.Count > 0)
                            {
                                foreach (var column in table.Columns.Cast<Column>().AsQueryable())
                                {
                                    var dbColumnItem = new DbColumn();

                                    dbColumnItem.Name = column.Name;
                                    dbColumnItem.Description = column.ExtendedProperties.Count > 0 ? column.ExtendedProperties["MS_Description"].Value.ToString() : string.Empty;
                                    
                                    dbColumnItem.IsPrimaryKey = column.InPrimaryKey;
                                    dbColumnItem.IsIdentityColumn = column.Identity;

                                    dbColumnItem.ColumnType = column.DataType.SqlDataType.ToString();
                                    dbColumnItem.AllowEmpty = column.Nullable;
                                    dbColumnItem.DefaultValue = column.Default;

                                    dbTableItem.Columns.Add(dbColumnItem);
                                }
                            }

                            dbItem.Tables.Add(dbTableItem);
                        }
                    }

                    databaseCollection.Add(dbItem);
                }
            }

            return databaseCollection;
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