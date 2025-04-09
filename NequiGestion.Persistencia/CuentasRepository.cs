using NecliGestion.Entidades2._0;
using Microsoft.Data.SqlClient;

namespace NecliGestion.Persistencia
{
    public class CuentasRepository
    {
        private readonly string _cadena_conexion = "Server=SystemCamilo; Database=NecliDB; Trusted_connection=true; TrustServerCertificate=True;";
        private string sql;

        public bool CrearCuenta(Cuentas cuenta)
        {
            using (var conexion = new SqlConnection(_cadena_conexion))
            {
                conexion.Open();

                // Verificar si el usuario existe antes de crear la cuenta
                string verificarUsuario = "SELECT COUNT(1) FROM Usuario WHERE IdUsuario = @IdUsuario";
                using (var comandoVerificar = new SqlCommand(verificarUsuario, conexion))
                {
                    comandoVerificar.Parameters.AddWithValue("@IdUsuario", cuenta.IdUsuario);
                    int usuarioExiste = (int)comandoVerificar.ExecuteScalar();

                    if (usuarioExiste == 0)
                    {
                        throw new Exception($"El IdUsuario {cuenta.IdUsuario} no existe en la base de datos.");
                    }
                }

                // Si el usuario existe, crear la cuenta
                sql = @"INSERT INTO Cuenta (IdUsuario, Saldo, FechaCreacion)
                VALUES(@IdUsuario, @Saldo, @FechaCreacion)";

                using (var comando = new SqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@IdUsuario", cuenta.IdUsuario);
                    comando.Parameters.AddWithValue("@Saldo", cuenta.Saldo);
                    comando.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);

                    comando.ExecuteNonQuery();
                }
            }
            return true;
        }

        public async Task<Cuentas> ObtenerCuentaPorId(int numeroCuenta)
        {
            using (var conexion = new SqlConnection(_cadena_conexion))
            {
                string sql = @"SELECT NumeroCuenta, IdUsuario, Saldo, FechaCreacion 
                               FROM dbo.Cuenta WHERE NumeroCuenta = @NumeroCuenta";

                using (var comando = new SqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@NumeroCuenta", numeroCuenta);
                    conexion.Open();

                    using (var reader = await comando.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            return new Cuentas
                            {
                                NumeroCuenta = reader.GetInt32(0),
                                IdUsuario = reader.GetInt32(1),
                                Saldo = reader.GetDecimal(2),
                                FechaCreacion = reader.GetDateTime(3)
                            };
                        }
                        return null;
                    }
                }
            }
        }

        

        public async Task<List<Cuentas>> ObtenerTodasLasCuentas()
        {
            var listaCuentas = new List<Cuentas>();

            using (var conexion = new SqlConnection(_cadena_conexion))
            {
                string sql = @"SELECT NumeroCuenta, IdUsuario, Saldo, FechaCreacion FROM dbo.Cuenta";

                using (var comando = new SqlCommand(sql, conexion))
                {
                    conexion.Open();
                    using (var reader = await comando.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            listaCuentas.Add(new Cuentas
                            {
                                NumeroCuenta = reader.GetInt32(0),
                                IdUsuario = reader.GetInt32(1),
                                Saldo = reader.GetDecimal(2),
                                FechaCreacion = reader.GetDateTime(3)
                            });
                        }
                    }
                }
                return listaCuentas;
            }


    
        
        }

        public async Task<bool> EliminarCuenta(int numeroCuenta)
        {
            using (var conexion = new SqlConnection(_cadena_conexion))
            {
                string sql = @"DELETE FROM Cuenta WHERE NumeroCuenta = @NumeroCuenta";

                using (var comando = new SqlCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@NumeroCuenta", numeroCuenta);
                    conexion.Open();

                    int filasAfectadas = await comando.ExecuteNonQueryAsync();
                    return filasAfectadas > 0;
                }
            }
        }
    }

}