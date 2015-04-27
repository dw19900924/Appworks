// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Database.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The database.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.DbSchema
{
    /// <summary>
    /// The database.
    /// </summary>
    public class Database : DbObject
    {
        #region Fields

        /// <summary>
        /// The arguments dictionary.
        /// </summary>
        internal readonly GenericDictionary<string> ArgumentsDictionary = new GenericDictionary<string>();

        /// <summary>
        /// The table collection.
        /// </summary>
        internal readonly DbTableCollection TableCollection = new DbTableCollection();

        /// <summary>
        /// The connection string.
        /// </summary>
        private string connectionString = string.Empty;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return this.connectionString;
            }

            set
            {
                this.connectionString = value;
            }
        }

        /// <summary>
        /// Gets the table count.
        /// </summary>
        public int TableCount
        {
            get
            {
                return this.Tables.Count;
            }
        }

        /// <summary>
        /// Gets or sets the tables.
        /// </summary>
        public DbTableCollection Tables
        {
            get
            {
                return this.TableCollection;
            }

            set
            {
                if (value != this.TableCollection)
                {
                    this.TableCollection.Clear();
                    if (value != null && value.Count > 0)
                    {
                        this.TableCollection.AddRange(value);
                    }
                }
            }
        }

        #endregion
    }
}