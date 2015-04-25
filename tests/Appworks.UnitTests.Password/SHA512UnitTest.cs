// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SHA512UnitTest.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The sh a 512 unit test.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.UnitTests.Password
{
    using Appworks.Password;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The sh a 512 unit test.
    /// </summary>
    [TestClass]
    public class SHA512UnitTest
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
            this.hashPassword = new SHA512HashPassword();
        }

        /// <summary>
        /// The validate test.
        /// </summary>
        [TestMethod]
        public void ValidateTest()
        {
            const string TestPwd = "abc123";

            string hashValue = this.hashPassword.Generate(TestPwd);
            Assert.IsNotNull(hashValue);

            bool result = this.hashPassword.Validate(TestPwd, hashValue);
            Assert.IsTrue(result);
        }

        #endregion
    }
}