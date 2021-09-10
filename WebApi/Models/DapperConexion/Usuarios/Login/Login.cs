using System.Resources;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using WebApi.Models.Contracts;
using WebApi.Data;
using WebApi.Seguridad.TokenSeguridad.Seguridad;
using System;
using WebApi.Models.DapperConexion.Usuarios.Aplicacion.ManejadorError;

namespace WebApi.Models.DapperConexion.Usuarios.Login
{
    public class Login
    {
        public class Ejecuta : IRequest<usuarioDTO>
        {
            public string Email { get; set; }
            public string PasswordHash { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.PasswordHash).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta, usuarioDTO>
        {

            private readonly UserManager<UsuarioModel> _userManager;
            private readonly SignInManager<UsuarioModel> _signInManager;
            private readonly IJwtGenerador _jwtGenerador;
            public Manejador(UserManager<UsuarioModel> userManager, SignInManager<UsuarioModel> signInManager, IJwtGenerador jwtGenerador)
            {
                _userManager = userManager;
                _jwtGenerador = jwtGenerador;
                _signInManager = signInManager;
            }

            public async Task<usuarioDTO> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByEmailAsync(request.Email);
                if (usuario == null)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);
                }

                var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, request.PasswordHash, false);
                var resultadoRoles = await _userManager.GetRolesAsync(usuario);
                var listaRoles = new List<string>(resultadoRoles);

                if (resultado.Succeeded)
                {
                    return new usuarioDTO
                    {
                        Email = usuario.Email,
                        Token = _jwtGenerador.CrearToken(usuario, listaRoles),
                        Username = usuario.UserName,
                    };

                }
                    throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);
            }
        }
    }
}