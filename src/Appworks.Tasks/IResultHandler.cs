// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IResultHandler.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The ResultHandler interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Tasks
{
    /// <summary>
    /// The ResultHandler interface.
    /// </summary>
    /// <typeparam name="T">
    /// The task result generic type.
    /// </typeparam>
    public interface IResultHandler<in T>
        where T : IResult
    {
        #region Public Methods and Operators

        /// <summary>
        /// The process.
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        void Process(T result);

        #endregion
    }
}