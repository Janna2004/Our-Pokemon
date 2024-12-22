using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public int cellSize;
    public int boardSize;
    public int CellCount => boardSize * boardSize;
    private Transform[] cellTransforms;

    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        cellTransforms = new Transform[CellCount];
        for (int i = 0; i < CellCount; i++)
        {
            cellTransforms[i] = transform.GetChild(i);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void UpdateCells(byte[,] mapData)
    {
        for (int x = 0; x < boardSize * 2; x++)
        {
            for (int y = 0; y < boardSize * 2; y++)
            {
                int cellX = x / 2;
                int cellY = y / 2;
                int cellIdx = cellX * boardSize + cellY;
                Transform cellTransform = cellTransforms[cellIdx];
                CellManager cellManager = cellTransform.GetComponent<CellManager>();
                if (mapData[x, y] == 1)
                {
                    cellManager.cellType = CellType.Grass;
                }
                else if (mapData[x, y] == 2)
                {
                    cellManager.cellType = CellType.Water;
                }
            }
        }
    }

    public int OnCardStartDrag(Transform cardTransform)
    {
        int cellIdx = GetCellIdxByPointer();
        return cellIdx;
    }

    public bool OnCardEndDrag(Transform cardTransform, int startCellIdx)
    {
        int cellIdx = GetCellIdxByPointer();
        if (cellIdx == -1)
        {
            return false;
        }
        if (!EmptyAtIdx(cellIdx))
        {
            return false;
        }
        if (startCellIdx != -1)
        {
            CellManager startCellManager = cellTransforms[startCellIdx].GetComponent<CellManager>();
            startCellManager.cellState = CellState.Empty;
        }
        return AddCard(cardTransform, cellIdx);
    }

    private bool AddCard(Transform cardTransform, int cellIdx)
    {
        if (cellIdx == -1 || !EmptyAtIdx(cellIdx))
        {
            return false;
        }
        cardTransform.SetParent(cellTransforms[cellIdx]);
        // 重置卡牌位置到单元格中心
        cardTransform.localPosition = Vector3.zero;
        CellManager cellManager = cellTransforms[cellIdx].GetComponent<CellManager>();
        cellManager.cellState = CellState.Occupied;
        return true;
    }

    private bool EmptyAtIdx(int cellIdx)
    {
        if (cellIdx == -1)
        {
            return false;
        }
        CellManager cellManager = cellTransforms[cellIdx].GetComponent<CellManager>();
        return cellManager.cellState == CellState.Empty;
    }

    private int GetCellIdxByPointer()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return GetCellIdxByPosition(mousePosition);
    }

    private int GetCellIdxByPosition(Vector3 position)
    {
        float x = position.x;
        float y = position.y;
        float halfCellSize = 0.5f * cellSize;
        float minX = cellTransforms[0].position.x - halfCellSize;
        float maxX = cellTransforms[CellCount - 1].position.x + halfCellSize;
        float minY = cellTransforms[CellCount - 1].position.y - halfCellSize;
        float maxY = cellTransforms[0].position.y + halfCellSize;
        if (x < minX || x > maxX || y < minY || y > maxY)
        {
            return -1;
        }
        int row = (int)((maxY - y) / cellSize);
        int col = (int)((x - minX) / cellSize);

        return row * boardSize + col;
    }

    // 禁用指定位置的单元格
    public void DisableCell(int cellIdx)
    {
        if (cellIdx == -1)
        {
            return;
        }
        CellManager cellManager = cellTransforms[cellIdx].GetComponent<CellManager>();
        cellManager.cellState = CellState.Blocked;
    }
}