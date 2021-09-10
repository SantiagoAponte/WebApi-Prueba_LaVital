using System.Collections.Generic;
using WebApi.Models.DapperConexion.Usuarios;

namespace WebApi.Models.Contracts
{
    public interface IJwtGenerador
    {
        string CrearToken(UsuarioModel usuario, List<string> roles);
    }
}
