using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextScene : MonoBehaviour
{

    private static SpriteRenderer enemy;
    //private static bool enemyFadeOut = false;
    private float enemyOutAlpha = 1.0f;
    private float enemyFadeTime = 2.0f;

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
            }

            //フェード用Imageの色・透明度設定
            if (FlagManager.enemyFadeOut)
            {
                enemy.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, enemyOutAlpha);

            }
        }

    }
}
