// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISpecificationParser.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The SpecificationParser interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Specifications
{
    /// <summary>
    /// The SpecificationParser interface.
    /// </summary>
    /// <typeparam name="TCriteria">
    /// The criteria type.
    /// </typeparam>
    public interface ISpecificationParser<out TCriteria>
    {
        #region Public Methods and Operators

        /// <summary>
        /// The parse.
        /// </summary>
        /// <param name="specification">
        /// The specification.
        /// </param>
        /// <typeparam name="T">
        /// The generic type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="TCriteria"/>.
        /// </returns>
        TCriteria Parse<T>(ISpecification<T> specification);

        #endregion
    }
}