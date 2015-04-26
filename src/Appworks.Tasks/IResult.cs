// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IResult.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The Result interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Tasks
{
    using System;

    /// <summary>
    /// The Result interface.
    /// </summary>
    public interface IResult
    {
        #region Public Properties

        /// <summary>
        /// Gets the create time.
        /// </summary>
        DateTime CreateTime { get; }

        /// <summary>
        /// Gets the task info.
        /// </summary>
        ITaskInfo TaskInfo { get; }

        #endregion
    }
}