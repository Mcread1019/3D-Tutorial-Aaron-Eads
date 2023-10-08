using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameSetup : MonoBehaviour
{
    int redBallsRemaining = 7;
    int blueBallsRemaining = 7;
    float ballRadius;
    float ballDiameter;
    float ballDiameterwithBuffer;
    [SerializeField] GameObject ballPrefab;
    [SerializeField] Transform cueBallPosition;
    [SerializeField] Transform headBallPosition;

    private void Awake()
    {
        ballRadius = ballPrefab.GetComponent<SphereCollider>().radius * 100;
        ballDiameter = ballRadius * 2f;
        PlaceAllballs();
    }

   void PlaceAllballs()
    {
        PlaceCueBall();
        PlaceRandomBall();
    }
    void PlaceCueBall()
    {
        GameObject ball = Instantiate(ballPrefab,cueBallPosition.position, Quaternion.identity);
        ball.GetComponent<Ball>().MakeCueBall();
    }
    void PlaceEightBall(Vector3 position)
    {
        GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity);
        ball.GetComponent<Ball>().MakeEightBall();
    }
    void PlaceRandomBall()
    {
        int NumInThisRow = 1;
        int Rand;
        Vector3 firstInRowPosition = headBallPosition.position;
        Vector3 currentPosition = firstInRowPosition;

        void PlaceRedball(Vector3 position)
        {
            GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity);
            ball.GetComponent<Ball>().BallSetup(true);
            redBallsRemaining--;
        }
        void PlaceBlueBall(Vector3 position)
        {
            GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity);
            ball.GetComponent<Ball>().BallSetup(false);
            blueBallsRemaining--;
        }
        //Outer Loop is the five rows
        for(int i = 0;i<5;i++)
        {
            //Inner Loop are the balls in each row
            for(int j = 0;j<NumInThisRow;j++)
            {
                //checks if its in the middle spot where the 8ball goes
                if (i == 2 && j == 1)
                {
                    PlaceEightBall(currentPosition);
                }
                //if there are red and blue balls remaing, randomly choose one and place it
                else if(redBallsRemaining >0&&blueBallsRemaining > 0)
                {
                    Rand = Random.Range(0, 2);
                    if (Rand == 0)
                    {
                        PlaceRedball(currentPosition);
                    }
                    else
                    {
                        PlaceBlueBall(currentPosition);
                    }
                }
                // if only red balls remained, place them
                else if (redBallsRemaining > 0)
                {
                    PlaceRedball(currentPosition);
                }
                //otherwise place a blue ball
                else
                {
                    PlaceBlueBall(currentPosition);
                }
                //Move the current position for the next ball to the right
                currentPosition += new Vector3(1, 0, 0).normalized * ballDiameter;
            }
            //Once all the balls have been placed, move on to the next row
            firstInRowPosition += Vector3.back * (Mathf.Sqrt(3) * ballRadius) + Vector3.left * ballRadius;
            currentPosition = firstInRowPosition;
            NumInThisRow++;
        }
    }
}
