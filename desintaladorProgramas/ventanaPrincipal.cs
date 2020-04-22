using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace desintaladorProgramas
{
    public partial class ventanaPrincipal : Form
    {

        private List<registro> listaRegistros { get; set; }
        private registro registroSeleccionado { get; set; }

        private List<string> listaIconosMsi { get; set; }
        public ventanaPrincipal()
        {
            InitializeComponent();
            this.listaRegistros = new List<registro>();
            this.listaIconosMsi = new List<string>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblVersion.Text = "Version "+desintaladorProgramas.Properties.Resources.version;




            //Buscando iconos




            string rutaIconosRegedit = @"Installer\Products";
            RegistryKey registroIconos = Registry.ClassesRoot.CreateSubKey(rutaIconosRegedit);
            string[] subkeys_registroIconos = registroIconos.GetSubKeyNames();
            foreach (string subkey in subkeys_registroIconos) {
                RegistryKey registro = registroIconos.OpenSubKey(subkey);

                string rutaIconoMSI = registro.GetValue("ProductIcon") as string;


                if (!string.IsNullOrWhiteSpace(rutaIconoMSI)) {
                    if (rutaIconoMSI.Contains(".exe") || rutaIconoMSI.ToLower().Contains(".ico")) {

                        string productIcon = registro.GetValue("ProductIcon") as string;
                        productIcon = productIcon.Split(',')[0];

                        listaIconosMsi.Add(productIcon);
                        
                    }
                }
            }


            //------------------

            string rutaProgramas = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

            Microsoft.Win32.RegistryKey registerLocalMachine32bits =
                Microsoft.Win32.RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(rutaProgramas);

            string[] registerKeys = registerLocalMachine32bits.GetSubKeyNames();
            foreach (string key in registerKeys) {

                RegistryKey registroIndividual = registerLocalMachine32bits.OpenSubKey(key);

                if (!string.IsNullOrWhiteSpace(Convert.ToString(registroIndividual.GetValue("DisplayName")))) {

                    if (Convert.ToString(registroIndividual.GetValue("DisplayName")).Contains("Light")) {

                    }

                    bool seEncuentra = listaIconosMsi.Any(o => o.Contains(key));
                    string rutaIcono = string.Empty;
                    if (seEncuentra) {
                        rutaIcono = listaIconosMsi.Where(o => o.Contains(key)).First();
                    }

                    registro obj = new registro {
                        DisplayName = registroIndividual.GetValue("DisplayName") as string,
                        DisplayIcon = (!seEncuentra)?registroIndividual.GetValue("DisplayIcon") as string:rutaIcono,
                        DisplayVersion = registroIndividual.GetValue("DisplayVersion") as string,
                        InstallDate = registroIndividual.GetValue("InstallDate") as string,
                        NoModify = registroIndividual.GetValue("NoModify") as string,
                        NoRepair = registroIndividual.GetValue("NoRepair") as string,
                        Publisher = registroIndividual.GetValue("Publisher") as string,
                        UninstallString = registroIndividual.GetValue("UninstallString") as string,
                        key = key,
                        tipo_registro=32,
                        ModifyPath = registroIndividual.GetValue("ModifyPath") as string,
                        size = registroIndividual.GetValue("Size") as string
                    };


                    listaRegistros.Add(obj);

                }
            }

            Microsoft.Win32.RegistryKey registerLocalMachine64bits =
              Microsoft.Win32.RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(rutaProgramas);

            registerKeys = registerLocalMachine64bits.GetSubKeyNames();
            foreach (string key in registerKeys)
            {
                

                RegistryKey registroIndividual = registerLocalMachine64bits.OpenSubKey(key);



                if (!string.IsNullOrWhiteSpace(Convert.ToString(registroIndividual.GetValue("DisplayName"))))
                {
                    if (Convert.ToString(registroIndividual.GetValue("DisplayName")).Contains("Bonjour"))
                    {

                    }

                    bool seEncuentra = listaIconosMsi.Any(o => o.Contains(key));
                    string rutaIcono = string.Empty;
                    if (seEncuentra)
                    {
                        rutaIcono = listaIconosMsi.Where(o => o.Contains(key)).First();
                    }
                    registro obj = new registro
                    {
                        DisplayName = registroIndividual.GetValue("DisplayName") as string,
                        DisplayIcon = (!seEncuentra) ? registroIndividual.GetValue("DisplayIcon") as string : rutaIcono,
                        DisplayVersion = registroIndividual.GetValue("DisplayVersion") as string,
                        InstallDate = registroIndividual.GetValue("InstallDate") as string,
                        NoModify = registroIndividual.GetValue("NoModify") as string,
                        NoRepair = registroIndividual.GetValue("NoRepair") as string,
                        Publisher = registroIndividual.GetValue("Publisher") as string,
                        UninstallString = registroIndividual.GetValue("UninstallString") as string,
                        key = key,
                        tipo_registro = 64,
                        ModifyPath = registroIndividual.GetValue("ModifyPath") as string,
                        size = registroIndividual.GetValue("Size") as string
                    };


                    listaRegistros.Add(obj);

                }
            }



            foreach (registro o in listaRegistros) {
                //if (!string.IsNullOrWhiteSpace(o.DisplayIcon))
                //{
                //    Icon ic = Icon.ExtractAssociatedIcon(o.DisplayIcon);
                //    lista.Rows.Add(ic, o.DisplayName, o.Publisher, o.InstallDate);
                //}
                //else {
                   
                //}

                try
                {
                  

                    string rutaIcono = o.DisplayIcon.Split(',')[0];
                    rutaIcono = rutaIcono.Replace("\"","");


                    Icon ic = Icon.ExtractAssociatedIcon(rutaIcono);
                    lista.Rows.Add(ic, o.DisplayName, o.Publisher, o.InstallDate,o.size);
                    
                }
                catch {

                 //  Icon ic = Icon.ExtractAssociatedIcon(o.DisplayIcon);
                    lista.Rows.Add(null, o.DisplayName, o.Publisher, o.InstallDate, o.DisplayIcon);
                }

            }


        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_EnabledChanged(object sender, EventArgs e)
        {
            Button b1 = sender as Button;
            b1.BackColor = (!b1.Enabled) ? Color.LightGray : Color.FromArgb(68, 165, 229);
            b1.ForeColor = (!b1.Enabled) ? Color.DimGray : Color.White;
        }

        private void lista_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;


            int row = e.RowIndex;

            string displayname = this.lista.Rows[e.RowIndex].Cells[1].Value as string;

            registroSeleccionado = this.listaRegistros.Where(o => o.DisplayName.Contains(displayname)).First() ;

            bool repara = !string.IsNullOrWhiteSpace(this.registroSeleccionado.ModifyPath);

            this.btnDesinstalar.Enabled = true;
            this.btnReparar.Enabled = repara;
            
        }

        private void btnDesinstalar_Click(object sender, EventArgs e)
        {

            desinstalar("/X ");
        }

        private void btnReparar_Click(object sender, EventArgs e)
        {
            desinstalar("/F ");
        }

        public void desinstalar(string argumento)
        {
            string cadenaDesintalador = this.registroSeleccionado.UninstallString;
            Process procesar = new Process();

            cadenaDesintalador = cadenaDesintalador.Replace("/I", argumento).Replace("/i", argumento);

            cadenaDesintalador = cadenaDesintalador.ToLower().Contains("msiexec") ? cadenaDesintalador : $"\"{cadenaDesintalador}\"";

            ProcessStartInfo infoparametros = new ProcessStartInfo("cmd.exe");
            //infoparametros.FileName = "cmd";
            infoparametros.Arguments = "/c " + cadenaDesintalador;
            procesar.StartInfo = infoparametros;
            //infoparametros.CreateNoWindow = true;
            infoparametros.WindowStyle = ProcessWindowStyle.Hidden;
            procesar.Start();


            string icono = registroSeleccionado.DisplayIcon.Split(',')[0];
            Icon ic = Icon.ExtractAssociatedIcon(icono);
            lista.Rows.Add(ic);
        }

        private void ventanaPrincipal_Shown(object sender, EventArgs e)
        {
            actualizacion.actualizacion actualizacion = new desintaladorProgramas.actualizacion.actualizacion();
            if (actualizacion.verificaActualizacion()) {
                DialogResult dialogo = MessageBox.Show("Hay una actualización disponible ¿Desea actualizar?","Actualización",MessageBoxButtons.YesNo,MessageBoxIcon.Information);
                if (dialogo == DialogResult.Yes) {

                    //Aqui se empieza a actualizar.......
                    barra.Value = 0;
                    barra.Visible = true;


                    actualizacion.descargarArchivo(this.barra);
                    
                }
            }
        }

      
    }
}
