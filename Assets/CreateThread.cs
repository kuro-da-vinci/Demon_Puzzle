using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//参考資料
//https://nn-hokuson.hatenablog.com/entry/2019/02/28/205346
//https://3dunity.org/game-create-lesson/action-pazzle-game/touch-array-introduction/

public class CreateThread : MonoBehaviour
{

    public int count = 0;
    public int threadCount = 0;
    public int threadChkCount = 0;
    public bool startFlg = false;

    //　カメラ内にいるかどうか
    private bool isInsideCamera;

    GameObject clickedGameObject;
    Vector3 OpeningThreadSetPoint;


    // Start is called before the first frame update
    void Start()
    {
        OpeningThreadSetPoint = GameObject.Find("OpeningThread").transform.position;
    }

    


    // Update is called once per frame
    void Update()
    {
        //Debug.Log(isInsideCamera+"カウント："+count);
        //隙の糸処理開始
        if (count <= 500 && startFlg == true)
        {
            Transform Parentfrom = GameObject.Find("Canvas").GetComponent<Transform>();
            GameObject OT = GameObject.Find("OpeningThread");

            //隙の糸がカメラ内なら線を引く
            if (isInsideCamera)
            {
                GameObject OTL = (GameObject)Resources.Load("OpeningThreadLine");
                GameObject OTLCl = Instantiate(OTL) as GameObject;
                OTLCl.transform.SetParent(Parentfrom);

                OTLCl.transform.localPosition = OT.transform.localPosition;
                OTLCl.transform.localScale = OT.transform.localScale;
                
            }
            Check("Thread");
            count += 1;
        }
        else
        {
            GameObject.Find("OpeningThread").transform.position = OpeningThreadSetPoint;
        }
        
        //隙の糸がなぞられたら不透明にする
        var mousePos = Input.mousePosition;
        if (Input.GetMouseButton(0))
        {
            clickedGameObject = null;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hit2d = Physics2D.CircleCastAll((Vector2)ray.origin, 0.5f,(Vector2)ray.direction);

            if (hit2d.Length > 0)
            {
                if (hit2d[0].collider.tag == "Thread")
                {
                    clickedGameObject = hit2d[0].transform.gameObject;
                    clickedGameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    clickedGameObject.tag = "ThreadChk";
                    //Destroy(hit2d[0].collider.gameObject);
                }
            }
        }


    }

    //隙の糸をランダムで描く
    
    public void OpeningThreadStart()
    {
        int num = Random.Range(3, 6);
        float x = 0;
        float y = 0;
        float z = -3.0f;
        Vector3[] path = new Vector3[num];

        GameObject.Find("OpeningThreadPanel").GetComponent<Image>().enabled = true;
        GameObject.Find("OpeningThreadPanel").GetComponent<GraphicRaycaster>().enabled = true;

        for (int i = 0; i < num; i++)
        {
            if (i == 0)
            {
                x = -900.0f;
                y = Random.Range(0.0f, 1000.0f);
            }else if(i == num-1)
            {
                x = 900.0f;
                y = Random.Range(0.0f, 1000.0f);
            }
            else
            {
                x = Random.Range(-750.0f, 750.0f);
                y = Random.Range(0.0f, 1000.0f);
            }
            path[i] = new Vector3(x, y, z);
        }


        startFlg = true;
        transform.DOLocalPath(path, 1.5f, PathType.CatmullRom)
                .SetEase(Ease.Linear)
                //.SetLookAt(0.00f, Vector3.forward)
                .SetOptions(false, AxisConstraint.Z);
    }

    //隙の糸終了
    GameObject[] tagChkObjects;
    public bool OpeningThreadEnd()
    {
        startFlg = false;
        count = 0;

        tagChkObjects = GameObject.FindGameObjectsWithTag("ThreadChk");
        threadChkCount = tagChkObjects.Length;
        //Debug.Log(threadChkCount + ":" + threadCount+":" + (threadCount * 0.7));

        if (threadChkCount > (threadCount * 0.7))
        {
            //Debug.Log(threadChkCount +":"+ threadCount);
            return true;
        }
        else
        {
            var clones1 = GameObject.FindGameObjectsWithTag("Thread");
            var clones2 = GameObject.FindGameObjectsWithTag("ThreadChk");
            foreach (var clone in clones1)
            {
                Destroy(clone);
            }
            foreach (var clone in clones2)
            {
                Destroy(clone);
            }
            return false;
        }
    }

    //カメラ内の隙の糸を数える
    GameObject[] tagObjects;
    void Check(string tagname)
    {
        tagObjects = GameObject.FindGameObjectsWithTag(tagname);
        threadCount = tagObjects.Length;
        //Debug.Log(tagObjects.Length); //tagObjects.Lengthはオブジェクトの数
        
    }

    //　カメラから外れた
    private void OnBecameInvisible()
    {
        isInsideCamera = false;
    }
    //　カメラ内に入った
    private void OnBecameVisible()
    {
        isInsideCamera = true;
    }

}
