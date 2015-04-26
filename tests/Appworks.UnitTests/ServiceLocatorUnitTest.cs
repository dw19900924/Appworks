// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceLocatorUnitTest.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The service locator unit test.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.UnitTests
{
    using Appworks.Application;
    using Appworks.Repositories.Dapper;

    using Autofac;
    using Autofac.Builder;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The service locator unit test.
    /// </summary>
    [TestClass]
    public class ServiceLocatorUnitTest
    {
        #region Public Methods and Operators

        /// <summary>
        /// The init.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        [ClassInitialize]
        public static void Init(TestContext context)
        {
            var app = new App(ContainerBuildOptions.None);
            AppRuntime.Instance.SetCurrentApp(app);

            app.Initialize += AppInitialize;
            app.Start();
        }

        /// <summary>
        /// The get service test.
        /// </summary>
        [TestMethod]
        public void GetServiceTest()
        {
            var unitOfWork = ServiceLocator.Instance.GetService<IUnitOfWork>();
            Assert.IsNotNull(unitOfWork);

            object repositoryFactory = ServiceLocator.Instance.GetService(typeof(IRepositoryFactory));
            Assert.IsNotNull(repositoryFactory);
        }

        /// <summary>
        /// The registered test.
        /// </summary>
        [TestMethod]
        public void RegisteredTest()
        {
            bool unitOfWorkRegistered = ServiceLocator.Instance.Registered(typeof(IUnitOfWork));
            Assert.IsTrue(unitOfWorkRegistered);

            bool repositoryFactoryRegistered = ServiceLocator.Instance.Registered<IRepositoryFactory>();
            Assert.IsTrue(repositoryFactoryRegistered);

            bool repositoryRegistered = ServiceLocator.Instance.Registered<IRepository<object>>();
            Assert.IsFalse(repositoryRegistered);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The app initialize.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void AppInitialize(object sender, AppInitEventArgs e)
        {
            e.ContainerBuilder.RegisterType<DapperContext>().As<IRepositoryContext>();
            e.ContainerBuilder.RegisterType<UnitOfWork>().As<IUnitOfWork>().SingleInstance();
            e.ContainerBuilder.RegisterType<RepositoryFactory>().As<IRepositoryFactory>().SingleInstance();
        }

        #endregion
    }
}