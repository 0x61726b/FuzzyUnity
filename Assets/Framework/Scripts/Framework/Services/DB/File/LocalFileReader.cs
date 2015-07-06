using System.IO;

public class LocalFileReader {
    private readonly string m_fileName;
    protected readonly StreamReader m_reader;

    public LocalFileReader( string i_fileName ) {
        m_fileName = i_fileName;
#if UNITY_ANDROID
        if ( !System.IO.File.Exists( m_fileName ) ) {
            System.IO.File.CreateText( m_fileName ).Close();
        }
#endif
        m_reader = File.OpenText( m_fileName );
    }

    ~LocalFileReader() {
        Close();
    }

    public virtual string ReadLine() {
        if( !m_reader.EndOfStream ) {
            return m_reader.ReadLine();
        }
        return null;
    }

    public void Close() {
        m_reader.Close();
    }
}