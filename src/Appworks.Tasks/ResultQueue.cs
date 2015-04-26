// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResultQueue.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The result queue.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Tasks
{
    /// <summary>
    /// The result queue.
    /// </summary>
    public class ResultQueue : ResultQueueBase<IResult>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Prevents a default instance of the <see cref="ResultQueue"/> class from being created.
        /// </summary>
        private ResultQueue()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static ResultQueue Instance
        {
            get
            {
                return Nested.Inner;
            }
        }

        #endregion

        /// <summary>
        /// The nested.
        /// </summary>
        private static class Nested
        {
            #region Static Fields

            /// <summary>
            ///     The inner.
            /// </summary>
            internal static readonly ResultQueue Inner = new ResultQueue();

            #endregion
        }
    }
}