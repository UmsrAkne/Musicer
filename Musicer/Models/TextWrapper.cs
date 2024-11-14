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

        private string Title
        {
            get => string.IsNullOrWhiteSpace(Version)
                ? title
                : title + " version : " + Version;
            set => SetProperty(ref title, value);
        }

        private string Version { get => version; set => SetProperty(ref version, value); }

        public override string ToString()
        {
            return Title;
        }

        [Conditional("RELEASE")]
        private void SetVersion()
        {
            Version = "20240912" + "a" + ", " + "Database ver" + "20240831";
        }

        [Conditional("DEBUG")]
        private void AddDebugMark()
        {
            Title += " (Debug)";
        }
    }
}