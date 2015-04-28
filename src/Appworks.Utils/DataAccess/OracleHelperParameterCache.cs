// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OracleHelperParameterCache.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The oracle helper parameter cache.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Utils.DataAccess
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Data.OracleClient;

    /// <summary>
    /// The oracle helper parameter cache.
    /// </summary>
    public static class OracleHelperParameterCache
    {
        #region Static Fields

        /// <summary>
        /// The param cache.
        /// </summary>
        private static readonly Hashtable ParamCache = Hashtable.Synchronized(new Hashtable());

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The cache parameter set.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <param name="commandText">
        /// The command text.
        /// </param>
        /// <param name="commandParameters">
        /// The command parameters.
        /// </param>
        public static void CacheParameterSet(
            string connectionString, 
            string commandText, 
            params OracleParameter[] commandParameters)
        {
            string hashKey = connectionString + ":" + commandText;

            ParamCache[hashKey] = commandParameters;
        }

        /// <summary>
        /// The get cached parameter set.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <param name="commandText">
        /// The command text.
        /// </param>
        /// <returns>
        /// The <see>
        ///     <cref>OracleParameter[]</cref>
        /// </see>
        ///     .
        /// </returns>
        public static OracleParameter[] GetCachedParameterSet(string connectionString, string commandText)
        {
            string hashKey = connectionString + ":" + commandText;

            var cachedParameters = (OracleParameter[])ParamCache[hashKey];

            if (cachedParameters == null)
            {
                return null;
            }

            return CloneParameters(cachedParameters);
        }

        /// <summary>
        /// The get sp parameter set.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <param name="spName">
        /// The sp name.
        /// </param>
        /// <returns>
        /// The <see>
        ///     <cref>OracleParameter[]</cref>
        /// </see>
        ///     .
        /// </returns>
        public static OracleParameter[] GetSpParameterSet(string connectionString, string spName)
        {
            return GetSpParameterSet(connectionString, spName, false);
        }

        /// <summary>
        /// The get sp parameter set.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <param name="spName">
        /// The sp name.
        /// </param>
        /// <param name="includeReturnValueParameter">
        /// The include return value parameter.
        /// </param>
        /// <returns>
        /// The <see>
        ///     <cref>OracleParameter[]</cref>
        /// </see>
        ///     .
        /// </returns>
        public static OracleParameter[] GetSpParameterSet(
            string connectionString, 
            string spName, 
            bool includeReturnValueParameter)
        {
            string hashKey = connectionString + ":" + spName
                             + (includeReturnValueParameter ? ":include ReturnValue Parameter" : string.Empty);

            OracleParameter[] cachedParameters;

            cachedParameters = (OracleParameter[])ParamCache[hashKey];

            if (cachedParameters == null)
            {
                cachedParameters =
                    (OracleParameter[])
                    (ParamCache[hashKey] = DiscoverSpParameterSet(connectionString, spName, includeReturnValueParameter));
            }

            return CloneParameters(cachedParameters);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The clone parameters.
        /// </summary>
        /// <param name="originalParameters">
        /// The original parameters.
        /// </param>
        /// <returns>
        /// The <see>
        ///     <cref>OracleParameter[]</cref>
        /// </see>
        ///     .
        /// </returns>
        private static OracleParameter[] CloneParameters(OracleParameter[] originalParameters)
        {
            var clonedParameters = new OracleParameter[originalParameters.Length];

            for (int i = 0, j = originalParameters.Length; i < j; i++)
            {
                clonedParameters[i] = (OracleParameter)((ICloneable)originalParameters[i]).Clone();
            }

            return clonedParameters;
        }

        /// <summary>
        /// The discover sp parameter set.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <param name="spName">
        /// The sp name.
        /// </param>
        /// <param name="includeReturnValueParameter">
        /// The include return value parameter.
        /// </param>
        /// <returns>
        /// The <see>
        ///     <cref>OracleParameter[]</cref>
        /// </see>
        ///     .
        /// </returns>
        private static OracleParameter[] DiscoverSpParameterSet(
            string connectionString, 
            string spName, 
            bool includeReturnValueParameter)
        {
            using (var cn = new OracleConnection(connectionString))
            using (var cmd = new OracleCommand(spName, cn))
            {
                cn.Open();
                cmd.CommandType = CommandType.StoredProcedure;

                OracleCommandBuilder.DeriveParameters(cmd);

                if (!includeReturnValueParameter)
                {
                    if (ParameterDirection.ReturnValue == cmd.Parameters[0].Direction)
                    {
                        cmd.Parameters.RemoveAt(0);
                    }
                }

                var discoveredParameters = new OracleParameter[cmd.Parameters.Count];

                cmd.Parameters.CopyTo(discoveredParameters, 0);

                return discoveredParameters;
            }
        }

        #endregion
    }
}