using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dongjun.LevelEditor
{
    public enum MapLayer
    {
        BackGround,
        Main,
        ForeGround
    }

    public class LevelEditorData : MonoBehaviour
    {
        [Header("Map Data")]
        [SerializeField] private MapData mapData;

        [Header("Tile DB")]
        [SerializeField] private Tile[] tilePrefabs_BG;
        [SerializeField] private Tile[] tilePrefabs_Main;
        [SerializeField] private Tile[] tilePrefabs_FG;

        private Dictionary<MapLayer, Tile[,]> grids = new Dictionary<MapLayer, Tile[,]>();
        private Dictionary<MapLayer, Transform> layerParents = new Dictionary<MapLayer, Transform>();
        private Dictionary<MapLayer, Tile> curTileData = new Dictionary<MapLayer, Tile>();

        public Dictionary<MapLayer, Tile[]> TilePrefabDB
        { get; private set; } = new Dictionary<MapLayer, Tile[]>();

        public MapData MapData => mapData;
        public MapLayer Layer
        { get; private set; } = MapLayer.Main;
        public Tile[,] Grid => grids[Layer];
        public Transform LayerParent => layerParents[Layer];
        public Tile CurTile => curTileData[Layer];

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            // Init Tile Prefab DB
            TilePrefabDB.Add(MapLayer.BackGround, tilePrefabs_BG);
            TilePrefabDB.Add(MapLayer.Main, tilePrefabs_Main);
            TilePrefabDB.Add(MapLayer.ForeGround, tilePrefabs_FG);

            void InitGrid(MapLayer layer)
            {
                grids.Add(layer, new Tile[mapData.MaxSizeX, mapData.MaxSizeY]);
            }
            void InitLayerParent(MapLayer layer)
            {
                var layerParent = new GameObject();
                layerParent.name = layer.ToString();
                layerParent.transform.SetParent(mapData.transform);
                layerParents.Add(layer, layerParent.transform);
            }
            void InitCurTilePrefab(MapLayer layer)
            {
                curTileData.Add(layer, null);
            }
            for (int i = 0; i < Enum.GetValues(typeof(MapLayer)).Length; i++)
            {
                var layer = (MapLayer)i;
                InitGrid(layer);
                InitLayerParent(layer);
                InitCurTilePrefab(layer);
            }
        }

        public Tile GetTilePrefab(string name)
        {
            Tile result;

            result = Array.Find(tilePrefabs_BG, i => i.gameObject.name == name);
            if (result != null)
                return result;

            result = Array.Find(tilePrefabs_Main, i => i.gameObject.name == name);
            if (result != null)
                return result;

            result = Array.Find(tilePrefabs_FG, i => i.gameObject.name == name);
            if (result != null)
                return result;

            return null;
        }

        public void SetCurLayer(MapLayer layer)
        {
            Layer = layer;
        }
        public void SetCurTilePrefab(Tile prefab)
        {
            curTileData[Layer] = prefab;
        }

        public bool IsLayerVisable(MapLayer maplayer)
        {
            return layerParents[maplayer].gameObject.activeSelf;
        }
        public void ShowLayer(MapLayer maplayer)
        {
            layerParents[maplayer].gameObject.SetActive(true);
        }
        public void HideLayer(MapLayer maplayer)
        {
            layerParents[maplayer].gameObject.SetActive(false);
        }
    }
}

