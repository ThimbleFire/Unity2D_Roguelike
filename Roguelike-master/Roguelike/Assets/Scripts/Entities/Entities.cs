using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entities : MonoBehaviour
{
    public static List<Entity> Search( Vector3Int coordinates ) => entities.FindAll( x => x.coordinates == coordinates );
    public static Transform transform;

    private static List<Entity> entities = new List<Entity>();
    private static int Turn = 0;

    private void Awake()
    {
        transform = gameObject.transform;
    }

    public static void RollMob( Vector3Int spawnPosition, int difficulty )
    {
        GameObject prefab = ResourceRepository.GetUnit("Imp");
        GameObject instance = Instantiate( prefab, spawnPosition, Quaternion.identity, transform );
        Entity entity = instance.GetComponent<Entity>();
        entity.Teleport( spawnPosition );

        entities.Add( entity );
    }

    public static void PlayerSpawn(Vector3Int spawnPosition)
    {
        GameObject prefab = ResourceRepository.GetUnit("PlayerCharacter");
        GameObject instance = Instantiate( prefab, spawnPosition, Quaternion.identity, transform );
        Entity entity = instance.GetComponent<PlayerCharacter>();
        CameraController.SetFollowTarget( entity.transform );
        entity.Teleport( spawnPosition );

        entities.Add( entity );
    }

    public static void RollFriend( Vector3Int spawnPosition )
    {

    }

    public static void Action()
    {
        entities[Turn].Action(entities[0].coordinates);
    }

    public static void Step()
    {
        Turn++;

        if ( Turn >= entities.Count )
            Turn = 0;

        if ( Turn == 0 )
        {
            HUDControls.Show();
        }
        else
        {
            Action();
        }
    }

    public static List<Vector3Int> GetObstacles() {
        List<Vector3Int> occupiedPositions = new List<Vector3Int>();
        foreach ( Entity entity in entities )
            occupiedPositions.Add( entity.coordinates );        
        return occupiedPositions;
    }
}
