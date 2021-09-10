using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models.Contracts;
using WebApi.Models.DapperConexion.Usuarios.Aplicacion.ManejadorError;

namespace WebApi.Models.DapperConexion.Usuarios.Login
{
    public class Registrar
    {
        public class Ejecuta : IRequest<usuarioDTO>
        {
            public string NombreCompleto { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }

            public string Username { get; set; }
        }

        public class EjecutaValidador : AbstractValidator<Ejecuta>
        {
            public EjecutaValidador()
            {
                RuleFor(x => x.NombreCompleto).NotEmpty();
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.Username).NotEmpty();
            }
        }



        public class Manejador : IRequestHandler<Ejecuta, usuarioDTO>
        {
            private readonly PruebaLaVitalContext _context;
            private readonly UserManager<UsuarioModel> _userManager;
            private readonly IJwtGenerador _jwtGenerador;
            public Manejador(PruebaLaVitalContext context, UserManager<UsuarioModel> userManager, IJwtGenerador jwtGenerador)
            {
                _context = context;
                _userManager = userManager;
                _jwtGenerador = jwtGenerador;
            }

            public async Task<usuarioDTO> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var existe = await _context.Users.Where(x => x.Email == request.Email).AnyAsync();
                if (existe)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new { mensaje = "Existe ya un usuario registrado con este email" });
                }

                var existeUserName = await _context.Users.Where(x => x.UserName == request.Username).AnyAsync();
                if (existeUserName)
                {
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new { mensaje = "Existe ya un usuario con este username" });
                }


                var usuario = new UsuarioModel
                {
                    Email = request.Email,
                    UserName = request.Username
                };

                var resultado = await _userManager.CreateAsync(usuario, request.Password);
                if (resultado.Succeeded)
                {
                    return new usuarioDTO
                    {
                       Token = _jwtGenerador.CrearToken(usuario, null),
                       Username = usuario.UserName,
                       Email = usuario.Email
                        
                    };                
                }
                throw new Exception("No se pudo agregar al nuevo usuario");
            }
        }
    }
}
