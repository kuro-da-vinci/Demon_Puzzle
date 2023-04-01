using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlagManager : MonoBehaviour
{
    //�N���A�����i�[�p�ϐ�
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
    public static void ClearCHK(MonoBehaviour i_behaviour, bool ChangeFlag)
    {
        switch (stageFlag)
        {
            case 1: //�X�e�[�W�P�N���A����:3�R���{��2��
                i_behaviour.StartCoroutine(Stage1_Process(ChangeFlag));
                break;
                

            case 2: //�X�e�[�W2�N���A����:�c���1�R���{(���v10�R���{�ȏ�ŏo��)
                i_behaviour.StartCoroutine(Stage2_Process(ChangeFlag));
                break;
        }
    }

   


    //�R���{���X�V(���Z�b�g)
    public static void ComboReset()
    {
        for (int i = 0; i < 6; i++) {
            for (int j = 0; j < 3; j++)
            {
                Debug.Log("�R���{���Z�b�g");
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
            //�_���[�W
            gameOverFlag = GameObject.Find("Panel").GetComponent<LifeGaugeManager>().Damage(1);
            if (gameOverFlag == true)
            {
                stageClearFlag = true;
                yield return GameOverScene();
                stageFlag = 4;
            }

            EHitCl.GetComponent<SpriteRenderer>().enabled = false;
        }

        Debug.Log("�G�l�~�[�A�^�b�N�F" + OpeningThreadChkFlag);
        OpeningThreadChkFlag = false;
        OpeningThreadFlag = false;
    }

    private static IEnumerator HPRecovery(int rec)
    {
        yield return new WaitForSeconds(0.5f);
        //HP��
        GameObject.Find("Panel").GetComponent<LifeGaugeManager>().Damage(rec * (-1));
    }

    private static IEnumerator NextScene()
    {
        yield return new WaitForSeconds(0.0f);

        if (OpeningThreadChkFlag)
        {
            Debug.Log("�N���A");
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
        Debug.Log("�l�N�X�g�V�[���F" + OpeningThreadChkFlag);
        Debug.Log("�p��");

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
        //3�R���{�ȏ�Ȃ�U���G�t�F�N�g
        if (stageComboCount >= 3)
        {
            stage1ComboCount += 1;
            yield return  AttackEffect();

            //�񕜏���
            if (comboData[5, 0] != 0)
                yield return HPRecovery(comboData[5, 0]);

            //3�R���{�ȉ�1�R���{�ȏ�Ȃ�Hit�G�t�F�N�g���_���[�W����
        }
        else if (stageComboCount >= 0 && cFlag == true)
        {
            //�񕜈ȊO�̃h���b�v�������Ă����Hit�G�t�F�N�g
            if (comboData[0, 0] != 0 || comboData[1, 0] != 0 ||
                comboData[2, 0] != 0 || comboData[3, 0] != 0 || comboData[4, 0] != 0)
            {
                int HitNum = stageComboCount - comboData[5, 0];
                yield return HitEffect(HitNum);
            }
            //�񕜏���
            if (comboData[5, 0] != 0)
                yield return HPRecovery(comboData[5, 0]);

            //�_���[�W����
            yield return EnemyAttack();
        }

        //�N���A����
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
        //���������J���̏����F���v10�R���{�ȏ�
        //���������F�c���R���{

        //�g�[�^���R���{���̃J�E���g
        stageTotalComboCount += stageComboCount;

        //�����J��CHK
        if (stageTotalComboCount >= 10)
        {
            stage2ClearFlag = true;
            GameObject.Find("ConditionsText").GetComponent<Text>().enabled = false;
            GameObject.Find("VictoryConditions").GetComponent<Text>().enabled = true;
        }

        //�������������̏c���1�R���{CHK
        if (stage2ClearFlag == true && comboData[0, 2] >= 1 || stage2ClearFlag == true && comboData[1, 2] >= 1
            || stage2ClearFlag == true && comboData[2, 2] >= 1 || stage2ClearFlag == true && comboData[3, 2] >= 1
            || stage2ClearFlag == true && comboData[4, 2] >= 1)
        {
            //���̎��C�x���g
            OpeningThreadFlag = true;

            GameObject.Find("OpeningThread").GetComponent<CreateThread>().OpeningThreadStart();
            yield return new WaitForSeconds(5.0f);
            OpeningThreadChkFlag = GameObject.Find("OpeningThread").GetComponent<CreateThread>().OpeningThreadEnd();
            yield return new WaitForSeconds(1.5f);
            yield return ThreadEnd();
        }
        else if (stageComboCount >= 0 && cFlag == true)
        {
            //�񕜈ȊO�̃h���b�v�������Ă����Hit�G�t�F�N�g
            if (comboData[0, 0] != 0 || comboData[1, 0] != 0 ||
                comboData[2, 0] != 0 || comboData[3, 0] != 0 || comboData[4, 0] != 0)
            {
                int HitNum = stageComboCount - comboData[5, 0];
                Debug.Log("�U�������J�n");
                yield return HitEffect(HitNum);
            }
            Debug.Log("�U�������I��");
            //�񕜏���
            Debug.Log("�񕜁F" + comboData[5, 0]);
            if (comboData[5, 0] != 0)
            {
                Debug.Log("�񕜏����J�n");
                yield return HPRecovery(comboData[5, 0]);
            }
            Debug.Log("�񕜏����I��");

            Debug.Log("�_���[�W�����J�n");
            //�_���[�W����
            yield return EnemyAttack();
            Debug.Log("�_���[�W�����I��");
        }
        stageComboCount = 0;
    }

}
