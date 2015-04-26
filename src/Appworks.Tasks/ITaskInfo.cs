// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITaskInfo.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The TaskInfo interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Tasks
{
    using System;

    /// <summary>
    /// The TaskInfo interface.
    /// </summary>
    public interface ITaskInfo
    {
        #region Public Properties

        /// <summary>
        /// Gets the create time.
        /// </summary>
        DateTime CreateTime { get; }

        /// <summary>
        /// Gets the interval.
        /// </summary>
        int Interval { get; }

        /// <summary>
        /// Gets the task id.
        /// </summary>
        Guid TaskId { get; }

        #endregion
    }
}