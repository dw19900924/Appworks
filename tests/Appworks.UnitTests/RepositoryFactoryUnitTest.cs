// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepositoryFactoryUnitTest.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The repository factory unit test.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.UnitTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The repository factory unit test.
    /// </summary>
    [TestClass]
    public class RepositoryFactoryUnitTest
    {
        #region Public Methods and Operators

        /// <summary>
        /// The create repository test.
        /// </summary>
        [TestMethod]
        public void CreateRepositoryTest()
        {
            IRepositoryFactory factory = new RepositoryFactory();
            IRepository<object> repository = factory.CreateRepository<object>();

            Assert.IsNotNull(repository);
            Assert.IsNotNull(repository.Context);
        }

        #endregion
    }
}