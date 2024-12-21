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
    public Color filterColor = new Color(1, 1, 1, 0.3f);
    private Image FilterImg;

    // Start is called before the first frame update
    private void Start()
    {
        Transform filterCanvas = transform.Find("FilterCanvas");
        FilterImg = filterCanvas.GetComponentInChildren<Image>();
        FilterImg.color = new Color(1, 1, 1, 0);
        FilterImg.gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnMouseEnter()
    {
        FilterImg.color = filterColor;
        FilterImg.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        FilterImg.color = new Color(1, 1, 1, 0);
        FilterImg.gameObject.SetActive(false);
    }
}