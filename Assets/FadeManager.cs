
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{

    //フェード用のCanvasとImage
    private static Canvas fadeCanvas;
    private static Image fadeImage;

    //フェード用Imageの透明度
    private static float inAlpha = 1.0f;
    private static float outAlpha = 0.0f;

    //フェードインアウトのフラグ
    public static bool isFadeIn = false;
    public static bool isFadeOut = false;

    //フェードしたい時間（単位は秒）
    private static float fadeTime = 1.0f;

    //遷移先のシーン番号
    private static int nextScene = 1;

    //フェード用のCanvasとImage生成
    static void Init()
    {
        //フェード用のCanvas生成
        GameObject FadeCanvasObject = new GameObject("CanvasFade");
        fadeCanvas = FadeCanvasObject.AddComponent<Canvas>();
        FadeCanvasObject.AddComponent<GraphicRaycaster>();
        fadeCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        FadeCanvasObject.AddComponent<FadeManager>();

        //最前面になるよう適当なソートオーダー設定
        fadeCanvas.sortingOrder = 100;

        //フェード用のImage生成
        fadeImage = new GameObject("ImageFade").AddComponent<Image>();
        fadeImage.transform.SetParent(fadeCanvas.transform, false);
        fadeImage.rectTransform.anchoredPosition = Vector3.zero;

        //Imageサイズは適当に大きく設定してください
        fadeImage.rectTransform.sizeDelta = new Vector2(9999, 9999);
    }

    //フェードイン開始
    public static void FadeIn()
    {
        //Debug.Log("フェードイン開始");
        if (fadeImage == null) Init();
        fadeImage.color = Color.black;
        isFadeIn = true;
    }

    //フェードアウト開始
    public static void FadeOut(int n)
    {
        //Debug.Log("フェードアウト開始");
        if (fadeImage == null) Init();
        nextScene = n;
        fadeImage.color = Color.clear;
        fadeCanvas.enabled = true;
        isFadeOut = true;
    }

    void Update()
    {
        //フラグ有効なら毎フレームフェードイン/アウト処理
        if (isFadeIn)
        {
            //Debug.Log("フェードイン中");
            //経過時間から透明度計算
            inAlpha -= Time.deltaTime / fadeTime;
            //Debug.Log(Time.deltaTime);

            //フェードイン終了判定
            if (inAlpha <= 0.0f)
            {
                isFadeIn = false;
                inAlpha = 1.0f;
                fadeCanvas.enabled = false;
                //Debug.Log("フェードイン終了");
            }

            //フェード用Imageの色・透明度設定
            fadeImage.color = new Color(0.0f, 0.0f, 0.0f, inAlpha);
        }
        else if (isFadeOut)
        {
            //経過時間から透明度計算
            outAlpha += Time.deltaTime / fadeTime;

            //フェードアウト終了判定
            if (outAlpha >= 1.0f)
            {
                //Debug.Log("フェードアウト終了");
                isFadeOut = false;
                outAlpha = 0.0f;

                //次のシーンへ遷移
                SceneManager.LoadScene(nextScene);
            }

            //フェード用Imageの色・透明度設定
            if (isFadeOut) {
                fadeImage.color = new Color(0.0f, 0.0f, 0.0f, outAlpha);
            }
        }
    }
}