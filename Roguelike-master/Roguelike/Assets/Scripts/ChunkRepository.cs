using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ChunkRepository : MonoBehaviour
{
    private static List<Chunk> chunksInMemory;

    private void Awake()
    {
        // load maps

        chunksInMemory = new List<Chunk>();

        string[] filenames = Directory.GetFiles( Application.streamingAssetsPath, "*.xml" );
        for ( int i = 0; i < filenames.Length; i++ )
        {
            filenames[i] = filenames[i].Remove( 0, Application.streamingAssetsPath.Length + 1 );
            Chunk r = XMLUtility.Load<Chunk>( filenames[i] );
            chunksInMemory.Add( r );
        }
    }

    public static string Get( int width, int height )
    {
        List<Chunk> chunksMatchingDimensions = new List<Chunk>( chunksInMemory.FindAll( x => x.Width == width && x.Height == height ) );

        if ( chunksMatchingDimensions.Count == 0 )
            return string.Empty;

        return chunksMatchingDimensions[Random.Range( 0, chunksMatchingDimensions.Count )].Name;
    }

    public static Chunk Get( string filename )
    {
        return chunksInMemory.Find( x => x.Name == filename ).Clone();
    }

    public static string GetRandom(ref int width, ref int height)
    {
        Chunk c = chunksInMemory[Random.Range( 0, chunksInMemory.Count )];

        width = c.Width;
        height = c.Height;

        return c.Name;
    }
}
