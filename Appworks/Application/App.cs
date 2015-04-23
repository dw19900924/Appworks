// --------------------------------------------------------------------------------------------------------------------
// <copyright file="App.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The app.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Application
{
    using System;

    using Autofac;
    using Autofac.Builder;
    using Autofac.Configuration;

    /// <summary>
    /// The app.
    /// </summary>
    public class App : IApp
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
            : this(ContainerBuildOptions.None)
        {
            this.ContainerBuilder = new ContainerBuilder();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        /// <param name="buildOptions">
        /// The build options.
        /// </param>
        public App(ContainerBuildOptions buildOptions)
        {
            this.ContainerBuildOptions = buildOptions;
        }

        #endregion

        #region Public Events

        /// <summary>
        /// The initialize.
        /// </summary>
        public event EventHandler<AppInitEventArgs> Initialize;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the container.
        /// </summary>
        public IContainer Container { get; private set; }

        /// <summary>
        /// Gets the container build options.
        /// </summary>
        public ContainerBuildOptions ContainerBuildOptions { get; private set; }

        /// <summary>
        /// Gets the container builder.
        /// </summary>
        public ContainerBuilder ContainerBuilder { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The register module.
        /// </summary>
        /// <typeparam name="T">
        /// The register module type.
        /// </typeparam>
        public void RegisterModule<T>() where T : Module, new()
        {
            this.ContainerBuilder.RegisterModule<T>();
        }

        /// <summary>
        /// The register module.
        /// </summary>
        /// <param name="configSection">
        /// The config section.
        /// </param>
        public void RegisterModule(string configSection)
        {
            this.ContainerBuilder.RegisterModule(new ConfigurationSettingsReader(configSection));
        }

        /// <summary>
        /// The start.
        /// </summary>
        public void Start()
        {
            this.DoInitialize();
            this.OnStart();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The on start.
        /// </summary>
        protected virtual void OnStart()
        {
        }

        /// <summary>
        /// The do initialize.
        /// </summary>
        private void DoInitialize()
        {
            EventHandler<AppInitEventArgs> handler = this.Initialize;
            if (handler != null)
            {
                handler(this, new AppInitEventArgs(this.ContainerBuilder));
            }

            this.Container = this.ContainerBuilder.Build(this.ContainerBuildOptions);
        }

        #endregion
    }
}