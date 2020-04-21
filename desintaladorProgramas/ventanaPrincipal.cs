using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        public ventanaPrincipal()
        {
            InitializeComponent();
            this.listaRegistros = new List<registro>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {



            //------------------

            string rutaProgramas = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

            Microsoft.Win32.RegistryKey registerLocalMachine32bits =
                Microsoft.Win32.RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(rutaProgramas);

            string[] registerKeys = registerLocalMachine32bits.GetSubKeyNames();
            foreach (string key in registerKeys) {

                RegistryKey registroIndividual = registerLocalMachine32bits.OpenSubKey(key);

                if (!string.IsNullOrWhiteSpace(Convert.ToString(registroIndividual.GetValue("DisplayName")))) {

                    registro obj = new registro {
                        DisplayName = registroIndividual.GetValue("DisplayName") as string,
                        DisplayIcon = registroIndividual.GetValue("DisplayIcon") as string,
                        DisplayVersion = registroIndividual.GetValue("DisplayVersion") as string,
                        InstallDate = registroIndividual.GetValue("InstallDate") as string,
                        NoModify = registroIndividual.GetValue("NoModify") as string,
                        NoRepair = registroIndividual.GetValue("NoRepair") as string,
                        Publisher = registroIndividual.GetValue("Publisher") as string,
                        UninstallString = registroIndividual.GetValue("UninstallString") as string,
                        key = key,
                        tipo_registro=32,
                        ModifyPath = registroIndividual.GetValue("ModifyPath") as string
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
                    if (Convert.ToString(registroIndividual.GetValue("DisplayName")).Contains("Adobe"))
                    {

                    }
                    registro obj = new registro
                    {
                        DisplayName = registroIndividual.GetValue("DisplayName") as string,
                        DisplayIcon = registroIndividual.GetValue("DisplayIcon") as string,
                        DisplayVersion = registroIndividual.GetValue("DisplayVersion") as string,
                        InstallDate = registroIndividual.GetValue("InstallDate") as string,
                        NoModify = registroIndividual.GetValue("NoModify") as string,
                        NoRepair = registroIndividual.GetValue("NoRepair") as string,
                        Publisher = registroIndividual.GetValue("Publisher") as string,
                        UninstallString = registroIndividual.GetValue("UninstallString") as string,
                        key = key,
                        tipo_registro = 64,
                        ModifyPath = registroIndividual.GetValue("ModifyPath") as string
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

                    
                  
                   // Icon ic = Icon.ExtractAssociatedIcon(o.DisplayIcon.Split(',')[0]);
                    lista.Rows.Add(null, o.DisplayName, o.Publisher, o.InstallDate,o.DisplayIcon);
                    
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
    }
}
