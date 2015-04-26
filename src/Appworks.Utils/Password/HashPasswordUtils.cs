// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HashPasswordUtils.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The hash password utils.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Utils.Password
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;

    /// <summary>
    /// The hash password utils.
    /// </summary>
    public static class HashPasswordUtils
    {
        #region Public Methods and Operators

        /// <summary>
        /// The block copy.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// The
        /// <see>
        ///     <cref>byte[]</cref>
        /// </see>
        /// .
        /// </returns>
        public static byte[] BlockCopy(string input)
        {
            var bytes = new byte[input.Length * sizeof(char)];
            Buffer.BlockCopy(input.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        /// <summary>
        /// The combine.
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        /// <param name="b">
        /// The b.
        /// </param>
        /// <returns>
        /// The
        /// <see>
        ///     <cref>byte[]</cref>
        /// </see>
        /// .
        /// </returns>
        public static byte[] Combine(byte[] a, byte[] b)
        {
            return a.Concat(b).ToArray();
        }

        /// <summary>
        /// The random salt.
        /// </summary>
        /// <param name="length">
        /// The length.
        /// </param>
        /// <returns>
        /// The
        /// <see>
        ///     <cref>byte[]</cref>
        /// </see>
        /// .
        /// </returns>
        public static byte[] RandomSalt(int length)
        {
            var rng = new RNGCryptoServiceProvider();
            var salt = new byte[length];
            rng.GetBytes(salt);
            return salt;
        }

        /// <summary>
        /// The slow equals.
        /// </summary>
        /// <param name="a">
        /// The a.
        /// </param>
        /// <param name="b">
        /// The b.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }

            return diff == 0;
        }

        #endregion
    }
}