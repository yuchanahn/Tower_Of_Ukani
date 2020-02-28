using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dongjun.LevelEditor 
{
    public class LevelEditorUI : LevelEditorComponent
    {
        [Header("Current State")]
        [SerializeField] private Text curTileName;

        [Header("Tile Inventory")]
        [SerializeField] private TileInventoryUI tileInventoryUI;

        [Header("Map Layer UI")]
        [SerializeField] private MapLayerUI mapLayerUI;

        private readonly int HotbarSize = 8;
        private Dictionary<MapLayer, List<TileItemUI>> hotbars = new Dictionary<MapLayer, List<TileItemUI>>();
        private Dictionary<MapLayer, TileItemUI> selectedItems = new Dictionary<MapLayer, TileItemUI>();

        private List<TileItemUI> CurHotbar => hotbars[data.Layer];
        private TileItemUI CurItem 
        { 
            get => selectedItems[data.Layer];
            set => selectedItems[data.Layer] = value;
        }
        public bool IsInventoryOpened
        { get; private set; } = false;

        private void Start()
        {
            SetCurTile(null);
            InitTileInventory();
            mapLayerUI.Init();
        }

        private void InitTileInventory()
        {
            tileInventoryUI.OnLayerChange(() => curTileName.text = CurItem == null ? "None" : CurItem.TilePrefab.gameObject.name);

            for (int i = 0; i < Enum.GetValues(typeof(MapLayer)).Length; i++)
            {
                var layer = (MapLayer)i;

                // Init Inventory UI
                tileInventoryUI.Init(layer);

                if (!selectedItems.ContainsKey(layer))
                    selectedItems.Add(layer, null);

                // Init Hotbar
                hotbars.Add(layer, new List<TileItemUI>());
                var curHotbar = hotbars[layer];

                foreach (var tilePrefab in data.TilePrefabDB[layer])
                {
                    var sprite = tilePrefab.icon;
                    var prefab = tilePrefab;

                    // Spawn Tile Item in Inventory
                    var tileItem = tileInventoryUI.SpawnInventoryItem(layer);
                    tileItem.InitData(sprite, prefab, () =>
                    {
                        // Check Condition
                        if (curHotbar.Count == HotbarSize || curHotbar.FindIndex(item => item != null ? item.TilePrefab == tileItem.TilePrefab : false) != -1)
                            return;

                        // Spawn Hotbar Item
                        var hotbarItem = tileInventoryUI.SpawnHotbarItem(layer);
                        curHotbar.Add(hotbarItem);
                        hotbarItem.InitData(sprite, prefab, () =>
                        {
                            Destroy(hotbarItem.gameObject);
                            curHotbar.Remove(hotbarItem);
                            SetCurTile(CurItem);
                        });

                        // Set Current Tile
                        if (CurItem == null) SetCurTile(hotbarItem);
                    });
                }
            }
        }

        private void SetCurTile(TileItemUI tileItem)
        {
            if (tileItem == null || hotbars[data.Layer].Count == 0)
            {
                CurItem = null;
                data.SetCurTilePrefab(null);
            }
            else
            {
                CurItem = tileItem;
                data.SetCurTilePrefab(tileItem.TilePrefab);
            }

            // Set Text
            curTileName.text = tileItem == null ? "None" : tileItem.TilePrefab.gameObject.name;
        }

        public void ToggleTileSlots()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                IsInventoryOpened = !tileInventoryUI.gameObject.activeSelf;
                tileInventoryUI.gameObject.SetActive(IsInventoryOpened);

                foreach (var item in CurHotbar)
                {
                    if (item == null)
                        continue;

                    if (tileInventoryUI.gameObject.activeSelf)
                    {
                        item.OnClick(() =>
                        {
                            Destroy(item.gameObject);
                            CurHotbar.Remove(item);
                            SetCurTile(null);
                        });
                    }
                    else
                    {
                        item.OnClick(() =>
                        {
                            SetCurTile(item);
                        });
                    }
                }
            }
        }
        public void SelectHotbarItem()
        {
            foreach (KeyCode keyCode in new KeyCode[]
            {
                KeyCode.Alpha1,
                KeyCode.Alpha2,
                KeyCode.Alpha3,
                KeyCode.Alpha4,
                KeyCode.Alpha5,
                KeyCode.Alpha6,
                KeyCode.Alpha7,
                KeyCode.Alpha8,
            })
            {
                if (!Input.GetKeyDown(keyCode))
                    continue;

                if (int.TryParse(keyCode.ToString().Replace("Alpha", ""), out var index))
                {
                    SetCurTile(CurHotbar[index - 1]);
                    break;
                }
            }
        }
    }
}

