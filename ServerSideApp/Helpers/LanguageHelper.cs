using System.Collections.Generic;
using System.Globalization;

namespace ServerSideApp.Helpers
{
    public enum Languages
    {
        NL,
        EN
    }
    public static class LanguageHelper
    {
        public const string LANGUAGECOOKIE = "SsaLang";

        public static Dictionary<Languages, string> LanguageStrings;

        static LanguageHelper() {
            LanguageStrings = new Dictionary<Languages, string>{
                { Languages.NL, "nl-BE"},
                { Languages.EN, "en-US"}};
        }
        public static string DefaultLanguage => LanguageStrings[Languages.EN];
    }
}