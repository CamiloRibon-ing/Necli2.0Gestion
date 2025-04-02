using NecliGestion.Entidades2._0;
using NecliGestion.Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecliGestion.Logica.Services
{
    public class UsuarioService
    {
        private readonly UsuarioRepository _usuarioRepository;

        public UsuarioService()
        {
            _usuarioRepository = new UsuarioRepository();
        }

        public async Task<Usuario> ConsultarUsuario(int idUsuario)
        {
            return await _usuarioRepository.ConsultarUsuario(idUsuario);
        }

        public async Task<bool> ActualizarUsuario(Usuario usuario)
        {
            try
            {
                var usuarioExistente = await _usuarioRepository.ConsultarUsuario(usuario.IdUsuario);
                if (usuarioExistente == null)
                {
                    return false; // No se encontró el usuario
                }
                usuarioExistente.Identificacion = usuario.Identificacion;
                usuarioExistente.Nombre = usuario.Nombre;
                usuarioExistente.Apellidos = usuario.Apellidos;
                usuarioExistente.Email = usuario.Email;
                usuarioExistente.Telefono = usuario.Telefono;
                usuarioExistente.ContraseñaHash = usuario.ContraseñaHash;

                return await _usuarioRepository.ActualizarUsuario(usuarioExistente);
            }
            catch (ArgumentException ex)
            {
                // Aquí podrías manejar la excepción según la lógica de negocio
                throw new Exception($"Error al actualizar el usuario: {ex.Message}");
            }
            catch (Exception)
            {
                return false; // Error inesperado en el sistema
            }
        }
    }
}
