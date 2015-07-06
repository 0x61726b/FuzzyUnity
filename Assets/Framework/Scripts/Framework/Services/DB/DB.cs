using Assets.Scripts.Framework.Services;

internal interface DB : Provider {
    bool HasKey( string key );
    void SetFloat( string key, float value, bool flush = false );
    float GetFloat( string key, float def );
    void SetLong( string key, long value, bool flush = false );
    long GetLong( string key, long def );
    void SetInt( string key, int value, bool flush = false );
    int GetInt( string key, int def );
    void SetString( string key, string value, bool flush = false );
    string GetString( string key, string def );
    void SetBool( string key, bool value, bool flush = false );
    bool GetBool( string key, bool def );
    void Remove( string key );
    void Flush();
}