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
            data.Grid[pos.x, pos.y].gameObject.name = prefab.gameObject.name;
            data.Grid[pos.x, pos.y].transform.position = GridDisplay.Inst.GridToWorld(pos);
        }
        private void _DestroyTile(Vector2Int pos)
        {
            data.Grid[pos.x, pos.y].Sleep();
            data.Grid[pos.x, pos.y] = null;
        }
        private List<TilePlacementData> _Pop(List<List<TilePlacementData>> list)
        {
            List<TilePlacementData> result = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            return result;
        }
        private void _Push(List<List<TilePlacementData>> list, List<TilePlacementData> data)
        {
            list.Add(data);

            if (list.Count > MaxUndoRedo)
                list.RemoveAt(0);
        }

        private void EditTile(in TilePlacementData placementData, in PlacementMode mode)
        {
            if (placementData.prefab != null)
            {
                if (data.Grid[placementData.pos.x, placementData.pos.y] != null)
                    return;

                _Push(mode == PlacementMode.Do ? undoList : redoList, new List<TilePlacementData>() { new TilePlacementData(placementData.pos, null) });
                _SpawnTile(placementData.pos, placementData.prefab);
            }
            else
            {
                if (data.Grid[placementData.pos.x, placementData.pos.y] == null)
                    return;

                _Push(mode == PlacementMode.Do ? undoList : redoList,
                    new List<TilePlacementData>() 
                    { 
                        new TilePlacementData(placementData.pos, data.GetTilePrefab(data.Grid[placementData.pos.x, placementData.pos.y].gameObject.name)) 
                    });
                _DestroyTile(placementData.pos);
            }
        }
        private void EditTile(in List<TilePlacementData> placementData, in PlacementMode mode)
        {
            List<TilePlacementData> actionData = new List<TilePlacementData>();

            foreach (var curData in placementData)
            {
                if (curData.prefab != null)
                {
                    if (data.Grid[curData.pos.x, curData.pos.y] != null)
                        continue;

                    actionData.Insert(0, new TilePlacementData(curData.pos, null));
                    _SpawnTile(curData.pos, curData.prefab);
                }
                else
                {
                    if (data.Grid[curData.pos.x, curData.pos.y] == null)
                        continue;

                    actionData.Insert(0, new TilePlacementData(curData.pos, data.GetTilePrefab(data.Grid[curData.pos.x, curData.pos.y].gameObject.name)));
                    _DestroyTile(curData.pos);
                }
            }

            if (actionData.Count == 0)
                return;

            _Push(mode == PlacementMode.Do ? undoList : redoList, actionData);
        }
        private void EditTile_OnlyPlaceTile(in TilePlacementData placementData)
        {
            if (placementData.prefab != null)
            {
                if (data.Grid[placementData.pos.x, placementData.pos.y] == null)
                    _SpawnTile(placementData.pos, placementData.prefab);
            }
            else
            {
                if (data.Grid[placementData.pos.x, placementData.pos.y] != null)
                    _DestroyTile(placementData.pos);
            }
        }
        private void EditTile_OnlyPlaceTile(in List<TilePlacementData> placementData)
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
        private void EditTile_OnlyRecordData(in List<TilePlacementData> placementData, in PlacementMode mode)
        {
            List<TilePlacementData> actionData = new List<TilePlacementData>();

            foreach (var curData in placementData)
               actionData.Insert(0,
                   new TilePlacementData(curData.pos, curData.prefab != null ? null : data.GetTilePrefab(data.Grid[curData.pos.x, curData.pos.y].gameObject.name)));

            _Push(mode == PlacementMode.Do ? undoList : redoList, actionData);
        }

        private bool IsSameTile(Tile tile, string tileName)
        {
            if (tileName == null)
                return tile == null;

            return tile != null && tile.gameObject.name == tileName;
        }
        private bool IsSameTile(int x, int y, string tileName)
        {
            if (x < 0 || y < 0 || x >= data.MapData.MaxSizeX || y >= data.MapData.MaxSizeY)
                return false;

            if (tileName == null)
                return data.Grid[x, y] == null;

            return data.Grid[x, y] != null && data.Grid[x, y].gameObject.name == tileName;
        }
        private bool IsSameTile(Vector2Int pos, string tileName)
        {
            if (pos.x < 0 || pos.y < 0 || pos.x >= data.MapData.MaxSizeX || pos.y >= data.MapData.MaxSizeY)
                return false;

            if (tileName == null)
                return data.Grid[pos.x, pos.y] == null;

            return data.Grid[pos.x, pos.y] != null && data.Grid[pos.x, pos.y].gameObject.name == tileName;
        }

        private enum Dir { Up, Down, Right, Left }
        private void FillTile(Vector2Int pos)
        {
            string replaceTile = data.Grid[pos.x, pos.y]?.gameObject.name ?? null;

            if (IsSameTile(data.CurTile, replaceTile))
                return;

            List<TilePlacementData> actionData = new List<TilePlacementData>();
            List<Vector2Int> snakeStartPos = new List<Vector2Int>();

            void Fill(Vector2Int _pos)
            {
                // Remove From Snake Start Pos
                if (snakeStartPos.Contains(_pos))
                    snakeStartPos.Remove(_pos);

                if (replaceTile != null)
                {
                    actionData.Add(new TilePlacementData(_pos, null));
                    EditTile_OnlyPlaceTile(new TilePlacementData(_pos, null));
                }
                if (data.CurTile != null)
                {
                    actionData.Add(new TilePlacementData(_pos, data.CurTile));
                    EditTile_OnlyPlaceTile(new TilePlacementData(_pos, data.CurTile));
                }
            }
            bool CanbeFilled(int x, int y) => IsSameTile(x, y, replaceTile);
            void AddToSnakeStartPos(Vector2Int _pos)
            {
                if (CanbeFilled(_pos.x, _pos.y) && !snakeStartPos.Contains(_pos))
                    snakeStartPos.Add(_pos);
            }

            // Snake Fill Algorithm
            void SnakeFill(Vector2Int startPos, Dir startDir = Dir.Up)
            {
                if (!CanbeFilled(startPos.x, startPos.y))
                    return;

                // Init
                Vector2Int curPos = startPos;
                Vector2Int dir = new Vector2Int();
                switch (startDir)
                {
                    case Dir.Up: dir = new Vector2Int(0, 1);
                        break;
                    case Dir.Down: dir = new Vector2Int(0, -1);
                        break;
                    case Dir.Right: dir = new Vector2Int(1, 0);
                        break;
                    case Dir.Left: dir = new Vector2Int(-1, 0);
                        break;
                }

                // Fill Tile At Staring Pos
                Fill(startPos);

                // Loop
                while (true)
                {
                    // Add Empty Pos to Snake Start Pos
                    if (dir.x == 0)
                    {
                        AddToSnakeStartPos(new Vector2Int(curPos.x + 1, curPos.y));
                        AddToSnakeStartPos(new Vector2Int(curPos.x - 1, curPos.y));
                    }
                    else if (dir.y == 0)
                    {
                        AddToSnakeStartPos(new Vector2Int(curPos.x, curPos.y + 1));
                        AddToSnakeStartPos(new Vector2Int(curPos.x, curPos.y - 1));
                    }

                    // Check Wall
                    if (!CanbeFilled(curPos.x + dir.x, curPos.y + dir.y))
                    {
                        // Up
                        if (CanbeFilled(curPos.x, curPos.y + 1))
                            dir = new Vector2Int(0, 1);
                        // Down
                        else if (CanbeFilled(curPos.x, curPos.y - 1))
                            dir = new Vector2Int(0, -1);
                        // Right
                        else if (CanbeFilled(curPos.x + 1, curPos.y))
                            dir = new Vector2Int(1, 0);
                        // Left
                        else if (CanbeFilled(curPos.x - 1, curPos.y))
                            dir = new Vector2Int(-1, 0);
                        // Stuck
                        else
                            break;
                    }

                    // Set Current Pos
                    curPos += dir;

                    // Fill Tile
                    Fill(curPos);
                }
            }

            // Fill Clicked Pos
            Fill(pos);

            // Start Initial 4 Way Snake
            SnakeFill(new Vector2Int(pos.x, pos.y + 1), Dir.Up);
            SnakeFill(new Vector2Int(pos.x, pos.y - 1), Dir.Down);
            SnakeFill(new Vector2Int(pos.x + 1, pos.y), Dir.Right);
            SnakeFill(new Vector2Int(pos.x - 1, pos.y), Dir.Left);

            // Start Other Snake
            while (snakeStartPos.Count != 0)
                SnakeFill(snakeStartPos[0]);

            // Record Undo
            EditTile_OnlyRecordData(actionData, PlacementMode.Do);
        }
        private void ReplaceTile(Vector2Int pos)
        {
            if (data.Grid[pos.x, pos.y] == null || data.CurTile == null)
                return;

            if (IsSameTile(pos, data.CurTile.gameObject.name))
                return;

            EditTile(
                new List<TilePlacementData>() 
                { 
                    new TilePlacementData(pos, null),
                    new TilePlacementData(pos, data.CurTile) 
                },
                PlacementMode.Do);
        }
        private void AddTile(Vector2Int pos)
        {
            if (data.CurTile == null)
                return;

            EditTile(new TilePlacementData(pos, data.CurTile), PlacementMode.Do);
        }
        private void RemoveTile(Vector2Int pos)
        {
            EditTile(new TilePlacementData(pos, null), PlacementMode.Do);
        }

        private void Undo()
        {
            if (undoList.Count == 0)
                return;

            EditTile(_Pop(undoList), PlacementMode.Undo);
        }
        private void Redo()
        {
            if (redoList.Count == 0)
                return;

            EditTile(_Pop(redoList), PlacementMode.Do);
        }

        private bool IsValidMousePos(out Vector2Int pos)
        {
            Vector2 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            pos = GridDisplay.Inst.WorldToGrid(mousePos);

            if (0 > pos.x || 0 > pos.y || data.Grid.GetLength(0) - 1 < pos.x || data.Grid.GetLength(1) - 1 < pos.y)
                return false;

            return true;
        }

        public void UserEdit()
        {
            if (!data.IsLayerVisable(data.Layer)
                || EventSystem.current.IsPointerOverGameObject()
                || EventSystem.current.currentSelectedGameObject != null)
                return;

            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (Input.GetKeyDown(KeyCode.Z) || (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Z)))
                {
                    Undo();
                    return;
                }

                if (Input.GetKeyDown(KeyCode.Y) || (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Y)))
                {
                    Redo();
                    return;
                }
            }

            if (!IsValidMousePos(out var pos))
                return;

            if (Input.GetKey(KeyCode.F) && Input.GetKeyDown(KeyCode.Mouse0))
            {
                FillTile(pos);
                return;
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Mouse0))
            {
                ReplaceTile(pos);
                return;
            }

            if (Input.GetKey(KeyCode.Mouse0))
            {
                AddTile(pos);
                return;
            }

            if (Input.GetKey(KeyCode.Mouse1))
            {
                RemoveTile(pos);
                return;
            }
        }
    }
}

