using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Ctrl
{
    abstract class Controlador
    {
        #region Estructuras
        public class Tanque
        {
            public int producto { get; set; }
        }

        public class Producto
        {
            public int id { get; set; }
            public double precio { get; set; }
        }

        public class Surtidor
        {
            public Surtidor()
            {
                productoPorManguera = new List<int>();
            }
            public int nivelDePrecios { get; set; }
            public List<int> productoPorManguera { get; set; }
        }

        public class ConfigEstacion
        {
            public ConfigEstacion(Conector.ConfiguracionEstacion config)
            {
                protocolo = config.protocolo;
                surtidores = new List<Surtidor>();
                tanques = new List<Tanque>();
                productos = new List<Producto>();

                foreach (var surtidor in config.surtidores)
                {
                    Surtidor tmp = new Surtidor();
                    tmp.nivelDePrecios = surtidor.nivelDePrecios;
                    tmp.productoPorManguera = new List<int>();
                    tmp.productoPorManguera = surtidor.productoPorManguera;
                    surtidores.Add(tmp);
                }

                foreach (var producto in config.productoPorTanque)
                {
                    Tanque tmp = new Tanque();
                    tmp.producto = producto;
                    tanques.Add(tmp);
                }

                foreach (var producto in config.productos)
                {
                    Producto tmp = new Producto();
                    tmp.id = Convert.ToInt32(producto.id);
                    tmp.precio = Convert.ToDouble(producto.id);
                    productos.Add(tmp);
                }
            }

            public int protocolo { get; set; }
            public List<Surtidor> surtidores { get; set; }
            public List<Tanque> tanques { get; set; }
            public List<Producto> productos { get; set; }
        }
        #endregion
        #region Atributos
        // Instancia de Singleton
        static private Controlador instancia = null;
        // Instancia del conector que va a utilizar
        protected Conector conector;
        // Instancia de la configuración de la estación cargada en ejecución
        protected ConfigEstacion configEstacion;
        // Hilo para manejar el proceso principal de consulta al controlador en paralelo
        // al resto de la ejecución
        static private Thread procesoPrincipal = null;

        // Mutex para control del hilo del proceso principal
        static public Mutex working = new Mutex();
        // Tiempo de espera entre cada procesamiento en segundos.
        static private int loopDelaySeconds = 2;
        #endregion

        /// <summary>
        /// Metodo para obtener la instancia al conector
        /// </summary>
        /// <returns>Instancia de Conector</returns>
        static public Conector getConector()
        {
            return instancia.conector;
        }

        /// <summary>
        /// Este método debe consultar los despachos al controlador y 
        /// grabarlos en la base de datos SQLite
        /// </summary>
        public abstract void grabarDespachos();

        /// <summary>
        /// Este método debe consultar el estado de los tanques y
        /// actualizar los valores en la base de datos (tabla tanques)
        /// </summary>
        public abstract void actualizarTanques();

        /// <summary>
        /// Verifica para cada despacho en la tabla despachos que tenga una fecha
        /// mas antigua que el valor cargado en la configuración de facturación automática y
        /// lo marca como facturado. En caso de que no esté habilitada la facturación
        /// automática no debe hacer nada este método.
        /// </summary>
        public abstract void facturaDespachos();

        /// <summary>
        /// Verifica la tabla cierreBandera y en caso de tener que hacer un cierre
        /// envía el comando correspondiente y graba la tabla cierres con los totales
        /// </summary>
        public abstract void compruebaCierre();

        public abstract void ObtenerSurtidores();

        /// <summary>
        /// Este método estático es el encargado de crear la instancia del controlador
        /// correspondiente y ejecutar el hilo del proceso automático
        /// </summary>
        /// <param name="config"> La configuración extraída del archivo de configuración </param>
        /// <returns> true si se pudo inicializar correctamente </returns>
        static public bool init(Configuracion.InfoConfig config)
        {
            if (instancia == null)
            {

                switch (config.tipo)
                {
                    case Configuracion.TipoControlador.CEM:
                        instancia = new ControladorCEM();
                        break;
                        /*
                    case Configuracion.TipoControlador.WOLF:
                        instancia = new ControladorWOLF();
                        break;
                    case Configuracion.TipoControlador.VOX:
                        instancia = new ControladorVOX();
                        break;
                    case Configuracion.TipoControlador.MIDEX:
                        instancia = new ControladorMIDEX();
                        break;
                    case Configuracion.TipoControlador.PAM:
                        instancia = new ControladorPAM();
                        break;
                        */
                    default:
                        return false;
                }
            }
            if (procesoPrincipal == null)
            {
                procesoPrincipal = new Thread(new ThreadStart(loop1));

                if (!procesoPrincipal.IsAlive)
                {
                    procesoPrincipal.Start();
                    Log.Instance.writeLog("Proceso de carga de despachos iniciado", Log.LogType.t_info);
                }
            }

            return true;
        }

        static private void loop1()
        {
            while (working.WaitOne(1))
            {
                try
                {
                    instancia.grabarDespachos();
                    /*
                    instancia.ObtenerSurtidores();
                    */


                    /// Devuelvo el mutex para escuchar eventos
                    working.ReleaseMutex();

                    /// Espera para procesar nuevamente
                    Thread.Sleep(loopDelaySeconds * 10);
                }
                catch (Exception e)
                {
                    working.ReleaseMutex();
                    Log.Instance.writeLog("Error en el loop del controlador. Excepcion: " + e.Message, Log.LogType.t_error);
                }
            }
        }

        /// <summary>
        /// Este método estático crea una instancia del controlador correspondiente, 
        /// y lo deja en modo manual (a diferencia de la llamada al método "init")
        /// </summary>
        /// <param name="config"> La configuración extraída del archivo de configuración </param>
        /// <returns> true si se pudo inicializar correctamente </returns>
        static public bool configInit(Configuracion.InfoConfig config)
        {
            if (instancia != null)
                return true;

            switch (config.tipo)
            {
                case Configuracion.TipoControlador.CEM:
                    instancia = new ControladorCEM();
                    break;
                    /*
                case Configuracion.TipoControlador.WOLF:
                    instancia = new ControladorWOLF();
                    break;
                case Configuracion.TipoControlador.VOX:
                    instancia = new ControladorVOX();
                    break;
                case Configuracion.TipoControlador.MIDEX:
                    instancia = new ControladorMIDEX();
                    break;
                case Configuracion.TipoControlador.PAM:
                    instancia = new ControladorPAM();
                    break;
                default:
                    return false;
                    */
            }

            return true;
        }
    }
    class ControladorCEM : Controlador
    {
        public ControladorCEM()
        {
            conector = new ConectorCEM();

            configEstacion = new ConfigEstacion(conector.configuracionDeEstacion());
        }

        public override void actualizarTanques()
        {
            //Implementar metodo ActualizarTanques
        }

        public override void grabarDespachos()
        {
            try
            {
                // Por cada surtidor grabamos el ultimo despacho que no esté grabado
                for (int surtidor = 1; surtidor < configEstacion.surtidores.Count + 1; surtidor++)
                {
                    // Traigo del CEM el despacho
                    Conector.Despacho despacho = conector.traerDespacho(surtidor);

                    if (despacho.status == Conector.Despacho.EstadoDespacho.DESPACHANDO || despacho.nroVenta == 0 || Convert.ToInt32(despacho.id.Trim().Substring(1)) == 0)
                        continue;

                    Log.Instance.writeLog("SELECT * FROM despachos WHERE surtidor = " + surtidor + " AND id = '" + despacho.id.Trim().Substring(1) + "'", Log.LogType.t_debug);
                    DataTable tabla = ConectorSQLite.dt_query("SELECT * FROM despachos WHERE surtidor = " + surtidor + " AND id = '" + despacho.id.Trim().Substring(1) + "'");

                    if (tabla.Rows.Count == 0)
                    {
                        string campos = "id,surtidor,producto,monto,volumen,ppu,YPFruta";

                        DataTable producto = ConectorSQLite.dt_query(String.Format("SELECT id,precio FROM productos WHERE idSecundario = {0}", despacho.producto));

                        int idProducto = 0;
                        int YPFruta = 0;
                        if (producto.Rows.Count != 0)
                        {
                            idProducto = Convert.ToInt32(producto.Rows[0]["id"]);
                            if (Convert.ToDouble(producto.Rows[0]["precio"]) != Convert.ToDouble(despacho.PPU))
                                YPFruta = 1;
                        }
                        else
                            idProducto = despacho.producto;

                        string valores = String.Format(
                            "'{0}',{1},{2},'{3}','{4}','{5}',{6}",
                            despacho.id.Trim().Substring(1),
                            surtidor,
                            idProducto,
                            despacho.monto.Trim(),
                            despacho.volumen.Trim(),
                            despacho.PPU.Trim(),
                            YPFruta);
                        Log.Instance.writeLog("Insertando despacho " + despacho.id, Log.LogType.t_debug);
                        ConectorSQLite.query(String.Format("INSERT INTO despachos ({0}) VALUES ({1})", campos, valores));
                        string rutaBaseDatos = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Configuracion.leerConfiguracion().rutaProyNuevo + "\\Fusion\\interfusion.mdb"; // Ruta de tu base de datos Access
                    }

                    if (despacho.id_old == null || despacho.id_old == "")
                        return;

                    if (Convert.ToInt32(despacho.id_old.Trim().Substring(1)) == 0)
                        continue;

                    Log.Instance.writeLog("SELECT * FROM despachos WHERE surtidor = " + surtidor + " AND id = '" + despacho.id_old.Trim().Substring(1) + "'", Log.LogType.t_debug);
                    tabla = ConectorSQLite.dt_query("SELECT * FROM despachos WHERE surtidor = " + surtidor + " AND id = '" + despacho.id_old.Trim().Substring(1) + "'");

                    if (tabla.Rows.Count == 0)
                    {
                        string campos = "id,surtidor,producto,monto,volumen,ppu,YPFruta";

                        DataTable producto = ConectorSQLite.dt_query(String.Format("SELECT id,precio FROM productos WHERE idSecundario = {0}", despacho.producto_old));

                        int idProducto = 0;
                        int YPFruta = 0;
                        if (producto.Rows.Count != 0)
                        {
                            idProducto = Convert.ToInt32(producto.Rows[0]["id"]);
                            if (Convert.ToDouble(producto.Rows[0]["precio"]) != Convert.ToDouble(despacho.PPU_old))
                                YPFruta = 1;
                        }
                        else
                            idProducto = despacho.producto_old;

                        string valores = String.Format(
                            "'{0}',{1},{2},'{3}','{4}','{5}',{6}",
                            despacho.id_old.Trim().Substring(1),
                            surtidor,
                            idProducto,
                            despacho.monto_old.Trim(),
                            despacho.volumen_old.Trim(),
                            despacho.PPU_old.Trim(),
                            YPFruta);
                        Log.Instance.writeLog("Insertando despacho " + despacho.id_old, Log.LogType.t_debug);
                        ConectorSQLite.query(String.Format("INSERT INTO despachos ({0}) VALUES ({1})", campos, valores));
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error en el método grabarDespachos. Excepcion: " + e.Message);
            }
        }

        public override void facturaDespachos()
        {
            //  Implementar metodo
        }

        public override void compruebaCierre()
        {
           // Implementar metodo
        }
        public override void ObtenerSurtidores()
        {
            /*
            string filePath = Configuracion.leerConfiguracion().rutaProyNuevo + "\\Fusion\\surtidores.txt";
            int surtidorID = 1; // Variable para asignar un ID a cada surtidor

            try
            {
                StringBuilder sb = new StringBuilder();

                foreach (var surtidor in configEstacion.surtidores)
                {
                    sb.AppendLine($"Surtidor ID: {surtidorID}");

                    for (int i = 0; i < surtidor.productoPorManguera.Count; i++)
                    {
                        string productoId = surtidor.productoPorManguera[i].ToString();
                        double precio = ConectorSQLite.ObtenerPrecioProducto(productoId);

                        // Formatear el precio antes de agregarlo al archivo de texto
                        string precioFormateado = FormatearPrecio(precio);

                        // Formatear el precio para guardar en la base de datos
                        string precioFormateadosql = FormatearPrecioParaBD(precio);

                        // Verificar si ya existe un registro con los mismos valores
                        if (!ConectorSQLite.ExisteRegistro(surtidorID, i + 1, productoId))
                        {
                            // Si no existe, insertar los datos en la base de datos "surtidores"
                            ConectorSQLite.InsertarDatosSurtidores(surtidorID, i + 1, productoId, Convert.ToDouble(precioFormateado));
                        }

                        sb.AppendLine($"- Manguera {i + 1}: Producto {productoId} - Precio: {precioFormateado}");
                    }

                    sb.AppendLine(); // Separador entre surtidores
                    surtidorID++; // Incrementar el ID para el próximo surtidor
                }

                // Escribir en un archivo de texto
                File.WriteAllText(filePath, sb.ToString());
            }
            catch (Exception e)
            {
                string fecha = DateTime.Today.Date.ToString().Substring(0, 10);
                string hora = $"{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}";

                if (!File.Exists(Application.StartupPath + "\\log\\erroresObtenerSurtidor.txt"))
                {
                    File.Create(Application.StartupPath + "\\log\\erroresObtenerSurtidor.txt").Close();
                }

                using (StreamWriter scan = new StreamWriter(Application.StartupPath + "\\log\\erroresObtenerSurtidor.txt", true))
                {
                    string datos = $"{DateTime.Today} {DateTime.Now} - {e.Message} Linea: {e.StackTrace}";
                    scan.WriteLine(datos);
                }
            }
            */
        }
        private string FormatearPrecio(double precio)
        {
            string formattedPrice = String.Format("{0:#,0}", precio).Replace('.', ',');
            return formattedPrice;
        }
        private string FormatearPrecioParaBD(double precio)
        {
            string precioFormateado = precio.ToString("F2"); // "F2" muestra el número con 2 decimales
            return precioFormateado;
        }
    }
}
