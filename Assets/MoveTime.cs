using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MoveTime : MonoBehaviour
{

    public Image image;
    public bool timer = false;
    public bool timerFlg = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButton(0))
        if (timer == true)
        {
            image.fillAmount -= Time.deltaTime / 4;

            if (image.fillAmount <= 0) {
                Debug.Log("タイマー" + image.fillAmount);
                GameObject.Find("PuzllBlock").GetComponent<Image>().enabled = true;
                GameObject.Find("PuzllBlock").GetComponent<GraphicRaycaster>().enabled = true;
                //EventTrigger et = GameObject.Find("drop(Clone)").GetComponent<EventTrigger>();
                //et.enabled = false;
                timerFlg = false;
                //GameObject.Find("drop(Clone)").GetComponent<DropCnt>().SetDrop();
            }
        }
        else
        {

            image.fillAmount = 1;
            timerFlg = true;
        }
    }

    public void TimerStart()
    {
        timer = true;
    }

    public void TimerStop()
    {
        timer = false;
        //timerFlg = true;
    }

    public bool TimerFlg()
    {
        //Debug.Log(timerFlg + "TimerFlg");
        return timerFlg;
    }

    public void TimerFlgRset()
    {
        timerFlg = true;
    }

}

