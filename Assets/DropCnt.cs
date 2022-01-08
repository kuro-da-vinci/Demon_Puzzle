using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class DropCnt : MonoBehaviour
{
    //DropCnt�̃C���X�y�N�^�[�E�B���h�E�Ƀh���b�v�e�N�X�`���p��sp�t�B�[���h��ǉ�
    [SerializeField] Sprite[] sp;
    //�h���b�v����ւ��pID�̕ύX�v���p�e�B�̐錾
    public int ID1
    {
        get;
        set;
    }
    public int ID2
    {
        get;
        set;
    }
    //��ʃ^�b�`�t���O�ϐ��̐錾(true,false)
    bool touchFlag;
    //�h���b�v�ړ��p�̍��W�ϐ�(Vector2�^)�̐錾
    Vector2 m;
    //GUI�pTransform�̕ϐ�(RectTransform�^)�̐錾
    RectTransform r, r2;
    //�h���b�v�����ʒu�ۑ��p�ϐ��̐錾
    public Vector2 P1
    {
        get;
        set;
    }
    public Vector2 P2
    {
        get;
        set;
    }

    Director d;

    // Start is called before the first frame update
    void Start()
    {

        //GUI�pTransform(RectTransform�^)���擾
        //��r = transform as RectTransform;�̕��������������炵��(GetComponent���x���炵��)
        r = GetComponent<RectTransform>();
        //�e��GUI�pTransform(RectTransform�^)���擾
        r2 = transform.parent.GetComponent<RectTransform>();
        //
        d = GameObject.Find("D").GetComponent<Director>();
    }

    // Update is called once per frame
    void Update()
    {
        //�h���b�v�^�b�`���Ȃ烋�[�v(touchFlag��true�Ȃ烋�[�v)
        if(touchFlag)
        {
            //pos��Vector2(0, 0)���i�[
            var pos = Vector2.zero;
            //��ʃ^�b�`�������W���擾(�J����(Camera.main)��̃X�N���[����ԍ��W)
            m = Input.mousePosition;
            //�J����(Camera.main)��̃X�N���[����ԍ��W(m)��GUI�pTransform(RectTransform�^)�̍��W�ɕϊ����Apos�ɃA�E�g�v�b�g
            RectTransformUtility.ScreenPointToLocalPointInRectangle
                (r2, m, Camera.main, out pos);
            //�e��Transform�猩�����ΓI�Ȉʒu��pos��ʂ��^�b�`�����ʒu�ɕύX
            r.localPosition = pos;
        }

        //�h���b�v�����ʒu�����ۑ��̏ꍇ
        if(P1.x == 0)
        {
            //RectTransform��position���擾
            P1 = GetComponent<RectTransform>().position;
            //RectTransform�����[���h���W�ɕύX����
            P2 = RectTransformUtility.WorldToScreenPoint(Camera.main, P1);
        }
        else
        {
            //�h���b�v���^�b�`���Ă��Ȃ�(������)�ꍇ
            if(!touchFlag)
            {
                //�����ʒu�֖߂�
                GetComponent<RectTransform>().position = P1;
            }
        }
    }

    public void Set(int n)
    {
        //�R���|�[�l���g(SpriteRenderer)���擾���Asprite�Ƀ����_���l�����Ƃ�sp����e�N�X�`����ݒ肷��
        GetComponent<SpriteRenderer>().sprite = sp[n];
    }

    //drop�I�u�W�F�N�g�̃C�x���g�g���K�[�Ŕ���
    //�h���b�v���^�b�`���Ă���ꍇ
    public void GetDrop()
    {
        touchFlag = true;
    }
    //�h���b�v�𗣂����ꍇ
    public void SetDrop()
    {
        touchFlag = false;
        //Delete�֐����R�[��
        Delete();
    }
    //�h���b�v�������ƏՓ˂����ꍇ�̏���
    private void OnCollisionStay2D(Collision2D collision)
    {
        //�h���b�v�^�b�`���̏ꍇ
        if(touchFlag)
        {
            //�^�b�`���̃h���b�v�ƏՓ˂����h���b�v���߂��ꍇ
            if(d.CheckPos(m, collision.gameObject.GetComponent<DropCnt>().P2))
            {
                //�h���b�v�����ւ���
                d.ChangePos(gameObject, collision.gameObject);
            }
        }
    }
    //Delete����(�񓯊�)
    private async void Delete()
    {
        while (true)
        {
            //�Ֆʃ��b�N
            GameObject.Find("PuzllBlock").GetComponent<Image>().enabled = true;
            GameObject.Find("PuzllBlock").GetComponent<GraphicRaycaster>().enabled = true;
            //�h���b�v�폜�������R�[��
            d.DeleteDrop();
            //�Ֆʂɍ폜�e�N�X�`����������΃��[�v�𔲂���
            if (d.Check())
            {
                GameObject.Find("D").GetComponent<ComboSystem>().ResetComboText();
                int ComboCount = FlagManager.stageComboCount;
                FlagManager.ClearCHK(this);
                FlagManager.ComboReset();
                
                if (!FlagManager.stageClearFlag)
                {
                    //�_���[�W�����p���Ԓ���
                    if (ComboCount > 0)
                        await Task.Delay(1500);
                    //�Ֆʃ��b�N����
                    GameObject.Find("PuzllBlock").GetComponent<GraphicRaycaster>().enabled = false;
                    GameObject.Find("PuzllBlock").GetComponent<Image>().enabled = false;
                }
                break;
            }
            //�R���{���I���܂őҋ@
            int wait = d.Wait();
            //Debug.Log(wait);
            await Task.Delay(wait);
            //�h���b�v�����������R�[��
            d.DownDrop();
            await Task.Delay(500);
            //�h���b�v�ǉ��������R�[��
            d.ResetDrop();
            await Task.Delay(500);
            
        }
    }
}

