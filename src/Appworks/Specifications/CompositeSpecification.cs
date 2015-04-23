// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompositeSpecification.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The composite specification.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Specifications
{
    /// <summary>
    /// The composite specification.
    /// </summary>
    /// <typeparam name="T">
    /// The generic type.
    /// </typeparam>
    public abstract class CompositeSpecification<T> : Specification<T>, ICompositeSpecification<T>
    {
        #region Fields

        /// <summary>
        /// The left.
        /// </summary>
        private readonly ISpecification<T> left;

        /// <summary>
        /// The right.
        /// </summary>
        private readonly ISpecification<T> right;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeSpecification{T}"/> class.
        /// </summary>
        /// <param name="left">
        /// The left.
        /// </param>
        /// <param name="right">
        /// The right.
        /// </param>
        protected CompositeSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            this.left = left;
            this.right = right;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the left.
        /// </summary>
        public ISpecification<T> Left
        {
            get
            {
                return this.left;
            }
        }

        /// <summary>
        /// Gets the right.
        /// </summary>
        public ISpecification<T> Right
        {
            get
            {
                return this.right;
            }
        }

        #endregion
    }
}