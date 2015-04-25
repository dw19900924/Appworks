// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SHA256UnitTest.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The sh a 256 unit test.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.UnitTests.Password
{
    using Appworks.Password;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The sh a 256 unit test.
    /// </summary>
    [TestClass]
    public class SHA256UnitTest
    {
        #region Fields

        /// <summary>
        /// The hash password.
        /// </summary>
        private IHashPassword hashPassword;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The init.
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            this.hashPassword = new SHA256HashPassword();
        }

        /// <summary>
        /// The validate test.
        /// </summary>
        [TestMethod]
        public void ValidateTest()
        {
            const string TestPwd = "abcd1234";

            string hashValue = this.hashPassword.Generate(TestPwd);
            Assert.IsNotNull(hashValue);

            bool result = this.hashPassword.Validate(TestPwd, hashValue);
            Assert.IsTrue(result);
        }

        #endregion
    }
}