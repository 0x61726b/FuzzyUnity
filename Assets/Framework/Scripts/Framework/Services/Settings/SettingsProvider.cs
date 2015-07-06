using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Framework.Services;

class SettingsProvider : Settings {
    private const string KEY_LANGUAGE = "key_language";
    private const string KEY_SFX = "key_sfx";
    private const string KEY_MUSIC = "key_music";
    private const string KEY_DAY_NOTIFICATIONS = "key_day_notifications";
    private const string KEY_LIFE_NOTIFICATIONS = "key_life_notifications";

    private List<Action<Language>> languageListeners;
    private List<Action<bool>> sfxStateListeners;
    private List<Action<bool>> musicStateListeners;
    private List<Action<bool>> dayNotificationsStateListeners;
    private List<Action<bool>> lifeNotificationsStateListeners;

    private static Language currentLanguage;
    private static bool currentSFXStatus;
    private static bool currentMusicStatus;
    private static bool currentDayNotificationsState;
    private static bool currentLifeNotificationsState;

    public void Start() {
        languageListeners = new List<Action<Language>>();
        sfxStateListeners = new List<Action<bool>>();
        musicStateListeners = new List<Action<bool>>();
        dayNotificationsStateListeners = new List<Action<bool>>();
        lifeNotificationsStateListeners = new List<Action<bool>>();

        // Language initialization
        if( !hasDBData( KEY_LANGUAGE ) ) {
            Language lang = LocalizationUtil.GetLanguageTypeBySystemLanguage( Application.systemLanguage );
            SetCurrentLanguage( lang );
        } else {
            currentLanguage = ( Language ) Enum.Parse( typeof( Language ), getDBData( KEY_LANGUAGE ) );
        }

        // SFX initialization
        if( !hasDBData( KEY_SFX ) ) {
            SetCurrentSFXState( true );
        } else {
            currentSFXStatus = bool.Parse( getDBData( KEY_SFX ) );
        }

        // Music initialization
        if( !hasDBData( KEY_MUSIC ) ) {
            SetCurrentMusicState( true );
        } else {
            currentMusicStatus = bool.Parse( getDBData( KEY_MUSIC ) );
        }

        // Day Notifications initialization
        if( !hasDBData( KEY_DAY_NOTIFICATIONS ) ) {
            SetCurrentDayNotificationsState( true );
        } else {
            currentDayNotificationsState = bool.Parse( getDBData( KEY_DAY_NOTIFICATIONS ) );
        }

        // Life Notifications initialization
        if( !hasDBData( KEY_LIFE_NOTIFICATIONS ) ) {
            SetCurrentLifeNotificationsState( true );
        } else {
            currentLifeNotificationsState = bool.Parse( getDBData( KEY_LIFE_NOTIFICATIONS ) );
        }
	}
	
	public void Awake() {
		
	}
	
	public void Destroy() {
		
	}

    public Language GetCurrentLanguage() {
        return currentLanguage;
    }

    public void SetCurrentLanguage( Language language ) {
        currentLanguage = language;
        setDBData( KEY_LANGUAGE, currentLanguage.ToString() );

        foreach( Action<Language> listener in languageListeners ) {
            listener( currentLanguage );
        }
    }

    public void AddLanguageListener( Action<Language> listener ) {
        languageListeners.Add( listener );
    }

    public void RemoveLanguageListener( Action<Language> listener ) {
        languageListeners.Remove( listener );
    }

    public bool GetCurrentSFXState() {
        return currentSFXStatus;
    }

    public void SetCurrentSFXState( bool state ) {
        currentSFXStatus = state;
        setDBData( KEY_SFX, currentSFXStatus.ToString() );

        foreach( Action<bool> listener in sfxStateListeners ) {
            listener( currentSFXStatus );
        }
    }

    public void AddSFXStateListener( Action<bool> listener ) {
        sfxStateListeners.Add( listener );
    }

    public void RemoveSFXStateListener( Action<bool> listener ) {
        sfxStateListeners.Remove( listener );
    }

    public bool GetCurrentMusicState() {
        return currentMusicStatus;
    }

    public void SetCurrentMusicState( bool state ) {
        currentMusicStatus = state;
        setDBData( KEY_MUSIC, currentMusicStatus.ToString() );

        foreach( Action<bool> listener in musicStateListeners ) {
            listener( currentMusicStatus );
        }
    }

    public void AddMusicStateListener( Action<bool> listener ) {
        musicStateListeners.Add( listener );
    }

    public void RemoveMusicStateListener( Action<bool> listener ) {
        musicStateListeners.Remove( listener );
    }

    public bool GetCurrentDayNotificationsState() {
        return currentDayNotificationsState;
    }

    public void SetCurrentDayNotificationsState( bool state ) {
        currentDayNotificationsState = state;
        setDBData( KEY_DAY_NOTIFICATIONS, currentDayNotificationsState.ToString() );

        foreach( Action<bool> listener in dayNotificationsStateListeners ) {
            listener( currentDayNotificationsState );
        }
    }

    public void AddDayNotificationsStateListener( Action<bool> listener ) {
        dayNotificationsStateListeners.Add( listener );
    }

    public void RemoveDayNotificationsStateListener( Action<bool> listener ) {
        dayNotificationsStateListeners.Remove( listener );
    }

    public bool GetCurrentLifeNotificationsState() {
        return currentLifeNotificationsState;
    }

    public void SetCurrentLifeNotificationsState( bool state ) {
        currentLifeNotificationsState = state;
        setDBData( KEY_LIFE_NOTIFICATIONS, currentLifeNotificationsState.ToString() );

        foreach( Action<bool> listener in lifeNotificationsStateListeners ) {
            listener( currentLifeNotificationsState );
        }
    }

    public void AddLifeNotificationsStateListener( Action<bool> listener ) {
        lifeNotificationsStateListeners.Add( listener );
    }

    public void RemoveLifeNotificationsStateListener( Action<bool> listener ) {
        lifeNotificationsStateListeners.Remove( listener );
    }



    private bool hasDBData( string key ) {
		return ServiceLocator.GetDB().HasKey( key );
    }

    private string getDBData( string key ) {
		return ServiceLocator.GetDB().GetString( key, "" );
    }

    private void setDBData( string key, string value ) {
		ServiceLocator.GetDB().SetString( key, value, true );
    }
}