// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISpecification.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The Specification interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Specifications
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// The Specification interface.
    /// </summary>
    /// <typeparam name="T">
    /// The generic type.
    /// </typeparam>
    public interface ISpecification<T>
    {
        #region Public Properties

        /// <summary>
        /// Gets the prefix.
        /// </summary>
        string Prefix { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The and.
        /// </summary>
        /// <param name="other">
        /// The other.
        /// </param>
        /// <returns>
        /// The <see cref="ISpecification{T}"/>.
        /// </returns>
        ISpecification<T> And(ISpecification<T> other);

        /// <summary>
        /// The and not.
        /// </summary>
        /// <param name="other">
        /// The other.
        /// </param>
        /// <returns>
        /// The <see cref="ISpecification{T}"/>.
        /// </returns>
        ISpecification<T> AndNot(ISpecification<T> other);

        /// <summary>
        /// The get expression.
        /// </summary>
        /// <returns>
        /// The <see cref="Expression" />.
        /// </returns>
        Expression<Func<T, bool>> GetExpression();

        /// <summary>
        /// The is satisfied by.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool IsSatisfiedBy(T obj);

        /// <summary>
        /// The not.
        /// </summary>
        /// <returns>
        /// The <see cref="ISpecification{T}" />.
        /// </returns>
        ISpecification<T> Not();

        /// <summary>
        /// The or.
        /// </summary>
        /// <param name="other">
        /// The other.
        /// </param>
        /// <returns>
        /// The <see cref="ISpecification{T}"/>.
        /// </returns>
        ISpecification<T> Or(ISpecification<T> other);

        /// <summary>
        /// The set prefix.
        /// </summary>
        /// <param name="prefix">
        /// The prefix.
        /// </param>
        void SetPrefix(string prefix);

        #endregion
    }
}