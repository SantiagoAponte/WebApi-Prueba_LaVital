using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace WebApi.Models.DapperConexion.Usuarios
{
    public class usuarioRepositorio : IUsuario
    {
        private readonly IFactoryConnection _factoryConnection;

        public usuarioRepositorio(IFactoryConnection factoryConnection)
        {
            _factoryConnection = factoryConnection;
        }

        public async Task<int> Actualiza(Guid id, string email, string Username, string Password)
        {
            var storeProcedure = "usp_usuarios_editar";
            try
            {

                var connection = _factoryConnection.GetConnection();
                var resultados = await connection.ExecuteAsync(
                    storeProcedure,
                    new
                    {
                        Id = id,
                        Email = email,
                        UserName = Username,
                        PasswordHash = Password
                    },
                    commandType: CommandType.StoredProcedure
                );

                _factoryConnection.CloseConnection();
                return resultados;

            }
            catch (Exception e)
            {
                throw new Exception("No se pudo editar la información del usuario", e);
            }

        }


        public async Task<int> Elimina(Guid id)
        {
            var storeProcedure = "usp_usuario_eliminar";
            try
            {
                var connection = _factoryConnection.GetConnection();
                var resultado = await connection.ExecuteAsync(
                    storeProcedure,
                    new
                    {
                        Id = id
                    },
                    commandType: CommandType.StoredProcedure
                );
                _factoryConnection.CloseConnection();
                return resultado;
            }
            catch (Exception e)
            {
                throw new Exception("No se pudo eliminar la información del usuario", e);
            }
        }


        public async Task<int> Nuevo(string username, string email, string password, bool emailConfirmed, bool phoneConfirmed, bool twoFactorEnabled, bool lockoutEnabled, int AccessFailedCount)
        {
            var storeProcedure = "usp_usuario_nuevo";

            try
            {
                var connection = _factoryConnection.GetConnection();
                var resultado = await connection.ExecuteAsync(
                storeProcedure,
                new
                {
                    Id = Guid.NewGuid(),
                    UserName = username,
                    Email = email,
                    PasswordHash = password,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = true,
                    LockoutEnabled = true,
                    AccessFailedCount = 9999

                },
                commandType: CommandType.StoredProcedure
                );

                _factoryConnection.CloseConnection();

                return resultado;
            }
            catch (Exception e)
            {
                throw new Exception("No se pudo guardar el nuevo usuario", e);
            }
        }

        public async Task<int> Nuevo2(string username, string email, string password, bool emailConfirmed, bool phoneConfirmed, bool twoFactorEnabled, bool lockoutEnabled, int AccessFailedCount)
        {
            var storeProcedure = "usp_usuario_nuevo";

            try
            {
                var connection = _factoryConnection.GetConnection();
                var resultado = await connection.ExecuteAsync(
                storeProcedure,
                new
                {
                    Id = 1,
                    Email = email,
                    UserName = username,
                    PasswordHash = password,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = true,
                    LockoutEnabled = true,
                    AccessFailedCount = 9999

                },
                commandType: CommandType.StoredProcedure
                );

                _factoryConnection.CloseConnection();

                return resultado;
            }
            catch (Exception e)
            {
                throw new Exception("No se pudo guardar el nuevo usuario", e);
            }
        }

        public async Task<IEnumerable<UsuarioModel>> ObtenerLista()
        {
            IEnumerable<UsuarioModel> usuarioList = null;
            var storeProcedure = "usp_obtener_usuarios";
            try
            {
                var connection = _factoryConnection.GetConnection();
               usuarioList = await connection.QueryAsync<UsuarioModel>(storeProcedure, null, commandType: CommandType.StoredProcedure);

            }
            catch (Exception e)
            {
                throw new Exception("Error en la consulta de datos", e);
            }
            finally
            {
                _factoryConnection.CloseConnection();
            }
            return usuarioList;
        }

        public async Task<UsuarioModel> ObtenerPorId(Guid id)
        {
            var storeProcedure = "usp_obtener_usuario_por_id";
            UsuarioModel usuario = null;
            try
            {
                var connection = _factoryConnection.GetConnection();
                usuario = await connection.QueryFirstAsync<UsuarioModel>(
                    storeProcedure,
                    new
                    {
                        Id = id
                    },
                    commandType: CommandType.StoredProcedure
                );

                return usuario;

            }
            catch (Exception e)
            {
                throw new Exception("No se pudo encontrar el usuario", e);
            }


        }
    }
}
