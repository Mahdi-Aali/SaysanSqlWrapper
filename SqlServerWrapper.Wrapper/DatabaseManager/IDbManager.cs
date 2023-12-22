using Microsoft.Data.SqlClient;
using System.Data;

namespace SqlServerWrapper.Wrapper.DatabaseManager;

public interface IDbManager
{
    public Task<DataTable> ExecuteRawSqlAsync(string cmd, CancellationToken cancellationToken = default);
    public Task<DataTable> ExecuteRawSqlWithParametersAsync<Parameters>(string cmd, Parameters parameters, CancellationToken cancellationToken = default);


    public Task<int> ExecuteNonQueryAsync(string cmd, CancellationToken cancellationToken = default);
    public Task<int> ExecuteNonQueryWithParametersAsync<Parameters>(string cmd, Parameters parameters, CancellationToken cancellationToken = default);


    public Task<TResponse> ExecuteScalarAsync<TResponse>(string cmd, CancellationToken cancellationToken = default);
    public Task<TResponse> ExecuteScalarWithParametersAsync<TResponse>(string cmd, object parameters, CancellationToken cancellationToken = default);

    public Task<TResponse> ExecuteAndGetResponseAsync<TResponse>(string cmd, CancellationToken cancellationToken = default) where TResponse : class;
    public Task<TResponse> ExecuteAndGetResponseWithParametersAsync<TResponse>(string cmd, object parameters, CancellationToken cancellationToken = default) where TResponse : class;

    public Task<ICollection<TResponse>> ExecuteAndGetResponsesAsync<TResponse>(string cmd, CancellationToken cancellationToken = default) where TResponse : class;
    public Task<ICollection<TResponse>> ExecuteAndGetResponsesWithParametersAsync<TResponse>(string cmd, object parameters, CancellationToken cancellationToken = default) where TResponse : class;

    public Task<TResponse> CallSingleRowProcedureAsync<TResponse>(string procedureName, CancellationToken cancellationToken = default) where TResponse : class;
    public Task<TResponse> CallSingleRowProcedureWithParametersAsync<TResponse>(string procedureName, object parameters, CancellationToken cancellationToken = default) where TResponse : class;


    public Task<ICollection<TResponse>> CallProcedureAsync<TResponse>(string procedureName, CancellationToken cancellationToken = default) where TResponse : class;
    public Task<ICollection<TResponse>> CallProcedureWithParametersAsync<TResponse>(string procedureName, object parameters, CancellationToken cancellationToken = default) where TResponse : class;

    public Task<TResponse> CallScalarProcedureAsync<TResponse>(string procedureName, CancellationToken cancellationToken = default);
    public Task<TResponse> CallScalarProcedureWithParametersAsync<TResponse>(string procedureName, object parameters, CancellationToken cancellationToken = default);
}
