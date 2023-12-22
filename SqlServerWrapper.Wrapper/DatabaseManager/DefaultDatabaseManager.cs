using Microsoft.Data.SqlClient;
using SqlServerWrapper.Wrapper.Attributes;
using SqlServerWrapper.Wrapper.Connection;
using System.Data;
using System.Reflection;

namespace SqlServerWrapper.Wrapper.DatabaseManager;

public sealed class DefaultDatabaseManager : IDbManager
{
    private ISqlServerConnection _sqlConnection;
    
    public DefaultDatabaseManager(ISqlServerConnection sqlConnection)
    {
        _sqlConnection = sqlConnection;
    }

    
    public async Task<TResponse> CallSingleRowProcedureAsync<TResponse>(string procedureName, CancellationToken cancellationToken = default) where TResponse : class
        => await CallSingleRowProcedureWithParametersAsync<TResponse>(procedureName, null!, cancellationToken);

    
    public async Task<TResponse> CallSingleRowProcedureWithParametersAsync<TResponse>(string procedureName, object parameters, CancellationToken cancellationToken = default) where TResponse : class
    {
        using (SqlConnection sqlConnection = await _sqlConnection.NewSafeConnectionAsync())
        {
            using (SqlCommand command = new(procedureName, sqlConnection))
            {
                if (VerifyParameters(parameters))
                {
                    foreach(PropertyInfo property in GetProperties(parameters))
                    {
                        command.Parameters.AddWithValue($"@{property.Name}", property.GetValue(parameters));
                    }
                }
                command.CommandType = CommandType.StoredProcedure;
                await sqlConnection.OpenAsync(cancellationToken);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    TResponse response = Activator.CreateInstance<TResponse>();

                    while (await reader.ReadAsync(cancellationToken))
                    {
                        foreach (PropertyInfo property in GetProperties(response))
                        {
                            try
                            {
                                if (property.GetCustomAttribute<FieldAttribute>(false) != null)
                                {
                                    property.SetValue(response, reader[property.GetCustomAttribute<FieldAttribute>(false)!.Name]);
                                }
                                else
                                {
                                    property.SetValue(response, reader[property.Name]);
                                }
                            }
                            catch
                            {
                                property.SetValue(response, null);
                            }
                        }

                        continue;
                    }

                    return response;
                }
            }
        }
    }


    public async Task<ICollection<TResponse>> CallProcedureAsync<TResponse>(string procedureName, CancellationToken cancellationToken = default) where TResponse : class
        => await CallProcedureWithParametersAsync<TResponse>(procedureName, null!, cancellationToken);

    public async Task<ICollection<TResponse>> CallProcedureWithParametersAsync<TResponse>(string procedureName, object parameters, CancellationToken cancellationToken = default) where TResponse : class
    {
        using (SqlConnection sqlConnection = await _sqlConnection.NewSafeConnectionAsync())
        {
            using (SqlCommand command = new(procedureName, sqlConnection))
            {
                if (VerifyParameters(parameters))
                {
                    foreach (PropertyInfo property in GetProperties(parameters))
                    {
                        command.Parameters.AddWithValue($"@{property.Name}", property.GetValue(parameters));
                    }
                }
                command.CommandType = CommandType.StoredProcedure;
                await sqlConnection.OpenAsync(cancellationToken);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {

                    List<TResponse> result = new();
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        TResponse response = Activator.CreateInstance<TResponse>();
                        foreach (PropertyInfo property in GetProperties(response))
                        {
                            try
                            {
                                if (property.GetCustomAttribute<FieldAttribute>(false) != null)
                                {
                                    property.SetValue(response, reader[property.GetCustomAttribute<FieldAttribute>(false)!.Name]);
                                }
                                else
                                {
                                    property.SetValue(response, reader[property.Name]);
                                }
                            }
                            catch
                            {
                                property.SetValue(response, null);
                            }
                        }

                        result.Add(response);
                    }

                    return result;
                }
            }
        }
    }



    public async Task<TResponse> CallScalarProcedureAsync<TResponse>(string procedureName, CancellationToken cancellationToken = default)
    {
        using (SqlConnection sqlConnection = await _sqlConnection.NewSafeConnectionAsync())
        {
            using (SqlCommand command = new(procedureName, sqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;

                await sqlConnection.OpenAsync(cancellationToken);

                TResponse result = (TResponse)await command.ExecuteScalarAsync(cancellationToken);

                return result;
            }
        }
    }

    public async Task<TResponse> CallScalarProcedureWithParametersAsync<TResponse>(string procedureName, object parameters, CancellationToken cancellationToken = default)
    {
        using (SqlConnection sqlConnection = await _sqlConnection.NewSafeConnectionAsync())
        {
            using (SqlCommand command = new(procedureName, sqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;

                if (VerifyParameters(parameters))
                {
                    foreach(PropertyInfo property in parameters.GetType().GetProperties())
                    {
                        command.Parameters.AddWithValue($"@{property.Name}", property.GetValue(parameters));
                    }
                }

                await sqlConnection.OpenAsync(cancellationToken);

                TResponse result = (TResponse)await command.ExecuteScalarAsync(cancellationToken);

                return result;
            }
        }
    }


    public async Task<int> ExecuteNonQueryAsync(string cmd, CancellationToken cancellationToken = default) =>
        await ExecuteNonQueryWithParametersAsync(cmd, new { }, cancellationToken);

    
    public async Task<int> ExecuteNonQueryWithParametersAsync<Parameters>(string cmd, Parameters parameters, CancellationToken cancellationToken = default)
    {
        using (SqlConnection sqlConnection = await _sqlConnection.NewSafeConnectionAsync())
        {
            using (SqlCommand command = new(cmd, sqlConnection))
            {
                if (VerifyParameters(parameters!))
                {
                    command.Parameters.AddRange(GetParameters(parameters!, cmd));
                }
                await sqlConnection.OpenAsync(cancellationToken);
                return await command.ExecuteNonQueryAsync(cancellationToken);
            }
        }
    }




    
    public async Task<DataTable> ExecuteRawSqlAsync(string cmd, CancellationToken cancellationToken = default) =>
        await ExecuteRawSqlWithParametersAsync(cmd, new { }, cancellationToken);

    
    public async Task<DataTable> ExecuteRawSqlWithParametersAsync<Parameters>(string cmd, Parameters parameters, CancellationToken cancellationToken = default)
    {
        using (SqlConnection connection = await _sqlConnection.NewSafeConnectionAsync(cancellationToken))
        {
            using (SqlCommand command = new(cmd, connection))
            {
                if (VerifyParameters(parameters!))
                {
                    command.Parameters.AddRange(GetParameters(parameters!, cmd));
                }

                await connection.OpenAsync(cancellationToken);
                using (SqlDataAdapter adapter = new(command))
                {
                    DataTable dataTable = new();

                    adapter.Fill(dataTable);

                    return dataTable;
                }
            }
        }
    }



    
    public async Task<TResponse> ExecuteScalarAsync<TResponse>(string cmd, CancellationToken cancellationToken = default) =>
        await ExecuteScalarWithParametersAsync<TResponse>(cmd, null!, cancellationToken);
    
    public async Task<TResponse> ExecuteScalarWithParametersAsync<TResponse>(string cmd, object parameters, CancellationToken cancellationToken = default)
    {
        using (SqlConnection sqlConnection = await _sqlConnection.NewSafeConnectionAsync())
        {
            using (SqlCommand command = new(cmd, sqlConnection))
            {
                if (VerifyParameters(parameters!))
                {
                    command.Parameters.AddRange(GetParameters(parameters!, cmd));
                }
                await sqlConnection.OpenAsync(cancellationToken);
                TResponse response = (TResponse)await command.ExecuteScalarAsync(cancellationToken);
                return response;
            }
        }
    }


    
    public async Task<TResponse> ExecuteAndGetResponseAsync<TResponse>(string cmd, CancellationToken cancellationToken = default) where TResponse : class
        => await ExecuteAndGetResponseWithParametersAsync<TResponse>(cmd, null!, cancellationToken);
    
    public async Task<TResponse> ExecuteAndGetResponseWithParametersAsync<TResponse>(string cmd, object parameters, CancellationToken cancellationToken = default) where TResponse : class
    {
        using (SqlConnection sqlConnection = await _sqlConnection.NewSafeConnectionAsync())
        {
            using (SqlCommand command = new(cmd, sqlConnection))
            {
                if (VerifyParameters(parameters))
                {
                    command.Parameters.AddRange(GetParameters(parameters, cmd));
                }
                await sqlConnection.OpenAsync(cancellationToken);

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    TResponse response = Activator.CreateInstance<TResponse>();

                    while (await reader.ReadAsync(cancellationToken))
                    {
                        foreach (PropertyInfo property in GetProperties(response))
                        {
                            try
                            {
                                if (property.GetCustomAttribute<FieldAttribute>(false) != null)
                                {
                                    property.SetValue(response, reader[property.GetCustomAttribute<FieldAttribute>(false)!.Name]);
                                }
                                else
                                {
                                    property.SetValue(response, reader[property.Name]);
                                }
                            }
                            catch
                            {
                                property.SetValue(response, null);
                            }
                        }

                        continue;
                    }

                    return response;
                }
            }
        }
    }


    
    public async Task<ICollection<TResponse>> ExecuteAndGetResponsesAsync<TResponse>(string cmd, CancellationToken cancellationToken = default) where TResponse : class
        => await ExecuteAndGetResponsesWithParametersAsync<TResponse>(cmd, null!, cancellationToken);

    
    public async Task<ICollection<TResponse>> ExecuteAndGetResponsesWithParametersAsync<TResponse>(string cmd, object parameters, CancellationToken cancellationToken = default) where TResponse : class
    {
        using (SqlConnection sqlConnection = await _sqlConnection.NewSafeConnectionAsync())
        {
            using (SqlCommand command = new(cmd, sqlConnection))
            {
                if (VerifyParameters(parameters))
                {
                    command.Parameters.AddRange(GetParameters(parameters, cmd));
                }

                await sqlConnection.OpenAsync(cancellationToken);
                using (SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    List<TResponse> result = new();
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        TResponse response = Activator.CreateInstance<TResponse>();
                        foreach(PropertyInfo property in typeof(TResponse).GetProperties())
                        {
                            try
                            {
                                if (property.GetCustomAttribute<FieldAttribute>(false) != null)
                                {
                                    FieldAttribute attribute = property.GetCustomAttribute<FieldAttribute>(false)!;
                                    property.SetValue(response, reader[attribute.Name]);
                                }
                                else
                                {
                                    property.SetValue(response, reader[property.Name]);
                                }
                            }
                            catch
                            {
                                property.SetValue(response, null);
                            }
                        }

                        result.Add(response);
                    }

                    return result;
                }
            }
        }
    }


    
    private PropertyInfo[] GetProperties(object parameters) =>
        parameters.GetType().GetProperties();

    private SqlParameter[] GetParameters(object parameters, string cmd)
    {
        List<SqlParameter> sqlParameters = new();
        foreach(PropertyInfo property in GetProperties(parameters))
        {
            if (cmd.Contains($"@{property.Name}"))
            {
                sqlParameters.Add(new SqlParameter($"@{property.Name}", property.GetValue(parameters)));
            }
        }
        return sqlParameters.ToArray();
    }

    private bool VerifyParameters(object parameters) => parameters != null && GetProperties(parameters).Count() > 0;

}
