// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OracleHelper.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The oracle connection ownership.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Utils.DataAccess
{
    using System;
    using System.Data;
    using System.Data.OracleClient;

    /// <summary>
    /// The oracle connection ownership.
    /// </summary>
    public enum OracleConnectionOwnership
    {
        /// <summary>
        /// The internal.
        /// </summary>
        Internal, 

        /// <summary>
        /// The external.
        /// </summary>
        External
    }

    /// <summary>
    /// The oracle helper.
    /// </summary>
    public class OracleHelper
    {
        #region Constructors and Destructors

        /// <summary>
        /// Prevents a default instance of the <see cref="OracleHelper"/> class from being created.
        /// </summary>
        private OracleHelper()
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The execute dataset.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <param name="commandType">
        /// The command type.
        /// </param>
        /// <param name="commandText">
        /// The command text.
        /// </param>
        /// <returns>
        /// The <see cref="DataSet"/>.
        /// </returns>
        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
        {
            // pass through the call providing null for the set of OracleParameters
            return ExecuteDataset(connectionString, commandType, commandText, null);
        }

        /// <summary>
        /// The execute dataset.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <param name="commandType">
        /// The command type.
        /// </param>
        /// <param name="commandText">
        /// The command text.
        /// </param>
        /// <param name="commandParameters">
        /// The command parameters.
        /// </param>
        /// <returns>
        /// The <see cref="DataSet"/>.
        /// </returns>
        public static DataSet ExecuteDataset(
            string connectionString, 
            CommandType commandType, 
            string commandText, 
            params OracleParameter[] commandParameters)
        {
            // create & open an OracleConnection, and dispose of it after we are done.
            using (var cn = new OracleConnection(connectionString))
            {
                cn.Open();

                // call the overload that takes a connection in place of the connection string
                return ExecuteDataset(cn, commandType, commandText, commandParameters);
            }
        }

        /// <summary>
        /// The execute dataset.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <param name="spName">
        /// The sp name.
        /// </param>
        /// <param name="parameterValues">
        /// The parameter values.
        /// </param>
        /// <returns>
        /// The <see cref="DataSet"/>.
        /// </returns>
        public static DataSet ExecuteDataset(string connectionString, string spName, params object[] parameterValues)
        {
            // if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // pull the parameters for this stored procedure from the parameter cache (or discover them & populet the cache)
                OracleParameter[] commandParameters = OracleHelperParameterCache.GetSpParameterSet(
                    connectionString, 
                    spName);

                // assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // call the overload that takes an array of OracleParameters
                return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
                
                // otherwise we can just call the SP without params
            return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName);
        }

        /// <summary>
        /// The execute dataset.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="commandType">
        /// The command type.
        /// </param>
        /// <param name="commandText">
        /// The command text.
        /// </param>
        /// <returns>
        /// The <see cref="DataSet"/>.
        /// </returns>
        public static DataSet ExecuteDataset(OracleConnection connection, CommandType commandType, string commandText)
        {
            // pass through the call providing null for the set of OracleParameters
            return ExecuteDataset(connection, commandType, commandText, null);
        }

        /// <summary>
        /// The execute dataset.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="commandType">
        /// The command type.
        /// </param>
        /// <param name="commandText">
        /// The command text.
        /// </param>
        /// <param name="commandParameters">
        /// The command parameters.
        /// </param>
        /// <returns>
        /// The <see cref="DataSet"/>.
        /// </returns>
        public static DataSet ExecuteDataset(
            OracleConnection connection, 
            CommandType commandType, 
            string commandText, 
            params OracleParameter[] commandParameters)
        {
            // create a command and prepare it for execution
            var cmd = new OracleCommand();
            PrepareCommand(cmd, connection, null, commandType, commandText, commandParameters);

            // create the DataAdapter & DataSet
            var da = new OracleDataAdapter(cmd);
            var ds = new DataSet();

            // fill the DataSet using default values for DataTable names, etc.
            da.Fill(ds);

            // return the dataset
            return ds;
        }

        /// <summary>
        /// The execute dataset.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="spName">
        /// The sp name.
        /// </param>
        /// <param name="parameterValues">
        /// The parameter values.
        /// </param>
        /// <returns>
        /// The <see cref="DataSet"/>.
        /// </returns>
        public static DataSet ExecuteDataset(
            OracleConnection connection, 
            string spName, 
            params object[] parameterValues)
        {
            // if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                OracleParameter[] commandParameters =
                    OracleHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);

                // assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // call the overload that takes an array of OracleParameters
                return ExecuteDataset(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
                
                // otherwise we can just call the SP without params
            return ExecuteDataset(connection, CommandType.StoredProcedure, spName);
        }

        /// <summary>
        /// The execute dataset.
        /// </summary>
        /// <param name="transaction">
        /// The transaction.
        /// </param>
        /// <param name="commandType">
        /// The command type.
        /// </param>
        /// <param name="commandText">
        /// The command text.
        /// </param>
        /// <returns>
        /// The <see cref="DataSet"/>.
        /// </returns>
        public static DataSet ExecuteDataset(OracleTransaction transaction, CommandType commandType, string commandText)
        {
            // pass through the call providing null for the set of OracleParameters
            return ExecuteDataset(transaction, commandType, commandText, null);
        }

        /// <summary>
        /// The execute dataset.
        /// </summary>
        /// <param name="transaction">
        /// The transaction.
        /// </param>
        /// <param name="commandType">
        /// The command type.
        /// </param>
        /// <param name="commandText">
        /// The command text.
        /// </param>
        /// <param name="commandParameters">
        /// The command parameters.
        /// </param>
        /// <returns>
        /// The <see cref="DataSet"/>.
        /// </returns>
        public static DataSet ExecuteDataset(
            OracleTransaction transaction, 
            CommandType commandType, 
            string commandText, 
            params OracleParameter[] commandParameters)
        {
            // create a command and prepare it for execution
            var cmd = new OracleCommand();
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);

            // create the DataAdapter & DataSet
            var da = new OracleDataAdapter(cmd);
            var ds = new DataSet();

            // fill the DataSet using default values for DataTable names, etc.
            da.Fill(ds);

            // return the dataset
            return ds;
        }

        /// <summary>
        /// The execute dataset.
        /// </summary>
        /// <param name="transaction">
        /// The transaction.
        /// </param>
        /// <param name="spName">
        /// The sp name.
        /// </param>
        /// <param name="parameterValues">
        /// The parameter values.
        /// </param>
        /// <returns>
        /// The <see cref="DataSet"/>.
        /// </returns>
        public static DataSet ExecuteDataset(
            OracleTransaction transaction, 
            string spName, 
            params object[] parameterValues)
        {
            // if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                OracleParameter[] commandParameters =
                    OracleHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);

                // assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // call the overload that takes an array of OracleParameters
                return ExecuteDataset(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
                
                // otherwise we can just call the SP without params
            return ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
        }

        /// <summary>
        /// The execute non query.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <param name="commandType">
        /// The command type.
        /// </param>
        /// <param name="commandText">
        /// The command text.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText)
        {
            // pass through the call providing null for the set of OracleParameters
            return ExecuteNonQuery(connectionString, commandType, commandText, null);
        }

        /// <summary>
        /// The execute non query.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <param name="commandType">
        /// The command type.
        /// </param>
        /// <param name="commandText">
        /// The command text.
        /// </param>
        /// <param name="commandParameters">
        /// The command parameters.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int ExecuteNonQuery(
            string connectionString, 
            CommandType commandType, 
            string commandText, 
            params OracleParameter[] commandParameters)
        {
            // create & open an OracleConnection, and dispose of it after we are done.
            using (var cn = new OracleConnection(connectionString))
            {
                cn.Open();

                // call the overload that takes a connection in place of the connection string
                return ExecuteNonQuery(cn, commandType, commandText, commandParameters);
            }
        }

        /// <summary>
        /// The execute non query.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <param name="spName">
        /// The sp name.
        /// </param>
        /// <param name="parameterValues">
        /// The parameter values.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int ExecuteNonQuery(string connectionString, string spName, params object[] parameterValues)
        {
            // if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                OracleParameter[] commandParameters = OracleHelperParameterCache.GetSpParameterSet(
                    connectionString, 
                    spName);

                // assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // call the overload that takes an array of OracleParameters
                return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
                
                // otherwise we can just call the SP without params
            return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName);
        }

        /// <summary>
        /// The execute non query.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="commandType">
        /// The command type.
        /// </param>
        /// <param name="commandText">
        /// The command text.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int ExecuteNonQuery(OracleConnection connection, CommandType commandType, string commandText)
        {
            // pass through the call providing null for the set of OracleParameters
            return ExecuteNonQuery(connection, commandType, commandText, null);
        }

        /// <summary>
        /// The execute non query.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="commandType">
        /// The command type.
        /// </param>
        /// <param name="commandText">
        /// The command text.
        /// </param>
        /// <param name="commandParameters">
        /// The command parameters.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int ExecuteNonQuery(
            OracleConnection connection, 
            CommandType commandType, 
            string commandText, 
            params OracleParameter[] commandParameters)
        {
            // create a command and prepare it for execution
            var cmd = new OracleCommand();
            PrepareCommand(cmd, connection, null, commandType, commandText, commandParameters);

            // finally, execute the command.
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// The execute non query.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="spName">
        /// The sp name.
        /// </param>
        /// <param name="parameterValues">
        /// The parameter values.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int ExecuteNonQuery(OracleConnection connection, string spName, params object[] parameterValues)
        {
            // if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                OracleParameter[] commandParameters =
                    OracleHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);

                // assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // call the overload that takes an array of OracleParameters
                return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
                
                // otherwise we can just call the SP without params
            return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
        }

        /// <summary>
        /// The execute non query.
        /// </summary>
        /// <param name="transaction">
        /// The transaction.
        /// </param>
        /// <param name="commandType">
        /// The command type.
        /// </param>
        /// <param name="commandText">
        /// The command text.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int ExecuteNonQuery(OracleTransaction transaction, CommandType commandType, string commandText)
        {
            // pass through the call providing null for the set of OracleParameters
            return ExecuteNonQuery(transaction, commandType, commandText, null);
        }

        /// <summary>
        /// The execute non query.
        /// </summary>
        /// <param name="transaction">
        /// The transaction.
        /// </param>
        /// <param name="commandType">
        /// The command type.
        /// </param>
        /// <param name="commandText">
        /// The command text.
        /// </param>
        /// <param name="commandParameters">
        /// The command parameters.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int ExecuteNonQuery(
            OracleTransaction transaction, 
            CommandType commandType, 
            string commandText, 
            params OracleParameter[] commandParameters)
        {
            // create a command and prepare it for execution
            var cmd = new OracleCommand();
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);

            // finally, execute the command.
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// The execute non query.
        /// </summary>
        /// <param name="transaction">
        /// The transaction.
        /// </param>
        /// <param name="spName">
        /// The sp name.
        /// </param>
        /// <param name="parameterValues">
        /// The parameter values.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int ExecuteNonQuery(OracleTransaction transaction, string spName, params object[] parameterValues)
        {
            // if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // pull the parameters for this stored procedure from the parameter cache (or discover them & populet the cache)
                OracleParameter[] commandParameters =
                    OracleHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);

                // assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // call the overload that takes an array of OracleParameters
                return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
                
                // otherwise we can just call the SP without params
            return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
        }

        /// <summary>
        /// The execute reader.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <param name="commandType">
        /// The command type.
        /// </param>
        /// <param name="commandText">
        /// The command text.
        /// </param>
        /// <returns>
        /// The <see cref="OracleDataReader"/>.
        /// </returns>
        public static OracleDataReader ExecuteReader(
            string connectionString, 
            CommandType commandType, 
            string commandText)
        {
            // pass through the call providing null for the set of OracleParameters
            return ExecuteReader(connectionString, commandType, commandText, null);
        }

        /// <summary>
        /// The execute reader.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <param name="commandType">
        /// The command type.
        /// </param>
        /// <param name="commandText">
        /// The command text.
        /// </param>
        /// <param name="commandParameters">
        /// The command parameters.
        /// </param>
        /// <returns>
        /// The <see cref="OracleDataReader"/>.
        /// </returns>
        public static OracleDataReader ExecuteReader(
            string connectionString, 
            CommandType commandType, 
            string commandText, 
            params OracleParameter[] commandParameters)
        {
            // create & open an OraclebConnection
            var cn = new OracleConnection(connectionString);
            cn.Open();

            try
            {
                // call the private overload that takes an internally owned connection in place of the connection string
                return ExecuteReader(
                    cn, 
                    null, 
                    commandType, 
                    commandText, 
                    commandParameters, 
                    OracleConnectionOwnership.Internal);
            }
            catch
            {
                // if we fail to return the OracleDataReader, we need to close the connection ourselves
                cn.Close();
                throw;
            }
        }

        /// <summary>
        /// The execute reader.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <param name="spName">
        /// The sp name.
        /// </param>
        /// <param name="parameterValues">
        /// The parameter values.
        /// </param>
        /// <returns>
        /// The <see cref="OracleDataReader"/>.
        /// </returns>
        public static OracleDataReader ExecuteReader(
            string connectionString, 
            string spName, 
            params object[] parameterValues)
        {
            // if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                OracleParameter[] commandParameters = OracleHelperParameterCache.GetSpParameterSet(
                    connectionString, 
                    spName);

                // assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // call the overload that takes an array of OracleParameters
                return ExecuteReader(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
                
                // otherwise we can just call the SP without params
            return ExecuteReader(connectionString, CommandType.StoredProcedure, spName);
        }

        /// <summary>
        /// The execute reader.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="commandType">
        /// The command type.
        /// </param>
        /// <param name="commandText">
        /// The command text.
        /// </param>
        /// <returns>
        /// The <see cref="OracleDataReader"/>.
        /// </returns>
        public static OracleDataReader ExecuteReader(
            OracleConnection connection, 
            CommandType commandType, 
            string commandText)
        {
            // pass through the call providing null for the set of OracleParameters
            return ExecuteReader(connection, commandType, commandText, null);
        }

        /// <summary>
        /// The execute reader.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="commandType">
        /// The command type.
        /// </param>
        /// <param name="commandText">
        /// The command text.
        /// </param>
        /// <param name="commandParameters">
        /// The command parameters.
        /// </param>
        /// <returns>
        /// The <see cref="OracleDataReader"/>.
        /// </returns>
        public static OracleDataReader ExecuteReader(
            OracleConnection connection, 
            CommandType commandType, 
            string commandText, 
            params OracleParameter[] commandParameters)
        {
            // pass through the call to the private overload using a null transaction value and an externally owned connection
            return ExecuteReader(
                connection, 
                null, 
                commandType, 
                commandText, 
                commandParameters, 
                OracleConnectionOwnership.External);
        }

        /// <summary>
        /// The execute reader.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="spName">
        /// The sp name.
        /// </param>
        /// <param name="parameterValues">
        /// The parameter values.
        /// </param>
        /// <returns>
        /// The <see cref="OracleDataReader"/>.
        /// </returns>
        public static OracleDataReader ExecuteReader(
            OracleConnection connection, 
            string spName, 
            params object[] parameterValues)
        {
            // if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                OracleParameter[] commandParameters =
                    OracleHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteReader(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
                
                // otherwise we can just call the SP without params
            return ExecuteReader(connection, CommandType.StoredProcedure, spName);
        }

        /// <summary>
        /// The execute reader.
        /// </summary>
        /// <param name="transaction">
        /// The transaction.
        /// </param>
        /// <param name="commandType">
        /// The command type.
        /// </param>
        /// <param name="commandText">
        /// The command text.
        /// </param>
        /// <returns>
        /// The <see cref="OracleDataReader"/>.
        /// </returns>
        public static OracleDataReader ExecuteReader(
            OracleTransaction transaction, 
            CommandType commandType, 
            string commandText)
        {
            // pass through the call providing null for the set of OracleParameters
            return ExecuteReader(transaction, commandType, commandText, null);
        }

        /// <summary>
        /// The execute reader.
        /// </summary>
        /// <param name="transaction">
        /// The transaction.
        /// </param>
        /// <param name="commandType">
        /// The command type.
        /// </param>
        /// <param name="commandText">
        /// The command text.
        /// </param>
        /// <param name="commandParameters">
        /// The command parameters.
        /// </param>
        /// <returns>
        /// The <see cref="OracleDataReader"/>.
        /// </returns>
        public static OracleDataReader ExecuteReader(
            OracleTransaction transaction, 
            CommandType commandType, 
            string commandText, 
            params OracleParameter[] commandParameters)
        {
            // pass through to private overload, indicating that the connection is owned by the caller
            return ExecuteReader(
                transaction.Connection, 
                transaction, 
                commandType, 
                commandText, 
                commandParameters, 
                OracleConnectionOwnership.External);
        }

        /// <summary>
        /// The execute reader.
        /// </summary>
        /// <param name="transaction">
        /// The transaction.
        /// </param>
        /// <param name="spName">
        /// The sp name.
        /// </param>
        /// <param name="parameterValues">
        /// The parameter values.
        /// </param>
        /// <returns>
        /// The <see cref="OracleDataReader"/>.
        /// </returns>
        public static OracleDataReader ExecuteReader(
            OracleTransaction transaction, 
            string spName, 
            params object[] parameterValues)
        {
            // if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                OracleParameter[] commandParameters =
                    OracleHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
                
                // otherwise we can just call the SP without params
            return ExecuteReader(transaction, CommandType.StoredProcedure, spName);
        }

        /// <summary>
        /// The execute scalar.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <param name="commandType">
        /// The command type.
        /// </param>
        /// <param name="commandText">
        /// The command text.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText)
        {
            // pass through the call providing null for the set of OracleParameters
            return ExecuteScalar(connectionString, commandType, commandText, null);
        }

        /// <summary>
        /// The execute scalar.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <param name="commandType">
        /// The command type.
        /// </param>
        /// <param name="commandText">
        /// The command text.
        /// </param>
        /// <param name="commandParameters">
        /// The command parameters.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object ExecuteScalar(
            string connectionString, 
            CommandType commandType, 
            string commandText, 
            params OracleParameter[] commandParameters)
        {
            // create & open an OracleConnection, and dispose of it after we are done.
            using (var cn = new OracleConnection(connectionString))
            {
                cn.Open();

                // call the overload that takes a connection in place of the connection string
                return ExecuteScalar(cn, commandType, commandText, commandParameters);
            }
        }

        /// <summary>
        /// The execute scalar.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <param name="spName">
        /// The sp name.
        /// </param>
        /// <param name="parameterValues">
        /// The parameter values.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object ExecuteScalar(string connectionString, string spName, params object[] parameterValues)
        {
            // if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // pull the parameters for this stored procedure from the parameter cache (or discover them & populet the cache)
                OracleParameter[] commandParameters = OracleHelperParameterCache.GetSpParameterSet(
                    connectionString, 
                    spName);

                // assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // call the overload that takes an array of OracleParameters
                return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
                
                // otherwise we can just call the SP without params
            return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName);
        }

        /// <summary>
        /// The execute scalar.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="commandType">
        /// The command type.
        /// </param>
        /// <param name="commandText">
        /// The command text.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object ExecuteScalar(OracleConnection connection, CommandType commandType, string commandText)
        {
            // pass through the call providing null for the set of OracleParameters
            return ExecuteScalar(connection, commandType, commandText, null);
        }

        /// <summary>
        /// The execute scalar.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="commandType">
        /// The command type.
        /// </param>
        /// <param name="commandText">
        /// The command text.
        /// </param>
        /// <param name="commandParameters">
        /// The command parameters.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object ExecuteScalar(
            OracleConnection connection, 
            CommandType commandType, 
            string commandText, 
            params OracleParameter[] commandParameters)
        {
            // create a command and prepare it for execution
            var cmd = new OracleCommand();
            PrepareCommand(cmd, connection, null, commandType, commandText, commandParameters);

            // execute the command & return the results
            return cmd.ExecuteScalar();
        }

        /// <summary>
        /// The execute scalar.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="spName">
        /// The sp name.
        /// </param>
        /// <param name="parameterValues">
        /// The parameter values.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object ExecuteScalar(OracleConnection connection, string spName, params object[] parameterValues)
        {
            // if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // pull the parameters for this stored procedure from the parameter cache (or discover them & populet the cache)
                OracleParameter[] commandParameters =
                    OracleHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);

                // assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // call the overload that takes an array of OracleParameters
                return ExecuteScalar(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
                
                // otherwise we can just call the SP without params
            return ExecuteScalar(connection, CommandType.StoredProcedure, spName);
        }

        /// <summary>
        /// The execute scalar.
        /// </summary>
        /// <param name="transaction">
        /// The transaction.
        /// </param>
        /// <param name="commandType">
        /// The command type.
        /// </param>
        /// <param name="commandText">
        /// The command text.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object ExecuteScalar(OracleTransaction transaction, CommandType commandType, string commandText)
        {
            // pass through the call providing null for the set of OracleParameters
            return ExecuteScalar(transaction, commandType, commandText, null);
        }

        /// <summary>
        /// The execute scalar.
        /// </summary>
        /// <param name="transaction">
        /// The transaction.
        /// </param>
        /// <param name="commandType">
        /// The command type.
        /// </param>
        /// <param name="commandText">
        /// The command text.
        /// </param>
        /// <param name="commandParameters">
        /// The command parameters.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object ExecuteScalar(
            OracleTransaction transaction, 
            CommandType commandType, 
            string commandText, 
            params OracleParameter[] commandParameters)
        {
            // create a command and prepare it for execution
            var cmd = new OracleCommand();
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);

            // execute the command & return the results
            return cmd.ExecuteScalar();
        }

        /// <summary>
        /// The execute scalar.
        /// </summary>
        /// <param name="transaction">
        /// The transaction.
        /// </param>
        /// <param name="spName">
        /// The sp name.
        /// </param>
        /// <param name="parameterValues">
        /// The parameter values.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object ExecuteScalar(
            OracleTransaction transaction, 
            string spName, 
            params object[] parameterValues)
        {
            // if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // pull the parameters for this stored procedure from the parameter cache (or discover them & populet the cache)
                OracleParameter[] commandParameters =
                    OracleHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);

                // assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // call the overload that takes an array of OracleParameters
                return ExecuteScalar(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
                
                // otherwise we can just call the SP without params
            return ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The assign parameter values.
        /// </summary>
        /// <param name="commandParameters">
        /// The command parameters.
        /// </param>
        /// <param name="parameterValues">
        /// The parameter values.
        /// </param>
        /// <exception cref="ArgumentException">
        /// </exception>
        private static void AssignParameterValues(OracleParameter[] commandParameters, object[] parameterValues)
        {
            if ((commandParameters == null) || (parameterValues == null))
            {
                // do nothing if we get no data
                return;
            }

            // we must have the same number of values as we pave parameters to put them in
            if (commandParameters.Length != parameterValues.Length)
            {
                throw new ArgumentException("Parameter count does not match Parameter Value count.");
            }

            // iterate through the OracleParameters, assigning the values from the corresponding position in the 
            // value array
            for (int i = 0, j = commandParameters.Length; i < j; i++)
            {
                commandParameters[i].Value = parameterValues[i];
            }
        }

        /// <summary>
        /// The attach parameters.
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <param name="commandParameters">
        /// The command parameters.
        /// </param>
        private static void AttachParameters(OracleCommand command, OracleParameter[] commandParameters)
        {
            foreach (OracleParameter p in commandParameters)
            {
                // check for derived output value with no value assigned
                if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                {
                    p.Value = DBNull.Value;
                }

                command.Parameters.Add(p);
            }
        }

        /// <summary>
        /// The execute reader.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="transaction">
        /// The transaction.
        /// </param>
        /// <param name="commandType">
        /// The command type.
        /// </param>
        /// <param name="commandText">
        /// The command text.
        /// </param>
        /// <param name="commandParameters">
        /// The command parameters.
        /// </param>
        /// <param name="connectionOwnership">
        /// The connection ownership.
        /// </param>
        /// <returns>
        /// The <see cref="OracleDataReader"/>.
        /// </returns>
        private static OracleDataReader ExecuteReader(
            OracleConnection connection, 
            OracleTransaction transaction, 
            CommandType commandType, 
            string commandText, 
            OracleParameter[] commandParameters, 
            OracleConnectionOwnership connectionOwnership)
        {
            // create a command and prepare it for execution
            var cmd = new OracleCommand();
            PrepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters);

            // create a reader
            OracleDataReader dr;

            // call ExecuteReader with the appropriate CommandBehavior
            if (connectionOwnership == OracleConnectionOwnership.External)
            {
                dr = cmd.ExecuteReader();
            }
            else
            {
                dr = cmd.ExecuteReader((CommandBehavior)((int)CommandBehavior.CloseConnection));
            }

            return dr;
        }

        /// <summary>
        /// The prepare command.
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="transaction">
        /// The transaction.
        /// </param>
        /// <param name="commandType">
        /// The command type.
        /// </param>
        /// <param name="commandText">
        /// The command text.
        /// </param>
        /// <param name="commandParameters">
        /// The command parameters.
        /// </param>
        private static void PrepareCommand(
            OracleCommand command, 
            OracleConnection connection, 
            OracleTransaction transaction, 
            CommandType commandType, 
            string commandText, 
            OracleParameter[] commandParameters)
        {
            // if the provided connection is not open, we will open it
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            // associate the connection with the command
            command.Connection = connection;

            // set the command text (stored procedure name or Oracle statement)
            command.CommandText = commandText;

            // if we were provided a transaction, assign it.
            if (transaction != null)
            {
                command.Transaction = transaction;
            }

            // set the command type
            command.CommandType = commandType;

            // attach the command parameters if they are provided
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
        }

        #endregion
    }
}