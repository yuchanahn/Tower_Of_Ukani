using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using Dongjun.Helper;

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
        [SerializeField] private string curSaveLocation;
        [SerializeField] private MapData mapData;

        [Header("Tile DB")]
        [SerializeField] private Tile[] tilePrefabs_BG;
        [SerializeField] private Tile[] tilePrefabs_Main;
        [SerializeField] private Tile[] tilePrefabs_FG;

        private Dictionary<MapLayer, Tile[,]> grids = new Dictionary<MapLayer, Tile[,]>();
        private Dictionary<MapLayer, Transform> layerParents = new Dictionary<MapLayer, Transform>();
        private Dictionary<MapLayer, Tile> curTileData = new Dictionary<MapLayer, Tile>();

        public string CurSaveLocation => curSaveLocation;
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
        private void ResetGrid()
        {
            grids.Clear();

            for (int i = 0; i < Enum.GetValues(typeof(MapLayer)).Length; i++)
            {
                var layer = (MapLayer)i;
                grids.Add(layer, new Tile[mapData.MaxSizeX, mapData.MaxSizeY]);
            }
        }

        public void SetCurSaveLocation(string path)
        {
            curSaveLocation = path;
        }
        public void SaveTo(string path)
        {
            StringBuilder data = new StringBuilder();

            data.Append("[Info]\n");
            data.Append("Size X: ");
            data.Append(mapData.MaxSizeX);
            data.Append(";\nSize Y: ");
            data.Append(mapData.MaxSizeY);
            data.Append(";\n");

            for (int layer = 0; layer < EnumHelper.Count<MapLayer>(); layer++)
            {
                data.Append("[");
                data.Append(((MapLayer)layer).ToString());
                data.Append("]\n");

                for (int y = 0; y < mapData.MaxSizeY; y++)
                {
                    for (int x = 0; x < mapData.MaxSizeX; x++)
                    {
                        data.Append(grids[(MapLayer)layer][x, y]?.gameObject.name ?? "null");

                        if (x != mapData.MaxSizeX - 1)
                            data.Append(" || ");
                    }

                    if (y != mapData.MaxSizeY - 1)
                        data.Append(" ^\n");
                }
                data.Append(";\n");
            }

            File.WriteAllText(path, data.ToString(), Encoding.UTF8);
        }
        public void LoadFrom(string path)
        {
            // Get Raw Data
            string rawData = File.ReadAllText(path, Encoding.UTF8);
            rawData.Trim();

            // Get Info
            string info = rawData.Substring(rawData.IndexOf("[Info]") + "[Info]".Length);
            info = info.Substring(0, info.IndexOf('[')).Trim();

            var sizeX = info.Substring(info.IndexOf("Size X : ") + "Size X : ".Length);
            sizeX = sizeX.Substring(0, sizeX.IndexOf(';')).Trim();

            var sizeY = info.Substring(info.IndexOf("Size Y : ") + "Size Y : ".Length);
            sizeY = sizeY.Substring(0, sizeY.IndexOf(';')).Trim();

            mapData.SetSize(int.Parse(sizeX), int.Parse(sizeY));
            ResetGrid();

            string data = rawData.Replace("[Info]", "").Replace(info, "");

            for (int layer = 0; layer < EnumHelper.Count<MapLayer>(); layer++)
            {
                var curLayer = (MapLayer)layer;

                string thisLayer = data.Substring(data.IndexOf($"[{curLayer.ToString()}]") + $"[{curLayer.ToString()}]".Length);
                thisLayer = thisLayer.Substring(0, thisLayer.IndexOf(';'));

                string[] rows = thisLayer.Split(new string[] { "^" }, StringSplitOptions.None);
                for (int y = 0; y < rows.Length; y++)
                {
                    rows[y] = rows[y].Trim();
                    var tiles = rows[y].Split(new string[] { "||" }, StringSplitOptions.None);
                    for (int x = 0; x < tiles.Length; x++)
                    {
                        var prefab = GetTilePrefab(tiles[x].Trim());

                        if (prefab == null || tiles[x] == "null")
                            continue;

                        var tile = Instantiate(prefab.gameObject, layerParents[curLayer]).GetComponent<Tile>();
                        tile.gameObject.name = prefab.gameObject.name;
                        tile.transform.position = GridDisplay.Inst.GridToWorld(new Vector2Int(x, y));
                        grids[curLayer][x, y] = tile;
                    }
                }
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
        public Tile GetTilePrefab(Tile tile)
        {
            if (tile == null)
                return null;

            Tile result;

            result = Array.Find(tilePrefabs_BG, i => i.gameObject.name == tile.gameObject.name);
            if (result != null)
                return result;

            result = Array.Find(tilePrefabs_Main, i => i.gameObject.name == tile.gameObject.name);
            if (result != null)
                return result;

            result = Array.Find(tilePrefabs_FG, i => i.gameObject.name == tile.gameObject.name);
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

