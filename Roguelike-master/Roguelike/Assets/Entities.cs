using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entities : MonoBehaviour
{
    public PlayerCharacter playerCharacter;

    private void Awake()
    {
        entities.Add( playerCharacter );
    }

    public static List<Entity> Search( Vector3Int coordinates ) => entities.FindAll( x => x.coordinates == coordinates );

    public static List<Entity> entities = new List<Entity>();
}
