using System.Collections.Generic;
using Assets.Scripts.Framework.Services;

interface Localization : Provider {
    string GetString( Dict dict, string[] replacements = null );
    List<Language> GetAvailableLanguages();
}