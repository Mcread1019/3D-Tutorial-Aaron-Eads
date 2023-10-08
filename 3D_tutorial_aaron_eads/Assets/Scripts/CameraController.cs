using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    [SerializeField] Vector3 offset;
    [SerializeField] float downAngle;
    [SerializeField] float power;
    private float horizontalInput;
    [SerializeField] GameObject cueStick;
    private bool isTakingShot = false;
    [SerializeField] float maxDrawDistance;
    private float savedMousePosition;
    [SerializeField] TextMeshProUGUI powerText;

    Transform cueBall;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        foreach(GameObject ball in GameObject.FindGameObjectsWithTag("Ball"))
        {
            if (ball.GetComponent<Ball>().IsCueBall())
            {
                cueBall = ball.transform;
                break;
            }
        }
        ResetCamera();
    }

    // Update is called once per frame
    void Update()
    {
        if(cueBall != null&& !isTakingShot)
        {
            horizontalInput = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;

            transform.RotateAround(cueBall.position, Vector3.up, horizontalInput);
        }

        shoot();
    }

    public void ResetCamera()
    {
        cueStick.SetActive(true);
        transform.position = cueBall.position + offset;
        transform.LookAt(cueBall.position);
        transform.localEulerAngles = new Vector3(downAngle,transform.localEulerAngles.y,0);
    }

    void shoot()
    {
        if (gameObject.GetComponent<Camera>().enabled)
        {
            if (Input.GetButtonDown("Fire1") && !isTakingShot)
            {
                isTakingShot = true;
                savedMousePosition = 0f;
            }
            else if (isTakingShot)
            {
                if(savedMousePosition + Input.GetAxis("Mouse Y") <= 0)
                {
                    savedMousePosition += Input.GetAxis("Mouse Y");
                    if(savedMousePosition <= maxDrawDistance)
                    {
                        savedMousePosition = maxDrawDistance;
                    }
                    float powerValueNumber =((savedMousePosition - 0)/(maxDrawDistance-0))*(100-0)+0; 
                    int powerValueInt = Mathf.RoundToInt(powerValueNumber);
                    powerText.text ="Power: "+ powerValueInt+"%";
                }
                if (Input.GetButtonDown("Fire1"))
                {
                    Vector3 hitDirection = transform.forward;
                    hitDirection = new Vector3(hitDirection.x, 0, hitDirection.z).normalized;

                    cueBall.gameObject.GetComponent<Rigidbody>().AddForce(hitDirection * power, ForceMode.Impulse);
                    cueStick.SetActive(false);
                    gameManager.SwitchCameras();
                }
            }
        }
    }
}
