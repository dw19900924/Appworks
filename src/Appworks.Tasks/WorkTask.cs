// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkTask.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The work task.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Tasks
{
    using System.Collections.Generic;

    /// <summary>
    /// The work task.
    /// </summary>
    /// <typeparam name="T">
    /// The task info generic type.
    /// </typeparam>
    public abstract class WorkTask<T> : TaskBase where T : ITaskInfo
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkTask{T}"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="taskInfo">
        /// The task info.
        /// </param>
        protected WorkTask(string name, T taskInfo)
            : base(name)
        {
            this.TaskInfos = new List<T>(1) { taskInfo };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkTask{T}"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="taskInfos">
        /// The task infos.
        /// </param>
        protected WorkTask(string name, IList<T> taskInfos)
            : base(name)
        {
            this.TaskInfos = taskInfos;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the task info.
        /// </summary>
        public IList<T> TaskInfos { get; private set; }

        #endregion
    }
}