// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICompositeSpecification.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The CompositeSpecification interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Specifications
{
    /// <summary>
    /// The CompositeSpecification interface.
    /// </summary>
    /// <typeparam name="T">
    /// The generic type.
    /// </typeparam>
    public interface ICompositeSpecification<T> : ISpecification<T>
    {
        #region Public Properties

        /// <summary>
        /// Gets the left.
        /// </summary>
        ISpecification<T> Left { get; }

        /// <summary>
        /// Gets the right.
        /// </summary>
        ISpecification<T> Right { get; }

        #endregion
    }
}