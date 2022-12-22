using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class XMLUtility
{
    public static void Save<T>( T dataToSerialize, string filename )
    {
        XmlSerializer serializer = new XmlSerializer( typeof( T ) );

        using ( FileStream stream = new FileStream( Application.streamingAssetsPath + "/" + filename, FileMode.Create ) )
            serializer.Serialize( stream, dataToSerialize );
    }

    public static T Load<T>( string filename )
    {
        XmlSerializer xmlSerializer = new XmlSerializer( typeof( T ) );
        try
        {
            using ( TextReader reader = new StreamReader( Application.streamingAssetsPath + "/" + filename ) )
            {
                return ( T )xmlSerializer.Deserialize( reader );
            }
        }
        catch ( System.Exception )
        {
            Debug.LogError( filename + " not found" );
            return ( T )default;
        }
    }
}