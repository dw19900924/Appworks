// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParameterRebinder.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The parameter rebinder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Specifications
{
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    ///     The parameter rebinder.
    /// </summary>
    internal class ParameterRebinder : ExpressionVisitor
    {
        #region Fields

        /// <summary>
        /// The map.
        /// </summary>
        private readonly Dictionary<ParameterExpression, ParameterExpression> map;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterRebinder"/> class.
        /// </summary>
        /// <param name="map">
        /// The map.
        /// </param>
        internal ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The replace parameters.
        /// </summary>
        /// <param name="map">
        /// The map.
        /// </param>
        /// <param name="exp">
        /// The exp.
        /// </param>
        /// <returns>
        /// The <see cref="Expression"/>.
        /// </returns>
        internal static Expression ReplaceParameters(
            Dictionary<ParameterExpression, ParameterExpression> map, 
            Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }

        /// <summary>
        /// The visit parameter.
        /// </summary>
        /// <param name="p">
        /// The p.
        /// </param>
        /// <returns>
        /// The <see cref="Expression"/>.
        /// </returns>
        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression replacement;
            if (this.map.TryGetValue(p, out replacement))
            {
                p = replacement;
            }

            return base.VisitParameter(p);
        }

        #endregion
    }
}