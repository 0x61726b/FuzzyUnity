using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Assets.Scripts.Framework.Services;
using UnityEngine;

class LocalizationProvider : Localization {
    private readonly Regex rgx = new Regex( "\\[\\]" );
    private Dictionary<Dict, string> m_localizedFields = new Dictionary<Dict, string>();

    public void Awake() {
        
    }

    public void Start() {
		loadFromAsset( ServiceLocator.GetSettings().GetCurrentLanguage() );
        ServiceLocator.GetSettings().AddLanguageListener(onLanguageUpdated );
    }

    public void Destroy() {
        ServiceLocator.GetSettings().RemoveLanguageListener(onLanguageUpdated );
    }

    public string GetString( Dict dict, string[] replacements = null ) {
        if( m_localizedFields.ContainsKey( dict ) ) {
            string value = m_localizedFields[dict];
            if( replacements != null ) {
                foreach( string val in replacements ) {
                    value = rgx.Replace( value, val, 1 );
                }
            }
            return value;
        } else {
            Debug.Log( "Placeholder was returned for key: " + dict );
            return "PLACEHOLDER";
        }
    }

    public List<Language> GetAvailableLanguages() {
        return LocalizationUtil.GetAvailableLanguages();
    }

    private void onLanguageUpdated(Language current) {
		loadFromAsset(current);
    }

    private void loadFromAsset( Language language ) {
        TextAsset localizationFile = Resources.Load( "Localization/" + language.ToString() ) as TextAsset;
        m_localizedFields = new Dictionary<Dict, string>();
        string[] lines = localizationFile.text.Split( "\n"[0] );
        foreach( string line in lines ) {
            if( !line.Trim().Equals( string.Empty ) && !line.StartsWith( "#" ) ) {
                string[] pair = Regex.Split( line, "##" );
                m_localizedFields.Add( ( Dict ) Enum.Parse( typeof( Dict ), pair[0].Trim().ToUpper() ), pair[1].Trim().Replace( "\\n", "\n" ) );
            }
        }
    }
}