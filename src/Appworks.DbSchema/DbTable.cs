// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DbTable.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The db table.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.DbSchema
{
    using System.Linq;

    /// <summary>
    /// The db table.
    /// </summary>
    public class DbTable : DbObject
    {
        #region Fields

        /// <summary>
        /// The column collection.
        /// </summary>
        protected readonly DbColumnCollection ColumnCollection = new DbColumnCollection();

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the columns.
        /// </summary>
        public virtual DbColumnCollection Columns
        {
            get
            {
                return this.ColumnCollection;
            }

            set
            {
                if (value != this.ColumnCollection)
                {
                    this.ColumnCollection.Clear();
                    if (value != null)
                    {
                        this.ColumnCollection.AddRange(value);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the identity column.
        /// </summary>
        public virtual DbColumn IdentityColumn
        {
            get
            {
                return this.Columns.FirstOrDefault(column => column.IsIdentityColumn);
            }
        }

        /// <summary>
        /// Gets the primary keys.
        /// </summary>
        public virtual DbColumnCollection PrimaryKeys
        {
            get
            {
                var primaryKeys = new DbColumnCollection();

                primaryKeys.AddRange(this.Columns.Where(column => column.IsPrimaryKey));

                return primaryKeys;
            }
        }

        #endregion
    }
}