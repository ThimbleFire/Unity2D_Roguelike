using UnityEngine;
using UnityEngine.Tilemaps;

namespace AlwaysEast
{
    public class BoardManager : MonoBehaviour
    {

        public static Tilemap tileMapGround;
        public static Tilemap tileMapWalls;
        public static Tilemap tileMapCurios;

        public static int RoomLimit { get; set; }

        public static int Width { get; set; }
        public static int Height { get; set; }

        private static MapProfile MapProfile { get; set; }

        private void Awake()
        {
            tileMapGround = GameObject.Find("Ground").GetComponent<Tilemap>();
            tileMapWalls = GameObject.Find("Walls").GetComponent<Tilemap>();
            tileMapCurios = GameObject.Find("Curio").GetComponent<Tilemap>();

            Application.targetFrameRate = 60;
            Width = 64;
            Height = 64;

            MapProfile = Game.LoadState<MapProfile>("MapProfile.east");
            ResourceRepository.LoadMapData(MapProfile.MapIndex);
        }

        private void Start()
        {
            RoomLimit = 32;
            Build();
        }

        public static void Build()
        {
            tileMapGround.ClearAllTiles();
            tileMapWalls.ClearAllTiles();
            tileMapCurios.ClearAllTiles();

            MapFactory.Build(MapProfile.MapSeed);

            tileMapWalls.CompressBounds();
            tileMapGround.CompressBounds();
            tileMapCurios.CompressBounds();

            ShadowCaster2DFromComposite.RebuildAll();

            Pathfind.Setup(tileMapGround);
        }
    }
}