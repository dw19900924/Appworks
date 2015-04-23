// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppInitEventArgs.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The app init event args.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Application
{
    using System;

    using Autofac;

    /// <summary>
    /// The app init event args.
    /// </summary>
    public class AppInitEventArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AppInitEventArgs"/> class.
        /// </summary>
        public AppInitEventArgs()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppInitEventArgs"/> class.
        /// </summary>
        /// <param name="containerBuilder">
        /// The container builder.
        /// </param>
        public AppInitEventArgs(ContainerBuilder containerBuilder)
        {
            this.ContainerBuilder = containerBuilder;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the container builder.
        /// </summary>
        public ContainerBuilder ContainerBuilder { get; private set; }

        #endregion
    }
}