using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entities : MonoBehaviour
{
    public static List<Entity> entities = new List<Entity>();
    public static List<Entity> Search( Vector3Int coordinates ) => entities.FindAll( x => x.coordinates == coordinates );

    public static Transform transform;

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

    public void Action()
    {
        entities[0].Action();
    }
}
