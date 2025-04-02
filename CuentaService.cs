public async Task<bool> EliminarCuenta(EliminarCuentaDto eliminarCuentaDto)
{
    if (!int.TryParse(eliminarCuentaDto.NumeroCuenta, out int numeroCuenta))
    {
        throw new Exception("Número de cuenta inválido");
    }

    var cuenta = await _cuentaRepo.ObtenerCuentaPorId(numeroCuenta);

    if (cuenta == null)
    {
        throw new Exception("Cuenta no Encontrada");
    }
    if (cuenta.Saldo > 50000)
    {
        throw new Exception("No se puede eliminar la cuenta porque tiene saldo pendiente de $50.000");
    }
    return await _cuentaRepo.EliminarCuenta(numeroCuenta);
}
