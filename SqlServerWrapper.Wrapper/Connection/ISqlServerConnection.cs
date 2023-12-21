using Microsoft.Data.SqlClient;

namespace SqlServerWrapper.Wrapper.Connection;

public interface ISqlServerConnection
{
    public void UpdateConnectionString(string newConnectionString);

    public Task<bool> VerifyConnectionAsync(CancellationToken cancellationToken = default);
    public Task<bool> VerifyConnectionAsync(string connectionString, CancellationToken cancellationToken = default);

    public Task<SqlConnection> NewConnectionAsync(CancellationToken cancellationToken = default);
    public Task<SqlConnection> NewConnectionAsync(string connectionString, CancellationToken cancellationToken = default);


    public Task<SqlConnection> NewSafeConnectionAsync(CancellationToken cancellationToken = default);
    public Task<SqlConnection> NewSafeConnectionAsync(string connectionString, CancellationToken cancellationToken = default);



    public bool VerifyConnectionString();
}