using Jump;
using Jump.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Jump.ViewModels
{
    public class ConfigEtiquetasViewModel : ViewModelBase
    {
        private ConfiguracionesModel mConfiguracion;

        public ConfigEtiquetasViewModel(ConfiguracionesModel model)
        {
            Modelo = model;
        }

        public ConfiguracionesModel Modelo
        {
            get { return mConfiguracion; }
            set
            {
                mConfiguracion = value as ConfiguracionesModel;
            }
        }
    }
}
