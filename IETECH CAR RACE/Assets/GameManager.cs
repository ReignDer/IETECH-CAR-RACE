using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CarController MPH;
    public GameObject needle;
    private float minSpeed = 209f, maxSpeed = -30.89f;
    private float currentSpeed;

    public float vehicleSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        vehicleSpeed = MPH.MPH;
    }

    private void FixedUpdate()
    {
        updateNeedle();
    }
    private void updateNeedle()
    {
        currentSpeed = minSpeed - maxSpeed;
        float temp = vehicleSpeed / 260;
        needle.transform.eulerAngles = new Vector3 (0, 0, (minSpeed - temp * currentSpeed));
    }
}
