using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

//�Q�l����
//https://nn-hokuson.hatenablog.com/entry/2019/02/28/205346
//https://3dunity.org/game-create-lesson/action-pazzle-game/touch-array-introduction/


public class oldCreateThread : MonoBehaviour
{

    public bool startFlg = false;
    public bool endFlg = false;

    [SerializeField]    private RawImage m_image = null;    //�����`��p�C���[�W
    [SerializeField]    private RawImage m_image2 = null;   //�蓮�`��p�C���[�W
    [SerializeField]    GameObject image_Parent;            //�e�I�u�W�F�N�g�̎擾

    private Texture2D m_texture = null;                     //�����`��p2D�e�N�X�`��
    private Texture2D m_texture2 = null;                    //�蓮�`��p2D�e�N�X�`��

    [SerializeField]    private int m_width = 20;           //�`����̉��̑���
    [SerializeField]    private int m_height = 20;          //�`����̏c�̑���

    private Vector2 m_prePos, m_drawPos;        //�����`��p�|�W�V�����ϐ�
    private Vector2 m_preTouchPos, m_TouchPos;  //�蓮�`��p�|�W�V�����ϐ�
    private Vector2 m_disImagePos;              //2D�e�N�X�`���N�_�|�W�V�����ϐ�

    private float m_drawTime, m_preDrawTime;    //�����`��p
    private float m_clickTime, m_preClickTime;  //�蓮�`��p

    private Color setColor   = new Color32(255, 255, 255, 255); //�����`��p�J���[(��)
    private Color setColor2  = new Color32(125, 125, 125, 255); //�蓮�`��p�J���[(�D�F)
    private Color colorClear = new Color32(0, 0, 0, 0);         //�N���A�p�J���[(����)


    void Start()
    {

        //Image�T�C�Y��2D�e�N�X�`���쐬(�����`��p�A�蓮�`��p) 
        var rect = m_image.gameObject.GetComponent<RectTransform>().rect;
        m_texture = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGBA32, false);
        m_texture2 = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGBA32, false);
        m_image.texture = m_texture;
        m_image2.texture = m_texture2;

        //Image�̋N�_���擾
        var m_imagePos = m_image.gameObject.GetComponent<RectTransform>().anchoredPosition;
        m_disImagePos = new Vector2(m_imagePos.x - rect.width / 2, m_imagePos.y - rect.height / 2);
        Debug.Log(m_disImagePos);

        //2D�e�N�X�`���̃s�N�Z���f�[�^���擾
        var pixelData = m_texture.GetPixelData<Color32>(0);
        var pixelData2 = m_texture2.GetPixelData<Color32>(0);

        //2D�e�N�X�`���𓧖��ɐݒ�
        for (var i = 0; i < pixelData.Length; i++)
        {
            pixelData[i] = colorClear;
            pixelData2[i] = colorClear;
        }

        m_texture.Apply();
        m_texture2.Apply();
    }

    //���̎�(�����`��)
    public void OnDrag(Vector3 pos, float time )
    {
        //https://light11.hatenadiary.com/entry/2019/04/16/003642
        //LineRenderer�̍ŐV�̕`�掞�Ԃ��擾
        m_drawTime = time;
        Vector2 localPoint = pos;                   //���[�J�����W��Vector3����Vector2�֕ϊ�
        m_drawPos = localPoint - m_disImagePos;     //���[�J�����W���e�N�X�`�����W�ɍ��킹��

        float disTime = m_drawTime - m_preDrawTime; //�O��̃N���b�N�C�x���g�Ƃ̎���

        int width = m_width;  //�y���̑���(�s�N�Z��)
        int height = m_height; //�y���̑���(�s�N�Z��)

        var dir = m_prePos - m_drawPos; //���O�̃^�b�`���W�Ƃ̍�
        if (disTime > 0.01) dir = new Vector2(0, 0); //0.1�b�ȏ�Ԋu����������^�b�`���W�̍���0�ɂ���

        var dist = (int)dir.magnitude; //�^�b�`���W�x�N�g���̐�Βl

        dir = dir.normalized; //���K��

        //�w��̃y���̑���(�s�N�Z��)�ŁA�O��̃^�b�`���W���獡��̃^�b�`���W�܂œh��Ԃ�
        var pixelData = m_texture.GetPixelData<Color32>(0);
        var pixelData2 = m_texture2.GetPixelData<Color32>(0);
        for (int d = 0; d < dist; ++d)
        {
            var p_pos = m_drawPos + dir * d; //paint position
            p_pos.y -= height / 2.0f;
            p_pos.x -= width / 2.0f;

            for (int h = 0; h < height; ++h)
            {
                int y = (int)(p_pos.y + h);
                if (y < 0 || y >= m_texture.height) continue; //�^�b�`���W���e�N�X�`���̊O�̏ꍇ�A�`�揈�����s��Ȃ�
                for (int w = 0; w < width; ++w)
                {
                    int x = (int)(p_pos.x + w);
                    if (x >= 0 && x <= m_texture.width)
                    {
                        var i = (int)(x + y * 1644);
                        pixelData[i] = setColor;
                        pixelData2[i] = setColor2;
                    }
                }
            }
        }

        m_texture.Apply();
        m_texture2.Apply();
        m_prePos = m_drawPos;
        m_preDrawTime = m_drawTime;
    }

    //���̎��N���A(�蓮�`��p)
    public void ClearDrag(BaseEventData arg)
    {
        //�����`��I��������true
        if (endFlg)
        {
         
            PointerEventData _event = arg as PointerEventData; //�^�b�`�̏��擾

            m_TouchPos = _event.position;   //���݂̃X�N���[�����W
            m_clickTime = _event.clickTime; //�Ō�ɃN���b�N�C�x���g�����M���ꂽ���Ԃ��擾

            //https://light11.hatenadiary.com/entry/2019/04/16/003642
            // �N���b�N�ʒu�ɑΉ�����X�N���[�����W�����[�J�����W�ɕϊ�
            var localPoint = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(image_Parent.GetComponent<RectTransform>(), _event.position, Camera.main, out localPoint);
            m_TouchPos = localPoint - m_disImagePos;    //���[�J�����W���e�N�X�`�����W�ɍ��킹��

            float disTime = m_clickTime - m_preClickTime; //�O��̃N���b�N�C�x���g�Ƃ̎���

            int width = m_width * 3;  //�y���̑���(�s�N�Z��)
            int height = m_height * 3; //�y���̑���(�s�N�Z��)

            var dir = m_preTouchPos - m_TouchPos; //���O�̃^�b�`���W�Ƃ̍�
            if (disTime > 0.01) dir = new Vector2(0, 0); //0.1�b�ȏ�Ԋu����������^�b�`���W�̍���0�ɂ���

            var dist = (int)dir.magnitude; //�^�b�`���W�x�N�g���̐�Βl

            dir = dir.normalized; //���K��

            //�w��̃y���̑���(�s�N�Z��)�ŁA�O��̃^�b�`���W���獡��̃^�b�`���W�܂œh��Ԃ�
            var pixelData2 = m_texture2.GetPixelData<Color32>(0);
            for (int d = 0; d < dist; ++d)
            {
                var p_pos = m_TouchPos + dir * d; //paint position
                p_pos.y -= height/ 2.0f;
                p_pos.x -= width / 2.0f;

                for (int h = 0; h < height; ++h)
                {
                    int y = (int)(p_pos.y + h);
                    if (y < 0 || y >= m_texture2.height) continue; //�^�b�`���W���e�N�X�`���̊O�̏ꍇ�A�`�揈�����s��Ȃ�
                    for (int w = 0; w < width; ++w)
                    {
                        int x = (int)(p_pos.x + w);
                        if (x >= 0 && x <= m_texture2.width)
                        {
                            var i = (int)(x + y * 1644);
                            pixelData2[i] = colorClear;
                        }
                    }
                }
            }

            m_texture2.Apply();
            m_preTouchPos = m_TouchPos;
            m_preClickTime = m_clickTime;

        }
    }

    //���̎��̓h��Ԃ��ʂ��r(�����`�� vs �蓮�`��)
    public bool ColorChk()
    {
        var pixelData   = m_texture.GetPixelData<Color32>(0);   //�����`��
        var pixelData2  = m_texture2.GetPixelData<Color32>(0);  //�蓮�`��
        int compareColorA = 0;                                  //��r��(�����`��)
        int compareColorB = 0;                                  //��r��(�蓮�`��)

        //�s�N�Z�������[�v
        for (var i = 0; i < pixelData.Length; i++)
        {
            if (pixelData[i] == setColor)                       //�����`��ʂ��擾
                compareColorA++;
            if (pixelData2[i] == setColor2)                     //�蓮�`��ʂ��擾
                compareColorB++;
        }

        //2D�e�N�X�`���𓧖��Ƀ��Z�b�g
        for (var i = 0; i < pixelData.Length; i++)
        {
            pixelData[i] = colorClear;
            pixelData2[i] = colorClear;
        }

        m_texture.Apply();
        m_texture2.Apply();

        Debug.Log("compareColorA:" + compareColorA + "/compareColorB:" + compareColorB);
        //�����`���80���ȏ�h��Ԃ��Ă���Γ˔j
        if (compareColorA*0.2 >= compareColorB )
        {
            Debug.Log("�˔j"+ compareColorA * 0.8);
            return true;
        }

        return false;
        
    }


        void Update()
    {
        if (startFlg)
        {
            //�I�u�W�F�N�g�̃��[�J���|�W�V�������擾
            var rectpos = this.transform.localPosition;
            OnDrag(rectpos, Time.deltaTime);
        }
    }



    //���̎��̃��[�g�������_���Ō���
    public void OpeningThreadStart()
    {
        int num = Random.Range(3, 6);
        float x = 0;
        float y = 0;
        float z = -3.0f;
        Vector3[] path = new Vector3[num];

        GameObject.Find("OpeningThreadPanel").GetComponent<Image>().enabled = true;
        GameObject.Find("OpeningThreadPanel").GetComponent<GraphicRaycaster>().enabled = true;

        for (int i = 0; i < num; i++)
        {
            if (i == 0)                             //�X�^�[�g�ʒu
            {
                x = -900.0f;
                y = Random.Range(0.0f, 1000.0f);
            }
            else if (i == num - 1)                  //�S�[���ʒu
            {
                x = 900.0f;
                y = Random.Range(0.0f, 1000.0f);
            }
            else                                    //���Ԉʒu
            {
                x = Random.Range(-750.0f, 750.0f);
                y = Random.Range(0.0f, 1000.0f);
            }
            path[i] = new Vector3(x, y, z);
        }


        startFlg = true;
        transform.DOLocalPath(path, 1.5f, PathType.CatmullRom)
                .SetEase(Ease.Linear)
                .SetOptions(false, AxisConstraint.Z)
                .OnComplete(() => startFlg = false)
                .OnComplete(() => endFlg = true);
    }

    //���̎��I��
    public bool OpeningThreadEnd()
    {
        Debug.Log("���̎��I���t���O"+startFlg);
        endFlg = false;

        GameObject.Find("OpeningThreadPanel").GetComponent<Image>().enabled = false;
        GameObject.Find("OpeningThreadPanel").GetComponent<GraphicRaycaster>().enabled = false;

        return ColorChk();
        
    }

 
}
