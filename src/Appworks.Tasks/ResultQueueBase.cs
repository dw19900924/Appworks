// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResultQueueBase.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The result queue base.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Tasks
{
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// The result queue base.
    /// </summary>
    /// <typeparam name="T">
    /// The generic type.
    /// </typeparam>
    public abstract class ResultQueueBase<T> where T : class 
    {
        #region Fields

        /// <summary>
        /// The auto reset event.
        /// </summary>
        private readonly AutoResetEvent autoResetEvent;

        /// <summary>
        /// The queue.
        /// </summary>
        private readonly Queue<T> queue = new Queue<T>();

        /// <summary>
        /// The sync.
        /// </summary>
        private readonly object sync = new object();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultQueueBase{T}"/> class.
        /// </summary>
        protected ResultQueueBase()
        {
            this.autoResetEvent = new AutoResetEvent(false);
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

        /// <summary>
        /// Gets the count.
        /// </summary>
        public int Count
        {
            get
            {
                lock (this.sync)
                {
                    return this.queue.Count;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether has value.
        /// </summary>
        public bool HasValue
        {
            get
            {
                return this.Count != 0;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The de queue.
        /// </summary>
        /// <returns>
        /// The <see cref="T" />.
        /// </returns>
        public T DeQueue()
        {
            lock (this.sync)
            {
                if (this.queue.Count > 0)
                {
                    return this.queue.Dequeue();
                }

                return default(T);
            }
        }

        /// <summary>
        /// The en queue.
        /// </summary>
        /// <param name="target">
        /// The target.
        /// </param>
        public void EnQueue(T target)
        {
            lock (this.sync)
            {
                this.queue.Enqueue(target);

                this.AutoResetEvent.Set();
            }
        }

        #endregion
    }
}