using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Dongjun.LevelEditor
{
    public class MapLayerUI : LevelEditorComponent
    {
        [SerializeField] private Transform buttonGroup;
        [SerializeField] private MapLayerButton buttonPrefab;

        [SerializeField] private TileInventoryUI tileInventoryUI;

        private MapLayerButton curSelected = null;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            for (int i = 0; i < Enum.GetValues(typeof(MapLayer)).Length; i++)
            {
                MapLayer layer = (MapLayer)i;

                var button = Instantiate(buttonPrefab.gameObject, buttonGroup).GetComponent<MapLayerButton>();
                button.gameObject.name = layer.ToString();
                button.GetComponentInChildren<Text>().text = button.gameObject.name;
                button.Init(
                    () =>
                    {
                        curSelected?.OnSelected(false);
                        curSelected = button;
                        curSelected.OnSelected(true);
                        data.SetCurLayer(layer);
                        tileInventoryUI.ShowCurrentUI(layer);
                    },
                    () =>
                    {
                        if (data.IsLayerVisable(layer))
                        {
                            button.ShowLayer(false);
                            data.HideLayer(layer);
                        }
                        else
                        {
                            button.ShowLayer(true);
                            data.ShowLayer(layer);
                        }
                    });

                if (layer == MapLayer.Main)
                {
                    curSelected = button;
                    curSelected.OnSelected(true);
                    data.SetCurLayer(layer);
                    tileInventoryUI.ShowCurrentUI(layer);
                }
            }
        }
    }
}

