using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Director : MonoBehaviour
{
    //�S�h���b�v�Ǘ��p�z���錾
    GameObject[,] o = new GameObject[5, 6];
    public GameObject[,] Obj
    {
        get { return o; }
        set { o = value; }
    }
    //�Ֆʏ��Ǘ��p(�h���b�v�e�N�X�`��) �z��̐錾
    int[,] f = new int[5, 6];
    public int[,] Field
    {
        get { return f; }
        set { f = value; }
    }

    //public GameObject aspectKeeper;
    public AspectKeeper aspectKeeper;
    //private float MagRate = GameObject.Find("Main Camera").GetComponent<AspectKeeper>().GetMagRate();


    // Start is called before the first frame update
    void Start()
    {
        FadeManager.FadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�h���b�v�Ԃ̋�����}��A�h���b�v����ւ��\�����𔻒肷��
    public bool CheckPos(Vector2 p1,Vector2 p2)
    {
        //p1��p2�̃h���b�v���������߂�(�s�^�S���X�̒藝(a�~a+b�~b=c�~c))
        //p1��p2��x���W�̋���(a)
        float x = p1.x - p2.x;
        //p1��p2��y���W�̋���(b)
        float y = p1.y - p2.y;
        //Mathf.Sqrt�ŕ����������߂�(2��̋t)
        float r = Mathf.Sqrt(x * x + y * y);
        
        float PosRange = aspectKeeper.GetPosRange;


        //�h���b�v���m�̋������߂���
        if (r < PosRange)
        {
            //Debug.Log("�䗦" + aspectKeeper.GetMagRate);
            //Debug.Log("����1�F" + MagRate + " ����2" + r);
            return true;
        }
        return false;
    }

    //�h���b�v�����ւ�����(obj1:�^�b�`���̃h���b�v�Aobj2:�Փ˂����h���b�v)
    public void ChangePos(GameObject obj1,GameObject obj2)
    {
        //DropCnt������ϐ���p�ӂ��Aobj1,2�̃R���|�[�l���g(DropCnt)���擾
        DropCnt d1 = obj1.GetComponent<DropCnt>();
        DropCnt d2 = obj2.GetComponent<DropCnt>();
        //�h���b�v�{�̓���ւ��p�ϐ�
        GameObject tempObj;
        //
        Vector2 tempPos;
        int temp;

        //�h���b�v�{�̂̓���ւ�����
        tempObj = Obj[d1.ID1, d1.ID2];
        Obj[d1.ID1, d1.ID2] = Obj[d2.ID1, d2.ID2];
        Obj[d2.ID1, d2.ID2] = tempObj;

        //�h���b�v�e�N�X�`�����̓���ւ�����
        temp = Field[d1.ID1, d1.ID2];
        Field[d1.ID1, d1.ID2] = Field[d2.ID1, d2.ID2];
        Field[d2.ID1, d2.ID2] = temp;

        //�h���b�v���W(�����ʒu)�̓���ւ�����
        tempPos = d1.P1;
        d1.P1 = d2.P1;
        d2.P1 = tempPos;
        tempPos = d1.P2;
        d1.P2 = d2.P2;
        d2.P2 = tempPos;

        //�h���b�vID�̓���ւ�����
        temp = d1.ID1;
        d1.ID1 = d2.ID1;
        d2.ID1 = temp;
        temp = d1.ID2;
        d1.ID2 = d2.ID2;
        d2.ID2 = temp;
    }

    //�h���b�v�폜����
    private int[,] temp3 = new int[7, 8];
    private int[,] temp4 = new int[5, 6];
    int combCnt = 0;

    private void fill(int x, int y, int cl)
    { /* �h��Ԃ� */
        temp4[x - 1, y - 1] = combCnt;
        temp3[x, y] = -1; /* �[�P��u�� */
        if (temp3[x, y - 1] == cl) /* ��Ɏ����Ɠ����F�����邩 */
            fill(x, y - 1, cl); /* ����΂��̍��W�ōċA�Ăяo�� */

        if (temp3[x + 1, y] == cl) /* �E */
            fill(x + 1, y, cl);

        if (temp3[x, y + 1] == cl) /* �� */
            fill(x, y + 1, cl);

        if (temp3[x - 1, y] == cl) /* �� */
            fill(x - 1, y, cl);
    }

    public IEnumerator DeleteDrop()
    {
        //�אڃh���b�v�J�E���g�ϐ�(C)�A�h���b�v�e�N�X�`�����ϐ�(t)�̐錾
        int c = 0, t = 0;
        //����̃h���b�v�폜���Ǘ��p�z��̐錾
        int[,] temp = new int[5, 6];
        int[,] temp2 = new int[5, 6];
        combCnt = 0;

        //�z��̏�����
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 8; j++)
            { temp3[i, j] = -1; }
        }
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 6; j++)
            { temp4[i, j] = -1; }
        }

        //�폜�h���b�v�̑I��
        //����CHK���[�v
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                //��r�h���b�v���̐ݒ�(�����l���)
                if (j == 0)
                {
                    c = 1;
                    t = Field[i,j];
                    continue;
                }
                //��ƂȂ�̃h���b�v������e�N�X�`���̏ꍇ
                if(t == Field[i,j])
                {
                    //�J�E���g�A�b�v
                    c++;
                    //����e�N�X�`���̃h���b�v��3�ȏ�אڂ��Ă���ꍇ
                    if(c>=3)
                    {
                        //�폜�h���b�v������
                        temp[i, j] = c;
                    }
                    //@�h���b�v��4�ȏ�אڂ��Ă���ꍇ�A�Â��폜�h���b�v���̍폜
                    temp[i, j - 1] = 0;
                }
                //��ƂȂ�̃h���b�v������e�N�X�`���ł͂Ȃ��ꍇ
                else
                {
                    //��r�h���b�v�����X�V
                    c = 1;
                    t = Field[i,j];
                }
            }
        }
        //�c��CHK���[�v
        for (int j = 0; j < 6; j++)
        {
            for (int i = 0; i < 5; i++)
            {
                if (i == 0)
                {
                    c = 1;
                    t = Field[i,j];
                    continue;
                }
                if (t == Field[i, j])
                {
                    c++;
                    if (c >= 3)
                    {
                        temp2[i, j] = c;
                    }
                    //@�h���b�v��4�ȏ�אڂ��Ă���ꍇ�A�Â��폜�h���b�v���̍폜
                    temp2[i - 1, j] = 0;
                }
                else
                {
                    c = 1;
                    t = Field[i,j];
                }
            }
        }
        //@�폜�h���b�v�V�[�g�쐬
        for(int i = 0; i < 5; i++)
        {
            for(int j = 0; j < 6; j++)
            {
                //�����3�ȏ㓯��e�N�X�`���̃h���b�v���אڂ��Ă���ꍇ
                if (temp[i, j] >= 3)
                {
                    //�R���{�f�[�^�X�V(����)
                    if (temp[i,j] == 6)
                        FlagManager.RowComboCHK(Field[i,j]);

                    //�אڂ��Ă���h���b�v�������[�v
                    int temp_bk = temp[i, j];
                    for(int k = j; temp_bk > 0; k--, temp_bk--)
                    {
                        //�h���b�v�e�N�X�`������ǉ�
                        temp3[i+1, k+1] = Field[i, j];
                        //Debug.Log("i:" + i + ",j;" + k + " " + temp3[i, k]);
                    }
                }
                //�c���3�ȏ㓯��e�N�X�`���̃h���b�v���אڂ��Ă���ꍇ
                if (temp2[i, j] >= 3)
                {
                    //�R���{�f�[�^�X�V(�c��)
                    if (temp2[i, j] == 5)
                        FlagManager.FileComboCHK(Field[i, j]);

                    int temp_bk = temp2[i, j];
                    for (int k = i; temp_bk > 0; k--, temp_bk--)
                    {
                        temp3[k+1, j+1] = Field[i, j];
                        //Debug.Log("i:" + k + ",j;" + j + " " + temp3[k, j] + " " + Field[i,j]);
                    }
                }
            }
        }
        /*
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 8; j++)
            { Debug.Log("i:" + i + ",j;" + j + " " + temp3[i, j]); }
        }*/

        //@�R���{�V�[�g���쐬
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                //�R���{���������Ă���ꍇ
                if (temp3[i, j] >= 0 )
                {
                    //�R���{���X�V(�R���{��)
                    FlagManager.MaxComboCHK(temp3[i, j]);

                    //�h��Ԃ��������R�[��
                    //Debug.Log("�h��Ԃ��J�n");
                    combCnt += 1;
                    fill(i, j, temp3[i, j]);
                    //Debug.Log("�h��Ԃ��I��");
                }
            }
        }
        /*for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 6; j++)
            { Debug.Log("i:" + i + ",j;" + j + " " + temp4[i, j]); }
        }*/

        
        //�R���{�V�[�g�����ƂɃh���b�v�e�N�X�`���ύX����
        for (int n = 1; n <= combCnt; n++)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    //c�R���{�ڂȂ�
                    if (temp4[i, j] == n)
                    {
                        //�h���b�v�e�N�X�`�����ɍ폜�e�N�X�`��������
                        Field[i, j] = 6;
                        //GameObject��DropCnt����Set���R�[�����폜�e�N�X�`���ɕύX
                        Obj[i, j].GetComponent<DropCnt>().Set(6);
                    }
                }
            }
            //Debug.Log(n+"�R���{��");
            this.GetComponent<ComboSystem>().IncreaseCombo();
            //await Task.Delay(400);
            yield return new WaitForSeconds(0.4f);
        }
        //Debug.Log("conbCnt"+combCnt);
    }

    //�h���b�v��������
    public void DownDrop()
    {
        //Debug.Log("���������J�n");
        //�c�񃋁[�v
        for (int j = 0; j < 6; j++)
        {
            for(int i = 0; i < 5; i++)
            {
                //�폜�e�N�X�`���̏ꍇ
                if (Field[i,j] == 6)
                {
                    //��Ƀh���b�v������ꍇ���[�v
                    for(int k = i; k > 0; k--)
                    {
                        //��ɂ���h���b�v���폜�e�N�X�`���ȊO�̏ꍇ
                        if(Field[k - 1, j] != 6)
                        {
                            //��ɂ���h���b�v�Ɠ���ւ���
                            ChangePos(Obj[k, j], Obj[k - 1, j]);
                        }
                    }
                }
            }
        }
    }
    //�h���b�v�폜��̃h���b�v�ǉ�����
    public void ResetDrop()
    {
        for (int i =0 ; i < 5; i++)
        {
            for(int j = 0; j < 6; j++)
            {
                //�폜�e�N�X�`���̏ꍇ
                if(Field[i,j] == 6)
                {
                    //�����_���Ńh���b�v�e�N�X�`����I��
                    int type = Random.Range(0, 6);
                    //�h���b�v�e�N�X�`�������X�V
                    Field[i, j] = type;
                    //GameObject��DropCnt����Set���R�[�����e�N�X�`�����X�V
                    Obj[i, j].GetComponent<DropCnt>().Set(type);
                }
            }
        }
    }
    //�폜�e�N�X�`���L���`�F�b�N����
    public bool Check()
    {
        for(int i = 0; i < 5; i++)
        {
            for(int j = 0; j < 6; j++)
            {
                //�Ֆʂɍ폜�e�N�X�`��������ꍇ
                if(Field[i,j] == 6)
                {
                    return false;
                }
            }
        }
        return true;
    }
    //�ҋ@����
    public float Wait()
    {
        //int waitTime = combCnt * 400+500;
        float waitTime = combCnt * 0.4f + 0.5f;
        return waitTime;
    }
}


