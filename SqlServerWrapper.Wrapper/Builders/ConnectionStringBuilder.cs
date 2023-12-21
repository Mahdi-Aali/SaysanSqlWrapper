using SqlServerWrapper.Wrapper.Connection;
using System.Text;

namespace SqlServerWrapper.Wrapper.Builders;


/// <summary>
/// Default connection string builder class.
/// </summary>
public class ConnectionStringBuilder : IConnectionStringBuilder
{
    private StringBuilder _connectionString;

    public ConnectionStringBuilder()
    {
        _connectionString = new();
    }

    /// <summary>
    /// Set server address to connect. by default is ".".
    /// </summary>
    /// <param name="server">Server address to connect</param>
    /// <returns><see cref="IConnectionStringBuilder"/></returns>
    public IConnectionStringBuilder Server(string server = ".")
    {
        _connectionString.Append($" Server={server};");
        return this;
    }

    /// <summary>
    /// Set data base name on server.
    /// </summary>
    /// <param name="dataBase">Data base name</param>
    /// <returns><see cref="IConnectionStringBuilder"/></returns>
    public IConnectionStringBuilder DataBase(string dataBase = "master")
    {
        _connectionString.Append($" Database={dataBase};");
        return this;
    }

    /// <summary>
    /// Set user id or username of data base.
    /// </summary>
    /// <param name="userId">User name or user id</param>
    /// <returns><see cref="IConnectionStringBuilder"/></returns>
    public IConnectionStringBuilder UserId(string userId = "SA") 
    {
        _connectionString.Append($" User Id={userId};");
        return this;
    }


    /// <summary>
    /// Set password of data base.
    /// </summary>
    /// <param name="password">Password of data base</param>
    /// <returns><see cref="IConnectionStringBuilder"/></returns>
    public IConnectionStringBuilder Password(string password)
    {
        _connectionString.Append($" Password={password};");
        return this;
    }



    /// <summary>
    /// Enable encryption.
    /// </summary>
    /// <returns><see cref="IConnectionStringBuilder"/></returns>
    public IConnectionStringBuilder EnableEncryption()
    {
        _connectionString.Append(" Encrypt=true;");
        return this;
    }


    /// <summary>
    /// Enable encryption.
    /// </summary>
    /// <returns><see cref="IConnectionStringBuilder"/></returns>
    public IConnectionStringBuilder DisableEncryption()
    {
        _connectionString.Append(" Encrypt=false;");
        return this;
    }

    /// <summary>
    /// Enable trusted connection.
    /// </summary>
    /// <returns><see cref="IConnectionStringBuilder"/></returns>
    public IConnectionStringBuilder EnableTrustedConnection()
    {
        _connectionString.Append(" Trusted_Connection=true;");
        return this;
    }

    /// <summary>
    /// Enable multiple active result sets.
    /// </summary>
    /// <returns><see cref="IConnectionStringBuilder"/></returns>
    public IConnectionStringBuilder EnableMARS()
    {
        _connectionString.Append(" MultipleActiveResultSets=true;");
        return this;
    }

    /// <summary>
    /// Set your netwrok library.
    /// </summary>
    /// <param name="networkLibrary">Name of network library</param>
    /// <returns><see cref="IConnectionStringBuilder"/></returns>
    public IConnectionStringBuilder NetworkLibrary(string networkLibrary = "DBMSSOCN")
    {
        _connectionString.Append($" Network Library={networkLibrary};");
        return this;
    }

    /// <summary>
    /// Set attached data base file path.
    /// </summary>
    /// <param name="path">Path of .mdf file</param>
    /// <returns><see cref="IConnectionStringBuilder"/></returns>
    public IConnectionStringBuilder AttachDbFilename(string path)
    {
        _connectionString.Append($" AttachDbFilename={path}");
        return this;
    }


    /// <summary>
    /// Enable integration security.
    /// </summary>
    /// <returns><see cref="IConnectionStringBuilder"/></returns>
    public IConnectionStringBuilder EnableIntegratedSecurity()
    {
        _connectionString.Append(" Integrated Security=true;");
        return this;
    }

    /// <summary>
    /// Enable Asynchronous proccessing in query.
    /// </summary>
    /// <returns><see cref="IConnectionStringBuilder"/></returns>
    public IConnectionStringBuilder EnableAsynchronousProcessing()
    {
        _connectionString.Append(" Asynchronous Processing=true;");
        return this;
    }

    /// <summary>
    /// Enable user instance.
    /// </summary>
    /// <returns><see cref="IConnectionStringBuilder"/></returns>
    public IConnectionStringBuilder EnableUserInstance()
    {
        _connectionString.Append("User Instance = true;");
        return this;
    }


    /// <summary>
    /// Set maximum query packet size.
    /// </summary>
    /// <param name="size">Size of packet</param>
    /// <returns></returns>
    public IConnectionStringBuilder SetMaximumPacketSize(int size)
    {
        _connectionString.Append($"Packet Size={size}");
        return this;
    }


    /// <summary>
    /// Build and generate connection string.
    /// </summary>
    /// <returns>Generated connection string.</returns>
    public string BuildAsString() => _connectionString.ToString();

    /// <summary>
    /// Build as default connection string.
    /// </summary>
    /// <returns><see cref="IConnectionString"/></returns>
    public IConnectionString BuildAsDefaultConnectionString() => new DefaultConnectionString(_connectionString.ToString());

    /// <summary>
    /// Build as spedified connection string.
    /// </summary>
    /// <typeparam name="TCS">type of connection string</typeparam>
    /// <returns><see cref="IConnectionString"/></returns>
    public IConnectionString BuildAsSpecifiedConnectionString<TCS>() where TCS : IConnectionString
    {
        TCS instance = (TCS)Activator.CreateInstance(typeof(TCS), _connectionString.ToString())!;
        return instance;
    }
}