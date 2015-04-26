// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskBase.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The task base.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Tasks
{
    using FluentScheduler;

    /// <summary>
    /// The task base.
    /// </summary>
    public abstract class TaskBase : ITask
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskBase"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        protected TaskBase(string name)
        {
            this.Name = name;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The do.
        /// </summary>
        public abstract void Do();

        /// <summary>
        /// The execute.
        /// </summary>
        public virtual void Execute()
        {
            //// todo: before log
            
            this.Do();

            //// todo: after log
        }

        #endregion
    }
}