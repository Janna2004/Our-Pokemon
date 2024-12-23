using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoardController : MonoBehaviour
{
    public int cellSize;
    public int boardSize;
    public int CellCount => boardSize * boardSize;
    private GameManager gameManager;
    private Transform[] cellTransforms;
    private int[,] cardNumEachPlayer;
    public int[] maxCardNumInLevel;
    public GameObject SelectPrefab;
    private Object SelectInstance;
    private readonly Dictionary<CellManager, CellState> AttackableCells = new();
    private readonly Dictionary<CellManager, CellState> MovableCells = new();

    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        cellTransforms = new Transform[CellCount];
        for (int i = 0; i < CellCount; i++)
        {
            cellTransforms[i] = transform.GetChild(i);
        }
        maxCardNumInLevel = new int[4] { 0, 3, 2, 1 };
    }

    // Start is called before the first frame update
    private void Start()
    {
        gameManager = GameManager.instance;
        cardNumEachPlayer = new int[3, 4];
        for (int i = 1; i <= 2; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                cardNumEachPlayer[i, j] = 0;
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void UpdateCells(byte[,] mapData)
    {
        for (int x = 1; x <= boardSize * 2; x++)
        {
            for (int y = 1; y <= boardSize * 2; y++)
            {
                int cellX = (x - 1) / 2;
                int cellY = (y - 1) / 2;
                int cellIdx = (boardSize - cellY - 1) * boardSize + cellX;
                Transform cellTransform = cellTransforms[cellIdx];
                CellManager cellManager = cellTransform.GetComponent<CellManager>();
                if (mapData[x, y] == 1)
                {
                    cellManager.cellType = CellType.Water;
                }
                else if (mapData[x, y] == 2)
                {
                    cellManager.cellType = CellType.Tree;
                }
            }
        }
    }

    public void ResetSelect()
    {
        DestroySelect();
        // 获取场景的根物体作为父物体
        Scene boardScene = SceneManager.GetSceneByName("BoardScene");
        SelectInstance = Instantiate(SelectPrefab, boardScene);
    }

    public void DestroySelect()
    {
        if (SelectInstance != null)
        {
            Destroy(SelectInstance);
        }
    }

    public int OnCardStartDrag(Transform cardTransform)
    {
        int cellIdx = GetCellIdxByPointer();
        if (gameManager.gameState == GameState.Playing)
        {
            SetMovable(cellIdx);
            ShowAttackRange(cardTransform, cellIdx);
        }
        return cellIdx;
    }

    public bool OnCardEndDrag(Transform cardTransform, int startCellIdx)
    {
        int cellIdx = GetCellIdxByPointer();

        if (!CanCardOnCell(cardTransform, cellIdx) || cellIdx == startCellIdx)
        {
            ClearMovable();
            return false;
        }
        if (!EmptyAtIdx(cellIdx))
        {
            OnAttack(startCellIdx, cellIdx);
            ClearAttackRange();
            ClearMovable();
            return false;
        }
        ClearAttackRange();
        ClearMovable();
        if (startCellIdx != -1)
        {
            CellManager startCellManager = cellTransforms[startCellIdx].GetComponent<CellManager>();
            startCellManager.cardTransform = null;
            startCellManager.cellState = CellState.Empty;
        }
        return AddCard(cardTransform, cellIdx);
    }

    public bool CanCardOnCell(Transform cardTransform, int cellIdx)
    {
        if (cellIdx == -1)
        {
            return false;
        }
        CellManager cellManager = cellTransforms[cellIdx].GetComponent<CellManager>();
        CellType cellType = cellManager.cellType;
        Elemental cardElemental = cardTransform.GetComponent<CardController>().cardAsset.elemental;
        if (cellType == CellType.Water && cardElemental != Elemental.Water)
        {
            return false;
        }
        if (cellType == CellType.Tree && cardElemental != Elemental.Flying)
        {
            return false;
        }
        return true;
    }

    private bool AddCard(Transform cardTransform, int cellIdx)
    {
        cardTransform.SetParent(cellTransforms[cellIdx]);
        // 重置卡牌位置到单元格中心
        cardTransform.localPosition = Vector3.zero;
        CellManager cellManager = cellTransforms[cellIdx].GetComponent<CellManager>();
        cellManager.cardTransform = cardTransform;
        cellManager.cellState = CellState.Occupied;
        return true;
    }

    // 添加卡片到棋盘
    public void AddCardForPlayer(byte player, int level)
    {
        cardNumEachPlayer[player, level]++;
        bool allFull = true;
        for (int i = 1; i <= 3; i++)
        {
            if (cardNumEachPlayer[player, i] < maxCardNumInLevel[i])
            {
                allFull = false;
                break;
            }
        }
        if (allFull)
        {
            gameManager.SwitchPlayer();
        }
    }

    // 从棋盘移除卡片
    public void RemoveCardForPlayer(byte player, int level)
    {
        cardNumEachPlayer[player, level]--;
        bool allDead = true;
        for (int i = 1; i <= 3; i++)
        {
            if (cardNumEachPlayer[player, i] > 0)
            {
                allDead = false;
                break;
            }
        }
        if (allDead)
        {
            gameManager.WinGame(player);
        }
    }

    public bool CanAddCard(byte player, int level)
    {
        return cardNumEachPlayer[player, level] < maxCardNumInLevel[level];
    }

    private void OnAttack(int startCellIdx, int cellIdx)
    {
        Transform startCellTransform = cellTransforms[startCellIdx];
        Transform cellTransform = cellTransforms[cellIdx];
        CellManager targetCellManager = cellTransform.GetComponent<CellManager>();
        if (targetCellManager.cellState != CellState.Occupied || !targetCellManager.attackable)
        {
            return;
        }
        CardController startCardController = startCellTransform.GetComponent<CellManager>().cardTransform.GetComponent<CardController>();
        CardController cardController = cellTransform.GetComponent<CellManager>().cardTransform.GetComponent<CardController>();
        if (startCardController.owner == cardController.owner)
        {
            return;
        }
        gameManager.NextMove();
        int attackDemage = startCardController.cardAsset.attack;
        if (cardController.OnAttack(attackDemage) <= 0)
        {
            RemoveCardForPlayer(cardController.owner, cardController.cardAsset.level);
            cellTransform.GetComponent<CellManager>().cellState = CellState.Blocked;
        }
    }

    public void ShowAttackRange(Transform cardTransform, int centerIdx)
    {
        CardController cardController = cardTransform.GetComponent<CardController>();
        List<Vector2Int> attackRange = cardController.cardAsset.attackRange;
        for (int i = 0; i < attackRange.Count; i++)
        {
            int diffX = attackRange[i].x;
            int diffY = attackRange[i].y;
            if (cardController.owner == 1)
            {
                diffY = -diffY;
            }
            int targetX = centerIdx % boardSize + diffX;
            int targetY = centerIdx / boardSize + diffY;
            if (targetX >= 0 && targetX < boardSize && targetY >= 0 && targetY < boardSize)
            {
                int cellIdx = targetY * boardSize + targetX;
                CellManager cellManager = cellTransforms[cellIdx].GetComponent<CellManager>();
                AttackableCells.Add(cellManager, cellManager.cellState);
                cellManager.attackable = true;
            }
        }
    }

    public void ClearAttackRange()
    {
        foreach (KeyValuePair<CellManager, CellState> entry in AttackableCells)
        {
            entry.Key.cellState = entry.Value;
            entry.Key.attackable = false;
        }
        AttackableCells.Clear();
    }

    public void SetMovable(int centerIdx)
    {
        int[] stepX = { -1, 1 };
        int[] stepY = { -1, 1 };
        for (int i = 0; i < stepX.Length; i++)
        {
            int targetX = centerIdx % boardSize + stepX[i];
            int targetY = centerIdx / boardSize;
            if (targetX >= 0 && targetX < boardSize && targetY >= 0 && targetY < boardSize)
            {
                int cellIdx = targetY * boardSize + targetX;
                CellManager cellManager = cellTransforms[cellIdx].GetComponent<CellManager>();
                if (cellManager.cellState != CellState.Occupied)
                {
                    MovableCells.Add(cellManager, cellManager.cellState);
                    cellManager.cellState = CellState.Empty;
                }
            }
        }
        for (int i = 0; i < stepY.Length; i++)
        {
            int targetX = centerIdx % boardSize;
            int targetY = centerIdx / boardSize + stepY[i];
            if (targetX >= 0 && targetX < boardSize && targetY >= 0 && targetY < boardSize)
            {
                int cellIdx = targetY * boardSize + targetX;
                CellManager cellManager = cellTransforms[cellIdx].GetComponent<CellManager>();
                if (cellManager.cellState != CellState.Occupied)
                {
                    MovableCells.Add(cellManager, cellManager.cellState);
                    cellManager.cellState = CellState.Empty;
                }
            }
        }
    }

    public void ClearMovable()
    {
        foreach (KeyValuePair<CellManager, CellState> entry in MovableCells)
        {
            entry.Key.cellState = entry.Value;
        }
        MovableCells.Clear();
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
    public void BlockArea(byte player)
    {
        int startIdx = (player - 1) * CellCount / 2;
        int endIdx = player * CellCount / 2;
        for (int i = startIdx; i < endIdx; i++)
        {
            if (cellTransforms[i].GetComponent<CellManager>().cellState != CellState.Occupied)
            {
                cellTransforms[i].GetComponent<CellManager>().cellState = CellState.Blocked;
            }
        }
    }

    // 启用指定位置的单元格
    public void UnblockArea(byte player)
    {
        int startIdx = (player - 1) * CellCount / 2;
        int endIdx = player * CellCount / 2;
        for (int i = startIdx; i < endIdx; i++)
        {
            if (cellTransforms[i].GetComponent<CellManager>().cellState != CellState.Occupied)
            {
                cellTransforms[i].GetComponent<CellManager>().cellState = CellState.Empty;
            }
        }
    }
}