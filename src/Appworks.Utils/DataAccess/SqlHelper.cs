// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlHelper.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The sql helper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Utils.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Xml;
    
    /// <summary>
    /// The sql connection ownership.
    /// </summary>
    public enum SqlConnectionOwnership
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
    /// The sql helper.
    /// </summary>
    public class SqlHelper
    {
        #region Constructors and Destructors

        /// <summary>
        /// Prevents a default instance of the <see cref="SqlHelper"/> class from being created.
        /// </summary>
        private SqlHelper()
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The create command.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="spName">
        /// The sp name.
        /// </param>
        /// <param name="sourceColumns">
        /// The source columns.
        /// </param>
        /// <returns>
        /// The <see cref="SqlCommand"/>.
        /// </returns>
        public static SqlCommand CreateCommand(SqlConnection connection, string spName, params string[] sourceColumns)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            if (string.IsNullOrEmpty(spName))
            {
                throw new ArgumentNullException("spName");
            }

            // Create a SqlCommand
            var cmd = new SqlCommand(spName, connection) { CommandType = CommandType.StoredProcedure };

            // If we receive parameter values, we need to figure out where they go
            if ((sourceColumns != null) && (sourceColumns.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // Assign the provided source columns to these parameters based on parameter order
                for (int index = 0; index < sourceColumns.Length; index++)
                {
                    commandParameters[index].SourceColumn = sourceColumns[index];
                }

                // Attach the discovered parameters to the SqlCommand object
                AttachParameters(cmd, commandParameters);
            }

            return cmd;
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
        /// <returns>
        /// The <see cref="DataSet"/>.
        /// </returns>
        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
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
            params SqlParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            // Create & open a SqlConnection, and dispose of it after we are done
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Call the overload that takes a connection in place of the connection string
                return ExecuteDataset(connection, commandType, commandText, commandParameters);
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
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            if (string.IsNullOrEmpty(spName))
            {
                throw new ArgumentNullException("spName");
            }

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }

            // Otherwise we can just call the SP without params
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
        public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
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
            SqlConnection connection, 
            CommandType commandType, 
            string commandText, 
            params SqlParameter[] commandParameters)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            // Create a command and prepare it for execution
            var cmd = new SqlCommand();
            bool mustCloseConnection;
            PrepareCommand(cmd, connection, null, commandType, commandText, commandParameters, out mustCloseConnection);

            // Create the DataAdapter & DataSet
            using (var da = new SqlDataAdapter(cmd))
            {
                var ds = new DataSet();

                // Fill the DataSet using default values for DataTable names, etc
                da.Fill(ds);

                // Detach the SqlParameters from the command object, so they can be used again
                cmd.Parameters.Clear();

                if (mustCloseConnection)
                {
                    connection.Close();
                }

                // Return the dataset
                return ds;
            }
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
        public static DataSet ExecuteDataset(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            if (string.IsNullOrEmpty(spName))
            {
                throw new ArgumentNullException("spName");
            }

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteDataset(connection, CommandType.StoredProcedure, spName, commandParameters);
            }

            // Otherwise we can just call the SP without params
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
        public static DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
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
            SqlTransaction transaction, 
            CommandType commandType, 
            string commandText, 
            params SqlParameter[] commandParameters)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }

            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", 
                    "transaction");
            }

            // Create a command and prepare it for execution
            var cmd = new SqlCommand();
            bool mustCloseConnection;
            PrepareCommand(
                cmd, 
                transaction.Connection, 
                transaction, 
                commandType, 
                commandText, 
                commandParameters, 
                out mustCloseConnection);

            // Create the DataAdapter & DataSet
            using (var da = new SqlDataAdapter(cmd))
            {
                var ds = new DataSet();

                // Fill the DataSet using default values for DataTable names, etc
                da.Fill(ds);

                // Detach the SqlParameters from the command object, so they can be used again
                cmd.Parameters.Clear();

                // Return the dataset
                return ds;
            }
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
        public static DataSet ExecuteDataset(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }

            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", 
                    "transaction");
            }

            if (string.IsNullOrEmpty(spName))
            {
                throw new ArgumentNullException("spName");
            }

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(
                    transaction.Connection, 
                    spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteDataset(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }

            // Otherwise we can just call the SP without params
            return ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
        }

        /// <summary>
        /// The execute dataset typed params.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <param name="spName">
        /// The sp name.
        /// </param>
        /// <param name="dataRow">
        /// The data row.
        /// </param>
        /// <returns>
        /// The <see cref="DataSet"/>.
        /// </returns>
        public static DataSet ExecuteDatasetTypedParams(string connectionString, string spName, DataRow dataRow)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            if (string.IsNullOrEmpty(spName))
            {
                throw new ArgumentNullException("spName");
            }

            // If the row has values, the store procedure parameters must be initialized
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                return ExecuteDataset(
                    connectionString, 
                    CommandType.StoredProcedure, 
                    spName, 
                    commandParameters);
            }

            return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName);
        }

        /// <summary>
        /// The execute dataset typed params.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="spName">
        /// The sp name.
        /// </param>
        /// <param name="dataRow">
        /// The data row.
        /// </param>
        /// <returns>
        /// The <see cref="DataSet"/>.
        /// </returns>
        public static DataSet ExecuteDatasetTypedParams(SqlConnection connection, string spName, DataRow dataRow)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            if (string.IsNullOrEmpty(spName))
            {
                throw new ArgumentNullException("spName");
            }

            // If the row has values, the store procedure parameters must be initialized
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                return ExecuteDataset(connection, CommandType.StoredProcedure, spName, commandParameters);
            }

            return ExecuteDataset(connection, CommandType.StoredProcedure, spName);
        }

        /// <summary>
        /// The execute dataset typed params.
        /// </summary>
        /// <param name="transaction">
        /// The transaction.
        /// </param>
        /// <param name="spName">
        /// The sp name.
        /// </param>
        /// <param name="dataRow">
        /// The data row.
        /// </param>
        /// <returns>
        /// The <see cref="DataSet"/>.
        /// </returns>
        public static DataSet ExecuteDatasetTypedParams(SqlTransaction transaction, string spName, DataRow dataRow)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }

            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", 
                    "transaction");
            }

            if (string.IsNullOrEmpty(spName))
            {
                throw new ArgumentNullException("spName");
            }

            // If the row has values, the store procedure parameters must be initialized
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(
                    transaction.Connection, 
                    spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                return ExecuteDataset(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }

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
            // Pass through the call providing null for the set of SqlParameters
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
            params SqlParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            // Create & open a SqlConnection, and dispose of it after we are done
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Call the overload that takes a connection in place of the connection string
                return ExecuteNonQuery(connection, commandType, commandText, commandParameters);
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
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            if (string.IsNullOrEmpty(spName))
            {
                throw new ArgumentNullException("spName");
            }

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }

            // Otherwise we can just call the SP without params
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
        public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
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
            SqlConnection connection, 
            CommandType commandType, 
            string commandText, 
            params SqlParameter[] commandParameters)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            // Create a command and prepare it for execution
            var cmd = new SqlCommand();
            bool mustCloseConnection;
            PrepareCommand(cmd, connection, null, commandType, commandText, commandParameters, out mustCloseConnection);

            // Finally, execute the command
            int retval = cmd.ExecuteNonQuery();

            // Detach the SqlParameters from the command object, so they can be used again
            cmd.Parameters.Clear();
            if (mustCloseConnection)
            {
                connection.Close();
            }

            return retval;
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
        public static int ExecuteNonQuery(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            if (string.IsNullOrEmpty(spName))
            {
                throw new ArgumentNullException("spName");
            }

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, commandParameters);
            }

            // Otherwise we can just call the SP without params
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
        public static int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
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
            SqlTransaction transaction, 
            CommandType commandType, 
            string commandText, 
            params SqlParameter[] commandParameters)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }

            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", 
                    "transaction");
            }

            // Create a command and prepare it for execution
            var cmd = new SqlCommand();
            bool mustCloseConnection;
            PrepareCommand(
                cmd, 
                transaction.Connection, 
                transaction, 
                commandType, 
                commandText, 
                commandParameters, 
                out mustCloseConnection);

            // Finally, execute the command
            int retval = cmd.ExecuteNonQuery();

            // Detach the SqlParameters from the command object, so they can be used again
            cmd.Parameters.Clear();
            return retval;
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
        public static int ExecuteNonQuery(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }

            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", 
                    "transaction");
            }

            if (string.IsNullOrEmpty(spName))
            {
                throw new ArgumentNullException("spName");
            }

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(
                    transaction.Connection, 
                    spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }

            // Otherwise we can just call the SP without params
            return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
        }

        /// <summary>
        /// The execute non query typed params.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <param name="spName">
        /// The sp name.
        /// </param>
        /// <param name="dataRow">
        /// The data row.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int ExecuteNonQueryTypedParams(string connectionString, string spName, DataRow dataRow)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            if (string.IsNullOrEmpty(spName))
            {
                throw new ArgumentNullException("spName");
            }

            // If the row has values, the store procedure parameters must be initialized
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                return ExecuteNonQuery(
                    connectionString, 
                    CommandType.StoredProcedure, 
                    spName, 
                    commandParameters);
            }

            return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName);
        }

        /// <summary>
        /// The execute non query typed params.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="spName">
        /// The sp name.
        /// </param>
        /// <param name="dataRow">
        /// The data row.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int ExecuteNonQueryTypedParams(SqlConnection connection, string spName, DataRow dataRow)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            if (string.IsNullOrEmpty(spName))
            {
                throw new ArgumentNullException("spName");
            }

            // If the row has values, the store procedure parameters must be initialized
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, commandParameters);
            }

            return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
        }

        /// <summary>
        /// The execute non query typed params.
        /// </summary>
        /// <param name="transaction">
        /// The transaction.
        /// </param>
        /// <param name="spName">
        /// The sp name.
        /// </param>
        /// <param name="dataRow">
        /// The data row.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int ExecuteNonQueryTypedParams(SqlTransaction transaction, string spName, DataRow dataRow)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }

            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", 
                    "transaction");
            }

            if (string.IsNullOrEmpty(spName))
            {
                throw new ArgumentNullException("spName");
            }

            // Sf the row has values, the store procedure parameters must be initialized
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(
                    transaction.Connection, 
                    spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }

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
        /// The <see cref="SqlDataReader"/>.
        /// </returns>
        public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
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
        /// The <see cref="SqlDataReader"/>.
        /// </returns>
        public static SqlDataReader ExecuteReader(
            string connectionString, 
            CommandType commandType, 
            string commandText, 
            params SqlParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();

                // Call the private overload that takes an internally owned connection in place of the connection string
                return ExecuteReader(
                    connection, 
                    null, 
                    commandType, 
                    commandText, 
                    commandParameters, 
                    SqlConnectionOwnership.Internal);
            }
            catch
            {
                // If we fail to return the SqlDatReader, we need to close the connection ourselves
                if (connection != null)
                {
                    connection.Close();
                }

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
        /// The <see cref="SqlDataReader"/>.
        /// </returns>
        public static SqlDataReader ExecuteReader(
            string connectionString, 
            string spName, 
            params object[] parameterValues)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            if (string.IsNullOrEmpty(spName))
            {
                throw new ArgumentNullException("spName");
            }

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteReader(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }

            // Otherwise we can just call the SP without params
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
        /// The <see cref="SqlDataReader"/>.
        /// </returns>
        public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
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
        /// The <see cref="SqlDataReader"/>.
        /// </returns>
        public static SqlDataReader ExecuteReader(
            SqlConnection connection, 
            CommandType commandType, 
            string commandText, 
            params SqlParameter[] commandParameters)
        {
            // Pass through the call to the private overload using a null transaction value and an externally owned connection
            return ExecuteReader(
                connection, 
                null, 
                commandType, 
                commandText, 
                commandParameters, 
                SqlConnectionOwnership.External);
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
        /// The <see cref="SqlDataReader"/>.
        /// </returns>
        public static SqlDataReader ExecuteReader(
            SqlConnection connection, 
            string spName, 
            params object[] parameterValues)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            if (string.IsNullOrEmpty(spName))
            {
                throw new ArgumentNullException("spName");
            }

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteReader(connection, CommandType.StoredProcedure, spName, commandParameters);
            }

            // Otherwise we can just call the SP without params
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
        /// The <see cref="SqlDataReader"/>.
        /// </returns>
        public static SqlDataReader ExecuteReader(
            SqlTransaction transaction, 
            CommandType commandType, 
            string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
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
        /// The <see cref="SqlDataReader"/>.
        /// </returns>
        public static SqlDataReader ExecuteReader(
            SqlTransaction transaction, 
            CommandType commandType, 
            string commandText, 
            params SqlParameter[] commandParameters)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }

            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", 
                    "transaction");
            }

            // Pass through to private overload, indicating that the connection is owned by the caller
            return ExecuteReader(
                transaction.Connection, 
                transaction, 
                commandType, 
                commandText, 
                commandParameters, 
                SqlConnectionOwnership.External);
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
        /// The <see cref="SqlDataReader"/>.
        /// </returns>
        public static SqlDataReader ExecuteReader(
            SqlTransaction transaction, 
            string spName, 
            params object[] parameterValues)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }

            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", 
                    "transaction");
            }

            if (string.IsNullOrEmpty(spName))
            {
                throw new ArgumentNullException("spName");
            }

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(
                    transaction.Connection, 
                    spName);

                AssignParameterValues(commandParameters, parameterValues);

                return ExecuteReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }

            // Otherwise we can just call the SP without params
            return ExecuteReader(transaction, CommandType.StoredProcedure, spName);
        }

        /// <summary>
        /// The execute reader typed params.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <param name="spName">
        /// The sp name.
        /// </param>
        /// <param name="dataRow">
        /// The data row.
        /// </param>
        /// <returns>
        /// The <see cref="SqlDataReader"/>.
        /// </returns>
        public static SqlDataReader ExecuteReaderTypedParams(string connectionString, string spName, DataRow dataRow)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            if (string.IsNullOrEmpty(spName))
            {
                throw new ArgumentNullException("spName");
            }

            // If the row has values, the store procedure parameters must be initialized
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                return ExecuteReader(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }

            return ExecuteReader(connectionString, CommandType.StoredProcedure, spName);
        }

        /// <summary>
        /// The execute reader typed params.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="spName">
        /// The sp name.
        /// </param>
        /// <param name="dataRow">
        /// The data row.
        /// </param>
        /// <returns>
        /// The <see cref="SqlDataReader"/>.
        /// </returns>
        public static SqlDataReader ExecuteReaderTypedParams(SqlConnection connection, string spName, DataRow dataRow)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            if (string.IsNullOrEmpty(spName))
            {
                throw new ArgumentNullException("spName");
            }

            // If the row has values, the store procedure parameters must be initialized
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                return ExecuteReader(connection, CommandType.StoredProcedure, spName, commandParameters);
            }

            return ExecuteReader(connection, CommandType.StoredProcedure, spName);
        }

        /// <summary>
        /// The execute reader typed params.
        /// </summary>
        /// <param name="transaction">
        /// The transaction.
        /// </param>
        /// <param name="spName">
        /// The sp name.
        /// </param>
        /// <param name="dataRow">
        /// The data row.
        /// </param>
        /// <returns>
        /// The <see cref="SqlDataReader"/>.
        /// </returns>
        public static SqlDataReader ExecuteReaderTypedParams(SqlTransaction transaction, string spName, DataRow dataRow)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }

            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", 
                    "transaction");
            }

            if (string.IsNullOrEmpty(spName))
            {
                throw new ArgumentNullException("spName");
            }

            // If the row has values, the store procedure parameters must be initialized
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(
                    transaction.Connection, 
                    spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                return ExecuteReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }

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
            // Pass through the call providing null for the set of SqlParameters
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
            params SqlParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            // Create & open a SqlConnection, and dispose of it after we are done
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Call the overload that takes a connection in place of the connection string
                return ExecuteScalar(connection, commandType, commandText, commandParameters);
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
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            if (string.IsNullOrEmpty(spName))
            {
                throw new ArgumentNullException("spName");
            }

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }

            // Otherwise we can just call the SP without params
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
        public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
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
            SqlConnection connection, 
            CommandType commandType, 
            string commandText, 
            params SqlParameter[] commandParameters)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            // Create a command and prepare it for execution
            var cmd = new SqlCommand();

            bool mustCloseConnection;
            PrepareCommand(cmd, connection, null, commandType, commandText, commandParameters, out mustCloseConnection);

            // Execute the command & return the results
            object retval = cmd.ExecuteScalar();

            // Detach the SqlParameters from the command object, so they can be used again
            cmd.Parameters.Clear();

            if (mustCloseConnection)
            {
                connection.Close();
            }

            return retval;
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
        public static object ExecuteScalar(SqlConnection connection, string spName, params object[] parameterValues)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            if (string.IsNullOrEmpty(spName))
            {
                throw new ArgumentNullException("spName");
            }

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteScalar(connection, CommandType.StoredProcedure, spName, commandParameters);
            }

            // Otherwise we can just call the SP without params
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
        public static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
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
            SqlTransaction transaction, 
            CommandType commandType, 
            string commandText, 
            params SqlParameter[] commandParameters)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }

            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", 
                    "transaction");
            }

            // Create a command and prepare it for execution
            var cmd = new SqlCommand();
            bool mustCloseConnection;
            PrepareCommand(
                cmd, 
                transaction.Connection, 
                transaction, 
                commandType, 
                commandText, 
                commandParameters, 
                out mustCloseConnection);

            // Execute the command & return the results
            object retval = cmd.ExecuteScalar();

            // Detach the SqlParameters from the command object, so they can be used again
            cmd.Parameters.Clear();
            return retval;
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
        public static object ExecuteScalar(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }

            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", 
                    "transaction");
            }

            if (string.IsNullOrEmpty(spName))
            {
                throw new ArgumentNullException("spName");
            }

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // PPull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(
                    transaction.Connection, 
                    spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteScalar(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }

            // Otherwise we can just call the SP without params
            return ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
        }

        /// <summary>
        /// The execute scalar typed params.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <param name="spName">
        /// The sp name.
        /// </param>
        /// <param name="dataRow">
        /// The data row.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object ExecuteScalarTypedParams(string connectionString, string spName, DataRow dataRow)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            if (string.IsNullOrEmpty(spName))
            {
                throw new ArgumentNullException("spName");
            }

            // If the row has values, the store procedure parameters must be initialized
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }

            return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName);
        }

        /// <summary>
        /// The execute scalar typed params.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="spName">
        /// The sp name.
        /// </param>
        /// <param name="dataRow">
        /// The data row.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object ExecuteScalarTypedParams(SqlConnection connection, string spName, DataRow dataRow)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            if (string.IsNullOrEmpty(spName))
            {
                throw new ArgumentNullException("spName");
            }

            // If the row has values, the store procedure parameters must be initialized
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                return ExecuteScalar(connection, CommandType.StoredProcedure, spName, commandParameters);
            }

            return ExecuteScalar(connection, CommandType.StoredProcedure, spName);
        }

        /// <summary>
        /// The execute scalar typed params.
        /// </summary>
        /// <param name="transaction">
        /// The transaction.
        /// </param>
        /// <param name="spName">
        /// The sp name.
        /// </param>
        /// <param name="dataRow">
        /// The data row.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public static object ExecuteScalarTypedParams(SqlTransaction transaction, string spName, DataRow dataRow)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }

            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", 
                    "transaction");
            }

            if (string.IsNullOrEmpty(spName))
            {
                throw new ArgumentNullException("spName");
            }

            // If the row has values, the store procedure parameters must be initialized
            if (dataRow != null && dataRow.ItemArray.Length > 0)
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(
                    transaction.Connection, 
                    spName);

                // Set the parameters values
                AssignParameterValues(commandParameters, dataRow);

                return ExecuteScalar(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }

            return ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
        }

        /// <summary>
        /// The execute xml reader.
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
        /// The <see cref="XmlReader"/>.
        /// </returns>
        public static XmlReader ExecuteXmlReader(SqlConnection connection, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteXmlReader(connection, commandType, commandText, null);
        }

        /// <summary>
        /// The execute xml reader.
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
        /// The <see cref="XmlReader"/>.
        /// </returns>
        public static XmlReader ExecuteXmlReader(
            SqlConnection connection, 
            CommandType commandType, 
            string commandText, 
            params SqlParameter[] commandParameters)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            bool mustCloseConnection = false;

            // Create a command and prepare it for execution
            var cmd = new SqlCommand();
            try
            {
                PrepareCommand(
                    cmd, 
                    connection, 
                    null, 
                    commandType, 
                    commandText, 
                    commandParameters, 
                    out mustCloseConnection);

                // Create the DataAdapter & DataSet
                XmlReader retval = cmd.ExecuteXmlReader();

                // Detach the SqlParameters from the command object, so they can be used again
                cmd.Parameters.Clear();

                return retval;
            }
            catch
            {
                if (mustCloseConnection)
                {
                    connection.Close();
                }

                throw;
            }
        }

        /// <summary>
        /// The execute xml reader.
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
        /// The <see cref="XmlReader"/>.
        /// </returns>
        public static XmlReader ExecuteXmlReader(
            SqlConnection connection, 
            string spName, 
            params object[] parameterValues)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            if (string.IsNullOrEmpty(spName))
            {
                throw new ArgumentNullException("spName");
            }

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName, commandParameters);
            }

            // Otherwise we can just call the SP without params
            return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName);
        }

        /// <summary>
        /// The execute xml reader.
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
        /// The <see cref="XmlReader"/>.
        /// </returns>
        public static XmlReader ExecuteXmlReader(
            SqlTransaction transaction, 
            CommandType commandType, 
            string commandText)
        {
            // Pass through the call providing null for the set of SqlParameters
            return ExecuteXmlReader(transaction, commandType, commandText, null);
        }

        /// <summary>
        /// The execute xml reader.
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
        /// The <see cref="XmlReader"/>.
        /// </returns>
        public static XmlReader ExecuteXmlReader(
            SqlTransaction transaction, 
            CommandType commandType, 
            string commandText, 
            params SqlParameter[] commandParameters)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }

            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", 
                    "transaction");
            }

            // Create a command and prepare it for execution
            var cmd = new SqlCommand();
            bool mustCloseConnection;
            PrepareCommand(
                cmd, 
                transaction.Connection, 
                transaction, 
                commandType, 
                commandText, 
                commandParameters, 
                out mustCloseConnection);

            // Create the DataAdapter & DataSet
            XmlReader retval = cmd.ExecuteXmlReader();

            // Detach the SqlParameters from the command object, so they can be used again
            cmd.Parameters.Clear();
            return retval;
        }

        /// <summary>
        /// The execute xml reader.
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
        /// The <see cref="XmlReader"/>.
        /// </returns>
        public static XmlReader ExecuteXmlReader(
            SqlTransaction transaction, 
            string spName, 
            params object[] parameterValues)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }

            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", 
                    "transaction");
            }

            if (string.IsNullOrEmpty(spName))
            {
                throw new ArgumentNullException("spName");
            }

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(
                    transaction.Connection, 
                    spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }

            // Otherwise we can just call the SP without params
            return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName);
        }

        /// <summary>
        /// The fill dataset.
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
        /// <param name="dataSet">
        /// The data set.
        /// </param>
        /// <param name="tableNames">
        /// The table names.
        /// </param>
        public static void FillDataset(
            string connectionString, 
            CommandType commandType, 
            string commandText, 
            DataSet dataSet, 
            string[] tableNames)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            if (dataSet == null)
            {
                throw new ArgumentNullException("dataSet");
            }

            // Create & open a SqlConnection, and dispose of it after we are done
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Call the overload that takes a connection in place of the connection string
                FillDataset(connection, commandType, commandText, dataSet, tableNames);
            }
        }

        /// <summary>
        /// The fill dataset.
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
        /// <param name="dataSet">
        /// The data set.
        /// </param>
        /// <param name="tableNames">
        /// The table names.
        /// </param>
        /// <param name="commandParameters">
        /// The command parameters.
        /// </param>
        public static void FillDataset(
            string connectionString, 
            CommandType commandType, 
            string commandText, 
            DataSet dataSet, 
            string[] tableNames, 
            params SqlParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            if (dataSet == null)
            {
                throw new ArgumentNullException("dataSet");
            }

            // Create & open a SqlConnection, and dispose of it after we are done
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Call the overload that takes a connection in place of the connection string
                FillDataset(connection, commandType, commandText, dataSet, tableNames, commandParameters);
            }
        }

        /// <summary>
        /// The fill dataset.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <param name="spName">
        /// The sp name.
        /// </param>
        /// <param name="dataSet">
        /// The data set.
        /// </param>
        /// <param name="tableNames">
        /// The table names.
        /// </param>
        /// <param name="parameterValues">
        /// The parameter values.
        /// </param>
        public static void FillDataset(
            string connectionString, 
            string spName, 
            DataSet dataSet, 
            string[] tableNames, 
            params object[] parameterValues)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            if (dataSet == null)
            {
                throw new ArgumentNullException("dataSet");
            }

            // Create & open a SqlConnection, and dispose of it after we are done
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Call the overload that takes a connection in place of the connection string
                FillDataset(connection, spName, dataSet, tableNames, parameterValues);
            }
        }

        /// <summary>
        /// The fill dataset.
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
        /// <param name="dataSet">
        /// The data set.
        /// </param>
        /// <param name="tableNames">
        /// The table names.
        /// </param>
        public static void FillDataset(
            SqlConnection connection, 
            CommandType commandType, 
            string commandText, 
            DataSet dataSet, 
            string[] tableNames)
        {
            FillDataset(connection, commandType, commandText, dataSet, tableNames, null);
        }

        /// <summary>
        /// The fill dataset.
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
        /// <param name="dataSet">
        /// The data set.
        /// </param>
        /// <param name="tableNames">
        /// The table names.
        /// </param>
        /// <param name="commandParameters">
        /// The command parameters.
        /// </param>
        public static void FillDataset(
            SqlConnection connection, 
            CommandType commandType, 
            string commandText, 
            DataSet dataSet, 
            string[] tableNames, 
            params SqlParameter[] commandParameters)
        {
            FillDataset(connection, null, commandType, commandText, dataSet, tableNames, commandParameters);
        }

        /// <summary>
        /// The fill dataset.
        /// </summary>
        /// <param name="connection">
        /// The connection.
        /// </param>
        /// <param name="spName">
        /// The sp name.
        /// </param>
        /// <param name="dataSet">
        /// The data set.
        /// </param>
        /// <param name="tableNames">
        /// The table names.
        /// </param>
        /// <param name="parameterValues">
        /// The parameter values.
        /// </param>
        public static void FillDataset(
            SqlConnection connection, 
            string spName, 
            DataSet dataSet, 
            string[] tableNames, 
            params object[] parameterValues)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            if (dataSet == null)
            {
                throw new ArgumentNullException("dataSet");
            }

            if (string.IsNullOrEmpty(spName))
            {
                throw new ArgumentNullException("spName");
            }

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connection, spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames, commandParameters);
            }
            else
            {
                // Otherwise we can just call the SP without params
                FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames);
            }
        }

        /// <summary>
        /// The fill dataset.
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
        /// <param name="dataSet">
        /// The data set.
        /// </param>
        /// <param name="tableNames">
        /// The table names.
        /// </param>
        public static void FillDataset(
            SqlTransaction transaction, 
            CommandType commandType, 
            string commandText, 
            DataSet dataSet, 
            string[] tableNames)
        {
            FillDataset(transaction, commandType, commandText, dataSet, tableNames, null);
        }

        /// <summary>
        /// The fill dataset.
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
        /// <param name="dataSet">
        /// The data set.
        /// </param>
        /// <param name="tableNames">
        /// The table names.
        /// </param>
        /// <param name="commandParameters">
        /// The command parameters.
        /// </param>
        public static void FillDataset(
            SqlTransaction transaction, 
            CommandType commandType, 
            string commandText, 
            DataSet dataSet, 
            string[] tableNames, 
            params SqlParameter[] commandParameters)
        {
            FillDataset(
                transaction.Connection, 
                transaction, 
                commandType, 
                commandText, 
                dataSet, 
                tableNames, 
                commandParameters);
        }

        /// <summary>
        /// The fill dataset.
        /// </summary>
        /// <param name="transaction">
        /// The transaction.
        /// </param>
        /// <param name="spName">
        /// The sp name.
        /// </param>
        /// <param name="dataSet">
        /// The data set.
        /// </param>
        /// <param name="tableNames">
        /// The table names.
        /// </param>
        /// <param name="parameterValues">
        /// The parameter values.
        /// </param>
        public static void FillDataset(
            SqlTransaction transaction, 
            string spName, 
            DataSet dataSet, 
            string[] tableNames, 
            params object[] parameterValues)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }

            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", 
                    "transaction");
            }

            if (dataSet == null)
            {
                throw new ArgumentNullException("dataSet");
            }

            if (string.IsNullOrEmpty(spName))
            {
                throw new ArgumentNullException("spName");
            }

            // If we receive parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(
                    transaction.Connection, 
                    spName);

                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // Call the overload that takes an array of SqlParameters
                FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames, commandParameters);
            }
            else
            {
                // Otherwise we can just call the SP without params
                FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames);
            }
        }

        /// <summary>
        /// The update dataset.
        /// </summary>
        /// <param name="insertCommand">
        /// The insert command.
        /// </param>
        /// <param name="deleteCommand">
        /// The delete command.
        /// </param>
        /// <param name="updateCommand">
        /// The update command.
        /// </param>
        /// <param name="dataSet">
        /// The data set.
        /// </param>
        /// <param name="tableName">
        /// The table name.
        /// </param>
        public static void UpdateDataset(
            SqlCommand insertCommand, 
            SqlCommand deleteCommand, 
            SqlCommand updateCommand, 
            DataSet dataSet, 
            string tableName)
        {
            if (insertCommand == null)
            {
                throw new ArgumentNullException("insertCommand");
            }

            if (deleteCommand == null)
            {
                throw new ArgumentNullException("deleteCommand");
            }

            if (updateCommand == null)
            {
                throw new ArgumentNullException("updateCommand");
            }

            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException("tableName");
            }

            // Create a SqlDataAdapter, and dispose of it after we are done
            using (var dataAdapter = new SqlDataAdapter())
            {
                // Set the data adapter commands
                dataAdapter.UpdateCommand = updateCommand;
                dataAdapter.InsertCommand = insertCommand;
                dataAdapter.DeleteCommand = deleteCommand;

                // Update the dataset changes in the data source
                dataAdapter.Update(dataSet, tableName);

                // Commit all the changes made to the DataSet
                dataSet.AcceptChanges();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The assign parameter values.
        /// </summary>
        /// <param name="commandParameters">
        /// The command parameters.
        /// </param>
        /// <param name="dataRow">
        /// The data row.
        /// </param>
        private static void AssignParameterValues(IEnumerable<SqlParameter> commandParameters, DataRow dataRow)
        {
            if ((commandParameters == null) || (dataRow == null))
            {
                // Do nothing if we get no data
                return;
            }

            int i = 0;

            // Set the parameters values
            foreach (SqlParameter commandParameter in commandParameters)
            {
                // Check the parameter name
                if (commandParameter.ParameterName == null || commandParameter.ParameterName.Length <= 1)
                {
                    throw new Exception(
                        string.Format(
                            "Please provide a valid parameter name on the parameter #{0}, the ParameterName property has the following value: '{1}'.", 
                            i, 
                            commandParameter.ParameterName));
                }

                if (dataRow.Table.Columns.IndexOf(commandParameter.ParameterName.Substring(1)) != -1)
                {
                    commandParameter.Value = dataRow[commandParameter.ParameterName.Substring(1)];
                }

                i++;
            }
        }

        /// <summary>
        /// The assign parameter values.
        /// </summary>
        /// <param name="commandParameters">
        /// The command parameters.
        /// </param>
        /// <param name="parameterValues">
        /// The parameter values.
        /// </param>
        private static void AssignParameterValues(SqlParameter[] commandParameters, object[] parameterValues)
        {
            if ((commandParameters == null) || (parameterValues == null))
            {
                // Do nothing if we get no data
                return;
            }

            // We must have the same number of values as we pave parameters to put them in
            if (commandParameters.Length != parameterValues.Length)
            {
                throw new ArgumentException("Parameter count does not match Parameter Value count.");
            }

            // Iterate through the SqlParameters, assigning the values from the corresponding position in the 
            // value array
            for (int i = 0, j = commandParameters.Length; i < j; i++)
            {
                // If the current array value derives from IDbDataParameter, then assign its Value property
                if (parameterValues[i] is IDbDataParameter)
                {
                    var paramInstance = (IDbDataParameter)parameterValues[i];
                    if (paramInstance.Value == null)
                    {
                        commandParameters[i].Value = DBNull.Value;
                    }
                    else
                    {
                        commandParameters[i].Value = paramInstance.Value;
                    }
                }
                else if (parameterValues[i] == null)
                {
                    commandParameters[i].Value = DBNull.Value;
                }
                else
                {
                    commandParameters[i].Value = parameterValues[i];
                }
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
        private static void AttachParameters(SqlCommand command, IEnumerable<SqlParameter> commandParameters)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            if (commandParameters != null)
            {
                foreach (SqlParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // Check for derived output value with no value assigned
                        if ((p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Input)
                            && (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }

                        command.Parameters.Add(p);
                    }
                }
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
        /// The <see cref="SqlDataReader"/>.
        /// </returns>
        private static SqlDataReader ExecuteReader(
            SqlConnection connection, 
            SqlTransaction transaction, 
            CommandType commandType, 
            string commandText, 
            SqlParameter[] commandParameters, 
            SqlConnectionOwnership connectionOwnership)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            bool mustCloseConnection = false;

            // Create a command and prepare it for execution
            var cmd = new SqlCommand();
            try
            {
                PrepareCommand(
                    cmd, 
                    connection, 
                    transaction, 
                    commandType, 
                    commandText, 
                    commandParameters, 
                    out mustCloseConnection);

                // Create a reader
                SqlDataReader dataReader;

                // Call ExecuteReader with the appropriate CommandBehavior
                if (connectionOwnership == SqlConnectionOwnership.External)
                {
                    dataReader = cmd.ExecuteReader();
                }
                else
                {
                    dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }

                // Detach the SqlParameters from the command object, so they can be used again.
                // HACK: There is a problem here, the output parameter values are fletched 
                // when the reader is closed, so if the parameters are detached from the command
                // then the SqlReader cant set its values. 
                // When this happen, the parameters cant be used again in other command.
                bool canClear = true;
                foreach (SqlParameter commandParameter in cmd.Parameters)
                {
                    if (commandParameter.Direction != ParameterDirection.Input)
                    {
                        canClear = false;
                    }
                }

                if (canClear)
                {
                    cmd.Parameters.Clear();
                }

                return dataReader;
            }
            catch
            {
                if (mustCloseConnection)
                {
                    connection.Close();
                }

                throw;
            }
        }

        /// <summary>
        /// The fill dataset.
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
        /// <param name="dataSet">
        /// The data set.
        /// </param>
        /// <param name="tableNames">
        /// The table names.
        /// </param>
        /// <param name="commandParameters">
        /// The command parameters.
        /// </param>
        private static void FillDataset(
            SqlConnection connection, 
            SqlTransaction transaction, 
            CommandType commandType, 
            string commandText, 
            DataSet dataSet, 
            string[] tableNames, 
            params SqlParameter[] commandParameters)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            if (dataSet == null)
            {
                throw new ArgumentNullException("dataSet");
            }

            // Create a command and prepare it for execution
            var command = new SqlCommand();
            bool mustCloseConnection;
            PrepareCommand(
                command, 
                connection, 
                transaction, 
                commandType, 
                commandText, 
                commandParameters, 
                out mustCloseConnection);

            // Create the DataAdapter & DataSet
            using (var dataAdapter = new SqlDataAdapter(command))
            {
                // Add the table mappings specified by the user
                if (tableNames != null && tableNames.Length > 0)
                {
                    string tableName = "Table";
                    for (int index = 0; index < tableNames.Length; index++)
                    {
                        if (tableNames[index] == null || tableNames[index].Length == 0)
                        {
                            throw new ArgumentException(
                                "The tableNames parameter must contain a list of tables, a value was provided as null or empty string.", 
                                "tableNames");
                        }

                        dataAdapter.TableMappings.Add(tableName, tableNames[index]);
                        tableName += (index + 1).ToString(CultureInfo.InvariantCulture);
                    }
                }

                // Fill the DataSet using default values for DataTable names, etc
                dataAdapter.Fill(dataSet);

                // Detach the SqlParameters from the command object, so they can be used again
                command.Parameters.Clear();
            }

            if (mustCloseConnection)
            {
                connection.Close();
            }
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
        /// <param name="mustCloseConnection">
        /// The must close connection.
        /// </param>
        private static void PrepareCommand(
            SqlCommand command, 
            SqlConnection connection, 
            SqlTransaction transaction, 
            CommandType commandType, 
            string commandText, 
            IEnumerable<SqlParameter> commandParameters, 
            out bool mustCloseConnection)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            if (string.IsNullOrEmpty(commandText))
            {
                throw new ArgumentNullException("commandText");
            }

            // If the provided connection is not open, we will open it
            if (connection.State != ConnectionState.Open)
            {
                mustCloseConnection = true;
                connection.Open();
            }
            else
            {
                mustCloseConnection = false;
            }

            // Associate the connection with the command
            command.Connection = connection;

            // Set the command text (stored procedure name or SQL statement)
            command.CommandText = commandText;

            // If we were provided a transaction, assign it
            if (transaction != null)
            {
                if (transaction.Connection == null)
                {
                    throw new ArgumentException(
                        "The transaction was rollbacked or commited, please provide an open transaction.", 
                        "transaction");
                }

                command.Transaction = transaction;
            }

            // Set the command type
            command.CommandType = commandType;

            // Attach the command parameters if they are provided
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
        }

        #endregion
    }
}