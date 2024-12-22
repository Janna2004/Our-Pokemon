using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance;
    public BoardController boardController;
    private GameManager gameManager;
    private int[] cardNumEachPlayer;

    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        instance = this;
        boardController = GetComponent<BoardController>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        gameManager = GameManager.instance;
        cardNumEachPlayer = new int[gameManager.playerNum];
    }

    // Update is called once per frame
    private void Update()
    {
    }

    // ÃÌº”ø®∆¨µΩ∆Â≈Ã
    public void AddCardForPlayer(byte player)
    {
        cardNumEachPlayer[player - 1]++;
        if (cardNumEachPlayer[player - 1] == 3)
        {
            gameManager.SwitchPlayer();
        }
    }
}