using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Models.DapperConexion.Usuarios
{
    public interface IUsuario
    {
        
        Task<IEnumerable<UsuarioModel>> ObtenerLista();

        Task<UsuarioModel> ObtenerPorId(Guid id);

        Task<int> Nuevo(string username, string email, string password, bool emailConfirmed, bool phoneConfirmed, bool twoFactorEnabled, bool lockoutEnabled, int AccessFailedCount);

        Task<int> Actualiza(Guid id, string username, string email, string password);

        Task<int> Elimina(Guid id);
    }
}
