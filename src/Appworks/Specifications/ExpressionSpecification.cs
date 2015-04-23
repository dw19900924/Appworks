// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpressionSpecification.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The expression specification.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Specifications
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// The expression specification.
    /// </summary>
    /// <typeparam name="T">
    /// The generic type.
    /// </typeparam>
    internal sealed class ExpressionSpecification<T> : Specification<T>
    {
        #region Fields

        /// <summary>
        /// The expression.
        /// </summary>
        private readonly Expression<Func<T, bool>> expression;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionSpecification{T}"/> class.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        public ExpressionSpecification(Expression<Func<T, bool>> expression)
        {
            this.expression = expression;
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
            return this.expression;
        }

        #endregion
    }
}