using System.Collections.Generic;
using UnityEngine;

namespace AlwaysEast
{
    public class Entities : MonoBehaviour
    {
        public static Transform Transform;

        private static List<Entity> s_entities = new List<Entity>();
        private static int s_Turn = 0;
        public static Entity GetPCS => s_entities[0];
        public static Entity GetTurnTaker => s_entities[s_Turn];
        public static GameObject floatingTextParent;
        public static GameObject teleportsmoke;
        public static GameObject EnemyPrefab;

        private void Awake()
        {
            Transform = gameObject.transform;
            floatingTextParent = Resources.Load("Prefabs/Floating Text") as GameObject;
            teleportsmoke = Resources.Load("Prefabs/Puff of smoke") as GameObject;
            EnemyPrefab = Resources.Load<GameObject>("Prefabs/Entities/NPCs/Enemy");
        }

        public static List<Entity> Search(Vector3Int coordinates) => s_entities.FindAll(x => x._coordinates == coordinates);

        public static List<Vector3Int> GetOccupied()
        {
            List<Vector3Int> list = new List<Vector3Int>();
            foreach (Entity item in s_entities)
            {
                list.Add(item._coordinates);
            }
            return list;
        }

        public static void RollMob(Vector3Int spawnPosition, int difficulty)
        {

            EntityReplacement replacement = ResourceRepository.GetRandomAvailableEnemy();
            GameObject instance = Instantiate(EnemyPrefab, spawnPosition + Vector3.up * 0.75f + Vector3.right * 0.5f, Quaternion.identity, Transform);
            Entity entity = instance.GetComponent<Entity>();
            entity._coordinates = spawnPosition;
            entity.SetEntity(replacement);
            s_entities.Add(entity);
        }

        public static void DrawFloatingText(string message, Transform unitTransform, Color color)
        {
            GameObject go = GameObject.Instantiate(floatingTextParent, null);
            go.transform.position = unitTransform.position;

            TMPro.TextMeshPro text = go.GetComponentInChildren<TMPro.TextMeshPro>();
            text.text = message;
            text.color = color;
        }

        public static void DrawTeleport(Transform unitTransform)
        {
            GameObject go = GameObject.Instantiate(teleportsmoke, null);
            go.transform.position = unitTransform.position;
        }

        public static void PlayerSpawn(Vector3Int spawnPosition)
        {
            GameObject prefab = Resources.Load("Prefabs/Entities/NPCs/PlayerCharacter") as GameObject;
            GameObject instance = Instantiate(prefab, spawnPosition + Vector3.up * 0.75f + Vector3.right * 0.5f, Quaternion.identity, Transform);
            Entity entity = instance.GetComponent<PlayerCharacter>();
            entity._coordinates = spawnPosition;
            CameraController.SetFollowTarget(entity.transform);

            s_entities.Add(entity);
        }

        public static void RollFriend(Vector3Int spawnPosition)
        {
        }

        public static void Action()
        {
            s_entities[s_Turn].PreTurn();
            s_entities[s_Turn].Action();
        }

        public static void Move()
        {
            s_entities[s_Turn].Move();
        }

        public static void Attack()
        {
            s_entities[s_Turn].Attack();
        }

        public static void Attack(Vector3Int tile, int damage, float ar, float lvl)
        {
            s_entities.Find(x => x._coordinates == tile).RecieveDamage(damage, ar, lvl);
        }

        public static void Magic()
        {
            //s_entities[s_Turn].Magic();
        }

        public static void Examine()
        {
            //Log.Print(Entities.Search(TileMapCursor.SelectedTileCoordinates).ExamineText);
        }

        public static void Remove(Entity entity)
        {
            s_entities.Remove(entity);
            //GameObject.Destroy( entity.gameObject );
            entity.name += " (Dead)";
        }

        public static void Step(bool pause)
        {
            s_Turn++;

            if (s_Turn >= s_entities.Count)
                s_Turn = 0;

            if (s_entities[s_Turn].isAggressive || s_Turn == 0)
                CameraController.SetFollowTarget(s_entities[s_Turn].transform);

            if (s_Turn == 0)
            {
                GetPCS.PreTurn();
                HUDControls.Show();
            }
            else
            {
                if (pause)
                    running = true;
                else
                {
                    Action();
                }
            }
        }

        private static bool running = false;
        private static float timer = 0.5f;
        private const float interval = 0.5f;

        private void Update()
        {
            if (!running)
                return;

            timer -= Time.smoothDeltaTime;

            if (timer <= 0.0f)
            {
                timer = interval;
                running = false;
                Action();
            }
        }
    }
}