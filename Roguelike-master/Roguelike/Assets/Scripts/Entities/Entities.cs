using System.Collections.Generic;
using UnityEngine;

public class Entities : MonoBehaviour
{
    public static Transform Transform;

    private static List<Entity> s_entities = new List<Entity>();
    private static int s_Turn = 0;

    private void Awake()
    {
        Transform = gameObject.transform;
    }

    public static List<Entity> Search( Vector3Int coordinates ) => s_entities.FindAll( x => x.Coordinates == coordinates );

    public static void RollMob( Vector3Int spawnPosition, int difficulty )
    {
        GameObject prefab = ResourceRepository.GetUnit("Imp");
        GameObject instance = Instantiate( prefab, spawnPosition, Quaternion.identity, Transform );
        Entity entity = instance.GetComponent<Entity>();
        entity.Teleport( spawnPosition );

        s_entities.Add( entity );
    }

    public static void PlayerSpawn( Vector3Int spawnPosition )
    {
        GameObject prefab = ResourceRepository.GetUnit("PlayerCharacter");
        GameObject instance = Instantiate( prefab, spawnPosition, Quaternion.identity, Transform );
        Entity entity = instance.GetComponent<PlayerCharacter>();
        CameraController.SetFollowTarget( entity.transform );
        entity.Teleport( spawnPosition );

        s_entities.Add( entity );
    }

    public static void RollFriend( Vector3Int spawnPosition )
    {
    }

    public static void Action()
    {
        s_entities[s_Turn].Action( s_entities[0].Coordinates );
    }

    public static void Step()
    {
        s_Turn++;

        if ( s_Turn >= s_entities.Count )
            s_Turn = 0;

        if ( s_Turn == 0 )
        {
            HUDControls.Show();
        }
        else
        {
            Action();
        }
    }

    public static List<Vector3Int> GetObstacles()
    {
        List<Vector3Int> occupiedPositions = new List<Vector3Int>();
        foreach ( Entity entity in s_entities )
            occupiedPositions.Add( entity.Coordinates );
        return occupiedPositions;
    }
}