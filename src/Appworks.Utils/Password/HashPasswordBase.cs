// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HashPasswordBase.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The hash password base.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Utils.Password
{
    using System;

    /// <summary>
    /// The hash password base.
    /// </summary>
    public abstract class HashPasswordBase : IHashPassword
    {
        #region Public Properties

        /// <summary>
        /// Gets the hash byte length.
        /// </summary>
        public int HashByteLength
        {
            get
            {
                return this.GetHashByteLength();
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The generate.
        /// </summary>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string Generate(string password)
        {
            byte[] salt = this.GenerateSalt();
            byte[] hash = this.Generate(password, salt);
            return string.Format("{0}:{1}", Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }

        /// <summary>
        /// The validate.
        /// </summary>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <param name="hashValue">
        /// The hash value.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Validate(string password, string hashValue)
        {
            string[] splits = hashValue.Split(':');
            if (splits.Length == 2)
            {
                byte[] salt = Convert.FromBase64String(splits[0]);
                byte[] hash = Convert.FromBase64String(splits[1]);

                return this.Validate(password, salt, hash);
            }

            return false;
        }

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
        protected abstract byte[] ComputeHash(byte[] buffer);

        /// <summary>
        /// The generate.
        /// </summary>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <param name="salt">
        /// The salt.
        /// </param>
        /// <returns>
        /// The
        /// <see>
        ///     <cref>byte[]</cref>
        /// </see>
        /// .
        /// </returns>
        protected byte[] Generate(string password, byte[] salt)
        {
            byte[] bytes = HashPasswordUtils.BlockCopy(password);
            byte[] combined = HashPasswordUtils.Combine(salt, bytes);
            return this.ComputeHash(combined);
        }

        /// <summary>
        /// The generate salt.
        /// </summary>
        /// <returns>
        /// The
        /// <see>
        ///     <cref>byte[]</cref>
        /// </see>
        /// .
        /// </returns>
        protected byte[] GenerateSalt()
        {
            return HashPasswordUtils.RandomSalt(this.HashByteLength);
        }

        /// <summary>
        /// The get hash byte length.
        /// </summary>
        /// <returns>
        /// The <see cref="int" />.
        /// </returns>
        protected abstract int GetHashByteLength();

        /// <summary>
        /// The validate.
        /// </summary>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <param name="salt">
        /// The salt.
        /// </param>
        /// <param name="goodHash">
        /// The good hash.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected bool Validate(string password, byte[] salt, byte[] goodHash)
        {
            byte[] hash = this.Generate(password, salt);
            return HashPasswordUtils.SlowEquals(hash, goodHash);
        }

        #endregion
    }
}