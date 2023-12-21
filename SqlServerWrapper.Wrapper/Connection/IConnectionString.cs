namespace SqlServerWrapper.Wrapper.Connection;

public interface IConnectionString
{
    public string ConnectionString { get; set; }

    public bool VerifyConnectionString() => !string.IsNullOrEmpty(ConnectionString);
}
