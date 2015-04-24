// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppRuntime.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The app runtime.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Application
{
    /// <summary>
    /// The app runtime.
    /// </summary>
    public class AppRuntime
    {
        #region Fields

        /// <summary>
        /// The current app.
        /// </summary>
        private IApp currentApp;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Prevents a default instance of the <see cref="AppRuntime" /> class from being created.
        /// </summary>
        private AppRuntime()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static AppRuntime Instance
        {
            get
            {
                return Nested.Inner;
            }
        }

        /// <summary>
        /// Gets the current app.
        /// </summary>
        public IApp CurrentApp
        {
            get
            {
                return this.currentApp;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The set current app.
        /// </summary>
        /// <param name="app">
        /// The app.
        /// </param>
        public void SetCurrentApp(IApp app)
        {
            this.currentApp = app;
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
            internal static readonly AppRuntime Inner = new AppRuntime();

            #endregion
        }
    }
}