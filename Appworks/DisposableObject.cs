// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisposableObject.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The disposable object.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks
{
    using System;
    using System.Runtime.ConstrainedExecution;

    /// <summary>
    /// The disposable object.
    /// </summary>
    public abstract class DisposableObject : CriticalFinalizerObject, IDisposable
    {
        #region Constructors and Destructors

        /// <summary>
        /// Finalizes an instance of the <see cref="DisposableObject" /> class.
        /// </summary>
        ~DisposableObject()
        {
            this.Dispose(false);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            this.ExplicitDispose();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The dispose.
        /// </summary>
        /// <param name="disposing">
        /// The disposing.
        /// </param>
        protected abstract void Dispose(bool disposing);

        /// <summary>
        /// The explicit dispose.
        /// </summary>
        protected void ExplicitDispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}