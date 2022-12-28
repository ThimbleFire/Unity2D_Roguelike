using System.Collections.Generic;
using UnityEngine;

public class Entities : MonoBehaviour {
    public static Transform Transform;

    private static List<Entity> s_entities = new List<Entity>();
    private static int s_Turn = 0;
    public static Vector3Int GetPCCoordinates => s_entities[0]._coordinates;
    public static Entity GetTurnTaker { get { return s_entities[s_Turn]; } }

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
        GameObject instance = Instantiate( prefab, spawnPosition + Vector3.up * 0.75f + Vector3.right * 0.5f, Quaternion.identity, Transform );
        Entity entity = instance.GetComponent<Entity>();
        entity._coordinates = spawnPosition;

        s_entities.Add( entity );
    }

    public static void PlayerSpawn( Vector3Int spawnPosition ) {
        GameObject prefab = ResourceRepository.GetUnit("PlayerCharacter");
        GameObject instance = Instantiate( prefab, spawnPosition + Vector3.up * 0.75f + Vector3.right * 0.5f, Quaternion.identity, Transform );
        Entity entity = instance.GetComponent<PlayerCharacter>();
        entity._coordinates = spawnPosition;
        CameraController.SetFollowTarget( entity.transform );

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

    public static void Attack( Vector3Int tile, int damage, string attacker ) {
        s_entities.Find( x => x._coordinates == tile ).DealDamage( damage, attacker );
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

    public static void Step( bool pause ) {
        s_Turn++;

        if ( s_Turn >= s_entities.Count )
            s_Turn = 0;

        if ( s_entities[s_Turn].isAggressive || s_Turn == 0 )
            CameraController.SetFollowTarget( s_entities[s_Turn].transform );

        if ( s_Turn == 0 ) {
            HUDControls.Show();
        }
        else {
            if ( pause )
                running = true;
            else {
                Action();
            }
        }
    }

    private static bool running = false;
    private static float timer = 0.5f;
    private const float interval = 0.5f;

    private void Update() {
        if ( !running )
            return;

        timer -= Time.smoothDeltaTime;

        if ( timer <= 0.0f ) {
            timer = interval;
            running = false;
            Action();
        }
    }
}