using System.Collections.Generic;
using UnityEngine;

public class Entities : MonoBehaviour {
    public static Transform Transform;

    private static List<Entity> s_entities = new List<Entity>();
    private static int s_Turn = 0;

    public static Vector3Int GetPCCoordinates => s_entities[0]._coordinates;

    private void Awake() {
        Transform = gameObject.transform;
    }

    public static List<Entity> Search( Vector3Int coordinates ) => s_entities.FindAll( x => x._coordinates == coordinates );

    public static List<Vector3Int> GetOccupied() {
        List<Vector3Int> list = new List<Vector3Int>();
        foreach ( Entity item in s_entities ) {
            list.Add( item._coordinates );
        }
        return list;
    }

    public static void RollMob( Vector3Int spawnPosition, int difficulty ) {
        GameObject prefab = ResourceRepository.GetUnit("Imp");
        GameObject instance = Instantiate( prefab, spawnPosition, Quaternion.identity, Transform );
        Entity entity = instance.GetComponent<Entity>();
        entity.Teleport( spawnPosition );

        s_entities.Add( entity );
    }

    public static void PlayerSpawn( Vector3Int spawnPosition ) {
        GameObject prefab = ResourceRepository.GetUnit("PlayerCharacter");
        GameObject instance = Instantiate( prefab, spawnPosition, Quaternion.identity, Transform );
        Entity entity = instance.GetComponent<PlayerCharacter>();
        CameraController.SetFollowTarget( entity.transform );
        entity.Teleport( spawnPosition );

        s_entities.Add( entity );
    }

    public static void RollFriend( Vector3Int spawnPosition ) {
    }

    public static void Action() {
        s_entities[s_Turn].Action();
    }

    public static void Move() {
        s_entities[s_Turn].Move();
    }

    public static void Attack() {
        s_entities[s_Turn].Attack();
    }

    public static void Attack( Vector3Int tile, int damage ) {
        s_entities.Find( x => x._coordinates == tile ).DealDamage( damage );
    }

    public static void Magic() {
        //s_entities[s_Turn].Magic();
    }

    public static void Examine() {
        //Log.Print(Entities.Search(TileMapCursor.SelectedTileCoordinates).ExamineText);
    }

    public static void Remove( Entity entity ) {
        s_entities.Remove( entity );
        GameObject.Destroy( entity.gameObject );
    }

    public static void Step() {
        s_Turn++;

        if ( s_Turn >= s_entities.Count )
            s_Turn = 0;

        if ( s_Turn == 0 ) {
            HUDControls.Show();
        }
        else {
            Action();
        }
    }
}