using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance;
    public BoardController boardController;

    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        instance = this;
        boardController = GetComponent<BoardController>();
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}