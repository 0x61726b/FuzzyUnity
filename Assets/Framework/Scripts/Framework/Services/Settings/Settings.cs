using System;
using Assets.Scripts.Framework.Services;

interface Settings : Provider {
    Language GetCurrentLanguage();
    void SetCurrentLanguage( Language language );
    void AddLanguageListener( Action<Language> listener );
    void RemoveLanguageListener( Action<Language> listener );

    bool GetCurrentSFXState();
    void SetCurrentSFXState( bool state );
    void AddSFXStateListener( Action<bool> listener );
    void RemoveSFXStateListener( Action<bool> listener );

    bool GetCurrentMusicState();
    void SetCurrentMusicState( bool state );
    void AddMusicStateListener( Action<bool> listener );
    void RemoveMusicStateListener( Action<bool> listener );

    bool GetCurrentDayNotificationsState();
    void SetCurrentDayNotificationsState( bool state );
    void AddDayNotificationsStateListener( Action<bool> listener );
    void RemoveDayNotificationsStateListener( Action<bool> listener );

    bool GetCurrentLifeNotificationsState();
    void SetCurrentLifeNotificationsState( bool state );
    void AddLifeNotificationsStateListener( Action<bool> listener );
    void RemoveLifeNotificationsStateListener( Action<bool> listener );
}