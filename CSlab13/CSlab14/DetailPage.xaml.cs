using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static CSlab13.HttpRequestData;

namespace CSlab13
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailPage : ContentPage
    {
        public DetailPage()
        {
            InitializeComponent();
        }

        private void SaveDetail(object sender, EventArgs e)
        {
            var detail = (Detail)BindingContext;
            if (!String.IsNullOrEmpty(detail.Name))
            {
                if (detail.Id == 0)
                    AddDetail(detail);
                else

                    ChangeDetail(detail);
            }

            this.Navigation.PopAsync();
        }

        private void DeleteDetail(object sender, EventArgs e)
        {
            var detail = (Detail)BindingContext;
            HttpRequestData.DeleteDetail(detail);

            this.Navigation.PopAsync();
        }
    }
}