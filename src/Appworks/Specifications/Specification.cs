// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Specification.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The specification.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Specifications
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// The specification.
    /// </summary>
    /// <typeparam name="T">
    /// The generic type.
    /// </typeparam>
    public abstract class Specification<T> : ISpecification<T>
    {
        #region Public Properties

        /// <summary>
        /// Gets the prefix.
        /// </summary>
        public string Prefix { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The null.
        /// </summary>
        /// <returns>
        /// The <see cref="Specification{T}"/>.
        /// </returns>
        public static Specification<T> Null()
        {
            return new ExpressionSpecification<T>(null);
        }

        /// <summary>
        /// The eval.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <returns>
        /// The <see cref="Specification{T}"/>.
        /// </returns>
        public static Specification<T> Eval(Expression<Func<T, bool>> expression)
        {
            return new ExpressionSpecification<T>(expression);
        }

        /// <summary>
        /// The and.
        /// </summary>
        /// <param name="other">
        /// The other.
        /// </param>
        /// <returns>
        /// The <see cref="ISpecification{T}"/>.
        /// </returns>
        public ISpecification<T> And(ISpecification<T> other)
        {
            return new AndSpecification<T>(this, other);
        }

        /// <summary>
        /// The and not.
        /// </summary>
        /// <param name="other">
        /// The other.
        /// </param>
        /// <returns>
        /// The <see cref="ISpecification{T}"/>.
        /// </returns>
        public ISpecification<T> AndNot(ISpecification<T> other)
        {
            return new AndNotSpecification<T>(this, other);
        }

        /// <summary>
        ///     The get expression.
        /// </summary>
        /// <returns>
        ///     The <see cref="Expression" />.
        /// </returns>
        public abstract Expression<Func<T, bool>> GetExpression();

        /// <summary>
        /// The is satisfied by.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public virtual bool IsSatisfiedBy(T obj)
        {
            return this.GetExpression().Compile()(obj);
        }

        /// <summary>
        ///     The not.
        /// </summary>
        /// <returns>
        ///     The <see cref="ISpecification{T}" />.
        /// </returns>
        public ISpecification<T> Not()
        {
            return new NotSpecification<T>(this);
        }

        /// <summary>
        /// The or.
        /// </summary>
        /// <param name="other">
        /// The other.
        /// </param>
        /// <returns>
        /// The <see cref="ISpecification{T}"/>.
        /// </returns>
        public ISpecification<T> Or(ISpecification<T> other)
        {
            return new OrSpecification<T>(this, other);
        }

        /// <summary>
        /// The set prefix.
        /// </summary>
        /// <param name="prefix">
        /// The prefix.
        /// </param>
        public void SetPrefix(string prefix)
        {
            this.Prefix = prefix;
        }

        #endregion
    }
}