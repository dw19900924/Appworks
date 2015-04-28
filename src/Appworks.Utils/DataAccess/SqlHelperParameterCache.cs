// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlHelperParameterCache.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The sql helper parameter cache.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Appworks.Utils.DataAccess
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Data.SqlClient;

    /// <summary>
    /// The sql helper parameter cache.
    /// </summary>
    public static class SqlHelperParameterCache
    {
        #region Static Fields

        /// <summary>
        ///  The param cache.
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
            params SqlParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            if (string.IsNullOrEmpty(commandText))
            {
                throw new ArgumentNullException("commandText");
            }

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
        ///     <cref>SqlParameter[]</cref>
        /// </see>
        ///     .
        /// </returns>
        public static SqlParameter[] GetCachedParameterSet(string connectionString, string commandText)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            if (string.IsNullOrEmpty(commandText))
            {
                throw new ArgumentNullException("commandText");
            }

            string hashKey = connectionString + ":" + commandText;

            var cachedParameters = ParamCache[hashKey] as SqlParameter[];
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
        ///     <cref>SqlParameter[]</cref>
        /// </see>
        ///     .
        /// </returns>
        public static SqlParameter[] GetSpParameterSet(string connectionString, string spName)
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
        ///     <cref>SqlParameter[]</cref>
        /// </see>
        ///     .
        /// </returns>
        public static SqlParameter[] GetSpParameterSet(
            string connectionString, 
            string spName, 
            bool includeReturnValueParameter)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            if (string.IsNullOrEmpty(spName))
            {
                throw new ArgumentNullException("spName");
            }

            using (var connection = new SqlConnection(connectionString))
            {
                return GetSpParameterSetInternal(connection, spName, includeReturnValueParameter);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get sp parameter set.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="spName">
        /// The sp name.
        /// </param>
        /// <returns>
        /// The <see>
        ///     <cref>SqlParameter[]</cref>
        /// </see>
        ///     .
        /// </returns>
        internal static SqlParameter[] GetSpParameterSet(SqlConnection connection, string spName)
        {
            return GetSpParameterSet(connection, spName, false);
        }

        /// <summary>
        /// The get sp parameter set.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="spName">
        /// The sp name.
        /// </param>
        /// <param name="includeReturnValueParameter">
        /// The include return value parameter.
        /// </param>
        /// <returns>
        /// The <see>
        ///     <cref>SqlParameter[]</cref>
        /// </see>
        ///     .
        /// </returns>
        internal static SqlParameter[] GetSpParameterSet(
            SqlConnection connection, 
            string spName, 
            bool includeReturnValueParameter)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            using (var clonedConnection = (SqlConnection)((ICloneable)connection).Clone())
            {
                return GetSpParameterSetInternal(clonedConnection, spName, includeReturnValueParameter);
            }
        }

        /// <summary>
        /// The clone parameters.
        /// </summary>
        /// <param name="originalParameters">
        /// The original parameters.
        /// </param>
        /// <returns>
        /// The <see>
        ///     <cref>SqlParameter[]</cref>
        /// </see>
        ///     .
        /// </returns>
        private static SqlParameter[] CloneParameters(SqlParameter[] originalParameters)
        {
            var clonedParameters = new SqlParameter[originalParameters.Length];

            for (int i = 0, j = originalParameters.Length; i < j; i++)
            {
                clonedParameters[i] = (SqlParameter)((ICloneable)originalParameters[i]).Clone();
            }

            return clonedParameters;
        }

        /// <summary>
        /// The discover sp parameter set.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="spName">
        /// The sp name.
        /// </param>
        /// <param name="includeReturnValueParameter">
        /// The include return value parameter.
        /// </param>
        /// <returns>
        /// The <see>
        ///     <cref>SqlParameter[]</cref>
        /// </see>
        ///     .
        /// </returns>
        private static SqlParameter[] DiscoverSpParameterSet(
            SqlConnection connection, 
            string spName, 
            bool includeReturnValueParameter)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            if (string.IsNullOrEmpty(spName))
            {
                throw new ArgumentNullException("spName");
            }

            var cmd = new SqlCommand(spName, connection) { CommandType = CommandType.StoredProcedure };

            connection.Open();
            SqlCommandBuilder.DeriveParameters(cmd);
            connection.Close();

            if (!includeReturnValueParameter)
            {
                cmd.Parameters.RemoveAt(0);
            }

            var discoveredParameters = new SqlParameter[cmd.Parameters.Count];

            cmd.Parameters.CopyTo(discoveredParameters, 0);

            // Init the parameters with a DBNull value
            foreach (SqlParameter discoveredParameter in discoveredParameters)
            {
                discoveredParameter.Value = DBNull.Value;
            }

            return discoveredParameters;
        }

        /// <summary>
        /// The get sp parameter set internal.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="spName">
        /// The sp name.
        /// </param>
        /// <param name="includeReturnValueParameter">
        /// The include return value parameter.
        /// </param>
        /// <returns>
        /// The <see>
        ///     <cref>SqlParameter[]</cref>
        /// </see>
        ///     .
        /// </returns>
        private static SqlParameter[] GetSpParameterSetInternal(
            SqlConnection connection, 
            string spName, 
            bool includeReturnValueParameter)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            if (string.IsNullOrEmpty(spName))
            {
                throw new ArgumentNullException("spName");
            }

            string hashKey = connection.ConnectionString + ":" + spName
                             + (includeReturnValueParameter ? ":include ReturnValue Parameter" : string.Empty);

            var cachedParameters = ParamCache[hashKey] as SqlParameter[];
            if (cachedParameters == null)
            {
                var spParameters = DiscoverSpParameterSet(connection, spName, includeReturnValueParameter);
                ParamCache[hashKey] = spParameters;
                cachedParameters = spParameters;
            }

            return CloneParameters(cachedParameters);
        }

        #endregion
    }
}