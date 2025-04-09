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
                    throw new KeyNotFoundException("El usuario no existe."); // 404
                }

                usuarioExistente.Identificacion = usuario.Identificacion;
                usuarioExistente.Nombre = usuario.Nombre;
                usuarioExistente.Apellidos = usuario.Apellidos;
                usuarioExistente.Email = usuario.Email;
                usuarioExistente.Telefono = usuario.Telefono;
                usuarioExistente.ContraseñaHash = usuario.ContraseñaHash;

                bool actualizado = await _usuarioRepository.ActualizarUsuario(usuarioExistente);

                if (!actualizado)
                {
                    throw new Exception("No se pudo actualizar el usuario."); // 500
                }

                return true; // Si todo sale bien, retorna `true`
            }
            catch (KeyNotFoundException ex)
            {
                throw; // Propaga la excepción para que el controlador la maneje
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el usuario: {ex.Message}");
            }
        }

    }

}

