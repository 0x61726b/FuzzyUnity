public class EncryptedLocalWriter : LocalFileWriter {
    private readonly Crypter _crypter;

    public EncryptedLocalWriter( string i_fileName, string i_key, byte[] i_iv ) : base( i_fileName ) {
        _crypter = new Crypter( i_key, i_iv );
    }

    public override void WriteLine( string i_line ) {
        m_writer.WriteLine( _crypter.encrypt( i_line ) );
    }
}