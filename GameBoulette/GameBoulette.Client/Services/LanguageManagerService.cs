using GameBoulette.Client.Model.Languages;

using System.ComponentModel;

namespace GameBoulette.Client.Services
{
    public class LanguageManagerService : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string CurrentLanguageText = Constants.Languages.French.ShortName;
        public Language CurrentLanguage = Constants.Languages.French;

        public void CycleToNextLanguage()
        {
            if (CurrentLanguage == Constants.Languages.English)
            {
                CurrentLanguageText = Constants.Languages.French.ShortName;
                CurrentLanguage = Constants.Languages.French;
            }
            else
            {
                CurrentLanguageText = Constants.Languages.English.ShortName;
                CurrentLanguage = Constants.Languages.English;
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Language"));
        }
    }
}
