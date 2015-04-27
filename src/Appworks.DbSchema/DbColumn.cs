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
    using System;

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

        /// <summary>
        /// The max length.
        /// </summary>
        private int maxLength;

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
        /// Gets or sets the data type.
        /// </summary>
        public Type DataType { get; set; }

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

        /// <summary>
        /// Gets or sets a value indicating whether is uniquely.
        /// </summary>
        public bool IsUniquely { get; set; }

        /// <summary>
        /// Gets or sets the max length.
        /// </summary>
        public int MaxLength
        {
            get
            {
                return this.maxLength;
            }

            set
            {
                this.maxLength = value < 0 ? int.MaxValue : value;
            }
        }

        /// <summary>
        /// Gets or sets the united column type.
        /// </summary>
        public UnitedColumnType UnitedColumnType { get; set; }

        #endregion
    }
}