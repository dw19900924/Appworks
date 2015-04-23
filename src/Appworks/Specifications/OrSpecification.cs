// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OrSpecification.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The or specification.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Specifications
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// The or specification.
    /// </summary>
    /// <typeparam name="T">
    /// The generic type.
    /// </typeparam>
    public class OrSpecification<T> : CompositeSpecification<T>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OrSpecification{T}"/> class.
        /// </summary>
        /// <param name="left">
        /// The left.
        /// </param>
        /// <param name="right">
        /// The right.
        /// </param>
        public OrSpecification(ISpecification<T> left, ISpecification<T> right)
            : base(left, right)
        {
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
            // var body = Expression.OrElse(Left.GetExpression().Body, Right.GetExpression().Body);
            // return Expression.Lambda<Func<T, bool>>(body, Left.GetExpression().Parameters);
            if (null == this.Left.GetExpression())
            {
                return this.Right.GetExpression();
            }

            return this.Left.GetExpression().Or(this.Right.GetExpression());
        }

        #endregion
    }
}