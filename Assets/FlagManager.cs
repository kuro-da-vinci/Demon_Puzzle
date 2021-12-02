using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FlagManager : MonoBehaviour
{
    //クリア条件格納用変数
    private static int stageComboCount;
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
                if (stageComboCount >= 3)
                {
                    stage1ComboCount += 1;
                    i_behaviour.StartCoroutine(AttackEffect());
                }
                Debug.Log("クリアカウント" + stage1ComboCount);
                if (stage1ComboCount >= 2)
                {
                    stageClearFlag = true;
                    i_behaviour.StartCoroutine(NextScene());
                    stageFlag += 1;
                }
                stageComboCount = 0;
                
                break;

            case 2: //ステージ2クリア条件:縦一列1コンボ(5コンボ以上で出現)
                Debug.Log("コンボ数" + stageComboCount);
                if (stageComboCount >= 5)
                {
                    //隙の糸イベント

                    stage2ClearFlag = true;
                }
                if (stage2ClearFlag == true && comboData[0, 2] >= 1)
                {
                    i_behaviour.StartCoroutine(AttackEffect());
                    stageClearFlag = true;
                    //i_behaviour.StartCoroutine(NextScene());
                    stageFlag += 1;
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

    private static IEnumerator AttackEffect()
    {
        GameObject.Find("Zangeki").GetComponent<Image>().enabled = true;
        yield return new WaitForSeconds(0.5f);
        GameObject.Find("Zangeki").GetComponent<Image>().enabled = false;
    }

    private static IEnumerator NextScene()
    {
        yield return new WaitForSeconds(1.0f);
        enemyFadeOut = true;     
    }


}
