// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitedColumnTypeObject.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The united column type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.DbSchema
{
    using System;

    /// <summary>
    /// The united column type.
    /// </summary>
    public enum UnitedColumnType
    {
        /// <summary>
        /// The none.
        /// </summary>
        None = 0, 

        /// <summary>
        /// The string.
        /// </summary>
        String = 1, 

        /// <summary>
        /// The n string.
        /// </summary>
        NString = 2, 

        /// <summary>
        /// The text.
        /// </summary>
        Text = 3, 

        /// <summary>
        /// The n text.
        /// </summary>
        NText = 4, 

        /// <summary>
        /// The guid.
        /// </summary>
        Guid = 5, 

        /// <summary>
        /// The identity.
        /// </summary>
        Identity = 6, 

        /// <summary>
        /// The int.
        /// </summary>
        Int = 7, 

        /// <summary>
        /// The big int.
        /// </summary>
        BigInt = 8, 

        /// <summary>
        /// The double.
        /// </summary>
        Double = 9, 

        /// <summary>
        /// The binary.
        /// </summary>
        Binary = 10, 

        /// <summary>
        /// The boolean.
        /// </summary>
        Boolean = 11, 

        /// <summary>
        /// The date time.
        /// </summary>
        DateTime = 12, 
    }

    /// <summary>
    /// The united column type object.
    /// </summary>
    public class UnitedColumnTypeObject
    {
        #region Fields

        /// <summary>
        /// The column type.
        /// </summary>
        private UnitedColumnType columnType = UnitedColumnType.None;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitedColumnTypeObject"/> class.
        /// </summary>
        public UnitedColumnTypeObject()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitedColumnTypeObject"/> class.
        /// </summary>
        /// <param name="columnTypeString">
        /// The column type string.
        /// </param>
        public UnitedColumnTypeObject(string columnTypeString)
        {
            this.ColumnTypeString = columnTypeString;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitedColumnTypeObject"/> class.
        /// </summary>
        /// <param name="columnType">
        /// The column type.
        /// </param>
        public UnitedColumnTypeObject(UnitedColumnType columnType)
        {
            this.ColumnType = columnType;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the column type.
        /// </summary>
        public UnitedColumnType ColumnType
        {
            get
            {
                return this.columnType;
            }

            set
            {
                this.columnType = value;
            }
        }

        /// <summary>
        /// Gets or sets the column type string.
        /// </summary>
        public string ColumnTypeString
        {
            get
            {
                return this.columnType.ToString();
            }

            set
            {
                switch (value.Trim().ToLower())
                {
                    case "string":
                        this.columnType = UnitedColumnType.String;
                        break;
                    case "nstring":
                        this.columnType = UnitedColumnType.NString;
                        break;
                    case "text":
                        this.columnType = UnitedColumnType.Text;
                        break;
                    case "ntext":
                        this.columnType = UnitedColumnType.NText;
                        break;
                    case "guid":
                        this.columnType = UnitedColumnType.Guid;
                        break;
                    case "int":
                        this.columnType = UnitedColumnType.Int;
                        break;
                    case "bigint":
                        this.columnType = UnitedColumnType.BigInt;
                        break;
                    case "double":
                        this.columnType = UnitedColumnType.Double;
                        break;
                    case "binary":
                        this.columnType = UnitedColumnType.Binary;
                        break;
                    case "boolean":
                        this.columnType = UnitedColumnType.Boolean;
                        break;
                    case "datetime":
                        this.columnType = UnitedColumnType.DateTime;
                        break;
                    case "identity":
                        this.columnType = UnitedColumnType.Identity;
                        break;
                    case "none":
                        this.columnType = UnitedColumnType.None;
                        break;
                }
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get data type.
        /// </summary>
        /// <param name="columnType">
        /// The column type.
        /// </param>
        /// <returns>
        /// The <see cref="Type"/>.
        /// </returns>
        public static Type GetDataType(UnitedColumnType columnType)
        {
            switch (columnType)
            {
                case UnitedColumnType.String:
                    return typeof(string);
                case UnitedColumnType.NString:
                    return typeof(string);
                case UnitedColumnType.Text:
                    return typeof(string);
                case UnitedColumnType.NText:
                    return typeof(string);
                case UnitedColumnType.Guid:
                    return typeof(Guid);
                case UnitedColumnType.Identity:
                    return typeof(int);
                case UnitedColumnType.Int:
                    return typeof(int);
                case UnitedColumnType.BigInt:
                    return typeof(long);
                case UnitedColumnType.Double:
                    return typeof(double);
                case UnitedColumnType.Binary:
                    return typeof(byte[]);
                case UnitedColumnType.Boolean:
                    return typeof(bool);
                case UnitedColumnType.DateTime:
                    return typeof(DateTime);
                case UnitedColumnType.None:
                    return typeof(string);
                default:
                    return typeof(string);
            }
        }

        /// <summary>
        /// The get united column type.
        /// </summary>
        /// <param name="columnTypeString">
        /// The column type string.
        /// </param>
        /// <returns>
        /// The <see cref="UnitedColumnType"/>.
        /// </returns>
        public static UnitedColumnType GetUnitedColumnType(string columnTypeString)
        {
            var columnTypeObject = new UnitedColumnTypeObject(columnTypeString);
            return columnTypeObject.ColumnType;
        }

        #endregion
    }
}