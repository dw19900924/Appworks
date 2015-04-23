// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NoneSpecification.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The none specification.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Specifications
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// The none specification.
    /// </summary>
    /// <typeparam name="T">
    /// The generic type.
    /// </typeparam>
    public sealed class NoneSpecification<T> : Specification<T>
    {
        #region Public Methods and Operators

        /// <summary>
        /// The get expression.
        /// </summary>
        /// <returns>
        /// The <see cref="Expression" />.
        /// </returns>
        public override Expression<Func<T, bool>> GetExpression()
        {
            return o => false;
        }

        #endregion
    }
}