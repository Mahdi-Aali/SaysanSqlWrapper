namespace SqlServerWrapper.Wrapper.Connection;

public class DefaultConnectionString : IConnectionString
{
    public string ConnectionString { get; set; } = string.Empty;

    public DefaultConnectionString(string connectionString) => ConnectionString = connectionString;
}
