using APPMOR2.MainViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APPMOR2.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatePage : ContentPage
    {
        public StatePage()
        {
            InitializeComponent();
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.State.MostrarUnidad();
        }
    }
}