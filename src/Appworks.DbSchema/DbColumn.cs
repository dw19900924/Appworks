// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DbColumn.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The db column.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.DbSchema
{
    /// <summary>
    /// The db column.
    /// </summary>
    public class DbColumn : DbObject
    {
        #region Fields

        /// <summary>
        /// The column type.
        /// </summary>
        private string columnType;

        /// <summary>
        /// The default value.
        /// </summary>
        private object defaultValue;

        /// <summary>
        /// The description.
        /// </summary>
        private string description;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DbColumn"/> class.
        /// </summary>
        public DbColumn()
        {
            this.IsIdentityColumn = false;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether allow empty.
        /// </summary>
        public bool AllowEmpty { get; set; }

        /// <summary>
        /// Gets or sets the column type.
        /// </summary>
        public string ColumnType
        {
            get
            {
                return this.columnType;
            }

            set
            {
                if (value == null)
                {
                    value = string.Empty;
                }

                this.columnType = value;
            }
        }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        public object DefaultValue
        {
            get
            {
                return this.defaultValue;
            }

            set
            {
                if (value == null)
                {
                    value = string.Empty;
                }

                this.defaultValue = value;
            }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description
        {
            get
            {
                return this.description;
            }

            set
            {
                if (value == null)
                {
                    value = string.Empty;
                }

                this.description = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is identity column.
        /// </summary>
        public bool IsIdentityColumn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is primary key.
        /// </summary>
        public bool IsPrimaryKey { get; set; }

        #endregion
    }
}