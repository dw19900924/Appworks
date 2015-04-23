// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotNullAttribute.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The not null attribute.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Attributes
{
    using System;

    /// <summary>
    /// The not null attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NotNullAttribute : Attribute
    {
    }
}