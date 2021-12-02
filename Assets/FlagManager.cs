using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FlagManager : MonoBehaviour
{
    //�N���A�����i�[�p�ϐ�
    private static int stageComboCount;
    private static int stage1ComboCount;
    private static int stageFlag = 1;

    private static bool stage2ClearFlag = false;

    public static bool stageClearFlag = false;
    public static bool enemyFadeOut = false;


    
    //private static bool stage1MiddleFlag = false;
    //private static bool stage1ClearFlag = false;

    //�R���{�f�[�^�i�[�ϐ�(6�����F�@�R���{���A�A����t���O�A�B�c��t���O)
    private static int[,] comboData = new int[6, 3];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�N���A����CHK
    public static void ClearCHK(MonoBehaviour i_behaviour)
    {
        switch (stageFlag)
        {
            case 1: //�X�e�[�W�P�N���A����:3�R���{��2��
                Debug.Log("�R���{��" + stageComboCount);
                if (stageComboCount >= 3)
                {
                    stage1ComboCount += 1;
                    i_behaviour.StartCoroutine(AttackEffect());
                }
                Debug.Log("�N���A�J�E���g" + stage1ComboCount);
                if (stage1ComboCount >= 2)
                {
                    stageClearFlag = true;
                    i_behaviour.StartCoroutine(NextScene());
                    stageFlag += 1;
                }
                stageComboCount = 0;
                
                break;

            case 2: //�X�e�[�W2�N���A����:�c���1�R���{(5�R���{�ȏ�ŏo��)
                Debug.Log("�R���{��" + stageComboCount);
                if (stageComboCount >= 5)
                {
                    //���̎��C�x���g

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
            case 3: //�΃h���b�v
                comboData[2, 0] += 1;
                Debug.Log("�ΐ� " + comboData[2, 0]);
                break;
            case 4: //�Ńh���b�v
                comboData[3, 0] += 1;
                Debug.Log("�Ő� " + comboData[3, 0]);
                break;
            case 5: //���h���b�v
                comboData[4, 0] += 1;
                Debug.Log("���� " + comboData[4, 0]);
                break;
            case 6: //�n�[�g�h���b�v
                comboData[5, 0] += 1;
                Debug.Log("�n�[�g�� " + comboData[5, 0]);
                break;
        }
    }

   


    //�R���{���X�V(���Z�b�g)
    public static void ComboReset()
    {
        for (int i = 0; i < 6; i++) {
            for (int j = 0; j < 3; j++)
            {
                comboData[i, j] = 0;
            }
        }
    }
    //�R���{���X�V(�R���{��)
    public static void MaxComboCHK(int num)
    {
        switch (num)
        {
            case 0: //�΃h���b�v
                comboData[0, 0] += 1;
                stageComboCount += 1;
                //Debug.Log("�ΐ� " + comboData[0, 0]);
                break;
            case 1: //���h���b�v
                comboData[1, 0] += 1;
                stageComboCount += 1;
                //Debug.Log("���� " + comboData[1, 0]);
                break;
            case 2: //�΃h���b�v
                comboData[2, 0] += 1;
                stageComboCount += 1;
                //Debug.Log("�ΐ� " + comboData[2, 0]);
                break;
            case 3: //�Ńh���b�v
                comboData[3, 0] += 1;
                stageComboCount += 1;
                //Debug.Log("�Ő� " + comboData[3, 0]);
                break;
            case 4: //���h���b�v
                comboData[4, 0] += 1;
                stageComboCount += 1;
                //Debug.Log("���� " + comboData[4, 0]);
                break;
            case 5: //�n�[�g�h���b�v
                comboData[5, 0] += 1;
                stageComboCount += 1;
                //Debug.Log("�n�[�g�� " + comboData[5, 0]);
                break;
        }
    }
    //�R���{���X�V(����)
    public static void RowComboCHK(int colour)
    {
        switch(colour)
        {
            case 0: //�΃h���b�v
                comboData[0, 1] += 1;
                //Debug.Log("���� "+ comboData[0, 1]);
                break;
            case 1: //���h���b�v
                comboData[1, 1] += 1;
                //Debug.Log("���� " + comboData[1, 1]);
                break;
            case 2: //�΃h���b�v
                comboData[2, 1] += 1;
                //Debug.Log("���� " + comboData[2, 1]);
                break;
            case 3: //�Ńh���b�v
                comboData[3, 1] += 1;
                //Debug.Log("���� " + comboData[3, 1]);
                break;
            case 4: //���h���b�v
                comboData[4, 1] += 1;
                //Debug.Log("���� " + comboData[4, 1]);
                break;
            case 5: //�n�[�g�h���b�v
                comboData[5, 1] += 1;
                //Debug.Log("���n�[�g " + comboData[5, 1]);
                break;
        }
    }
    //�R���{���X�V(�c��)
    public static void FileComboCHK(int colour)
    {
        switch (colour)
        {
            case 0: //�΃h���b�v
                comboData[0, 2] += 1;
                //Debug.Log("�c�� " + comboData[0, 2]);
                break;
            case 1: //���h���b�v
                comboData[1, 2] += 1;
                //Debug.Log("�c�� " + comboData[1, 2]);
                break;
            case 2: //�΃h���b�v
                comboData[2, 2] += 1;
                //Debug.Log("�c�� " + comboData[2, 2]);
                break;
            case 3: //�Ńh���b�v
                comboData[3, 2] += 1;
                //Debug.Log("�c�� " + comboData[3, 2]);
                break;
            case 4: //���h���b�v
                comboData[4, 2] += 1;
                //Debug.Log("�c�� " + comboData[4, 2]);
                break;
            case 5: //�n�[�g�h���b�v
                comboData[5, 2] += 1;
                //Debug.Log("�c�n�[�g " + comboData[5, 2]);
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
