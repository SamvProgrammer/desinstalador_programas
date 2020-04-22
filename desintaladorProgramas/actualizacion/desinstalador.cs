using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace desintaladorProgramas.actualizacion
{
    class desinstalador
    {
        private bool handler;

        public void iniciar() {
            string obteniendoGuid = this.obtenerGuid();


            //Desinstalando el programa

            string desinstalar = $" /x \"{obteniendoGuid}\" /qb";
            ejecuta(desinstalar);


            //instalando el programa


            string rutaInstalador = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\instalador.msi";
            string instalar = $"/i {rutaInstalador} /qb";
            ejecuta(instalar);

            
        }

        private string obtenerGuid()
        {
            string uninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            RegistryKey registroUninstall = Registry.LocalMachine.OpenSubKey(uninstallKey);
            string[] subkeys = registroUninstall.GetSubKeyNames();
            string guid = string.Empty;
            foreach (string key in subkeys) {
               RegistryKey registro = registroUninstall.OpenSubKey(key);
                string nombrePrograma = registro.GetValue("DisplayName") as string;
                if (!string.IsNullOrWhiteSpace(nombrePrograma)) {
                    if (nombrePrograma.Contains("desinstaladorPro")) {
                        guid =registro.GetValue("UninstallString") as string;
                        guid = guid.Substring(guid.IndexOf('{'));
                        break;
                    }
                }
            }


            return guid;
        }


        public void ejecuta(string argumento) {
            handler = true;
            Process myProcess = new Process();
            myProcess.StartInfo.FileName = "msiexec.exe";
            myProcess.StartInfo.Arguments = argumento;
            myProcess.StartInfo.CreateNoWindow = true;
            myProcess.EnableRaisingEvents = true;
            myProcess.Exited += new EventHandler(myProcess_Exited);
            myProcess.Start();
            while (handler)
            {

            }
        }

        private void myProcess_Exited(object sender, EventArgs e)
        {
            handler = false;
        }
    }

}