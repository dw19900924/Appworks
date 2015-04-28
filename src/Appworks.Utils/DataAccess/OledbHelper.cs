// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OledbHelper.cs" company="Zhulei">
//   (C) 2015 Zhulei. All rights reserved.
// </copyright>
// <summary>
//   The ole db connection ownership.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Appworks.Utils.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OleDb;

    /// <summary>
    /// The ole db connection ownership.
    /// </summary>
    public enum OleDbConnectionOwnership
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
    /// The oledb helper.
    /// </summary>
    public static class OledbHelper
    {
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
            // pass through the call providing null for the set of OleDbParameters
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
            params OleDbParameter[] commandParameters)
        {
            // create & open an OleDbConnection, and dispose of it after we are done.
            using (var cn = new OleDbConnection(connectionString))
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
                OleDbParameter[] commandParameters = OledbHelperParameterCache.GetSpParameterSet(
                    connectionString, 
                    spName);

                // assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // call the overload that takes an array of OleDbParameters
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
        public static DataSet ExecuteDataset(OleDbConnection connection, CommandType commandType, string commandText)
        {
            // pass through the call providing null for the set of OleDbParameters
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
            OleDbConnection connection, 
            CommandType commandType, 
            string commandText, 
            params OleDbParameter[] commandParameters)
        {
            // create a command and prepare it for execution
            var cmd = new OleDbCommand();
            PrepareCommand(cmd, connection, null, commandType, commandText, commandParameters);

            // create the DataAdapter & DataSet
            var da = new OleDbDataAdapter(cmd);
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
        public static DataSet ExecuteDataset(OleDbConnection connection, string spName, params object[] parameterValues)
        {
            // if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                OleDbParameter[] commandParameters =
                    OledbHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);

                // assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // call the overload that takes an array of OleDbParameters
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
        public static DataSet ExecuteDataset(OleDbTransaction transaction, CommandType commandType, string commandText)
        {
            // pass through the call providing null for the set of OleDbParameters
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
            OleDbTransaction transaction, 
            CommandType commandType, 
            string commandText, 
            params OleDbParameter[] commandParameters)
        {
            // create a command and prepare it for execution
            var cmd = new OleDbCommand();
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);

            // create the DataAdapter & DataSet
            var da = new OleDbDataAdapter(cmd);
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
            OleDbTransaction transaction, 
            string spName, 
            params object[] parameterValues)
        {
            // if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                OleDbParameter[] commandParameters =
                    OledbHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);

                // assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // call the overload that takes an array of OleDbParameters
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
            // pass through the call providing null for the set of OleDbParameters
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
            params OleDbParameter[] commandParameters)
        {
            // create & open an OleDbConnection, and dispose of it after we are done.
            using (var cn = new OleDbConnection(connectionString))
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
                OleDbParameter[] commandParameters = OledbHelperParameterCache.GetSpParameterSet(
                    connectionString, 
                    spName);

                // assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // call the overload that takes an array of OleDbParameters
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
        public static int ExecuteNonQuery(OleDbConnection connection, CommandType commandType, string commandText)
        {
            // pass through the call providing null for the set of OleDbParameters
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
            OleDbConnection connection, 
            CommandType commandType, 
            string commandText, 
            params OleDbParameter[] commandParameters)
        {
            // create a command and prepare it for execution
            var cmd = new OleDbCommand();
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
        public static int ExecuteNonQuery(OleDbConnection connection, string spName, params object[] parameterValues)
        {
            // if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                OleDbParameter[] commandParameters =
                    OledbHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);

                // assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // call the overload that takes an array of OleDbParameters
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
        public static int ExecuteNonQuery(OleDbTransaction transaction, CommandType commandType, string commandText)
        {
            // pass through the call providing null for the set of OleDbParameters
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
            OleDbTransaction transaction, 
            CommandType commandType, 
            string commandText, 
            params OleDbParameter[] commandParameters)
        {
            // create a command and prepare it for execution
            var cmd = new OleDbCommand();
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
        public static int ExecuteNonQuery(OleDbTransaction transaction, string spName, params object[] parameterValues)
        {
            // if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // pull the parameters for this stored procedure from the parameter cache (or discover them & populet the cache)
                OleDbParameter[] commandParameters =
                    OledbHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);

                // assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // call the overload that takes an array of OleDbParameters
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
        /// The <see cref="OleDbDataReader"/>.
        /// </returns>
        public static OleDbDataReader ExecuteReader(
            string connectionString, 
            CommandType commandType, 
            string commandText)
        {
            // pass through the call providing null for the set of OleDbParameters
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
        /// The <see cref="OleDbDataReader"/>.
        /// </returns>
        public static OleDbDataReader ExecuteReader(
            string connectionString, 
            CommandType commandType, 
            string commandText, 
            params OleDbParameter[] commandParameters)
        {
            // create & open an OleDbbConnection
            var cn = new OleDbConnection(connectionString);
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
                    OleDbConnectionOwnership.Internal);
            }
            catch
            {
                // if we fail to return the OleDbDataReader, we neeed to close the connection ourselves
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
        /// The <see cref="OleDbDataReader"/>.
        /// </returns>
        public static OleDbDataReader ExecuteReader(
            string connectionString, 
            string spName, 
            params object[] parameterValues)
        {
            // if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
                OleDbParameter[] commandParameters = OledbHelperParameterCache.GetSpParameterSet(
                    connectionString, 
                    spName);

                // assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // call the overload that takes an array of OleDbParameters
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
        /// The <see cref="OleDbDataReader"/>.
        /// </returns>
        public static OleDbDataReader ExecuteReader(
            OleDbConnection connection, 
            CommandType commandType, 
            string commandText)
        {
            // pass through the call providing null for the set of OleDbParameters
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
        /// The <see cref="OleDbDataReader"/>.
        /// </returns>
        public static OleDbDataReader ExecuteReader(
            OleDbConnection connection, 
            CommandType commandType, 
            string commandText, 
            params OleDbParameter[] commandParameters)
        {
            // pass through the call to the private overload using a null transaction value and an externally owned connection
            return ExecuteReader(
                connection, 
                null, 
                commandType, 
                commandText, 
                commandParameters, 
                OleDbConnectionOwnership.External);
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
        /// The <see cref="OleDbDataReader"/>.
        /// </returns>
        public static OleDbDataReader ExecuteReader(
            OleDbConnection connection, 
            string spName, 
            params object[] parameterValues)
        {
            // if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                OleDbParameter[] commandParameters =
                    OledbHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);

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
        /// The <see cref="OleDbDataReader"/>.
        /// </returns>
        public static OleDbDataReader ExecuteReader(
            OleDbTransaction transaction, 
            CommandType commandType, 
            string commandText)
        {
            // pass through the call providing null for the set of OleDbParameters
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
        /// The <see cref="OleDbDataReader"/>.
        /// </returns>
        public static OleDbDataReader ExecuteReader(
            OleDbTransaction transaction, 
            CommandType commandType, 
            string commandText, 
            params OleDbParameter[] commandParameters)
        {
            // pass through to private overload, indicating that the connection is owned by the caller
            return ExecuteReader(
                transaction.Connection, 
                transaction, 
                commandType, 
                commandText, 
                commandParameters, 
                OleDbConnectionOwnership.External);
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
        /// The <see cref="OleDbDataReader"/>.
        /// </returns>
        public static OleDbDataReader ExecuteReader(
            OleDbTransaction transaction, 
            string spName, 
            params object[] parameterValues)
        {
            // if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                OleDbParameter[] commandParameters =
                    OledbHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);

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
            // pass through the call providing null for the set of OleDbParameters
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
            params OleDbParameter[] commandParameters)
        {
            // create & open a OleDbConnection, and dispose of it after we are done.
            using (var cn = new OleDbConnection(connectionString))
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
                OleDbParameter[] commandParameters = OledbHelperParameterCache.GetSpParameterSet(
                    connectionString, 
                    spName);

                // assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // call the overload that takes an array of OleDbParameters
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
        public static object ExecuteScalar(OleDbConnection connection, CommandType commandType, string commandText)
        {
            // pass through the call providing null for the set of OleDbParameters
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
            OleDbConnection connection, 
            CommandType commandType, 
            string commandText, 
            params OleDbParameter[] commandParameters)
        {
            // create a command and prepare it for execution
            var cmd = new OleDbCommand();
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
        public static object ExecuteScalar(OleDbConnection connection, string spName, params object[] parameterValues)
        {
            // if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // pull the parameters for this stored procedure from the parameter cache (or discover them & populet the cache)
                OleDbParameter[] commandParameters =
                    OledbHelperParameterCache.GetSpParameterSet(connection.ConnectionString, spName);

                // assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // call the overload that takes an array of OleDbParameters
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
        public static object ExecuteScalar(OleDbTransaction transaction, CommandType commandType, string commandText)
        {
            // pass through the call providing null for the set of OleDbParameters
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
            OleDbTransaction transaction, 
            CommandType commandType, 
            string commandText, 
            params OleDbParameter[] commandParameters)
        {
            // create a command and prepare it for execution
            var cmd = new OleDbCommand();
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
        public static object ExecuteScalar(OleDbTransaction transaction, string spName, params object[] parameterValues)
        {
            // if we got parameter values, we need to figure out where they go
            if ((parameterValues != null) && (parameterValues.Length > 0))
            {
                // pull the parameters for this stored procedure from the parameter cache (or discover them & populet the cache)
                OleDbParameter[] commandParameters =
                    OledbHelperParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);

                // assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);

                // call the overload that takes an array of OleDbParameters
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
        private static void AssignParameterValues(OleDbParameter[] commandParameters, object[] parameterValues)
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

            // iterate through the OleDbParameters, assigning the values from the corresponding position in the 
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
        private static void AttachParameters(OleDbCommand command, IEnumerable<OleDbParameter> commandParameters)
        {
            foreach (OleDbParameter p in commandParameters)
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
        /// The <see cref="OleDbDataReader"/>.
        /// </returns>
        private static OleDbDataReader ExecuteReader(
            OleDbConnection connection, 
            OleDbTransaction transaction, 
            CommandType commandType, 
            string commandText, 
            OleDbParameter[] commandParameters, 
            OleDbConnectionOwnership connectionOwnership)
        {
            // create a command and prepare it for execution
            var cmd = new OleDbCommand();
            PrepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters);

            // create a reader
            OleDbDataReader dr;

            // call ExecuteReader with the appropriate CommandBehavior
            if (connectionOwnership == OleDbConnectionOwnership.External)
            {
                dr = cmd.ExecuteReader();
            }
            else
            {
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
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
            OleDbCommand command, 
            OleDbConnection connection, 
            OleDbTransaction transaction, 
            CommandType commandType, 
            string commandText, 
            IEnumerable<OleDbParameter> commandParameters)
        {
            // if the provided connection is not open, we will open it
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            // associate the connection with the command
            command.Connection = connection;

            // set the command text (stored procedure name or OleDb statement)
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