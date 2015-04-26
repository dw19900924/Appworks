// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepositoryRuntimeUnitTest.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The repository runtime unit test.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.UnitTests
{
    using Appworks.Application;

    using Autofac;
    using Autofac.Builder;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The repository runtime unit test.
    /// </summary>
    [TestClass]
    public class RepositoryRuntimeUnitTest
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
        /// The get repository test.
        /// </summary>
        [TestMethod]
        public void GetRepositoryTest()
        {
            IRepository<string> strRepo = RepositoryRuntime.Instance.GetRepository<string>();
            IRepository<object> objRepo = RepositoryRuntime.Instance.GetRepository<object>();

            IRepository<string> strRepo1 = RepositoryRuntime.Instance.GetRepository<string>();

            Assert.IsNotNull(strRepo);
            Assert.IsNotNull(strRepo.Context);

            Assert.IsNotNull(objRepo);
            Assert.IsNotNull(objRepo.Context);

            Assert.AreEqual(strRepo, strRepo1);
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
            e.ContainerBuilder.RegisterType<RepositoryFactory>().As<IRepositoryFactory>().SingleInstance();
        }

        #endregion
    }
}