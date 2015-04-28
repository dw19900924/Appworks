// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlSchemaProviderUnitTest.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The sql schema provider unit test.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.UnitTests.DbSchema
{
    using Appworks.Application;
    using Appworks.DbSchema;
    using Appworks.DbSchema.SqlClient;

    using Autofac;
    using Autofac.Builder;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The sql schema provider unit test.
    /// </summary>
    [TestClass]
    public class SqlSchemaProviderUnitTest
    {
        #region Constants

        /// <summary>
        /// The connection string.
        /// </summary>
        private const string ConnectionString =
            "Server=.; Database=Master; User Id=sa; Password=~zl.; MultipleActiveResultSets=true; Persist Security Info=true;";

        #endregion

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
        /// The get database collection test.
        /// </summary>
        [TestMethod]
        public void GetDatabaseCollectionTest()
        {
            var schemaProvider = ServiceLocator.Instance.GetService<IDbSchemaProvider>();
            DatabaseCollection databaseCollection = schemaProvider.GetDatabaseCollection(ConnectionString);

            Assert.IsNotNull(databaseCollection);
            Assert.IsTrue(databaseCollection.Count > 0);
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
            e.ContainerBuilder.RegisterType<SqlSchemaProvider>().As<IDbSchemaProvider>().SingleInstance();
        }

        #endregion
    }
}