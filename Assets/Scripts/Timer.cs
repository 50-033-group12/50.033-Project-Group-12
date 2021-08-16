using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timer;
    // 2 mins
    public float timeRemaining = 181f;
    float minutes;
    float seconds;

    private static Timer _instance;

    public static Timer GetInstance()
    {
        return _instance;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        timer = this.GetComponent<Text>();
        _instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(timeRemaining - Time.deltaTime > 0){
            timeRemaining -= Time.deltaTime;
            ChangeTime();
        }
        else{
            timeRemaining = 0f;
            BattleResultUI.GetInstance().Timeout();
            ChangeTime();
            enabled = false;
        }
    }

    void ChangeTime(){
        minutes = Mathf.FloorToInt(timeRemaining/60);
        seconds = Mathf.FloorToInt(timeRemaining%60);
        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
