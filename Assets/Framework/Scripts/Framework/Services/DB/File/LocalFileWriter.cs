using System.IO;

public class LocalFileWriter {
    private readonly string m_fileName;
    protected readonly StreamWriter m_writer;

    public LocalFileWriter( string i_fileName ) {
        m_fileName = i_fileName;
        m_writer = File.CreateText( m_fileName );
        m_writer.AutoFlush = true;
    }

    ~LocalFileWriter() {
        Close();
    }

    public virtual void WriteLine( string i_line ) {
        m_writer.WriteLine( i_line );
    }

    public void Close() {
        m_writer.Close();
    }
}