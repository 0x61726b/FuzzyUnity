using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Assets.Scripts.Framework.Util;
using UnityEngine;

internal class FilePersistence : DB {
    private static readonly string filePath = Application.persistentDataPath + "/persist";
    private static readonly string signaturePath = Application.persistentDataPath + "/persign";
    private static readonly string salt = "completelyRandomLocalFileHashSaltUsedInSigningChangingThisWillResultInNotPersistingTheDataThroughBuildsSoDoNotChange";
    private static readonly string encryptionKey = "435eb4a36c5dd423aa3224e32ff43212";
    private static readonly byte[] encryptionIV = {3, 4, 1, 2, 2, 0, 4, 3, 12, 33, 17, 13, 42, 23, 11, 5};
    private Dictionary<string, string> _dataDict;

    public void Awake() {
        StringBuilder data = new StringBuilder();
        bool success = false;
        EncryptedLocalReader reader = null;
        EncryptedLocalReader signature = null;
        try {
            reader = new EncryptedLocalReader( filePath, encryptionKey, encryptionIV );
            signature = new EncryptedLocalReader( signaturePath, encryptionKey, encryptionIV );
            string sign = signature.ReadLine();
            string line = reader.ReadLine();
            data.Append( salt );
            while( line != null ) {
                _dataDict = Json.Deserialize<Dictionary<string, string>>( line );
                data.Append( line );
                line = reader.ReadLine();
            }
            data.Append( salt );
            success = Md5Tool.CalculateMd5Hash( data.ToString() ).Equals( sign );
            Debug.Log( "[PERSISTENCE] Successfully restored." );

            reader.Close();
            signature.Close();
        } catch( Exception ex ) {
            success = false;
            Debug.LogError( "[PERSISTENCE] Error reading." );
            Debug.LogError( ex );
        } finally {
            if( reader != null ) {
                reader.Close();
            }
            if( signature != null ) {
                signature.Close();
            }
        }
        if( success ) {
            return;
        }
        Debug.LogError( "[PERSISTENCE] Failed." );
        _dataDict = new Dictionary<string, string>();
    }

    public void Start() {}

    public bool HasKey( string key ) {
        return _dataDict.ContainsKey( key );
    }

    public void SetFloat( string key, float val, bool flush = false ) {
        PutData( key, val.ToString( CultureInfo.InvariantCulture ), flush );
    }

    public void SetLong( string key, long val, bool flush = false ) {
        PutData( key, val.ToString(), flush );
    }

    public void SetInt( string key, int val, bool flush = false ) {
        PutData( key, val.ToString(), flush );
    }

    public void SetString( string key, string val, bool flush = false ) {
        PutData( key, val, flush );
    }

    public void SetBool( string key, bool val, bool flush = false ) {
        PutData( key, val.ToString(), flush );
    }

    public float GetFloat( string key, float def ) {
        return _dataDict.ContainsKey( key ) ? float.Parse( _dataDict[key] ) : def;
    }

    public long GetLong( string key, long def ) {
        return _dataDict.ContainsKey( key ) ? long.Parse( _dataDict[key] ) : def;
    }

    public int GetInt( string key, int def ) {
        return _dataDict.ContainsKey( key ) ? int.Parse( _dataDict[key] ) : def;
    }

    public string GetString( string key, string def ) {
        return _dataDict.ContainsKey( key ) ? _dataDict[key] : def;
    }

    public bool GetBool( string key, bool def ) {
        return _dataDict.ContainsKey( key ) ? bool.Parse( _dataDict[key] ) : def;
    }

    public void Remove( string key ) {
        if( _dataDict.ContainsKey( key ) ) {
            _dataDict.Remove( key );
        }
        Persist();
    }

    public void Destroy() {
        Persist();
    }

    public void Flush() {
        Persist();
    }

    private void PutData( string key, string val, bool flush ) {
        if( _dataDict.ContainsKey( key ) ) {
            _dataDict.Remove( key );
        }
        _dataDict.Add( key, val );
        if( flush ) {
            Persist();
        }
    }

    private void Persist() {
        StringBuilder data = new StringBuilder();
        EncryptedLocalWriter writer = null;
        EncryptedLocalWriter signature = null;
        try {
            writer = new EncryptedLocalWriter( filePath, encryptionKey, encryptionIV );
            signature = new EncryptedLocalWriter( signaturePath, encryptionKey, encryptionIV );
            data.Append( salt );
            string line = Json.Serialize( _dataDict );
            data.Append( line );
            writer.WriteLine( line );
            data.Append( salt );
            writer.Close();
            signature.WriteLine( Md5Tool.CalculateMd5Hash( data.ToString() ) );
            signature.Close();
        } catch( Exception ex ) {
            Debug.LogError( "[PERSISTENCE] Error writing." );
            Debug.LogError( ex );
        } finally {
            if( writer != null ) {
                writer.Close();
            }
            if( signature != null ) {
                signature.Close();
            }
        }
    }
}