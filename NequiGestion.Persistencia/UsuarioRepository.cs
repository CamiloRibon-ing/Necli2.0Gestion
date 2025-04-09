using Microsoft.Data.SqlClient;
using NecliGestion.Entidades2._0;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NecliGestion.Persistencia
{
    public class UsuarioRepository
    {
        private readonly string _cadena_conexion = "Server=SystemCamilo; Database=NecliDB; Trusted_connection=true; TrustServerCertificate=True;";

        public async Task<Usuario> ConsultarUsuario(int idUsuario)
        {
            Usuario usuario = null;
            string sql = "SELECT IdUsuario, Nombres, Email FROM Usuario WHERE IdUsuario = @IdUsuario";

            using (var conexion = new SqlConnection(_cadena_conexion))
            using (var comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@IdUsuario", idUsuario);
                await conexion.OpenAsync();
                using (var reader = await comando.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        usuario = new Usuario
                        {
                            IdUsuario = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Email = reader.GetString(2)
                        };
                    }
                }
            }
            return usuario;
        }

        public async Task<bool> ActualizarUsuario(Usuario usuario)
        {
            if (!EsEmailValido(usuario.Email))
            {
                throw new ArgumentException("El email no tiene un formato válido.");
            }

            if (await EmailExiste(usuario.Email, usuario.Identificacion))
            {
                throw new ArgumentException("El email ya está registrado por otro usuario.");
            }

            string sql = @"UPDATE Usuario SET Identificacion = @identificacion, Nombres = @Nombre, Apellidos = @Apellidos, Email = @Email, " +
                         "Telefono = @Telefono, ContraseñaHash = @ContraseñaHash WHERE IdUsuario= @IdUsuario";

            using (var conexion = new SqlConnection(_cadena_conexion))
            using (var comando = new SqlCommand(sql, conexion))
            {
               
                comando.Parameters.AddWithValue("@Identificacion", usuario.Identificacion);
                comando.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                comando.Parameters.AddWithValue("@Apellidos", usuario.Apellidos);
                comando.Parameters.AddWithValue("@Email", usuario.Email);
                comando.Parameters.AddWithValue("@Telefono", usuario.Telefono.Trim());
                comando.Parameters.AddWithValue("@ContraseñaHash", usuario.ContraseñaHash);
                comando.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);
                await conexion.OpenAsync();
                int filasAfectadas = await comando.ExecuteNonQueryAsync();
                return filasAfectadas > 0;
            }
        }

        private bool EsEmailValido(string email)
        {
            string patron = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, patron);
        }

        private async Task<bool> EmailExiste(string email, string identificacion)
        {
            string sql = "SELECT COUNT(*) FROM Usuario WHERE Email = @Email AND Identificacion <> @Identificacion";

            using (var conexion = new SqlConnection(_cadena_conexion))
            using (var comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.AddWithValue("@Email", email);
                comando.Parameters.AddWithValue("@Identificacion", identificacion ?? (object)DBNull.Value);

                await conexion.OpenAsync();
                int count = (int)await comando.ExecuteScalarAsync();
                return count > 0;
            }
        }
    }
}