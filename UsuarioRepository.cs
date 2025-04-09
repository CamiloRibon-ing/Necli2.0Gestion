public async Task<bool> ActualizarUsuario(Usuario usuario)
{
    if (!EsEmailValido(usuario.Email))
    {
        throw new ArgumentException("El email no tiene un formato válido.");
    }

    if (await EmailExiste(usuario.Email, usuario.Identificacion.ToString()))
    {
        throw new ArgumentException("El email ya está registrado por otro usuario.");
    }

    string sql = "UPDATE Usuario SET Nombres = @Nombre, Apellidos = @Apellidos, Email = @Email, " +
                 "Telefono = @Telefono, ContraseñaHash = @ContraseñaHash WHERE Identificacion = @Identificacion";

    using (var conexion = new SqlConnection(_cadena_conexion))
    using (var comando = new SqlCommand(sql, conexion))
    {
        comando.Parameters.AddWithValue("@Identificacion", usuario.Identificacion);
        comando.Parameters.AddWithValue("@Nombre", usuario.Nombre);
        comando.Parameters.AddWithValue("@Apellidos", usuario.Apellidos);
        comando.Parameters.AddWithValue("@Email", usuario.Email);
        comando.Parameters.AddWithValue("@Telefono", usuario.Telefono);
        comando.Parameters.AddWithValue("@ContraseñaHash", usuario.ContraseñaHash); 

        await conexion.OpenAsync();
        int filasAfectadas = await comando.ExecuteNonQueryAsync();
        return filasAfectadas > 0;
    }
}
