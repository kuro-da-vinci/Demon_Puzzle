using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{

    private static SpriteRenderer enemy;
    //private static bool enemyFadeOut = false;
    private float enemyOutAlpha = 1.0f;
    private float enemyFadeTime = 2.0f;
    private int sceneCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        FlagManager.stageClearFlag = false;
        enemy = GameObject.Find("Enemy").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(FlagManager.enemyFadeOut);
        if (FlagManager.enemyFadeOut)
        {
            //経過時間から透明度計算
            enemyOutAlpha -= Time.deltaTime / enemyFadeTime;
            //フェードアウト終了判定
            if (enemyOutAlpha <= 0.0f)
            {
                FlagManager.enemyFadeOut = false;
                enemyOutAlpha = 1.0f;
                //画面遷移
                sceneCount = SceneManager.GetActiveScene().buildIndex + 1;
                FadeManager.FadeOut(sceneCount);
                //Debug.Log(sceneCount);
            }

            //フェード用Imageの色・透明度設定
            if (FlagManager.enemyFadeOut)
            {
                enemy.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, enemyOutAlpha);

            }

        }
        
        if (FlagManager.gameOverFadeOut)
        {
            //画面遷移
            sceneCount = 4;
            FadeManager.FadeOut(sceneCount);

        }

    }
}
