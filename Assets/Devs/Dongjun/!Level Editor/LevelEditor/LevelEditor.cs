using UnityEngine;
using UnityEngine.EventSystems;

namespace Dongjun.LevelEditor
{
    public class LevelEditor : MonoBehaviour
    {
        private LevelEditorData data;
        private TilePlacement tilePlacement;
        private LevelEditorUI ui;
        private CamController camController;

        private void Awake()
        {
            data = GetComponent<LevelEditorData>();
            ui = GetComponent<LevelEditorUI>();
            tilePlacement = GetComponent<TilePlacement>();
            camController = FindObjectOfType<CamController>();
            camController.cam.transform.position = new Vector3(data.MapData.MaxSizeX * 0.5f, data.MapData.MaxSizeY * 0.5f, camController.cam.transform.position.z);
        }

        private void Update()
        {
            ui.ToggleTileSlots();
            ui.SelectHotbarItem();

            if (!ui.IsInventoryOpened)
            {
                camController.Scroll();
                camController.Pan();

                tilePlacement.Undo();
                tilePlacement.Redo();

                if (!camController.IsPanning)
                {
                    tilePlacement.EditTile();
                }
            }
        }
    }
}
