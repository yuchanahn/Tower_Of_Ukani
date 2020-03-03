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
            if (!ui.IsStarted)
                return;

            // UI
            ui.ToggleTileSlots();
            ui.SelectHotbarItem();

            // Save
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S))
                data.SaveTo(data.CurSaveLocation);

            if (!ui.IsInventoryOpened)
            {
                // Camera
                camController.Scroll();
                camController.Pan();

                // Edit Tile
                if (!camController.IsPanning)
                    tilePlacement.UserEdit();
            }
        }
    }
}
