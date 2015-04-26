// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHashPassword.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The HashPassword interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Utils.Password
{
    /// <summary>
    /// The HashPassword interface.
    /// </summary>
    public interface IHashPassword
    {
        #region Public Properties

        /// <summary>
        ///     Gets the hash byte length.
        /// </summary>
        int HashByteLength { get; }

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
        string Generate(string password);

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
        bool Validate(string password, string hashValue);

        #endregion
    }
}