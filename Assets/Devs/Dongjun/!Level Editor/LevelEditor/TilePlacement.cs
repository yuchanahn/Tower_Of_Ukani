using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        private enum PlacementMode
        {
            Do,
            Undo
        }
        private readonly int MaxUndoRedo = 1000;
        private List<List<TilePlacementData>> undoList = new List<List<TilePlacementData>>();
        private List<List<TilePlacementData>> redoList = new List<List<TilePlacementData>>();

        private Camera mainCam;

        protected override void Awake()
        {
            base.Awake();
            mainCam = Camera.main;
        }

        private void _SpawnTile(Vector2Int pos, Tile prefab)
        {
            data.Grid[pos.x, pos.y] = prefab.Spawn(data.LayerParent, Vector2.zero);
            data.Grid[pos.x, pos.y].transform.position = GridDisplay.Inst.GridToWorld(pos);
            data.Grid[pos.x, pos.y].gameObject.name = prefab.gameObject.name;
        }
        private void _DestroyTile(Vector2Int pos)
        {
            data.Grid[pos.x, pos.y].Sleep();
            data.Grid[pos.x, pos.y] = null;
        }
        private void TileEdit(in TilePlacementData placementData, in PlacementMode mode)
        {
            if (placementData.prefab != null)
            {
                if (data.Grid[placementData.pos.x, placementData.pos.y] != null)
                    return;

                Push(mode == PlacementMode.Do ? undoList : redoList, new List<TilePlacementData>() { new TilePlacementData(placementData.pos, null) });
                _SpawnTile(placementData.pos, placementData.prefab);
            }
            else
            {
                if (data.Grid[placementData.pos.x, placementData.pos.y] == null)
                    return;

                Push(mode == PlacementMode.Do ? undoList : redoList,
                    new List<TilePlacementData>() { 
                        new TilePlacementData(placementData.pos, data.GetTilePrefab(data.Grid[placementData.pos.x, placementData.pos.y].gameObject.name)) });
                _DestroyTile(placementData.pos);
            }
        }
        private void TileEdit(in List<TilePlacementData> placementData, in PlacementMode mode)
        {
            List<TilePlacementData> actionData = new List<TilePlacementData>();

            foreach (var curData in placementData)
            {
                if (curData.prefab != null)
                {
                    if (data.Grid[curData.pos.x, curData.pos.y] != null)
                        continue;

                    actionData.Add(new TilePlacementData(curData.pos, null));
                    _SpawnTile(curData.pos, curData.prefab);
                }
                else
                {
                    if (data.Grid[curData.pos.x, curData.pos.y] == null)
                        continue;

                    actionData.Add(new TilePlacementData(curData.pos, data.GetTilePrefab(data.Grid[curData.pos.x, curData.pos.y].gameObject.name)));
                    _DestroyTile(curData.pos);
                }
            }

            if (actionData.Count == 0)
                return;

            Push(mode == PlacementMode.Do ? undoList : redoList, actionData.Reverse<TilePlacementData>().ToList());
        }
        private void TileEdit_OnlyPlacement(in TilePlacementData placementData)
        {
            if (placementData.prefab != null)
            {
                if (data.Grid[placementData.pos.x, placementData.pos.y] != null)
                    return;

                _SpawnTile(placementData.pos, placementData.prefab);
            }
            else
            {
                if (data.Grid[placementData.pos.x, placementData.pos.y] == null)
                    return;

                _DestroyTile(placementData.pos);
            }
        }
        private void TileEdit_OnlyPlacement(in List<TilePlacementData> placementData)
        {
            foreach (var curData in placementData)
            {
                if (curData.prefab != null)
                {
                    if (data.Grid[curData.pos.x, curData.pos.y] != null)
                        continue;

                    _SpawnTile(curData.pos, curData.prefab);
                }
                else
                {
                    if (data.Grid[curData.pos.x, curData.pos.y] == null)
                        continue;

                    _DestroyTile(curData.pos);
                }
            }
        }
        private void TileEdit_OnlyRecordUndoRedo(in List<TilePlacementData> placementData, in PlacementMode mode)
        {
            List<TilePlacementData> actionData = new List<TilePlacementData>();

            foreach (var curData in placementData)
            {
                if (curData.prefab != null)
                {
                    actionData.Add(new TilePlacementData(curData.pos, null));
                }
                else
                {
                    actionData.Add(new TilePlacementData(curData.pos, data.GetTilePrefab(data.Grid[curData.pos.x, curData.pos.y].gameObject.name)));
                }
            }

            Push(mode == PlacementMode.Do ? undoList : redoList, actionData.Reverse<TilePlacementData>().ToList());
        }

        private bool IsValidMousePos(out Vector2Int pos)
        {
            Vector2 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            pos = GridDisplay.Inst.WorldToGrid(mousePos);

            if (0 > pos.x || 0 > pos.y || data.Grid.GetLength(0) - 1 < pos.x || data.Grid.GetLength(1) - 1 < pos.y)
                return false;

            return true;
        }
        public void EditTile()
        {
            if (!data.IsLayerVisable(data.Layer)
                || EventSystem.current.IsPointerOverGameObject()
                || EventSystem.current.currentSelectedGameObject != null)
                return;

            if (FloodFill()) return;
            if (ReplaceTile()) return;
            if (AddTile()) return;
            if (RemoveTile()) return;
        }

        private bool FloodFill()
        {
            if (!Input.GetKey(KeyCode.F) || !Input.GetKeyDown(KeyCode.Mouse0))
                return false;

            if (!IsValidMousePos(out var pos))
                return false;

            bool IsSameTile(int x, int y, string tileName)
            {
                if (tileName == null)
                    return data.Grid[x, y] == null;

                return data.Grid[x, y] != null && data.Grid[x, y].gameObject.name == tileName;
            }

            string targetTileName = data.Grid[pos.x, pos.y]?.gameObject.name ?? null;
            if (targetTileName == (data.CurTile?.gameObject.name ?? null))
                return false;

            List<TilePlacementData> actionData = new List<TilePlacementData>();

            Stack<Vector2Int> points = new Stack<Vector2Int>();
            points.Push(pos);

            while (points.Count != 0)
            {
                Vector2Int temp = points.Pop();
                int y1 = temp.y;
                while (y1 >= 0 && IsSameTile(temp.x, y1, targetTileName))
                {
                    y1--;
                }

                y1++;
                bool spanLeft = false;
                bool spanRight = false;
                while (y1 < data.MapData.MaxSizeY && IsSameTile(temp.x, y1, targetTileName))
                {
                    TilePlacementData placementData = new TilePlacementData(new Vector2Int(temp.x, y1), data.CurTile);
                    actionData.Add(placementData);
                    TileEdit_OnlyPlacement(placementData);

                    if (!spanLeft && temp.x > 0 && IsSameTile(temp.x - 1, y1, targetTileName))
                    {
                        points.Push(new Vector2Int(temp.x - 1, y1));
                        spanLeft = true;
                    }
                    else if (spanLeft && temp.x - 1 == 0 && !IsSameTile(temp.x - 1, y1, targetTileName))
                    {
                        spanLeft = false;
                    }

                    if (!spanRight && temp.x < data.MapData.MaxSizeX - 1 && IsSameTile(temp.x + 1, y1, targetTileName))
                    {
                        points.Push(new Vector2Int(temp.x + 1, y1));
                        spanRight = true;
                    }
                    else if (spanRight && temp.x < data.MapData.MaxSizeX - 1 && !IsSameTile(temp.x + 1, y1, targetTileName))
                    {
                        spanRight = false;
                    }
                    y1++;
                }
            }

            TileEdit_OnlyRecordUndoRedo(actionData, PlacementMode.Do);
            return true;
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

            TileEdit(new List<TilePlacementData>() 
            { 
                new TilePlacementData(pos, null),
                new TilePlacementData(pos, data.CurTile) 
            }, PlacementMode.Do);
            return true;
        }
        private bool AddTile()
        {
            if (!Input.GetKey(KeyCode.Mouse0))
                return false;

            if (!IsValidMousePos(out var pos) || data.CurTile == null)
                return false;

            TileEdit(new TilePlacementData(pos, data.CurTile), PlacementMode.Do);
            return true;
        }
        private bool RemoveTile()
        {
            if (!Input.GetKey(KeyCode.Mouse1))
                return false;

            if (!IsValidMousePos(out var pos))
                return false;

            TileEdit(new TilePlacementData(pos, null), PlacementMode.Do);
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

                TileEdit(Pop(undoList), PlacementMode.Undo);
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

                TileEdit(Pop(redoList), PlacementMode.Do);
            }
        }

        private List<TilePlacementData> Pop(List<List<TilePlacementData>> list)
        {
            List<TilePlacementData> result = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            return result;
        }
        private void Push(List<List<TilePlacementData>> list, List<TilePlacementData> data)
        {
            list.Add(data);

            if (list.Count > MaxUndoRedo)
                list.RemoveAt(0);
        }
    }
}

