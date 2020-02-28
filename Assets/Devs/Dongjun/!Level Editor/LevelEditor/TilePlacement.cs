using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Dongjun.LevelEditor
{
    public class TilePlacement : LevelEditorComponent
    {
        private struct TilePlacementData
        {
            public Vector2Int pos;
            public Tile prefab;

            public TilePlacementData(Vector2Int pos, Tile prefab)
            {
                this.pos = pos;
                this.prefab = prefab;
            }
        }
        private readonly int MaxUndoRedo = 1000;
        private List<TilePlacementData> undoList = new List<TilePlacementData>();
        private List<TilePlacementData> redoList = new List<TilePlacementData>();

        private Camera mainCam;

        protected override void Awake()
        {
            base.Awake();
            mainCam = Camera.main;
        }

        private bool IsValidMousePos(out Vector2Int pos)
        {
            Vector2 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            pos = GridDisplay.Inst.WorldToGrid(mousePos);

            if (0 > pos.x || 0 > pos.y || data.Grid.GetLength(0) - 1 < pos.x || data.Grid.GetLength(1) - 1 < pos.y)
                return false;

            return true;
        }
        private void TilePlacementLogic(TilePlacementData placementData, bool recordUndo = false)
        {
            if (placementData.prefab != null)
            {
                if (data.Grid[placementData.pos.x, placementData.pos.y] != null) return;

                if (recordUndo)
                    Push(undoList, new TilePlacementData(placementData.pos, null));
                else
                    Push(redoList, new TilePlacementData(placementData.pos, null));

                data.Grid[placementData.pos.x, placementData.pos.y] = Instantiate(placementData.prefab.gameObject, data.LayerParent).GetComponent<Tile>();
                data.Grid[placementData.pos.x, placementData.pos.y].transform.position = GridDisplay.Inst.GridToWorld(placementData.pos);
                data.Grid[placementData.pos.x, placementData.pos.y].gameObject.name = placementData.prefab.gameObject.name;
            }
            else
            {
                if (data.Grid[placementData.pos.x, placementData.pos.y] == null) return;

                if (recordUndo)
                    Push(undoList, new TilePlacementData(placementData.pos, data.GetTilePrefab(this.data.Grid[placementData.pos.x, placementData.pos.y].gameObject.name)));
                else
                    Push(redoList, new TilePlacementData(placementData.pos, data.GetTilePrefab(this.data.Grid[placementData.pos.x, placementData.pos.y].gameObject.name)));

                Destroy(data.Grid[placementData.pos.x, placementData.pos.y].gameObject);
                data.Grid[placementData.pos.x, placementData.pos.y] = null;
            }
        }

        public void EditTile()
        {
            if (!data.IsLayerVisable(data.Layer)
                || EventSystem.current.IsPointerOverGameObject()
                || EventSystem.current.currentSelectedGameObject != null)
                return;

            if (ReplaceTile()) return;
            if (AddTile()) return;
            if (RemoveTile()) return;
        }
        private bool ReplaceTile()
        {
            if (!Input.GetKey(KeyCode.LeftShift) || !Input.GetKey(KeyCode.Mouse0))
                return false;

            if (!IsValidMousePos(out var pos) || data.CurTile == null)
                return false;

            if (data.Grid[pos.x, pos.y] == null)
                return false;

            if (data.GetTilePrefab(data.Grid[pos.x, pos.y].gameObject.name) == data.CurTile)
                return false;

            TilePlacementLogic(new TilePlacementData(pos, null), true);
            TilePlacementLogic(new TilePlacementData(pos, data.CurTile), true);
            return true;
        }
        private bool AddTile()
        {
            if (!Input.GetKey(KeyCode.Mouse0))
                return false;

            if (!IsValidMousePos(out var pos) || data.CurTile == null)
                return false;

            TilePlacementLogic(new TilePlacementData(pos, data.CurTile), true);
            return true;
        }
        private bool RemoveTile()
        {
            if (!Input.GetKey(KeyCode.Mouse1))
                return false;

            if (!IsValidMousePos(out var pos))
                return false;

            TilePlacementLogic(new TilePlacementData(pos, null), true);
            return true;
        }

        public void Undo()
        {
            if (!Input.GetKey(KeyCode.LeftControl))
                return;

            if (Input.GetKeyDown(KeyCode.Z) || (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Z)))
            {
                if (undoList.Count == 0)
                    return;

                TilePlacementLogic(Pop(undoList));
            }
        }
        public void Redo()
        {

            if (!Input.GetKey(KeyCode.LeftControl))
                return;

            if (Input.GetKeyDown(KeyCode.Y) || (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Y)))
            {
                if (redoList.Count == 0)
                    return;

                TilePlacementLogic(Pop(redoList), true);
            }
        }

        private TilePlacementData Pop(List<TilePlacementData> list)
        {
            TilePlacementData result = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            return result;
        }
        private void Push(List<TilePlacementData> list, TilePlacementData data)
        {
            list.Add(data);

            if (list.Count > MaxUndoRedo)
                list.RemoveAt(0);
        }
    }
}

