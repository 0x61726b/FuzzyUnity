using System;
using System.Collections.Generic;
using UnityEngine;

static class LocalizationUtil {
    public static string GetLanguageName( Language language ) {
        switch( language ) {
            case Language.EN:
                return "English";

            default:
                return string.Empty;
        }
    }

    public static Language GetLanguageTypeBySystemLanguage( SystemLanguage languageName ) {
        switch( languageName ) {
            case SystemLanguage.English:
                return Language.EN;

            default:
                return Language.EN;
        }
    }

    public static List<Language> GetAvailableLanguages() {
        List<Language> availableLanguages = new List<Language>();
        foreach( int lang in Enum.GetValues( typeof( Language ) ) ) {
            availableLanguages.Add( ( Language ) lang );
        }
        return availableLanguages;
    }
}