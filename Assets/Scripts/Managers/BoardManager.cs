using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance;
    public int cellSize;
    public int cellCount;
    private Transform[] cellTransforms;

    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        instance = this;
        // 初始化cellTransforms数组
        cellTransforms = new Transform[cellCount];
        for (int i = 0; i < cellCount; i++)
        {
            cellTransforms[i] = transform.GetChild(i);
        }
        Debug.Log("cellTransforms: " + cellTransforms);
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public bool AddCard(Transform cardTransform)
    {
        int cellIdx = GetCellIdxByPointer();
        if (cellIdx == -1)
        {
            return false;
        }
        cardTransform.SetParent(cellTransforms[cellIdx]);
        cardTransform.localPosition = Vector3.zero;
        return true;
    }

    public int GetCellIdxByPointer()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return GetCellIdxByPosition(mousePosition);
    }

    public int GetCellIdxByPosition(Vector3 position)
    {
        float x = position.x;
        float y = position.y;
        float halfCellSize = 0.5f * cellSize;
        float minX = cellTransforms[0].position.x - halfCellSize;
        float maxX = cellTransforms[cellCount - 1].position.x + halfCellSize;
        float minY = cellTransforms[cellCount - 1].position.y - halfCellSize;
        float maxY = cellTransforms[0].position.y + halfCellSize;
        if (x < minX || x > maxX || y < minY || y > maxY)
        {
            return -1;
        }
        int row = (int)((maxY - y) / (maxY - minY) * cellSize);
        int col = (int)((x - minX) / (maxX - minX) * cellSize);

        return row * cellSize + col;
    }
}