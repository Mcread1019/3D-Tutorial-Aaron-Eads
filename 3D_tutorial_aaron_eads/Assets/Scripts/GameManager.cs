using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    enum CurrentPlayer
    {
        Player1,
        Player2
    }

    CurrentPlayer currentPlayer;
    bool isWinningShotForPlayer1 = false;
    bool isWinningShotForPlayer2 = false;
    int player1BallsRemaining = 7;
    int player2BallsRemaining = 7;
    [SerializeField] TextMeshProUGUI player1BallsText;
    [SerializeField] TextMeshProUGUI player2BallsText;
    [SerializeField] TextMeshProUGUI currentTurnText;
    [SerializeField] TextMeshProUGUI MessageText;

    [SerializeField] GameObject restartButton;

    [SerializeField] Transform headPosition;

    // Start is called before the first frame update
    void Start()
    {
        currentPlayer = CurrentPlayer.Player1; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    bool Scratch()
    {
        if(currentPlayer == CurrentPlayer.Player1)
        {
            if (isWinningShotForPlayer1)
            {
                ScratchOnWinningShot("Player 1");
                return true;
            }
        }
        else
        {
            if (isWinningShotForPlayer2)
            {
                ScratchOnWinningShot("Player 2");
                return true;
            }
        }
        NextPlayerTurn();
        return false;
    }
    void earlytEightBall()
    {
        if(currentPlayer  == CurrentPlayer.Player1) {
            Lose("Player 1 hit in the eight ball too early and has lost!");
        }
        else
        {
            Lose("Player 2 hit in the eight ball too early and has lost!");
        }
    }
    void ScratchOnWinningShot(string player)
    {
        Lose(player + " Scratched on their Final shot and has Lost!");
    }

    void NoMoreBalls(CurrentPlayer player)
    {
        if (player== CurrentPlayer.Player1)
        {
            isWinningShotForPlayer1 = true;
        }
        else
        {
            isWinningShotForPlayer2 = true;
        }
    }

    bool CheckBall(Ball ball) {
        return true;
    }

    void Lose(string message)
    {
        MessageText.gameObject.SetActive(true);
        MessageText.text = message;
        restartButton.SetActive(true);
    }
    void Win(string Player)
    {
        MessageText.gameObject.SetActive(true);
        MessageText.text = Player + " has won!";
        restartButton.SetActive(true);
    }

    void NextPlayerTurn()
    {
        if(currentPlayer== CurrentPlayer.Player1)
        {
            currentPlayer = CurrentPlayer.Player2;
            currentTurnText.text = "Current Turn: Player 2";
        }
        else
        {
            currentPlayer = CurrentPlayer.Player1;
            currentTurnText.text = "Current Turn: Player 1";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Ball")
        {
            if(CheckBall(other.gameObject.GetComponent<Ball>())) {
                Destroy(other.gameObject);
            }
        }
        else
        {
            other.gameObject.transform.position = headPosition.position;
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }
}
