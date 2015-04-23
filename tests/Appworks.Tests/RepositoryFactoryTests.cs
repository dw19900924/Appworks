// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepositoryFactoryTests.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The repository factory tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Tests
{
    using NUnit.Framework;

    /// <summary>
    /// The repository factory tests.
    /// </summary>
    [TestFixture]
    public class RepositoryFactoryTests
    {
        #region Public Methods and Operators

        /// <summary>
        /// The create repository test.
        /// </summary>
        [Test]
        public void CreateRepositoryTest()
        {
            IRepositoryFactory factory = new RepositoryFactory();
            IRepository<object> repository = factory.CreateRepository<object>();

            Assert.IsNotNull(repository);
        }

        #endregion
    }
}