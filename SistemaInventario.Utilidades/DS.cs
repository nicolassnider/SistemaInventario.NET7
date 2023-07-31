namespace SistemaInventario.Utilidades
{
    public static class DS
    {
        public const string Exitosa = "Exitosa";
        public const string Error = "Error";

        public const string ImagenRuta = @"\img\producto\";

        public const string Role_Admin = "Admin";
        public const string Role_Cliente = "Cliente";
        public const string Role_Inventario = "Inventario";

        public const string Tipo_Entrada = "Entrada";
        public const string Tipo_Salida = "Salida";

        public const string ssCarroCompras = "Sesion Carro Compras";

        //Estado de ordenes
        public const string EstadoPendiente = "Pendiente";
        public const string EstadoAprobado = "Aprobado";
        public const string EstadoEnProceso = "Procesando";
        public const string EstadoEnviado = "Enviado";
        public const string EstadoCancelado = "Cancelado";
        public const string EstadoDevuelto = "Devuelto";

        //Estado de pagos
        public const string PagoEstadoPendiente = "Pendiente";
        public const string PagoEstadoAprobado = "Aprobado";
        public const string PagoEstadoRetrasado = "Retrasado";
        public const string PagoEstadoRechazado = "Rechazado";
    }
}
