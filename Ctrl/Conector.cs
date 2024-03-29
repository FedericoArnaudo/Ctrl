using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ctrl
{
    abstract class Conector
    {
        #region Estructuras
        public class Despacho
        {
            public enum EstadoDespacho
            {
                DISPONIBLE,
                EN_SOLICITUD,
                DESPACHANDO,
                AUTORIZADO,
                VENTA_FINALIZADA_IMPAGA,
                DEFECTUOSO,
                ANULADO,
                DETENIDO
            }
            public EstadoDespacho status { get; set; }
            public int nroVenta { get; set; }
            public int producto { get; set; }
            public string monto { get; set; }
            public string volumen { get; set; }
            public string PPU { get; set; }
            public bool facturada { get; set; }
            public string id { get; set; }

            public EstadoDespacho status_old { get; set; }
            public int nroVenta_old { get; set; }
            public int producto_old { get; set; }
            public string monto_old { get; set; }
            public string volumen_old { get; set; }
            public string PPU_old { get; set; }
            public bool facturada_old { get; set; }
            public string id_old { get; set; }
        }
        public class Cierre
        {
            public class Total
            {
                public string monto { get; set; }
                public string volumen { get; set; }
            }
            public Cierre()
            {
                totalesMediosDePago = new List<Total>();
                totalesPorProducto = new List<Total>();
                totalesPorMangueraPorSurtidor = new List<Total>();
                totalesPorMangueraSinControl = new List<Total>();
                totalesPorMangueraPruebas = new List<Total>();
                tanques = new List<Tanque>();
                productos = new List<Producto>();
                totalesPorPeriodoPorNivelPorProducto = new List<List<List<Total>>>();
            }

            public List<Total> totalesMediosDePago;
            public string impuesto1;
            public string impuesto2;
            public int nivelesDePrecios;
            public int periodosDePrecios;
            public List<List<List<Total>>> totalesPorPeriodoPorNivelPorProducto;
            public List<Total> totalesPorProducto;
            public List<Total> totalesPorMangueraPorSurtidor;
            public List<Total> totalesPorMangueraSinControl;
            public List<Total> totalesPorMangueraPruebas;
            public List<Tanque> tanques;
            public List<Producto> productos;
        }
        public class Tanque
        {
            public string producto { get; set; }
            public string agua { get; set; }
            public string vacio { get; set; }
        }
        public class Producto
        {
            public string id { get; set; }
            public string precio { get; set; }
            public string volumen { get; set; }
            public string agua { get; set; }
            public string vacio { get; set; }
            public string capacidad { get; set; }
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
        public class ConfiguracionEstacion
        {
            public ConfiguracionEstacion()
            {
                productos = new List<Producto>();
                surtidores = new List<Surtidor>();
                productoPorTanque = new List<int>();
            }
            public int protocolo { get; set; }
            public List<Surtidor> surtidores { get; set; }
            public List<int> productoPorTanque { get; set; }
            public List<Producto> productos { get; set; }
        }
        #endregion
        #region Métodos
        /// <summary>
        /// Este metodo no se usa en el programa, pero esta disponible por
        /// si es necesario para debug del controlador.
        /// </summary>
        /// <returns> true si se pudo conectar correctamente </returns>
        public abstract bool checkConexion();

        /// <summary>
        /// Trae el despacho del surtidor solicitado y crea una estructura
        /// para almacenar los datos de ese despacho
        /// </summary>
        /// <param name="surtidor"> Número de surtidor </param>
        /// <returns> Estructura que contiene los datos de ese despacho </returns>
        public abstract Despacho traerDespacho(int surtidor);

        /// <summary>
        /// Trae los datos del turno actual, no se utiliza en producción,
        /// pero sirve para probar la estructura de datos del cierre de turno.
        /// </summary>
        /// <returns> Una estructura que contiene los datos del turno actual </returns>
        public abstract Cierre traerTurnoEnCurso();

        /// <summary>
        /// Hace un cierre de turno y devuelve los datos del mismo.
        /// </summary>
        /// <returns> Una estructura que contiene los datos del cierre de turno </returns>
        public abstract Cierre cierreDeTurno();

        /// <summary>
        /// Solicita la información de la configuracion de la estacion (cantidad de surtidores,
        /// productos, tanques, mangueras, etc.) y carga los mismos ne una estructura de datos.
        /// </summary>
        /// <returns> Una estructura con la configuración de la estación </returns>
        public abstract ConfiguracionEstacion configuracionDeEstacion();

        /// <summary>
        /// Consulta el volumen de los tanques y crea una lista con la información obtenida.
        /// </summary>
        /// <param name="cantidadTanques"> Cantidad de tanques configurados </param>
        /// <returns> Una lista de tanques con el volumen actual y total de cada uno </returns>
        public abstract List<Tanque> traerTanques(int cantidadTanques);

        /// <summary>
        /// Método de uso interno para ejecutar comandos del tipo byte
        /// </summary>
        /// <param name="comando"> Comando a ejecutar </param>
        /// <returns> Respuesta al comando </returns>
        protected abstract byte[] enviarComando(byte[] comando);
        #endregion
    }
    class ConectorCEM : Conector
    {
        #region Comandos
        struct ComandoCHECK
        {
            public static byte[] mensaje = new byte[] { 0x0 };
            public static byte[] respuesta = new byte[] { 0x0 };
        }

        struct ComandoConfigEstacion
        {
            public static byte[] mensaje = new byte[] { 0x65 };
            public static int confirmacion = 0;                     //<CONFIRMACION>    = 0x00
            public static int surtidores = 1;                       //<Surtidores>      = [0x01 - 0x1F]
            //Islas -> CEM44 no maneja el concepto de islas         //<Lislas>          = 0x00
            public static int tanques = 3;                          //<Tanques>         = [0x01 - 0x0F]
            public static int productos = 4;                        //<Productos>       = [0x01 - 0x06]
        }

        struct ComandoTraerDespacho
        {
            public static byte[] mensaje = new byte[] { 0x70 };
            public static int confirmacion = 0;
            public static int status = 1;
            public static int nro_venta = 2;
            public static int codigo_producto = 3;
        }

        struct ComandoTraerTanques
        {
            public static byte[] mensaje = new byte[] { 0x68 };
            public static int confirmacion = 0;
        }

        struct ComandoTurnoEnCurso
        {
            public static byte[] mensaje = new byte[] { 0x08 };
            public static int estadoTurno = 0;
        }

        struct ComandoCierreDeTurno
        {
            public static byte[] mensaje = new byte[] { 0x01 };     
            public static int estadoTurno = 0;
        }

        #endregion

        public ConectorCEM()
        {
            var config = Configuracion.leerConfiguracion();
            if (config.protocoloSurtidores == 16)
            {
                ComandoConfigEstacion.mensaje = new byte[] { 0x65 };
                ComandoTraerDespacho.mensaje = new byte[] { 0x70 };
                ComandoTraerTanques.mensaje = new byte[] { 0x68 };
            }
            else
            {
                ComandoConfigEstacion.mensaje = new byte[] { 0xB5 };
                ComandoTraerDespacho.mensaje = new byte[] { 0xC0 };
                ComandoTraerTanques.mensaje = new byte[] { 0xB8 };
            }
        }

        private readonly byte separador = 0x7E;                         //<SepCampo>

        private readonly string nombreDelPipe = "CEM44POSPIPE";

        private void descartarCampoVariable(byte[] data, ref int pos)
        {
            while (data[pos] != separador)
                pos++;
            pos++;
        }

        private string leerCampoVariable(byte[] data, ref int pos)
        {
            string ret = "";
            ret += Encoding.ASCII.GetString(new byte[] { data[pos] });
            int i = pos + 1;
            while (data[i] != separador)
            {
                ret += Encoding.ASCII.GetString(new byte[] { data[i] });
                i++;
            }
            i++;
            pos = i;
            return ret;
        }

        protected override byte[] enviarComando(byte[] comando)
        {
            byte[] buffer;
            string ip = Configuracion.leerConfiguracion().ip;

            try
            {
                using (var pipeClient = new NamedPipeClientStream(ip, nombreDelPipe))
                {
                    pipeClient.Connect();

                    pipeClient.Write(comando, 0, comando.Length);

                    buffer = new byte[pipeClient.OutBufferSize];

                    pipeClient.Read(buffer, 0, buffer.Length);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error al enviar el comando. Excepción: " + e.Message);
            }
            return buffer;
        }

        public override bool checkConexion()
        {
            byte[] res = enviarComando(ComandoCHECK.mensaje);

            if (res[0] == ComandoCHECK.respuesta[0])
                return true;

            return false;
        }

        public override Despacho traerDespacho(int surtidor)
        {
            Despacho ret = new Despacho();
            try
            {
                Log.Instance.writeLog("Mensaje: " + BitConverter.ToString(new byte[] { (byte)(ComandoTraerDespacho.mensaje[0] + Convert.ToByte(surtidor)) }), Log.LogType.t_debug);
                // Envio del comando para obtener la informacion del surtidor
                byte[] res = enviarComando(new byte[] { (byte)(ComandoTraerDespacho.mensaje[0] + Convert.ToByte(surtidor)) });

                if (res[ComandoTraerDespacho.confirmacion] != 0x0)
                    throw new Exception("No se recibió mensaje de confirmación al solicitar info del surtidor");

                // Procesamiento de cada campo segun la documentación
                byte status = res[ComandoTraerDespacho.status];
                Log.Instance.writeLog("Status despacho: " + BitConverter.ToString(new byte[] { status }), Log.LogType.t_debug);

                bool despachoDisponible = true;

                switch (status)
                {
                    case 0x01:
                        ret.status = Despacho.EstadoDespacho.DISPONIBLE;
                        break;
                    case 0x02:
                        ret.status = Despacho.EstadoDespacho.EN_SOLICITUD;
                        break;
                    case 0x03:
                        ret.status = Despacho.EstadoDespacho.DESPACHANDO;
                        despachoDisponible = false;
                        break;
                    case 0x04:
                        ret.status = Despacho.EstadoDespacho.AUTORIZADO;
                        break;
                    case 0x05:
                        ret.status = Despacho.EstadoDespacho.VENTA_FINALIZADA_IMPAGA;
                        break;
                    case 0x08:
                        ret.status = Despacho.EstadoDespacho.DEFECTUOSO;
                        break;
                    case 0x09:
                        ret.status = Despacho.EstadoDespacho.ANULADO;
                        break;
                    case 0x0A:
                        ret.status = Despacho.EstadoDespacho.DETENIDO;
                        despachoDisponible = false;
                        break;
                }

                int iterador;

                if (despachoDisponible)
                {
                    ret.nroVenta = res[ComandoTraerDespacho.nro_venta];
                    Log.Instance.writeLog("Numero venta: " + ret.nroVenta, Log.LogType.t_debug);

                    ret.producto = res[ComandoTraerDespacho.codigo_producto];
                    Log.Instance.writeLog("Producto: " + ret.producto, Log.LogType.t_debug);

                    iterador = ComandoTraerDespacho.codigo_producto + 1;
                    ret.monto = leerCampoVariable(res, ref iterador);
                    Log.Instance.writeLog("Monto: " + ret.monto, Log.LogType.t_debug);

                    ret.volumen = leerCampoVariable(res, ref iterador);
                    Log.Instance.writeLog("Volumen: " + ret.volumen, Log.LogType.t_debug);

                    ret.PPU = leerCampoVariable(res, ref iterador);
                    Log.Instance.writeLog("PPU: " + ret.PPU, Log.LogType.t_debug);

                    ret.facturada = Convert.ToBoolean(res[iterador]);
                    Log.Instance.writeLog("Facturada: " + ret.facturada, Log.LogType.t_debug);

                    iterador++;
                    ret.id = leerCampoVariable(res, ref iterador);
                    Log.Instance.writeLog("Id: " + ret.id, Log.LogType.t_debug);
                }
                else
                {
                    ret.nroVenta = 0;
                    ret.producto = 0;
                    ret.monto = "";
                    ret.volumen = "";
                    ret.PPU = "";
                    ret.facturada = false;
                    ret.id = "";
                    ret.id_old = "";
                    iterador = 2;
                }
                byte status_old = res[iterador];
                iterador++;
                Log.Instance.writeLog("Status despacho anterior: " + BitConverter.ToString(new byte[] { status_old }), Log.LogType.t_debug);

                switch (status)
                {
                    case 0x01:
                        ret.status_old = Despacho.EstadoDespacho.DISPONIBLE;
                        break;
                    case 0x05:
                        ret.status_old = Despacho.EstadoDespacho.VENTA_FINALIZADA_IMPAGA;
                        break;
                }

                ret.nroVenta_old = res[iterador];
                iterador++;
                Log.Instance.writeLog("Numero venta anterior: " + ret.nroVenta_old, Log.LogType.t_debug);

                ret.producto_old = res[iterador];
                iterador++;
                Log.Instance.writeLog("Producto anterior: " + ret.producto_old, Log.LogType.t_debug);

                ret.monto_old = leerCampoVariable(res, ref iterador);
                Log.Instance.writeLog("Monto anterior: " + ret.monto_old, Log.LogType.t_debug);

                ret.volumen_old = leerCampoVariable(res, ref iterador);
                Log.Instance.writeLog("Volumen anterior: " + ret.volumen_old, Log.LogType.t_debug);

                ret.PPU_old = leerCampoVariable(res, ref iterador);
                Log.Instance.writeLog("PPU anterior: " + ret.PPU_old, Log.LogType.t_debug);

                ret.facturada_old = Convert.ToBoolean(res[iterador]);
                Log.Instance.writeLog("Facturada anterior: " + ret.facturada_old, Log.LogType.t_debug);

                iterador++;
                ret.id_old = leerCampoVariable(res, ref iterador);
                Log.Instance.writeLog("Id: " + ret.id_old, Log.LogType.t_debug);
            }
            catch (Exception e)
            {
                throw new Exception("Error al traer información del surtidor " + surtidor + ". Excepcion: " + e.Message);
            }
            return ret;
        }

        public override Cierre traerTurnoEnCurso()
        {
            Cierre ret = new Cierre();
            try
            {
                Log.Instance.writeLog("Mensaje: " + BitConverter.ToString(new byte[] { ComandoTurnoEnCurso.mensaje[0] }), Log.LogType.t_debug);
                // Envio del comando para obtener la info del turno actual
                byte[] res = enviarComando(new byte[] { ComandoTurnoEnCurso.mensaje[0] });

                if (res[ComandoTurnoEnCurso.estadoTurno] == 0xFF)
                {
                    Log.Instance.writeLog("Turno sin ventas", Log.LogType.t_info);
                    return ret;
                }

                int iterador = 0;

                for (int i = 0; i < 8; i++)
                {
                    var tmp = new Cierre.Total();

                    tmp.monto = leerCampoVariable(res, ref iterador);
                    tmp.volumen = leerCampoVariable(res, ref iterador);

                    Log.Instance.writeLog("Medio de pago: " + i + ". Monto: " + tmp.monto + ". Volumen: " + tmp.volumen, Log.LogType.t_debug);

                    ret.totalesMediosDePago.Add(tmp);
                }

                ret.impuesto1 = leerCampoVariable(res, ref iterador);
                ret.impuesto2 = leerCampoVariable(res, ref iterador);
                Log.Instance.writeLog("Impuesto 1: " + ret.impuesto1, Log.LogType.t_debug);
                Log.Instance.writeLog("Impuesto 2: " + ret.impuesto2, Log.LogType.t_debug);

                ret.periodosDePrecios = res[iterador];
                iterador++;

                var configEstacion = configuracionDeEstacion();
                Log.Instance.writeLog("Periodos de precios: " + ret.periodosDePrecios, Log.LogType.t_debug);

                for (int i = 0; i < ret.periodosDePrecios; i++)
                {
                    ret.nivelesDePrecios = res[iterador];
                    iterador++;
                    Log.Instance.writeLog("Niveles de precios: " + ret.nivelesDePrecios, Log.LogType.t_debug);

                    List<List<Cierre.Total>> tmp = new List<List<Cierre.Total>>();
                    for (int j = 0; j < ret.nivelesDePrecios; j++)
                    {
                        List<Cierre.Total> tmp1 = new List<Cierre.Total>();
                        for (int k = 0; k < configEstacion.productos.Count; k++)
                        {
                            var tmp2 = new Cierre.Total();

                            tmp2.monto = leerCampoVariable(res, ref iterador);
                            tmp2.volumen = leerCampoVariable(res, ref iterador);

                            // En la documentación no aparece este campo, pero si existe.
                            // Es el precio unitario de la nafta. No lo necesitamos en esta parte por eso se descarta
                            descartarCampoVariable(res, ref iterador);

                            Log.Instance.writeLog(
                                "Totales producto: " + k + ". " +
                                "Nivel de precios: " + j + ". " +
                                "Periodo de precios: " + i + ". " +
                                "Monto: " + tmp2.monto + ". " +
                                "Volumen: " + tmp2.volumen + ". ",
                                Log.LogType.t_debug);

                            tmp1.Add(tmp2);
                        }
                        tmp.Add(tmp1);
                    }
                    ret.totalesPorPeriodoPorNivelPorProducto.Add(tmp);
                }
                for (int i = 0; i < configEstacion.surtidores.Count; i++)
                {
                    for (int j = 0; j < configEstacion.surtidores[i].productoPorManguera.Count; j++)
                    {
                        var tmp = new Cierre.Total();

                        tmp.monto = leerCampoVariable(res, ref iterador);
                        tmp.volumen = leerCampoVariable(res, ref iterador);

                        Log.Instance.writeLog("Total surtidor: " + i + " manguera: " + j + ". Monto: " + tmp.monto + ". Volumen: " + tmp.volumen, Log.LogType.t_debug);

                        ret.totalesPorMangueraPorSurtidor.Add(tmp);

                        tmp = new Cierre.Total();

                        tmp.monto = leerCampoVariable(res, ref iterador);
                        tmp.volumen = leerCampoVariable(res, ref iterador);

                        Log.Instance.writeLog("Total sin control surtidor: " + i + " manguera: " + j + ". Monto: " + tmp.monto + ". Volumen: " + tmp.volumen, Log.LogType.t_debug);

                        ret.totalesPorMangueraSinControl.Add(tmp);

                        tmp = new Cierre.Total();

                        tmp.monto = leerCampoVariable(res, ref iterador);
                        tmp.volumen = leerCampoVariable(res, ref iterador);

                        Log.Instance.writeLog("Total pruebas surtidor: " + i + " manguera: " + j + ". Monto: " + tmp.monto + ". Volumen: " + tmp.volumen, Log.LogType.t_debug);

                        ret.totalesPorMangueraPruebas.Add(tmp);
                    }
                }

                for (int i = 0; i < configEstacion.productoPorTanque.Count; i++)
                {
                    var tmp = new Tanque();

                    tmp.producto = leerCampoVariable(res, ref iterador);
                    tmp.agua = leerCampoVariable(res, ref iterador);
                    tmp.vacio = leerCampoVariable(res, ref iterador);
                    descartarCampoVariable(res, ref iterador);

                    Log.Instance.writeLog("Tanque: " + i + ". Producto: " + tmp.producto + ". Agua: " + tmp.agua + ". Vacio: " + tmp.vacio, Log.LogType.t_debug);

                    ret.tanques.Add(tmp);
                }

                for (int i = 0; i < configEstacion.productos.Count; i++)
                {
                    var tmp = new Producto();

                    tmp.volumen = leerCampoVariable(res, ref iterador);
                    tmp.agua = leerCampoVariable(res, ref iterador);
                    tmp.vacio = leerCampoVariable(res, ref iterador);
                    tmp.capacidad = leerCampoVariable(res, ref iterador);

                    Log.Instance.writeLog("Producto: " + i + ". Volumen: " + tmp.volumen + ". Agua: " + tmp.agua + ". Vacio: " + tmp.vacio + ". Capacidad: " + tmp.capacidad, Log.LogType.t_debug);

                    ret.productos.Add(tmp);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error al procesar comando para obtener el turno actual. Excepcion: " + e.Message);
            }
            return ret;
        }

        public override Cierre cierreDeTurno()
        {
            Cierre ret = new Cierre();
            try
            {
                Log.Instance.writeLog("Mensaje: " + BitConverter.ToString(new byte[] { ComandoCierreDeTurno.mensaje[0] }), Log.LogType.t_debug);
                // Envio del comando para obtener la info del turno actual
                byte[] res = enviarComando(new byte[] { ComandoCierreDeTurno.mensaje[0] });

                if (res[ComandoCierreDeTurno.estadoTurno] == 0xFF)
                {
                    Log.Instance.writeLog("Turno sin ventas", Log.LogType.t_info);
                    return ret;
                }

                int iterador = 0;

                for (int i = 0; i < 8; i++)
                {
                    var tmp = new Cierre.Total();

                    tmp.monto = leerCampoVariable(res, ref iterador);
                    tmp.volumen = leerCampoVariable(res, ref iterador);

                    Log.Instance.writeLog("Medio de pago: " + i + ". Monto: " + tmp.monto + ". Volumen: " + tmp.volumen, Log.LogType.t_debug);

                    ret.totalesMediosDePago.Add(tmp);
                }

                ret.impuesto1 = leerCampoVariable(res, ref iterador);
                ret.impuesto2 = leerCampoVariable(res, ref iterador);

                var configEstacion = configuracionDeEstacion();

                for (int i = 0; i < configEstacion.productos.Count; i++)
                {
                    var tmp = new Cierre.Total();

                    tmp.monto = leerCampoVariable(res, ref iterador);
                    tmp.volumen = leerCampoVariable(res, ref iterador);

                    Log.Instance.writeLog("Totales producto: " + i + ". Monto: " + tmp.monto + ". Volumen: " + tmp.volumen, Log.LogType.t_debug);

                    ret.totalesPorProducto.Add(tmp);
                }

                for (int i = 0; i < configEstacion.surtidores.Count; i++)
                {
                    for (int j = 0; j < configEstacion.surtidores[i].productoPorManguera.Count; j++)
                    {
                        var tmp = new Cierre.Total();

                        tmp.monto = leerCampoVariable(res, ref iterador);
                        tmp.volumen = leerCampoVariable(res, ref iterador);

                        Log.Instance.writeLog("Total surtidor: " + i + " manguera: " + j + ". Monto: " + tmp.monto + ". Volumen: " + tmp.volumen, Log.LogType.t_debug);

                        ret.totalesPorMangueraPorSurtidor.Add(tmp);

                        tmp = new Cierre.Total();

                        tmp.monto = leerCampoVariable(res, ref iterador);
                        tmp.volumen = leerCampoVariable(res, ref iterador);

                        Log.Instance.writeLog("Total sin control surtidor: " + i + " manguera: " + j + ". Monto: " + tmp.monto + ". Volumen: " + tmp.volumen, Log.LogType.t_debug);

                        ret.totalesPorMangueraSinControl.Add(tmp);

                        tmp = new Cierre.Total();

                        tmp.monto = leerCampoVariable(res, ref iterador);
                        tmp.volumen = leerCampoVariable(res, ref iterador);

                        Log.Instance.writeLog("Total pruebas surtidor: " + i + " manguera: " + j + ". Monto: " + tmp.monto + ". Volumen: " + tmp.volumen, Log.LogType.t_debug);

                        ret.totalesPorMangueraPruebas.Add(tmp);
                    }
                }

                for (int i = 0; i < configEstacion.productoPorTanque.Count; i++)
                {
                    var tmp = new Tanque();

                    tmp.producto = leerCampoVariable(res, ref iterador);
                    tmp.agua = leerCampoVariable(res, ref iterador);
                    tmp.vacio = leerCampoVariable(res, ref iterador);
                    descartarCampoVariable(res, ref iterador);

                    Log.Instance.writeLog("Tanque: " + i + ". Producto: " + tmp.producto + ". Agua: " + tmp.agua + ". Vacio: " + tmp.vacio, Log.LogType.t_debug);

                    ret.tanques.Add(tmp);
                }

                for (int i = 0; i < configEstacion.productos.Count; i++)
                {
                    var tmp = new Producto();

                    tmp.volumen = leerCampoVariable(res, ref iterador);
                    tmp.agua = leerCampoVariable(res, ref iterador);
                    tmp.vacio = leerCampoVariable(res, ref iterador);
                    tmp.capacidad = leerCampoVariable(res, ref iterador);

                    Log.Instance.writeLog("Producto: " + i + ". Volumen: " + tmp.volumen + ". Agua: " + tmp.agua + ". Vacio: " + tmp.vacio + ". Capacidad: " + tmp.capacidad, Log.LogType.t_debug);

                    ret.productos.Add(tmp);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error al procesar comando cierre de turno. Excepcion: " + e.Message);
            }
            return ret;
        }

        public override ConfiguracionEstacion configuracionDeEstacion()
        {
            ConfiguracionEstacion ret = new ConfiguracionEstacion();
            try
            {
                // Envio del comando para obtener la configuración de la estación
                byte[] res = enviarComando(ComandoConfigEstacion.mensaje);

                if (res[ComandoConfigEstacion.confirmacion] != 0x0)
                    throw new Exception("No se recibió mensaje de confirmación al solicitar info de la estación");

                // Procesamiento de cada campo segun la documentación
                int cantSurtidores = res[ComandoConfigEstacion.surtidores];

                Log.Instance.writeLog("Cantidad de surtidores: " + cantSurtidores, Log.LogType.t_debug);

                int cantTanques = res[ComandoConfigEstacion.tanques];

                Log.Instance.writeLog("Cantidad de tanques: " + cantTanques, Log.LogType.t_debug);

                int cantProductos = res[ComandoConfigEstacion.productos];

                Log.Instance.writeLog("Cantidad de productos: " + cantProductos, Log.LogType.t_debug);

                int iterador = ComandoConfigEstacion.productos + 1;

                for (int i = 0; i < cantProductos; i++)
                {
                    Producto tmp = new Producto();
                    tmp.id = leerCampoVariable(res, ref iterador);
                    tmp.precio = leerCampoVariable(res, ref iterador);
                    Log.Instance.writeLog("Precio producto " + tmp.id + ": " + tmp.precio, Log.LogType.t_debug);
                    descartarCampoVariable(res, ref iterador);
                    ret.productos.Add(tmp);

                    /*DataTable tabla = ConectorSQLite.dt_query(String.Format("SELECT id FROM productos WHERE id = {0}", tmp.id.Trim()));
                    if (tabla.Rows.Count == 0)
                        ConectorSQLite.query(String.Format("INSERT INTO productos (id,precio) VALUES ({0},'{1}')", tmp.id.Trim(), tmp.precio.Trim()));
                    else
                        ConectorSQLite.query(String.Format("UPDATE productos SET precio = '{0}' WHERE id = {1}", tmp.precio.Trim(), tmp.id.Trim()));
                    */
                }

                for (int i = 0; i < cantSurtidores; i++)
                {
                    Surtidor tmp = new Surtidor();

                    tmp.nivelDePrecios = res[iterador];
                    iterador++;
                    int cantMangueras = res[iterador] + 1;
                    iterador++;
                    for (int j = 0; j < cantMangueras; j++)
                    {
                        Log.Instance.writeLog("Surtidor " + i + " - Manguera " + j + " - Producto: " + res[iterador], Log.LogType.t_debug);
                        tmp.productoPorManguera.Add(res[iterador]);
                        iterador++;
                    }
                    ret.surtidores.Add(tmp);
                }

                for (int i = 0; i < cantTanques; i++)
                {
                    Log.Instance.writeLog("Tanque " + i + " - Producto: " + res[iterador], Log.LogType.t_debug);
                    ret.productoPorTanque.Add(res[iterador]);
                    iterador++;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error procesando la configuracion de estación. Excepcion: " + e.Message);
            }

            return ret;
        }

        public override List<Tanque> traerTanques(int cantidadTanques)
        {
            List<Tanque> ret = new List<Tanque>();
            try
            {
                Log.Instance.writeLog("Mensaje: " + BitConverter.ToString(new byte[] { ComandoTraerTanques.mensaje[0] }), Log.LogType.t_debug);
                // Envio del comando para obtener la informacion de los tanques
                byte[] res = enviarComando(new byte[] { ComandoTraerTanques.mensaje[0] });

                if (res[ComandoTraerTanques.confirmacion] != 0x0)
                    throw new Exception("No se recibió mensaje de confirmación al solicitar info de los tanques");

                int iterador = ComandoTraerTanques.confirmacion + 1;

                // Procesamiento de cada campo segun la documentación
                for (int i = 0; i < cantidadTanques; i++)
                {
                    Tanque tanque = new Tanque();

                    tanque.producto = leerCampoVariable(res, ref iterador);
                    Log.Instance.writeLog("Producto: " + tanque.producto, Log.LogType.t_debug);

                    tanque.agua = leerCampoVariable(res, ref iterador);
                    Log.Instance.writeLog("Agua: " + tanque.agua, Log.LogType.t_debug);

                    tanque.vacio = leerCampoVariable(res, ref iterador);
                    Log.Instance.writeLog("Vacio: " + tanque.vacio, Log.LogType.t_debug);

                    ret.Add(tanque);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error al traer información de tanques. Excepcion: " + e.Message);
            }
            return ret;
        }
    }
}
