using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace desintaladorProgramas
{
    class registro
    {
        internal string key;
        internal int tipo_registro;

        public string DisplayName { get; set; }
        public string DisplayVersion { get; set; }
        public string InstallDate { get; set; }
        public string UninstallString { get; set; }
        public string NoModify { get; set; }

        public string NoRepair { get; set; }
        public string Publisher { get; set; }

        public string DisplayIcon { get; set; }
        public string ModifyPath { get; internal set; }
    }
}
