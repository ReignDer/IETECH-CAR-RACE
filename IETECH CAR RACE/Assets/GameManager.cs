using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField]
    private Text timeText;

    [SerializeField]
    private Text speedText;
    [SerializeField]
    private Text lapText;
    private int lap = 0;

    [SerializeField]
    private Text finalTimeText;

    public GameObject endPanel;

    public CarController MPH;
    public GameObject needle;
    private float minSpeed = 209f, maxSpeed = -30.89f;
    private float currentSpeed;
    private bool startTimer = false;
    private float timeElapsed;

    public float vehicleSpeed;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        vehicleSpeed = MPH.MPH;
        if (startTimer)
        {
            Timer();
        }
    }

    private void FixedUpdate()
    {
        updateNeedle();
    }
    private void updateNeedle()
    {
        float speed = Mathf.Round(vehicleSpeed);
        speedText.text = string.Format("{0} MPH", speed); 
        currentSpeed = minSpeed - maxSpeed;
        float temp = vehicleSpeed / 260;
        needle.transform.eulerAngles = new Vector3 (0, 0, (minSpeed - temp * currentSpeed));
    }
    public void StartTimer()
    {
        startTimer = true;
    }
    public void EndTimer()
    {
        startTimer = false;
        lap++;
        lapText.text = string.Format("Lap {0}/1", lap);

        finalTimeText.text = "Final Time: " + timeText.text ;
        endPanel.SetActive(true);

    }
    private void Timer()
    {
        timeElapsed += Time.deltaTime;
        float minutes = Mathf.FloorToInt(timeElapsed / 60f);
        float seconds = Mathf.FloorToInt(timeElapsed % 60f);
        int milliseconds = Mathf.FloorToInt((timeElapsed % 1) * 1000);

        timeText.text = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
  
    }
    public void Exit()
    {
        Application.Quit();
    }
}
