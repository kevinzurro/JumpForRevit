using System;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;

namespace Jump
{
    public class ModelBase
    {
        private UIApplication uiApp = null;
        private UIDocument uiDoc = null;
        private Application app = null;
        private Document doc = null;
        private string idiomaDelPrograma;

        public UIApplication UIApp
        {
            get { return uiApp; }
            set { uiApp = value; }
        }

        public UIDocument UIDoc
        {
            get { return uiDoc; }
            set { uiDoc = value; }
        }

        public Application Appli 
        { 
            get { return app; }
            set { app = value; }
        }

        public Document Doc 
        { 
            get { return doc; }
            set { doc = value; }
        }

        public string IdiomaDelPrograma
        {
            get { return idiomaDelPrograma; }
            set { idiomaDelPrograma = value; }
        }
    }
}
