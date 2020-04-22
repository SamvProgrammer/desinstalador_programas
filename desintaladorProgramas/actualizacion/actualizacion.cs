using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace desintaladorProgramas.actualizacion
{
    class actualizacion
    {
        private ProgressBar barra { get; set; }
        public actualizacion() {
           ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
          
        }

        public bool verificaActualizacion() {

            WebClient cliente = new WebClient();
            cliente.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);

            string versionApp = desintaladorProgramas.Properties.Resources.version;

            string versionOnline = cliente.DownloadString("https://samvprogrammer.github.io/actualizacion_msi/version.txt");

            
            return versionApp != versionOnline;

        }



        public void descargarArchivo(ProgressBar barra) {
            

            this.barra = barra;

            string rutaParaArchivo = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\instalador.msi";
            string rutaOnline = "https://samvprogrammer.github.io/actualizacion_msi/desinstaladorPro.msi";

            WebClient cliente = new WebClient();
            cliente.DownloadFileAsync(new Uri(rutaOnline), rutaParaArchivo);
            cliente.DownloadFileCompleted += new
                AsyncCompletedEventHandler(finalizado);

            cliente.DownloadProgressChanged += new DownloadProgressChangedEventHandler(enProgreso);

        }

        private void enProgreso(object sender, DownloadProgressChangedEventArgs e)
        {
            barra.Value = e.ProgressPercentage;
        }

        private void finalizado(object sender, AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Descarga finalizada, se empezara actualizar el programa","Finalizado",MessageBoxButtons.OK,MessageBoxIcon.Information);
            instalarPrograma();


            File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)+ @"\instalador.msi");
            Application.Exit();
        }

        private void instalarPrograma()
        {
            desinstalador obj = new desinstalador();
            obj.iniciar();
        }
    }
}
