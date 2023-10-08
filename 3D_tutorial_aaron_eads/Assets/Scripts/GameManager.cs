using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    bool isWaitingforBallMovementToStop = false;
    bool willSwapPlayers = false;
    bool isGameOver = false;
    bool ballPocketed = false;
    [SerializeField] float shotTimer = 3;
    private float currentTimer;
    [SerializeField] float movementThreshold;
    
    [SerializeField] TextMeshProUGUI player1BallsText;
    [SerializeField] TextMeshProUGUI player2BallsText;
    [SerializeField] TextMeshProUGUI currentTurnText;
    [SerializeField] TextMeshProUGUI MessageText;

    [SerializeField] GameObject restartButton;

    [SerializeField] Transform headPosition;
    [SerializeField] Camera cueStickCamera;
    [SerializeField] Camera overheadCamera;
    Camera CurrentCamera;

    // Start is called before the first frame update
    void Start()
    {
        currentPlayer = CurrentPlayer.Player1;
        CurrentCamera = overheadCamera;
    }

    // Update is called once per frame
    void Update()
    {
        if(isWaitingforBallMovementToStop&& !isGameOver)
        {
            currentTimer -=Time.deltaTime;
            if(currentTimer > 0) 
            {
                return;
            }

            bool allStopped = true;
            foreach(GameObject ball in GameObject.FindGameObjectsWithTag("Ball"))
            {
                if(ball.GetComponent<Rigidbody>().velocity.magnitude >=movementThreshold)
                {
                    allStopped = false;
                    break;
                }
            }
            if(allStopped)
            {
                isWaitingforBallMovementToStop = false;
                if (willSwapPlayers|| !ballPocketed)
                {
                    NextPlayerTurn();
                }
                else
                {
                    SwitchCameras();
                }
                currentTimer = shotTimer;
                ballPocketed = false;
            }
        }
    }

    public void SwitchCameras()
    {
        if(CurrentCamera == cueStickCamera)
        {
            cueStickCamera.enabled = false;
            overheadCamera.enabled = true;
            CurrentCamera = overheadCamera;
            isWaitingforBallMovementToStop = true;
        }
        else
        {
            cueStickCamera.enabled=true;
            overheadCamera.enabled=false;
            CurrentCamera = cueStickCamera;
            CurrentCamera.gameObject.GetComponent<CameraController>().ResetCamera();
        }
    }
    public void RestarttheGame()
    {
        SceneManager.LoadScene(0);
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
        willSwapPlayers = true;
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

    bool CheckBall(Ball ball) {
        if(ball.IsCueBall())
        {
            if (Scratch())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (ball.IsEightBall())
        {
            if(currentPlayer == CurrentPlayer.Player1)
            {
                if (isWinningShotForPlayer1)
                {
                    Win("Player 1");
                    return true;
                }
            }
            else
            {
                if (isWinningShotForPlayer2)
                {
                    Win("Player 2");
                    return true;
                }
            }
            earlytEightBall();
        }
        else
        {
            //All other logic done when not cue or eight ball
            if (ball.isBallRed())
            {
                player1BallsRemaining--;
                player1BallsText.text = "Player 1 balls Remaining: " + player1BallsRemaining;
                if (player1BallsRemaining <= 0)
                {
                    isWinningShotForPlayer1 = true;
                }
                if(currentPlayer!= CurrentPlayer.Player1)
                {
                    //NextPlayerTurn();
                    //isWaitingforBallMovementToStop = true;
                    willSwapPlayers = true;
                }
            }
            else
            {
                player2BallsRemaining--;
                player2BallsText.text = "Player 2 balls Remaining: " + player2BallsRemaining;
                if (player2BallsRemaining <= 0)
                {
                    isWinningShotForPlayer2 = true;
                }
                if (currentPlayer != CurrentPlayer.Player2)
                {
                   // NextPlayerTurn();
                    //isWaitingforBallMovementToStop = true;
                    willSwapPlayers = true;
                }
            }
        }
        return true;
    }

    void Lose(string message)
    {
        isGameOver = true;
        MessageText.gameObject.SetActive(true);
        MessageText.text = message;
        restartButton.SetActive(true);
    }
    void Win(string Player)
    {
        isGameOver = true;
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
        willSwapPlayers = false;
        SwitchCameras();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Ball")
        {
            ballPocketed = true;
            if(CheckBall(other.gameObject.GetComponent<Ball>())) 
            {
                Destroy(other.gameObject);
            }
            else
            {
                other.gameObject.transform.position = headPosition.position;
                other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                other.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }
        }
    }
}
