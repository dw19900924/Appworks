// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SHA512HashPassword.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The sh a 512 hash password.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Utils.Password
{
    using System.Security.Cryptography;

    /// <summary>
    /// The sh a 512 hash password.
    /// </summary>
    public class SHA512HashPassword : HashPasswordBase
    {
        #region Constants

        /// <summary>
        ///     The hash byte length const.
        /// </summary>
        public const int HashByteLengthConst = 64;

        #endregion

        #region Methods

        /// <summary>
        /// The compute hash.
        /// </summary>
        /// <param name="buffer">
        /// The buffer.
        /// </param>
        /// <returns>
        /// The
        /// <see>
        ///     <cref>byte[]</cref>
        /// </see>
        /// .
        /// </returns>
        protected override byte[] ComputeHash(byte[] buffer)
        {
            var service = new SHA512CryptoServiceProvider();
            return service.ComputeHash(buffer);
        }

        /// <summary>
        /// The get hash byte length.
        /// </summary>
        /// <returns>
        /// The <see cref="int" />.
        /// </returns>
        protected override int GetHashByteLength()
        {
            return HashByteLengthConst;
        }

        #endregion
    }
}