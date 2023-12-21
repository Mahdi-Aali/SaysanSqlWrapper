using Microsoft.Data.SqlClient;

namespace SqlServerWrapper.Wrapper.Connection;

public class DefaultSqlServerConnection : ISqlServerConnection
{

    private IConnectionString _connectionString;
    public DefaultSqlServerConnection()
    {
        
    }

    public DefaultSqlServerConnection(IConnectionString connectionString) => _connectionString = connectionString;

    public async Task<SqlConnection> NewConnectionAsync(CancellationToken cancellationToken = default)
    {
        if (VerifyConnectionString())
        {
            return await NewConnectionAsync(_connectionString.ConnectionString, cancellationToken);
        }

        throw new NullReferenceException("Connection string is null.");
    }

    public async Task<SqlConnection> NewConnectionAsync(string connectionString, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString), "Connection string can not be null!");
        }

        if (!cancellationToken.IsCancellationRequested)
        {
            return await Task.FromResult(new SqlConnection(connectionString));
        }

        throw new TaskCanceledException("Task canceled before execution.");
    }

    public async Task<SqlConnection> NewSafeConnectionAsync(CancellationToken cancellationToken = default)
    {
        if (VerifyConnectionString())
        {
            return await NewSafeConnectionAsync(_connectionString.ConnectionString, cancellationToken);
        }

        throw new NullReferenceException("Connection string is null!");
    }

    public async Task<SqlConnection> NewSafeConnectionAsync(string connectionString, CancellationToken cancellationToken = default)
    {
        if (await VerifyConnectionAsync(connectionString, cancellationToken))
        {
            return await NewConnectionAsync(connectionString, cancellationToken);
        }
        throw new Exception("Connection not verified.");
    }

    public void UpdateConnectionString(string newConnectionString) => _connectionString = new DefaultConnectionString(newConnectionString);

    public async Task<bool> VerifyConnectionAsync(CancellationToken cancellationToken = default)
    {
        if (VerifyConnectionString())
        {
            return await VerifyConnectionAsync(_connectionString.ConnectionString, cancellationToken);
        }

        return false;
    }

    public async Task<bool> VerifyConnectionAsync(string connectionString, CancellationToken cancellationToken = default)
    {
        try
        {
            SqlConnection sqlConnection = await NewConnectionAsync(connectionString, cancellationToken);
            await sqlConnection.OpenAsync(cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public bool VerifyConnectionString() => _connectionString != null && _connectionString.VerifyConnectionString(); 
}
