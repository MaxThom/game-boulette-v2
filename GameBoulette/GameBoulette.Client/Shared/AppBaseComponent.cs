using GameBoulette.Client.Services;

using Microsoft.AspNetCore.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameBoulette.Client.Shared
{
    public class AppBaseComponent : ComponentBase, IDisposable
    {
        [Inject]
        public LanguageManagerService LanguageManager { get; set; }

        private async void LanguageManager_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            await InvokeAsync(() => StateHasChanged());
        }

        protected override void OnInitialized()
        {
            LanguageManager.PropertyChanged += LanguageManager_PropertyChanged;
        }

        public void Dispose()
        {
            LanguageManager.PropertyChanged -= LanguageManager_PropertyChanged;
        }
    }
}
