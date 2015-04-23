// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AndNotSpecification.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The and not specification.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Specifications
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// The and not specification.
    /// </summary>
    /// <typeparam name="T">
    /// The generic type.
    /// </typeparam>
    public class AndNotSpecification<T> : CompositeSpecification<T>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AndNotSpecification{T}"/> class.
        /// </summary>
        /// <param name="left">
        /// The left.
        /// </param>
        /// <param name="right">
        /// The right.
        /// </param>
        public AndNotSpecification(ISpecification<T> left, ISpecification<T> right)
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
            UnaryExpression bodyNot = Expression.Not(this.Right.GetExpression().Body);
            Expression<Func<T, bool>> bodyNotExpression = Expression.Lambda<Func<T, bool>>(
                bodyNot, 
                this.Right.GetExpression().Parameters);

            if (null == this.Left.GetExpression())
            {
                return bodyNotExpression;
            }

            return this.Left.GetExpression().And(bodyNotExpression);
        }

        #endregion
    }
}