using Dominio;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace Manager {
    public class VentaManager {
        private List<Venta> ventas = new List<Venta>();
        private string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Ventas.bin"); //me independizo para que cualquier usuario pueda acceder al archivo
        public void AgregarVenta(Venta venta) {
            FileStream fs = new FileStream(filePath, FileMode.Append);
            try {
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(venta.Codigo);
                bw.Write(venta.Precio);
                bw.Write(venta.FechaVenta.ToLongDateString());
                bw.Close();
            } catch(Exception) {
                throw;
            } finally {
                fs.Close();
            }
        }

        public decimal MontoRecaudadoDia(DateTime fecha) {
            decimal montoRecaudadoDia = 0;
            try {
                if(File.Exists("ventas.bin")) {
                    FileStream fs = new FileStream(filePath, FileMode.Open);
                    BinaryReader br = new BinaryReader(fs);
                    while(br.PeekChar()!=-1) {
                        Venta venta = new Venta();
                        venta.Codigo=br.ReadString();
                        venta.Precio=br.ReadDecimal();
                        venta.FechaVenta=Convert.ToDateTime(br.ReadString());
                        if(fecha.Date==venta.FechaVenta.Date) {
                            montoRecaudadoDia+=venta.Precio;
                        }
                    }
                    br.Close();
                    fs.Close();
                }
                return montoRecaudadoDia;
            } catch(Exception) {
                return montoRecaudadoDia;
            }
        }

        public List<Venta> MostrarVentas() {
            try {
                if(File.Exists(filePath)) {
                    FileStream fs = new FileStream(filePath, FileMode.Open);
                    BinaryReader br = new BinaryReader(fs);
                    while(br.PeekChar()!=-1) {
                        Venta venta = new Venta();
                        venta.Codigo=br.ReadString();
                        venta.Precio=br.ReadDecimal();
                        venta.FechaVenta=Convert.ToDateTime(br.ReadString());
                        ventas.Add(venta);
                    }
                    br.Close();
                    fs.Close();
                }
                return ventas;
            } catch(Exception) {
                throw;
            }
        }
    }
}
