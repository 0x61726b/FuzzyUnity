using System;
using System.Security.Cryptography;
using System.Text;

public class Crypter {
    private readonly RijndaelManaged _aes;

    public Crypter( string i_key, byte[] i_iv ) {
        _aes = new RijndaelManaged();
        _aes.Mode = CipherMode.CBC;
        _aes.Padding = PaddingMode.ISO10126;
        _aes.Key = fromHexString( i_key );
        _aes.IV = i_iv;
    }

    public string encrypt( string i_value ) {
        string res = string.Empty;
        if( i_value != null && i_value.Trim() != string.Empty ) {
            ICryptoTransform encryptor = _aes.CreateEncryptor();
            byte[] plainTextByte = Encoding.UTF8.GetBytes( i_value );
            byte[] CipherText = encryptor.TransformFinalBlock( plainTextByte, 0, plainTextByte.Length );
            res = Convert.ToBase64String( CipherText );
        }
        return res;
    }

    public string decrypt( string i_value ) {
        string res = string.Empty;
        if( i_value != null && i_value.Trim() != string.Empty ) {
            ICryptoTransform decryptor = _aes.CreateDecryptor();
            byte[] encryptedBytes = Convert.FromBase64CharArray( i_value.ToCharArray(), 0, i_value.Length );
            byte[] decryptedData = decryptor.TransformFinalBlock( encryptedBytes, 0, encryptedBytes.Length );
            res = Encoding.UTF8.GetString( decryptedData );
        }
        return res;
    }

    private byte[] fromHexString( string i_hexString ) {
        if( i_hexString == null || i_hexString.Trim() == string.Empty ) {
            return new byte[0];
        }
        byte[] result = new byte[i_hexString.Length/2];
        char[] enc = i_hexString.ToCharArray();
        for( int i = 0; i < enc.Length; i += 2 ) {
            result[i/2] = ( byte ) Convert.ToInt32( Convert.ToString( enc[i] ) + enc[i + 1], 16 );
        }
        return result;
    }
}