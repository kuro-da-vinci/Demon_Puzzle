using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class AspectKeeper : MonoBehaviour
{
    //�Q�l�����Fhttps://3dunity.org/game-create-lesson/clicker-game/mobile-adjustment/
    [SerializeField]
    private Camera targetCamera; //�ΏۂƂ���J����

    [SerializeField]
    private Vector2 aspectVec; //�ړI�𑜓x

    private float posRange = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var screenAspect = Screen.width / (float)Screen.height; //��ʂ̃A�X�y�N�g��
        var targetAspect = aspectVec.x / aspectVec.y; //�ړI�̃A�X�y�N�g��

        var magRate = targetAspect / screenAspect; //�ړI�A�X�y�N�g��ɂ��邽�߂̔{��

        var viewportRect = new Rect(0, 0, 1, 1); //Viewport�����l��Rect���쐬

        posRange = (aspectVec.x * (Screen.height / aspectVec.y)) / 12; //�h���b�v���������̑���

        /*
        Debug.Log("�@�S�̏c�T�C�Y:" + Screen.height);
        Debug.Log("�A�ڕW�c�T�C�Y:" + aspectVec.y);
        Debug.Log("�B�䗦:" + Screen.height / aspectVec.y);
        Debug.Log("�C����:" + aspectVec.x * (Screen.height / aspectVec.y));
        Debug.Log("�D12������:" + (aspectVec.x * (Screen.height / aspectVec.y)) / 12);
        Debug.Log("�E��ʉ�:" + Screen.width / 12);
        Debug.Log("�Fpos:" + posRange);
        */

        if (magRate < 1)
        {
            viewportRect.width = magRate; //�g�p���鉡����ύX
            viewportRect.x = 0.5f - viewportRect.width * 0.5f;//������
        }
        else
        {
            viewportRect.height = 1 / magRate; //�g�p����c����ύX
            viewportRect.y = 0.5f - viewportRect.height * 0.5f;//������
        }
        
        targetCamera.rect = viewportRect; //�J������Viewport�ɓK�p
    }

    public float GetPosRange
    {
        
        get { return this.posRange; }  //�擾�p
        private set { this.posRange = value; } //�l���͗p
        
    }

}
