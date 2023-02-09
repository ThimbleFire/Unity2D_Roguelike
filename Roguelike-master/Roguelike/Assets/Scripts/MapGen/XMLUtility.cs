using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class XMLUtility {

    public static void Save<T>( T item, string directory, string name ) {
        XmlSerializer serialWrite = new XmlSerializer( typeof(T));

        if (Directory.Exists(Application.dataPath + "/Resources/" + directory) == false)
            Directory.CreateDirectory(Application.dataPath + "/Resources/" + directory);

        Stream stream = new FileStream(Application.dataPath + "/Resources/" + directory + name + ".xml",   FileMode.Create , FileAccess.Write );
        serialWrite.Serialize( stream, item );
        stream.Close();
        stream.Dispose();
    }

    public static T Load<T>(UnityEngine.Object obj)
    {
        TextAsset asset = (TextAsset)obj;

        XmlSerializer parametersSerializer = new XmlSerializer(typeof(T));
        Stream reader = new MemoryStream(asset.bytes);
        StreamReader textReader = new StreamReader(reader);
        T product = (T)parametersSerializer.Deserialize(textReader);
        reader.Dispose();

        return product;
    }
    public static T Load<T>(TextAsset asset)
    {
        XmlSerializer parametersSerializer = new XmlSerializer(typeof(T));
        Stream reader = new MemoryStream(asset.bytes);
        StreamReader textReader = new StreamReader(reader);
        T product = (T)parametersSerializer.Deserialize(textReader);
        reader.Dispose();

        return product;
    }
    public static T Load<T>( string filename ) {
        XmlSerializer parametersSerializer = new XmlSerializer(typeof(T));

        UnityEngine.Object obj = Resources.Load(filename);
        TextAsset asset = (TextAsset)obj;
        byte[] data =  asset.bytes;
        Stream reader = new MemoryStream( data );
        StreamReader textReader = new StreamReader(reader);
        T product = ( T )parametersSerializer.Deserialize( textReader );
        reader.Dispose();

        return product;
    }
}