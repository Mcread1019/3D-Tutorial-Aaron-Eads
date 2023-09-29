using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Boolean isRed;
    private Boolean is8Ball = false;
    private Boolean isCueBall = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool isBallRed()
    {
        return isRed;
    }
   public bool IsCueBall() { 
        return isCueBall; 
    }
    public bool IsEightBall()
    {
        return is8Ball;
    }
    public void BallSetup(bool red)
    {
        isRed = red;
        if (isRed)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.blue;
        }
    }
    public void MakeCueBall() { 
        isCueBall = true;
    }
    public void MakeEightBall()
    {
        is8Ball = true;
        GetComponent<Renderer>().material.color = Color.black;
    }
}
