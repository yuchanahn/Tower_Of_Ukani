using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dongjun.LevelEditor
{
    public class TileInventoryUI : MonoBehaviour
    {
        [Header("Inventory")]
        [SerializeField] private Transform inventoryParent;
        [SerializeField] private Transform inventoryGroupPrefab;

        [Header("Hotbar")]
        [SerializeField] private Transform hotbarParent;
        [SerializeField] private Transform hotbarGroupPrefab;

        [Header("Item")]
        [SerializeField] private TileItemUI tileItem;

        private Dictionary<MapLayer, Transform> inventoryGroups = new Dictionary<MapLayer, Transform>();
        private Dictionary<MapLayer, Transform> hotbarGroups = new Dictionary<MapLayer, Transform>();
        private MapLayer curLayer = MapLayer.Main;

        private Action onLayerChange;

        public TileItemUI TileItemUI => tileItem;

        public void Init(MapLayer layer)
        {
            inventoryGroups.Add(layer, Instantiate(inventoryGroupPrefab.gameObject, inventoryParent).transform);
            hotbarGroups.Add(layer, Instantiate(hotbarGroupPrefab.gameObject, hotbarParent).transform);

            if (layer != curLayer)
            {
                inventoryGroups[layer].gameObject.SetActive(false);
                hotbarGroups[layer].gameObject.SetActive(false);
            }
        }
        public void OnLayerChange(Action onLayerChange)
        {
            this.onLayerChange = onLayerChange;
        }

        public TileItemUI SpawnInventoryItem(MapLayer layer)
        {
            return Instantiate(TileItemUI.gameObject, inventoryGroups[layer]).GetComponent<TileItemUI>();
        }
        public TileItemUI SpawnHotbarItem(MapLayer layer)
        {
            return Instantiate(TileItemUI.gameObject, hotbarGroups[layer]).GetComponent<TileItemUI>();
        }

        public void ShowCurrentUI(MapLayer layer)
        {
            inventoryGroups[curLayer].gameObject.SetActive(false);
            hotbarGroups[curLayer].gameObject.SetActive(false);

            curLayer = layer;
            onLayerChange.Invoke();

            inventoryGroups[curLayer].gameObject.SetActive(true);
            hotbarGroups[curLayer].gameObject.SetActive(true);
        }
    }
}

