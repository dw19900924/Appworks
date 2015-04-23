// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IgnoreAttribute.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The ignore attribute.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Attributes
{
    using System;

    /// <summary>
    /// The ignore attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreAttribute : Attribute
    {
    }
}