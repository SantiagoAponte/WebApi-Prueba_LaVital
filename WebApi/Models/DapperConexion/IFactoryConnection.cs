using System.Data;
namespace WebApi.Models.DapperConexion
{
    public interface IFactoryConnection
    {
        void CloseConnection();
        IDbConnection GetConnection();
    }
}
