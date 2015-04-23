// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IApp.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The App interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Application
{
    using System;

    using Autofac;
    using Autofac.Builder;

    /// <summary>
    /// The App interface.
    /// </summary>
    public interface IApp
    {
        #region Public Events

        /// <summary>
        /// The initialize.
        /// </summary>
        event EventHandler<AppInitEventArgs> Initialize;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the container.
        /// </summary>
        IContainer Container { get; }

        /// <summary>
        /// Gets the container build options.
        /// </summary>
        ContainerBuildOptions ContainerBuildOptions { get; }

        /// <summary>
        /// Gets the container builder.
        /// </summary>
        ContainerBuilder ContainerBuilder { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The register module.
        /// </summary>
        /// <typeparam name="T">
        /// The register module type.
        /// </typeparam>
        void RegisterModule<T>() where T : Module, new();

        /// <summary>
        /// The register module.
        /// </summary>
        /// <param name="configSection">
        /// The config section.
        /// </param>
        void RegisterModule(string configSection);

        /// <summary>
        /// The start.
        /// </summary>
        void Start();

        #endregion
    }
}