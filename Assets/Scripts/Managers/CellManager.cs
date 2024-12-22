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

public class CellManager : MonoBehaviour
{
    public CellType cellType = CellType.Grass;
    public CellState cellState = CellState.Empty;
    private CellController cellController;

    // Start is called before the first frame update
    private void Start()
    {
        cellController = GetComponent<CellController>();
        cellController.SetFilterColor(new Color(1, 1, 1, 0.5f));
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