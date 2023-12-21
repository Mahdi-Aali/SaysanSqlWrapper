using SqlServerWrapper.Wrapper.Connection;

namespace SqlServerWrapper.Wrapper.Builders;

public interface IConnectionStringBuilder
{
    public IConnectionStringBuilder Server(string server = ".");
    public IConnectionStringBuilder DataBase(string dataBase = "master");
    public IConnectionStringBuilder UserId(string userId = "SA");
    public IConnectionStringBuilder Password(string password);
    public IConnectionStringBuilder EnableEncryption();
    public IConnectionStringBuilder DisableEncryption();
    public IConnectionStringBuilder EnableTrustedConnection();
    public IConnectionStringBuilder EnableMARS();
    public IConnectionStringBuilder NetworkLibrary(string networkLibrary = "DBMSSOCN");
    public IConnectionStringBuilder AttachDbFilename(string path);
    public IConnectionStringBuilder EnableIntegratedSecurity();
    public IConnectionStringBuilder EnableAsynchronousProcessing();
    public IConnectionStringBuilder EnableUserInstance();
    public IConnectionStringBuilder SetMaximumPacketSize(int size);
    public string BuildAsString();
    public IConnectionString BuildAsDefaultConnectionString();
    public IConnectionString BuildAsSpecifiedConnectionString<TCS>() where TCS : IConnectionString;
}
