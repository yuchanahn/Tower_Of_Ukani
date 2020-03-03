using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Dongjun.LevelEditor
{
    public static class TilePlacementExtension
    {
        private const int MAX_UNDO_REDO = 1000;

        public static TilePlacement.TilePlacementData NewTilePlacementData(this LevelEditorData data, Vector2Int pos, Tile newTilePrefab)
        {
            return new TilePlacement.TilePlacementData()
            {
                pos = pos,
                newTilePrefab = newTilePrefab,
                prevTilePrefab = data.GetTilePrefab(data.Grid[pos.x, pos.y])
            };
        }

        public static List<TilePlacement.TilePlacementData> Pop(this List<List<TilePlacement.TilePlacementData>> list)
        {
            List<TilePlacement.TilePlacementData> result = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            return result;
        }
        public static void Push(this List<List<TilePlacement.TilePlacementData>> list, List<TilePlacement.TilePlacementData> data)
        {
            list.Add(data);

            if (list.Count > MAX_UNDO_REDO)
                list.RemoveAt(0);
        }
    }

    public class TilePlacement : LevelEditorComponent
    {
        public struct TilePlacementData
        {
            public Vector2Int pos;
            public Tile newTilePrefab;
            public Tile prevTilePrefab;
        }
        private enum PlacementMode
        {
            Do,
            Undo
        }

        private List<List<TilePlacementData>> undoList = new List<List<TilePlacementData>>();
        private List<List<TilePlacementData>> redoList = new List<List<TilePlacementData>>();

        private Camera mainCam;

        protected override void Awake()
        {
            base.Awake();
            mainCam = Camera.main;
        }

        private void SpawnTile(Vector2Int pos, Tile prefab)
        {
            data.Grid[pos.x, pos.y] = prefab.Spawn(data.LayerParent, Vector2.zero);
            data.Grid[pos.x, pos.y].gameObject.name = prefab.gameObject.name;
            data.Grid[pos.x, pos.y].transform.position = GridDisplay.Inst.GridToWorld(pos) + ((prefab.size - Vector2.one) * 0.5f);
        }
        private void DestroyTile(Vector2Int pos)
        {
            data.Grid[pos.x, pos.y].Sleep();
            data.Grid[pos.x, pos.y] = null;
        }

        private void EditTile(in TilePlacementData placementData, in PlacementMode mode)
        {
            if (placementData.newTilePrefab != null)
            {
                if (data.Grid[placementData.pos.x, placementData.pos.y] != null)
                    return;

                SpawnTile(placementData.pos, placementData.newTilePrefab);
            }
            else
            {
                if (data.Grid[placementData.pos.x, placementData.pos.y] == null)
                    return;

                DestroyTile(placementData.pos);
            }

            TilePlacementData reverseData = placementData;
            reverseData.newTilePrefab = placementData.prevTilePrefab;
            reverseData.prevTilePrefab = placementData.newTilePrefab;

            (mode == PlacementMode.Do ? undoList : redoList).Push(new List<TilePlacementData>() { reverseData });
        }
        private void EditTile(in List<TilePlacementData> placementData, in PlacementMode mode)
        {
            List<TilePlacementData> actionData = new List<TilePlacementData>();

            foreach (var curData in placementData)
            {
                if (curData.newTilePrefab != null)
                {
                    if (data.Grid[curData.pos.x, curData.pos.y] != null)
                        continue;

                    SpawnTile(curData.pos, curData.newTilePrefab);
                }
                else
                {
                    if (data.Grid[curData.pos.x, curData.pos.y] == null)
                        continue;

                    DestroyTile(curData.pos);
                }

                TilePlacementData reverseData = curData;
                reverseData.newTilePrefab = curData.prevTilePrefab;
                reverseData.prevTilePrefab = curData.newTilePrefab;

                actionData.Insert(0, reverseData);
            }

            if (actionData.Count == 0)
                return;

            (mode == PlacementMode.Do ? undoList : redoList).Push(actionData);
        }
        private void EditTile_OnlyPlaceTile(in TilePlacementData placementData)
        {
            if (placementData.newTilePrefab != null)
            {
                if (data.Grid[placementData.pos.x, placementData.pos.y] == null)
                    SpawnTile(placementData.pos, placementData.newTilePrefab);
            }
            else
            {
                if (data.Grid[placementData.pos.x, placementData.pos.y] != null)
                    DestroyTile(placementData.pos);
            }
        }
        private void EditTile_OnlyPlaceTile(in List<TilePlacementData> placementData)
        {
            foreach (var curData in placementData)
            {
                if (curData.newTilePrefab != null)
                {
                    if (data.Grid[curData.pos.x, curData.pos.y] != null)
                        continue;

                    SpawnTile(curData.pos, curData.newTilePrefab);
                }
                else
                {
                    if (data.Grid[curData.pos.x, curData.pos.y] == null)
                        continue;

                    DestroyTile(curData.pos);
                }
            }
        }
        private void EditTile_OnlyRecordData(in List<TilePlacementData> placementData, in PlacementMode mode)
        {
            List<TilePlacementData> actionData = new List<TilePlacementData>();

            foreach (var curData in placementData)
            {
                TilePlacementData reverseData = curData;
                reverseData.newTilePrefab = curData.prevTilePrefab;
                reverseData.prevTilePrefab = curData.newTilePrefab;

                actionData.Insert(0, reverseData);
            }
                
            (mode == PlacementMode.Do ? undoList : redoList).Push(actionData);
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
        private void FillTile(Vector2Int pos, Tile fillWith)
        {
            string replaceTile = data.Grid[pos.x, pos.y]?.gameObject.name ?? null;

            if (IsSameTile(fillWith, replaceTile))
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
                    TilePlacementData tilePlacementData = data.NewTilePlacementData(_pos, null); 
                    actionData.Add(tilePlacementData);
                    EditTile_OnlyPlaceTile(tilePlacementData);
                }
                if (fillWith != null)
                {
                    TilePlacementData tilePlacementData = data.NewTilePlacementData(_pos, fillWith);
                    actionData.Add(tilePlacementData);
                    EditTile_OnlyPlaceTile(tilePlacementData);
                }
            }
            bool CanbeFilled(int x, int y) => IsSameTile(x, y, replaceTile);
            void AddToSnakeStartPos(Vector2Int _pos)
            {
                if (CanbeFilled(_pos.x, _pos.y) && !snakeStartPos.Contains(_pos))
                    snakeStartPos.Add(_pos);
            }

            // Snake Fill Algorithm
            void SnakeFill(in Vector2Int startPos, in Dir startDir = Dir.Up)
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

            List<TilePlacementData> actionData = new List<TilePlacementData>();

            TilePlacementData removeData = data.NewTilePlacementData(pos, null);
            actionData.Add(removeData);
            EditTile_OnlyPlaceTile(removeData);

            TilePlacementData setData = data.NewTilePlacementData(pos, data.CurTile);
            actionData.Add(setData);
            EditTile_OnlyPlaceTile(setData);

            EditTile_OnlyRecordData(actionData, PlacementMode.Do);
        }
        private void AddTile(Vector2Int pos)
        {
            if (data.CurTile == null)
                return;

            EditTile(data.NewTilePlacementData(pos, data.CurTile), PlacementMode.Do);
        }
        private void RemoveTile(Vector2Int pos)
        {
            EditTile(data.NewTilePlacementData(pos, null), PlacementMode.Do);
        }

        private void Undo()
        {
            if (undoList.Count == 0)
                return;

            EditTile(undoList.Pop(), PlacementMode.Undo);
        }
        private void Redo()
        {
            if (redoList.Count == 0)
                return;

            EditTile(redoList.Pop(), PlacementMode.Do);
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

            if (Input.GetKey(KeyCode.F))
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                    FillTile(pos, data.CurTile);
                else if (Input.GetKeyDown(KeyCode.Mouse1))
                    FillTile(pos, null);

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

