// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PagedList.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The paged list.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// The paged list.
    /// </summary>
    /// <typeparam name="T">
    /// The paged list result type.
    /// </typeparam>
    public class PagedList<T> : ICollection<T>
    {
        #region Fields

        /// <summary>
        /// The data.
        /// </summary>
        private readonly IList<T> data;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{T}" /> class.
        /// </summary>
        public PagedList()
        {
            this.data = new List<T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{T}"/> class.
        /// </summary>
        /// <param name="totalRecords">
        /// The total records.
        /// </param>
        /// <param name="totalPages">
        /// The total pages.
        /// </param>
        /// <param name="pageSize">
        /// The page size.
        /// </param>
        /// <param name="pageNumber">
        /// The page number.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        public PagedList(int? totalRecords, int? totalPages, int? pageSize, int? pageNumber, IList<T> data)
        {
            this.TotalRecords = totalRecords;
            this.TotalPages = totalPages;
            this.PageSize = pageSize;
            this.PageNumber = pageNumber;
            this.data = data;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the count.
        /// </summary>
        public int Count
        {
            get
            {
                return this.data.Count;
            }
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        public IList<T> Data
        {
            get
            {
                return this.data;
            }
        }

        /// <summary>
        /// Gets a value indicating whether is read only.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        public int? PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the page size.
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// Gets or sets the total pages.
        /// </summary>
        public int? TotalPages { get; set; }

        /// <summary>
        /// Gets or sets the total records.
        /// </summary>
        public int? TotalRecords { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        public void Add(T item)
        {
            this.data.Add(item);
        }

        /// <summary>
        /// The clear.
        /// </summary>
        public void Clear()
        {
            this.data.Clear();
        }

        /// <summary>
        /// The contains.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Contains(T item)
        {
            return this.data.Contains(item);
        }

        /// <summary>
        /// The copy to.
        /// </summary>
        /// <param name="array">
        /// The array.
        /// </param>
        /// <param name="arrayIndex">
        /// The array index.
        /// </param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.data.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// The get enumerator.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerator" />.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this.data.GetEnumerator();
        }

        /// <summary>
        /// The remove.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Remove(T item)
        {
            return this.data.Remove(item);
        }

        #endregion

        #region Explicit Interface Methods

        /// <summary>
        /// The get enumerator.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerator" />.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.data.GetEnumerator();
        }

        #endregion
    }
}