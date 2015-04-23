// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IServiceLocator.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The ServiceLocator interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks
{
    using System;

    /// <summary>
    /// The ServiceLocator interface.
    /// </summary>
    public interface IServiceLocator
    {
        #region Public Methods and Operators

        /// <summary>
        /// The get service.
        /// </summary>
        /// <typeparam name="T">
        /// The generic type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="T" />.
        /// </returns>
        T GetService<T>() where T : class;

        /// <summary>
        /// The get service.
        /// </summary>
        /// <param name="arguments">
        /// The arguments.
        /// </param>
        /// <typeparam name="T">
        /// The generic type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        T GetService<T>(object arguments) where T : class;

        /// <summary>
        /// The get service.
        /// </summary>
        /// <param name="serviceType">
        /// The service type.
        /// </param>
        /// <param name="arguments">
        /// The arguments.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        object GetService(Type serviceType, object arguments);

        /// <summary>
        /// The registered.
        /// </summary>
        /// <typeparam name="T">
        /// The generic type.
        /// </typeparam>
        /// <returns>
        /// The <see cref="bool" />.
        /// </returns>
        bool Registered<T>();

        /// <summary>
        /// The registered.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool Registered(Type type);

        #endregion
    }
}