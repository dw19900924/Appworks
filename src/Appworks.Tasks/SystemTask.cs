// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemTask.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The system task.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Tasks
{
    /// <summary>
    /// The system task.
    /// </summary>
    public abstract class SystemTask : TaskBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemTask"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        protected SystemTask(string name)
            : base(name)
        {
        }

        #endregion
    }
}