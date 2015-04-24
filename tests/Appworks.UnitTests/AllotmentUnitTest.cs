// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AllotmentUnitTest.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The allotment unit test.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.UnitTests
{
    using System.Data.Common;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The allotment unit test.
    /// </summary>
    [TestClass]
    public class AllotmentUnitTest
    {
        #region Public Methods and Operators

        /// <summary>
        /// The get provider factory test.
        /// </summary>
        [TestMethod]
        public void GetProviderFactoryTest()
        {
            DbProviderFactory providerFactory = Allotment.Instance.GetProviderFactory();

            Assert.IsNotNull(providerFactory);

            Assert.IsNotNull(Allotment.Instance.ProviderName);
            Assert.IsNotNull(Allotment.Instance.ConnectionString);
        }

        #endregion
    }
}