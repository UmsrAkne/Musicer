using System.Diagnostics;
using Prism.Mvvm;

namespace Musicer.Models
{
    public class TextWrapper : BindableBase
    {
        private string title;
        private string version = string.Empty;

        public TextWrapper()
        {
            Title = "Musicer";

            SetVersion();
            AddDebugMark();
        }

        public string Title
        {
            get => string.IsNullOrWhiteSpace(Version)
                ? title
                : title + " version : " + Version;
            private set => SetProperty(ref title, value);
        }

        public override string ToString()
        {
            return Title;
        }

        private string Version { get => version; set => SetProperty(ref version, value); }

        [Conditional("RELEASE")]
        private void SetVersion()
        {
            Version = "20240827" + "a";
        }

        [Conditional("DEBUG")]
        private void AddDebugMark()
        {
            Title += " (Debug)";
        }
    }
}