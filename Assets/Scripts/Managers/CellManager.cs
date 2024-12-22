using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CellType
{
    Grass,
    Tree,
    Water
}

public enum CellState
{
    Empty,
    Occupied,
    Blocked
}

// ӳ�䵥Ԫ��״̬����ɫ
public class CellColor
{
    public static Dictionary<CellState, Color> cellColor = new Dictionary<CellState, Color>
    {
        {CellState.Empty, new Color(1, 1, 1, 0.5f)},
        {CellState.Occupied, new Color(1, 0, 0, 0.5f)},
        {CellState.Blocked, new Color(1, 0, 0, 0.5f)}
    };
}

public class CellManager : MonoBehaviour
{
    public CellType cellType = CellType.Grass;
    public CellState _cellState = CellState.Empty;

    public CellState cellState
    {
        set
        {
            if (_cellState != value)
            {
                _cellState = value;
                cellController.SetFilterColor(CellColor.cellColor[_cellState]);
            }
        }
        get
        {
            return _cellState;
        }
    }

    private CellController cellController;

    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        cellController = GetComponent<CellController>();
        cellController.SetFilterColor(new Color(1, 1, 1, 0.5f));
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnMouseEnter()
    {
        cellController.SetFilterActive(true);
    }

    private void OnMouseExit()
    {
        cellController.SetFilterActive(false);
    }
}