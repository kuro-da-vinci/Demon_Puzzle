using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlagManager : MonoBehaviour
{
    //クリア条件格納用変数
    public static int stageComboCount;
    public static int stageTotalComboCount;
    private static int stage1ComboCount;
    private static int stageFlag = SceneManager.GetActiveScene().buildIndex;
    //private static int stageFlag = 1;

    private static bool stage2ClearFlag = false;

    public static bool stageClearFlag = false;
    public static bool enemyFadeOut = false;
    public static bool gameOverFadeOut = false;
    public static bool OpeningThreadFlag = false;
    private static bool OpeningThreadChkFlag = false;
    private static bool gameOverFlag = false;
    


    //private static bool stage1MiddleFlag = false;
    //private static bool stage1ClearFlag = false;

    //コンボデータ格納変数(6属性：①コンボ数、②横列フラグ、③縦列フラグ)
    private static int[,] comboData = new int[6, 3];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //クリア条件CHK
    public static void ClearCHK(MonoBehaviour i_behaviour, bool ChangeFlag)
    {
        switch (stageFlag)
        {
            case 1: //ステージ１クリア条件:3コンボを2回
                i_behaviour.StartCoroutine(Stage1_Process(ChangeFlag));
                break;
                

            case 2: //ステージ2クリア条件:縦一列1コンボ(合計10コンボ以上で出現)
                i_behaviour.StartCoroutine(Stage2_Process(ChangeFlag));
                break;
        }
    }

   


    //コンボ情報更新(リセット)
    public static void ComboReset()
    {
        for (int i = 0; i < 6; i++) {
            for (int j = 0; j < 3; j++)
            {
                Debug.Log("コンボリセット");
                comboData[i, j] = 0;
            }
        }
    }

    //コンボ情報更新(コンボ数)
    public static void MaxComboCHK(int num)
    {
        switch (num)
        {
            case 0: //火ドロップ
                comboData[0, 0] += 1;
                stageComboCount += 1;
                //Debug.Log("火数 " + comboData[0, 0]);
                break;
            case 1: //水ドロップ
                comboData[1, 0] += 1;
                stageComboCount += 1;
                //Debug.Log("水数 " + comboData[1, 0]);
                break;
            case 2: //緑ドロップ
                comboData[2, 0] += 1;
                stageComboCount += 1;
                //Debug.Log("緑数 " + comboData[2, 0]);
                break;
            case 3: //闇ドロップ
                comboData[3, 0] += 1;
                stageComboCount += 1;
                //Debug.Log("闇数 " + comboData[3, 0]);
                break;
            case 4: //光ドロップ
                comboData[4, 0] += 1;
                stageComboCount += 1;
                //Debug.Log("光数 " + comboData[4, 0]);
                break;
            case 5: //ハートドロップ
                comboData[5, 0] += 1;
                stageComboCount += 1;
                //Debug.Log("ハート数 " + comboData[5, 0]);
                break;
        }
    }

    //コンボ情報更新(横列)
    public static void RowComboCHK(int colour)
    {
        switch(colour)
        {
            case 0: //火ドロップ
                comboData[0, 1] += 1;
                //Debug.Log("横火 "+ comboData[0, 1]);
                break;
            case 1: //水ドロップ
                comboData[1, 1] += 1;
                //Debug.Log("横水 " + comboData[1, 1]);
                break;
            case 2: //緑ドロップ
                comboData[2, 1] += 1;
                //Debug.Log("横緑 " + comboData[2, 1]);
                break;
            case 3: //闇ドロップ
                comboData[3, 1] += 1;
                //Debug.Log("横闇 " + comboData[3, 1]);
                break;
            case 4: //光ドロップ
                comboData[4, 1] += 1;
                //Debug.Log("横光 " + comboData[4, 1]);
                break;
            case 5: //ハートドロップ
                comboData[5, 1] += 1;
                //Debug.Log("横ハート " + comboData[5, 1]);
                break;
        }
    }

    //コンボ情報更新(縦列)
    public static void FileComboCHK(int colour)
    {
        switch (colour)
        {
            case 0: //火ドロップ
                comboData[0, 2] += 1;
                //Debug.Log("縦火 " + comboData[0, 2]);
                break;
            case 1: //水ドロップ
                comboData[1, 2] += 1;
                //Debug.Log("縦水 " + comboData[1, 2]);
                break;
            case 2: //緑ドロップ
                comboData[2, 2] += 1;
                //Debug.Log("縦緑 " + comboData[2, 2]);
                break;
            case 3: //闇ドロップ
                comboData[3, 2] += 1;
                //Debug.Log("縦闇 " + comboData[3, 2]);
                break;
            case 4: //光ドロップ
                comboData[4, 2] += 1;
                //Debug.Log("縦光 " + comboData[4, 2]);
                break;
            case 5: //ハートドロップ
                comboData[5, 2] += 1;
                //Debug.Log("縦ハート " + comboData[5, 2]);
                break;
        }
    }







    private static IEnumerator HitEffect(int num)
    {
        Transform Parentfrom = GameObject.Find("Canvas").GetComponent<Transform>();
        GameObject Hit = (GameObject)Resources.Load("Hit");
        float DelTime = 0.5f / num;

        for (int i = 0; i < num; i++)
        {
            GameObject HitCl = Instantiate(Hit) as GameObject;
            HitCl.transform.SetParent(Parentfrom);
            //x:-500~500 y:0~1200
            float x = Random.Range(-500.0f, 500.0f);
            float y = Random.Range(0.0f, 1200.0f);
            float z = Hit.transform.localPosition.z;

            HitCl.transform.localPosition = new Vector3(x, y, z);
            HitCl.transform.localScale = Hit.transform.localScale;

            HitCl.GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(DelTime);

        }

        var clones = GameObject.FindGameObjectsWithTag("HitObj");
        foreach (var clone in clones)
        {
            Destroy(clone);
        }
    }

    private static IEnumerator AttackEffect()
    {
        if (!OpeningThreadChkFlag && !OpeningThreadFlag)
        {
            GameObject.Find("Zangeki").GetComponent<Image>().enabled = true;
            yield return new WaitForSeconds(0.5f);
            GameObject.Find("Zangeki").GetComponent<Image>().enabled = false;
        }
        else if (OpeningThreadChkFlag)
        {
            GameObject.Find("Zangeki").GetComponent<Image>().enabled = true;
            yield return new WaitForSeconds(0.5f);
            GameObject.Find("Zangeki").GetComponent<Image>().enabled = false;
        }


    }

    private static IEnumerator EnemyAttack()
    {


        if (OpeningThreadFlag)
            yield return new WaitForSeconds(0.5f);

        if (!OpeningThreadChkFlag)
        {
            Transform Parentfrom = GameObject.Find("Canvas").GetComponent<Transform>();
            GameObject EHit = (GameObject)Resources.Load("EZangeki");
            GameObject EHitCl = Instantiate(EHit) as GameObject;
            EHitCl.transform.SetParent(Parentfrom);

            float x = 0;
            float y = -1150;
            float z = EHit.transform.localPosition.z;

            EHitCl.transform.localPosition = new Vector3(x, y, z);
            EHitCl.transform.localScale = EHit.transform.localScale;

            yield return new WaitForSeconds(0.6f);
            EHitCl.GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(0.5f);
            //ダメージ
            gameOverFlag = GameObject.Find("Panel").GetComponent<LifeGaugeManager>().Damage(1);
            if (gameOverFlag == true)
            {
                stageClearFlag = true;
                yield return GameOverScene();
                stageFlag = 4;
            }

            EHitCl.GetComponent<SpriteRenderer>().enabled = false;
        }

        Debug.Log("エネミーアタック：" + OpeningThreadChkFlag);
        OpeningThreadChkFlag = false;
        OpeningThreadFlag = false;
    }

    private static IEnumerator HPRecovery(int rec)
    {
        yield return new WaitForSeconds(0.5f);
        //HP回復
        GameObject.Find("Panel").GetComponent<LifeGaugeManager>().Damage(rec * (-1));
    }

    private static IEnumerator NextScene()
    {
        yield return new WaitForSeconds(0.0f);

        if (OpeningThreadChkFlag)
        {
            Debug.Log("クリア");
            stageClearFlag = true;
            stageFlag += 1;
            stageTotalComboCount = 0;
            enemyFadeOut = true;
        }
        else if (stageClearFlag)
        {
            stageClearFlag = true;
            stageFlag += 1;
            stageTotalComboCount = 0;
            enemyFadeOut = true;
        }
        Debug.Log("ネクストシーン：" + OpeningThreadChkFlag);
        Debug.Log("継続");

    }

    private static IEnumerator GameOverScene()
    {
        yield return new WaitForSeconds(0.0f);
        gameOverFadeOut = true;
    }

    private static IEnumerator ThreadEnd()
    {
        if (OpeningThreadChkFlag)
        {
            yield return AttackEffect();
            if (comboData[5, 0] != 0)
                yield return HPRecovery(comboData[5, 0]);

            yield return NextScene();
        }
        else
        {
            if (comboData[5, 0] != 0)
                yield return HPRecovery(comboData[5, 0]);
            yield return EnemyAttack();
        }
    }


    private static IEnumerator Stage1_Process(bool cFlag)
    {
        //3コンボ以上なら攻撃エフェクト
        if (stageComboCount >= 3)
        {
            stage1ComboCount += 1;
            yield return  AttackEffect();

            //回復処理
            if (comboData[5, 0] != 0)
                yield return HPRecovery(comboData[5, 0]);

            //3コンボ以下1コンボ以上ならHitエフェクト＆ダメージ処理
        }
        else if (stageComboCount >= 0 && cFlag == true)
        {
            //回復以外のドロップを消していればHitエフェクト
            if (comboData[0, 0] != 0 || comboData[1, 0] != 0 ||
                comboData[2, 0] != 0 || comboData[3, 0] != 0 || comboData[4, 0] != 0)
            {
                int HitNum = stageComboCount - comboData[5, 0];
                yield return HitEffect(HitNum);
            }
            //回復処理
            if (comboData[5, 0] != 0)
                yield return HPRecovery(comboData[5, 0]);

            //ダメージ処理
            yield return EnemyAttack();
        }

        //クリア判定
        if (stage1ComboCount >= 2)
        {
            stageClearFlag = true;
            yield return NextScene();
            //stageFlag += 1;
        }

        stageComboCount = 0;

    }

    private static IEnumerator Stage2_Process(bool cFlag)
    {
        //勝利条件開放の条件：合計10コンボ以上
        //勝利条件：縦一列コンボ

        //トータルコンボ数のカウント
        stageTotalComboCount += stageComboCount;

        //条件開放CHK
        if (stageTotalComboCount >= 10)
        {
            stage2ClearFlag = true;
            GameObject.Find("ConditionsText").GetComponent<Text>().enabled = false;
            GameObject.Find("VictoryConditions").GetComponent<Text>().enabled = true;
        }

        //勝利条件解放後の縦一列1コンボCHK
        if (stage2ClearFlag == true && comboData[0, 2] >= 1 || stage2ClearFlag == true && comboData[1, 2] >= 1
            || stage2ClearFlag == true && comboData[2, 2] >= 1 || stage2ClearFlag == true && comboData[3, 2] >= 1
            || stage2ClearFlag == true && comboData[4, 2] >= 1)
        {
            //隙の糸イベント
            OpeningThreadFlag = true;

            GameObject.Find("OpeningThread").GetComponent<CreateThread>().OpeningThreadStart();
            yield return new WaitForSeconds(5.0f);
            OpeningThreadChkFlag = GameObject.Find("OpeningThread").GetComponent<CreateThread>().OpeningThreadEnd();
            yield return new WaitForSeconds(1.5f);
            yield return ThreadEnd();
        }
        else if (stageComboCount >= 0 && cFlag == true)
        {
            //回復以外のドロップを消していればHitエフェクト
            if (comboData[0, 0] != 0 || comboData[1, 0] != 0 ||
                comboData[2, 0] != 0 || comboData[3, 0] != 0 || comboData[4, 0] != 0)
            {
                int HitNum = stageComboCount - comboData[5, 0];
                Debug.Log("攻撃処理開始");
                yield return HitEffect(HitNum);
            }
            Debug.Log("攻撃処理終了");
            //回復処理
            Debug.Log("回復：" + comboData[5, 0]);
            if (comboData[5, 0] != 0)
            {
                Debug.Log("回復処理開始");
                yield return HPRecovery(comboData[5, 0]);
            }
            Debug.Log("回復処理終了");

            Debug.Log("ダメージ処理開始");
            //ダメージ処理
            yield return EnemyAttack();
            Debug.Log("ダメージ処理終了");
        }
        stageComboCount = 0;
    }

}
