using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class SceneChange : MonoBehaviour
{
    public GameObject bk, tl;
    [SerializeField] RawImage[] video;
    [SerializeField] GameObject[] sr;
    public bool imgFadeOut = false;
    public bool textFadeOut = false;
    public float imgOutAlpha = 0.0f;
    public float imgFadeTime = 1.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Text>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        bk.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        tl.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        StartCoroutine(VPControl());
    }

    // Update is called once per frame
    void Update()
    {
     
        if (imgFadeOut)
        {
            //経過時間から透明度計算
            imgOutAlpha += Time.deltaTime / imgFadeTime;
            //フェードアウト終了判定
            if (imgOutAlpha >= 1.0f)
            {
                imgFadeOut = false;
                imgOutAlpha = 0.0f;
            }

            //フェード用Imageの色・透明度設定
            if (imgFadeOut)
            {
                bk.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, imgOutAlpha);
                tl.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, imgOutAlpha);
            }
        }
        
        if (textFadeOut)
        {
            //経過時間から透明度計算
            imgOutAlpha += Time.deltaTime / imgFadeTime;
            //フェードアウト終了判定
            if (imgOutAlpha >= 1.0f)
            {
                textFadeOut = false;
                imgOutAlpha = 0.0f;
                this.GetComponent<Text>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            }

            //フェード用Imageの色・透明度設定
            if (textFadeOut)
            {
                this.GetComponent<Text>().color = new Color(1.0f, 0.0f, 0.0f, imgOutAlpha);
            }
        }
    }

    IEnumerator VPControl()
    {
        yield return new WaitForSeconds(1.0f);
        video[1].GetComponent<RawImage>().enabled = true;
        var videoPlayer = video[1].GetComponent<VideoPlayer>();
        videoPlayer.Play(); // 動画を再生する。
                
        yield return new WaitForSeconds(1.7f);
        video[1].GetComponent<RawImage>().enabled = false;

        video[0].GetComponent<RawImage>().enabled = true;
        videoPlayer = video[0].GetComponent<VideoPlayer>();
        videoPlayer.time = 3.7f;
        videoPlayer.Play(); // 動画を再生する。

        yield return new WaitForSeconds(1.5f);
        video[0].GetComponent<RawImage>().enabled = false;
        video[3].GetComponent<RawImage>().enabled = true;
        sr[0].GetComponent<SpriteRenderer>().enabled = true;

        video[2].GetComponent<RawImage>().enabled = true;
        videoPlayer = video[2].GetComponent<VideoPlayer>();
        videoPlayer.Play(); // 動画を再生する。
        
        yield return new WaitForSeconds(1.2f);
        video[2].GetComponent<RawImage>().enabled = false;
        sr[2].GetComponent<SpriteRenderer>().enabled = true;


        videoPlayer = video[3].GetComponent<VideoPlayer>();
        videoPlayer.time = 3.7f;
        videoPlayer.Play(); // 動画を再生する。

        yield return new WaitForSeconds(1.4f);
        video[3].GetComponent<RawImage>().enabled = false;
        sr[1].GetComponent<SpriteRenderer>().enabled = true;

        video[4].GetComponent<RawImage>().enabled = true;
        videoPlayer = video[4].GetComponent<VideoPlayer>();
        videoPlayer.Play(); // 動画を再生する。

        yield return new WaitForSeconds(1.5f);
        imgFadeOut = true;

        yield return new WaitForSeconds(1.0f);
        textFadeOut = true;

    }


    public void Change()
    {
        Debug.Log("画面遷移");
        FadeManager.FadeOut(1);
    }
}
