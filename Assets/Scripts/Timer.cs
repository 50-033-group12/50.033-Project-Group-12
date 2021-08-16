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
    
    // Start is called before the first frame update
    void Start()
    {
        timer = this.GetComponent<Text>();
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
            ChangeTime();
        }
    }

    void ChangeTime(){
        minutes = Mathf.FloorToInt(timeRemaining/60);
        seconds = Mathf.FloorToInt(timeRemaining%60);
        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
