public class EncryptedLocalReader : LocalFileReader {
    private readonly Crypter _crypter;

    public EncryptedLocalReader( string i_fileName, string i_key, byte[] i_iv )
        : base( i_fileName ) {
        _crypter = new Crypter( i_key, i_iv );
    }

    public override string ReadLine() {
        if( !m_reader.EndOfStream ) {
            return _crypter.decrypt( m_reader.ReadLine() );
        }
        return null;
    }
}