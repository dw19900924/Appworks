// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitOfWorkUnitTest.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The unit of work unit test.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.UnitTests
{
    using System;
    using System.Data;

    using Appworks.Application;
    using Appworks.Repositories.Dapper;

    using Autofac;
    using Autofac.Builder;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The unit of work unit test.
    /// </summary>
    [TestClass]
    public class UnitOfWorkUnitTest
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
        /// The begin and commit transaction test.
        /// </summary>
        [TestMethod]
        public void BeginAndCommitTransactionTest()
        {
            var unitOfWork = ServiceLocator.Instance.GetService<IUnitOfWork>();
            unitOfWork.BeginTransaction(IsolationLevel.ReadCommitted);

            Assert.IsNotNull(unitOfWork.Transaction);
            Assert.IsNotNull(unitOfWork.Transaction.Connection);

            Assert.IsTrue(unitOfWork.Transaction.Connection.State == ConnectionState.Open);

            unitOfWork.Commit();

            Assert.IsNull(unitOfWork.Transaction.Connection);
        }

        /// <summary>
        /// The double begin transaction test.
        /// </summary>
        [TestMethod]
        public void DoubleBeginTransactionTest()
        {
            var unitOfWork = ServiceLocator.Instance.GetService<IUnitOfWork>();
            unitOfWork.BeginTransaction(IsolationLevel.ReadCommitted);

            Assert.IsNotNull(unitOfWork.Transaction);
            Assert.IsNotNull(unitOfWork.Transaction.Connection);

            Assert.IsTrue(unitOfWork.Transaction.Connection.State == ConnectionState.Open);

            try
            {
                unitOfWork.BeginTransaction(IsolationLevel.ReadCommitted);
            }
            catch (Exception exception)
            {
                Assert.IsTrue(exception.Message.StartsWith("Not finished"));
                unitOfWork.Transaction.Rollback();
            }

            Assert.IsNull(unitOfWork.Transaction.Connection);
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
        }

        #endregion
    }
}