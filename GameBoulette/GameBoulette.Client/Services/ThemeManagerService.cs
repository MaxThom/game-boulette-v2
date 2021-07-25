using Microsoft.AspNetCore.Components;

using MudBlazor;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace GameBoulette.Client.Services
{
    public class ThemeManagerService : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public MudTheme CurrentTheme = Constants.Themes.DarkTheme;
        public string CurrentThemeIcon = Icons.Material.Filled.WbSunny;

        public void CycleToNextTheme()
        {
            if (CurrentTheme == Constants.Themes.LightTheme)
            {
                CurrentTheme = Constants.Themes.DarkTheme;
                CurrentThemeIcon = Icons.Material.Filled.WbSunny;
            }
            else
            {
                CurrentTheme = Constants.Themes.LightTheme;
                CurrentThemeIcon = Icons.Material.Filled.NightlightRound;
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Theme"));
        }

        //[Inject] private ThemeManagerService ThemeManager { get; set; }
        //
        //private async void ThemeManager_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        //{
        //    await InvokeAsync(() => StateHasChanged());
        //}
        //
        //protected override void OnInitialized()
        //{
        //    ThemeManager.PropertyChanged += ThemeManager_PropertyChanged;
        //}
    }
}
