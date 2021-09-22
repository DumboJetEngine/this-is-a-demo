using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace KELPortableFileServer.Client
{
    public class SettingsService
    {
        public string ApplicationName => "KEL Portable File Server v.1.0";

        public readonly CultureInfo[] Cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
            .ToArray();

        public readonly TimeZoneInfo[] Timezones = TimeZoneInfo.GetSystemTimeZones()
            .ToArray();

        public readonly IReadOnlyList<KeyValuePair<string, string>> CacheTypes = new List<KeyValuePair<string, string>> {
            new KeyValuePair<string, string>("public", "Public"),
            new KeyValuePair<string, string>("private", "Private"),
            new KeyValuePair<string, string>("nocache", "No Cache"),
            new KeyValuePair<string, string>("nocache; nostore", "No Cache, No store"),
        };

        public readonly IReadOnlyList<KeyValuePair<string, Theme>> Themes = new List<KeyValuePair<string, Theme>> {
            new KeyValuePair<string, Theme>(
                "Default",
                new Theme{
                    DisplayName="Default",
                    CssClass = "theme-default",
                    BackgroundColor = "#fff",
                    ForegroundColor = "#444",
                    LinkColor="#0366d6",
                    EntryHoverColor="#ddf",
                    TableHeaderColor="#f0f8ff",
                    DocumentIconColor="#808080",
                    FolderIconColor="#c80",
                    LabelColor="#008000",
                    HighlightColor="#ccf",
                }),
            new KeyValuePair<string, Theme>(
                "Black",
                new Theme{
                    DisplayName="Dark",
                    CssClass = "theme-dark",
                    BackgroundColor = "#333",
                    ForegroundColor = "#fff",
                    LinkColor="#dd2",
                    EntryHoverColor="#555",
                    TableHeaderColor="#222",
                    DocumentIconColor="#fff",
                    FolderIconColor="#c80",
                    LabelColor="#aaa",
                    HighlightColor="#33f",
                }),
        };


        public class Theme
        {
            public string DisplayName { get; set; }
            public string CssClass { get; set; }
            public string BackgroundColor { get; set; }
            public string ForegroundColor { get; set; }
            public string LinkColor { get; set; }
            public string EntryHoverColor { get; set; }
            public string TableHeaderColor { get; set; }
            public string DocumentIconColor { get; set; }
            public string FolderIconColor { get; set; }
            public string LabelColor { get; set; }
            public string HighlightColor { get; set; }
        }

        public string CultureName { get; set; } = "en-US";
        public string TimezoneId { get; set; } = "UTC";
        public string NoCache { get; set; } = "private";

        private string themeId = "Default";

        public string ThemeId
        {
            get { return themeId; }
            set { themeId = value; OnStateChanged?.Invoke(this, EventArgs.Empty); }
        }

        public event EventHandler OnStateChanged;
    }
}