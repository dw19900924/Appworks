// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotSpecification.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The not specification.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Specifications
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// The not specification.
    /// </summary>
    /// <typeparam name="T">
    /// The generic type.
    /// </typeparam>
    public class NotSpecification<T> : Specification<T>
    {
        #region Fields

        /// <summary>
        /// The spec.
        /// </summary>
        private readonly ISpecification<T> spec;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NotSpecification{T}"/> class.
        /// </summary>
        /// <param name="specification">
        /// The specification.
        /// </param>
        public NotSpecification(ISpecification<T> specification)
        {
            this.spec = specification;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get expression.
        /// </summary>
        /// <returns>
        /// The <see cref="Expression" />.
        /// </returns>
        public override Expression<Func<T, bool>> GetExpression()
        {
            UnaryExpression body = Expression.Not(this.spec.GetExpression().Body);
            return Expression.Lambda<Func<T, bool>>(body, this.spec.GetExpression().Parameters);
        }

        #endregion
    }
}