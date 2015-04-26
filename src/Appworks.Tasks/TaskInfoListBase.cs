// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskInfoListBase.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The task info list base.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Threading;

    /// <summary>
    /// The task info list base.
    /// </summary>
    /// <typeparam name="T">
    /// The generic type. 
    /// </typeparam>
    public abstract class TaskInfoListBase<T> where T : class
    {
        #region Fields

        /// <summary>
        /// The list.
        /// </summary>
        protected readonly List<T> List;

        /// <summary>
        /// The sync.
        /// </summary>
        protected readonly object Sync = new object();

        /// <summary>
        /// The auto reset event.
        /// </summary>
        private readonly AutoResetEvent autoResetEvent;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskInfoListBase{T}" /> class.
        /// </summary>
        protected TaskInfoListBase()
        {
            this.autoResetEvent = new AutoResetEvent(false);
            this.List = new List<T>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the auto reset event.
        /// </summary>
        public AutoResetEvent AutoResetEvent
        {
            get
            {
                return this.autoResetEvent;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="target">
        /// The target.
        /// </param>
        public void Add(T target)
        {
            lock (this.Sync)
            {
                this.List.Add(target);

                this.AutoResetEvent.Set();
            }
        }

        /// <summary>
        /// The add range.
        /// </summary>
        /// <param name="targetList">
        /// The target list.
        /// </param>
        public void AddRange(List<T> targetList)
        {
            lock (this.Sync)
            {
                this.List.AddRange(targetList);

                this.AutoResetEvent.Set();
            }
        }

        /// <summary>
        /// The clear.
        /// </summary>
        public void Clear()
        {
            lock (this.Sync)
            {
                this.List.Clear();
            }
        }

        /// <summary>
        /// The first or default.
        /// </summary>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public T FirstOrDefault()
        {
            lock (this.Sync)
            {
                return this.List.FirstOrDefault();
            }
        }

        /// <summary>
        /// The clone.
        /// </summary>
        /// <param name="realObj">
        /// The real obj.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public T Clone(T realObj)
        {
            using (Stream objectStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, realObj);
                objectStream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(objectStream);
            }
        }

        /// <summary>
        /// The contains.
        /// </summary>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Contains(T target)
        {
            lock (this.Sync)
            {
                return this.EqualityComparer(target);
            }
        }

        /// <summary>
        /// The count.
        /// </summary>
        /// <returns>
        /// The <see cref="int" />.
        /// </returns>
        public int Count()
        {
            lock (this.Sync)
            {
                return this.List.Count;
            }
        }

        /// <summary>
        /// The set property value.
        /// </summary>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public void SetPropertyValue(T target, string propertyName, object value)
        {
            lock (this.Sync)
            {
                var propertyInfo = typeof(T).GetProperty(propertyName);
                propertyInfo.SetValue(target, value, null);
            }
        }

        /// <summary>
        /// The remove.
        /// </summary>
        /// <param name="target">
        /// The target.
        /// </param>
        public void Remove(T target)
        {
            lock (this.Sync)
            {
                this.List.Remove(this.List.FirstOrDefault(t => t.Equals(target)));
            }
        }

        /// <summary>
        /// The select.
        /// </summary>
        /// <param name="selector">
        /// The selector.
        /// </param>
        /// <returns>
        /// The <see cref="IList{T}"/>.
        /// </returns>
        public IList<T> Select(Func<T, T> selector)
        {
            lock (this.Sync)
            {
                return this.List.Select(selector).ToList();
            }
        }

        /// <summary>
        /// The find.
        /// </summary>
        /// <param name="match">
        /// The match.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public T Find(Predicate<T> match)
        {
            lock (this.Sync)
            {
                return this.List.Find(match);
            }
        }

        /// <summary>
        /// The find all.
        /// </summary>
        /// <param name="match">
        /// The match.
        /// </param>
        /// <returns>
        /// The <see cref="IList{T}"/>.
        /// </returns>
        public IList<T> FindAll(Predicate<T> match)
        {
            lock (this.Sync)
            {
                return this.List.FindAll(match).ToList();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The equality comparer.
        /// </summary>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected abstract bool EqualityComparer(T target);

        #endregion
    }
}