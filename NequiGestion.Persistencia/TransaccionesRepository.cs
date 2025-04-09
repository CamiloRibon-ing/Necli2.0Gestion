using Microsoft.Data.SqlClient;
using NecliGestion.Entidades2._0;
 // ✅ Importa la entidad correcta
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NequiGestion.Persistencia
{
    public class TransaccionesRepository

    {
        private readonly string _cadena_conexion = "Server=SystemCamilo; Database=NecliDB; Trusted_connection=true; TrustServerCertificate=True;";
        public async Task<bool> RegistrarTransaccion(Transacciones transaccion)
        {
            using (var conexion = new SqlConnection(_cadena_conexion))
            {
                await conexion.OpenAsync();
                SqlTransaction transaccionSql = conexion.BeginTransaction();

                try
                {
                    // 1. Actualizar saldo cuenta origen (restar)
                    string actualizarOrigen = @"UPDATE Cuenta SET Saldo = Saldo - @Monto WHERE NumeroCuenta = @CuentaOrigen";
                    using (var comandoOrigen = new SqlCommand(actualizarOrigen, conexion, transaccionSql))
                    {
                        comandoOrigen.Parameters.AddWithValue("@Monto", transaccion.Monto);
                        comandoOrigen.Parameters.AddWithValue("@CuentaOrigen", transaccion.NumeroCuentaOrigen);
                        await comandoOrigen.ExecuteNonQueryAsync();
                    }

                    // 2. Actualizar saldo cuenta destino (sumar)
                    string actualizarDestino = @"UPDATE Cuenta SET Saldo = Saldo + @Monto WHERE NumeroCuenta = @CuentaDestino";
                    using (var comandoDestino = new SqlCommand(actualizarDestino, conexion, transaccionSql))
                    {
                        comandoDestino.Parameters.AddWithValue("@Monto", transaccion.Monto);
                        comandoDestino.Parameters.AddWithValue("@CuentaDestino", transaccion.NumeroCuentaDestino);
                        await comandoDestino.ExecuteNonQueryAsync();
                    }

                    // 3. Insertar la transacción
                    string sql = @"INSERT INTO Transaccion(NumeroTransaccion,NumeroCuentaOrigen, NumeroCuentaDestino, Monto, Fecha, Tipo)
                                   VALUES (@NumeroTransaccion,@NumeroCuentaOrigen, @NumeroCuentaDestino, @Monto, @Fecha, @Tipo)";
                    using (var comando = new SqlCommand(sql, conexion, transaccionSql))
                    {
                        comando.Parameters.AddWithValue("@NumeroTransaccion", transaccion.NumeroTransaccion);
                        comando.Parameters.AddWithValue("@NumeroCuentaOrigen", transaccion.NumeroCuentaOrigen);
                        comando.Parameters.AddWithValue("@NumeroCuentaDestino", transaccion.NumeroCuentaDestino);
                        comando.Parameters.AddWithValue("@Monto", transaccion.Monto);
                        comando.Parameters.AddWithValue("@Fecha", transaccion.Fecha);
                        comando.Parameters.AddWithValue("@Tipo", transaccion.Tipo);

                        int filasAfectadas = await comando.ExecuteNonQueryAsync();

                        // Confirmar la transacción si todo fue bien
                        transaccionSql.Commit();
                        return filasAfectadas > 0;
                    }
                }
                catch
                {
                    // Revertir si hay error
                    transaccionSql.Rollback();
                    return false;
                }
            }
        }

        public async Task<Transacciones> ObtenerTransaccionPorId(Guid numeroTransaccion)
        {
            using (var conexion = new SqlConnection(_cadena_conexion))
            {
                string sql = "SELECT NumeroTransaccion, NumeroCuentaOrigen, NumeroCuentaDestino, Monto, Fecha,Tipo  FROM Transaccion WHERE NumeroTransaccion = @NumeroTransaccion";
                using (var comando = new SqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@NumeroTransaccion", numeroTransaccion);
                    await conexion.OpenAsync();
                    using (var reader = await comando.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            return new Transacciones
                            {
                                NumeroTransaccion = reader.GetGuid(0),
                                NumeroCuentaOrigen = reader.GetInt32(1),
                                NumeroCuentaDestino = reader.GetInt32(2),
                                Monto = reader.GetDecimal(3),
                                Fecha = reader.GetDateTime(4),
                                Tipo = reader.GetString(5)
                            };
                        }
                        return null;
                    }
                }
            }
        }

        public async Task<List<Transacciones>> ObtenerTodaslasTransacciones()
        {
            var listaTransacciones = new List<Transacciones>();

            using (var conexion = new SqlConnection(_cadena_conexion))
            {
                string sql = "SELECT NumeroTransaccion, NumeroCuentaOrigen, NumeroCuentaDestino, Monto, Fecha, Tipo FROM Transaccion";
                using (var comando = new SqlCommand(sql, conexion))
                {
                    await conexion.OpenAsync();
                    using (var reader = await comando.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            listaTransacciones.Add(new Transacciones
                            {
                                NumeroTransaccion = reader.GetGuid(0),
                                NumeroCuentaOrigen = reader.GetInt32(1),
                                NumeroCuentaDestino = reader.GetInt32(2),
                                Monto = reader.GetDecimal(3),
                                Fecha = reader.GetDateTime(4),
                                Tipo = reader.GetString(5)
                            });
                        }
                    }
                }

            }
            return listaTransacciones;
        }
    
    public async Task<bool> EliminarTransaccion(Guid numeroTransaccion)
        {
            using (var conexion = new SqlConnection(_cadena_conexion))
            {
                string sql = "DELETE FROM Transaccion WHERE NumeroTransaccion = @NumeroTransaccion";
                using (var comando = new SqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@NumeroTransaccion", numeroTransaccion);
                    await conexion.OpenAsync();
                    int filasAfectadas = await comando.ExecuteNonQueryAsync();
                    return filasAfectadas > 0;
                }
            }
        }
    }
}


