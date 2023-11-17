namespace MI_API_REST.Entities
{
    public class ModMenuSistema
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public int CodMes { get; set; }
        public string Accion { get; set; }
        public string Controlador { get; set; }

        public string Icono { get; set; }
        public int Orden { get; set; }

        public int TotalSubMenu { get; set; }
    }
}
