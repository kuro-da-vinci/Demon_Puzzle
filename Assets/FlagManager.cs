using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FlagManager : MonoBehaviour
{
    //クリア条件格納用変数
    public static int stageComboCount;
    private static int stage1ComboCount;
    private static int stageFlag = 1;

    private static bool stage2ClearFlag = false;

    public static bool stageClearFlag = false;
    public static bool enemyFadeOut = false;


    
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
    public static void ClearCHK(MonoBehaviour i_behaviour)
    {
        switch (stageFlag)
        {
            case 1: //ステージ１クリア条件:3コンボを2回
                Debug.Log("コンボ数" + stageComboCount);
                //3コンボ以上なら攻撃エフェクト
                if (stageComboCount >= 3)
                {
                    stage1ComboCount += 1;
                    i_behaviour.StartCoroutine(AttackEffect());

                //3コンボ以下1コンボ以上ならHitエフェクト＆ダメージ処理
                }else if (stageComboCount > 0)
                {
                    //回復以外のドロップを消していればHitエフェクト
                    if(comboData[0,0] != 0 || comboData[1, 0] != 0 ||
                        comboData[2, 0] != 0 || comboData[3, 0] != 0 || comboData[4, 0] != 0)
                    {
                        int HitNum = stageComboCount - comboData[5, 0];
                        i_behaviour.StartCoroutine(HitEffect(HitNum));
                    }
                    //ダメージ処理
                    i_behaviour.StartCoroutine(EnemyAttack());
                }

                Debug.Log("クリアカウント" + stage1ComboCount);
                //クリア判定
                if (stage1ComboCount >= 2)
                {
                    stageClearFlag = true;
                    i_behaviour.StartCoroutine(NextScene());
                    stageFlag += 1;
                }

                stageComboCount = 0;
                
                break;

            case 2: //ステージ2クリア条件:縦一列1コンボ(合計10コンボ以上で出現)
                Debug.Log("コンボ数" + stageComboCount);
                if (stageComboCount >= 10)
                {
                    //隙の糸イベント

                    stage2ClearFlag = true;
                    GameObject.Find("ConditionsText").GetComponent<Text>().enabled = false;
                    GameObject.Find("VictoryConditions").GetComponent<Text>().enabled = true;
                }
                if (stage2ClearFlag == true && comboData[0, 2] >= 1 || comboData[1, 2] >= 1
                    || comboData[2, 2] >= 1 || comboData[3, 2] >= 1 || comboData[4, 2] >= 1)
                {
                    i_behaviour.StartCoroutine(AttackEffect());
                    stageClearFlag = true;
                    i_behaviour.StartCoroutine(NextScene());
                    stageFlag += 1;
                    stageComboCount = 0;
                }else if (stageComboCount > 0)
                {
                    //回復以外のドロップを消していればHitエフェクト
                    if (comboData[0, 0] != 0 || comboData[1, 0] != 0 ||
                        comboData[2, 0] != 0 || comboData[3, 0] != 0 || comboData[4, 0] != 0)
                    {
                        int HitNum = stageComboCount - comboData[5, 0];
                        i_behaviour.StartCoroutine(HitEffect(HitNum));
                    }
                    //ダメージ処理
                    i_behaviour.StartCoroutine(EnemyAttack());
                }
                
                break;
            case 3: //緑ドロップ
                comboData[2, 0] += 1;
                Debug.Log("緑数 " + comboData[2, 0]);
                break;
            case 4: //闇ドロップ
                comboData[3, 0] += 1;
                Debug.Log("闇数 " + comboData[3, 0]);
                break;
            case 5: //光ドロップ
                comboData[4, 0] += 1;
                Debug.Log("光数 " + comboData[4, 0]);
                break;
            case 6: //ハートドロップ
                comboData[5, 0] += 1;
                Debug.Log("ハート数 " + comboData[5, 0]);
                break;
        }
    }

   


    //コンボ情報更新(リセット)
    public static void ComboReset()
    {
        for (int i = 0; i < 6; i++) {
            for (int j = 0; j < 3; j++)
            {
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
        //GameObject Hit = GameObject.Find("Hit");
        float DelTime = 0.5f / num;

        for (int i = 0; i < num; i++) {
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
            //HitCl.GetComponent<SpriteRenderer>().enabled = false;

            /*
            GameObject.Find("Hit").GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(0.5f);
            GameObject.Find("Hit").GetComponent<SpriteRenderer>().enabled = false;
            */
        }
        //GameObject.Find("Hit(Clone)").GetComponent<SpriteRenderer>().enabled = false;

        var clones = GameObject.FindGameObjectsWithTag("HitObj");
        foreach(var clone in clones)
        {
            Destroy(clone);
        }


    }

    private static IEnumerator AttackEffect()
    {
        GameObject.Find("Zangeki").GetComponent<Image>().enabled = true;
        yield return new WaitForSeconds(0.5f);
        GameObject.Find("Zangeki").GetComponent<Image>().enabled = false;
    }

    private static IEnumerator EnemyAttack()
    {
        
        yield return new WaitForSeconds(1.0f);
        //ダメージ
        GameObject.Find("Panel").GetComponent<LifeGaugeManager>().Damage(1);
    }

    private static IEnumerator NextScene()
    {
        yield return new WaitForSeconds(1.0f);
        enemyFadeOut = true;     
    }


}
