// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Tasks
{
    using System;

    using FluentScheduler;

    /// <summary>
    /// The extensions.
    /// </summary>
    public static class Extensions
    {
        #region Public Methods and Operators

        /// <summary>
        /// The start every.
        /// </summary>
        /// <param name="task">
        /// The task.
        /// </param>
        /// <param name="interval">
        /// The interval.
        /// </param>
        public static void StartEvery(this TaskBase task, int interval)
        {
            TaskManager.AddTask(task.Execute, t => t.WithName(task.Name).ToRunEvery(interval));
        }

        /// <summary>
        /// The start now.
        /// </summary>
        /// <param name="task">
        /// The task.
        /// </param>
        public static void StartNow(this TaskBase task)
        {
            TaskManager.AddTask(task.Execute, t => t.WithName(task.Name).ToRunNow());
        }

        /// <summary>
        /// The start once at.
        /// </summary>
        /// <param name="task">
        /// The task.
        /// </param>
        /// <param name="dateTime">
        /// The date time.
        /// </param>
        public static void StartOnceAt(this TaskBase task, DateTime dateTime)
        {
            TaskManager.AddTask(task.Execute, t => t.WithName(task.Name).ToRunOnceAt(dateTime));
        }

        /// <summary>
        /// The start once in.
        /// </summary>
        /// <param name="task">
        /// The task.
        /// </param>
        /// <param name="interval">
        /// The interval.
        /// </param>
        public static void StartOnceIn(this TaskBase task, int interval)
        {
            TaskManager.AddTask(task.Execute, t => t.WithName(task.Name).ToRunOnceIn(interval));
        }

        #endregion
    }
}