using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebApi.Models.DapperConexion.Usuarios.Aplicacion.ManejadorError
{
    public class ManejadorExcepcion : Exception
    {
        public HttpStatusCode Codigo { get; }
        public object Errors { get; }
        public ManejadorExcepcion(HttpStatusCode codigo, object errors = null)
        {
            Codigo = codigo;
            Errors = errors;
        }
    }
}
