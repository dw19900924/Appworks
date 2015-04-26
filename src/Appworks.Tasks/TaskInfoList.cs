// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskInfoList.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The task info list.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The task info list.
    /// </summary>
    public class TaskInfoList : TaskInfoListBase<ITaskInfo>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Prevents a default instance of the <see cref="TaskInfoList" /> class from being created.
        /// </summary>
        private TaskInfoList()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static TaskInfoList Instance
        {
            get
            {
                return Nested.Inner;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The select and remove.
        /// </summary>
        /// <param name="predicate">
        /// The predicate.
        /// </param>
        /// <returns>
        /// The <see cref="IList{T}"/>.
        /// </returns>
        public IList<ITaskInfo> SelectAndRemove(Func<ITaskInfo, bool> predicate)
        {
            lock (this.Sync)
            {
                List<Guid> taskIds = null;

                try
                {
                    List<ITaskInfo> result = this.List.Where(predicate).ToList();
                    taskIds = result.Select(item => item.TaskId).ToList();
                    return result;
                }
                finally
                {
                    if (taskIds != null && taskIds.Any())
                    {
                        foreach (Guid taskId in taskIds)
                        {
                            ITaskInfo item = this.List.First(i => i.TaskId == taskId);
                            this.List.Remove(item);
                        }
                    }
                }
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
        protected override bool EqualityComparer(ITaskInfo target)
        {
            return this.List.Any(t => t.TaskId == target.TaskId);
        }

        #endregion

        /// <summary>
        /// The nested.
        /// </summary>
        private static class Nested
        {
            #region Static Fields

            /// <summary>
            /// The inner.
            /// </summary>
            internal static readonly TaskInfoList Inner = new TaskInfoList();

            #endregion
        }
    }
}