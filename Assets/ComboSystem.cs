using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboSystem : MonoBehaviour
{
    [SerializeField] GameObject[] comboObj;
    [SerializeField] AnimationCurve scaleCurve;

    int counter = 0;
    float timer = 1.0f;
 
    Text comboNum;
    Text comboText;
    RectTransform comboNumScal;
    RectTransform comboTextScal;

    void Start()
    {
        comboNum = comboObj[0].GetComponent<Text>();
        comboText = comboObj[1].GetComponent<Text>();
        comboNumScal = comboObj[0].GetComponent<RectTransform>();
        comboTextScal = comboObj[1].GetComponent<RectTransform>();
        //�e�L�X�g�̔�\��
        comboNum.enabled = false;
        comboText.enabled = false;

    }


    void Update()
    {   
        //�R���{�e�L�X�g�̃A�j���[�V����
        timer += Time.deltaTime;
        comboNumScal.localScale = new Vector3(1, 1, 1) * (scaleCurve.Evaluate(timer));
    }

    public void IncreaseCombo()
    {
        //�R���{�J�n�F�R���{���J�E���g�A�b�v���e�L�X�g�\��
        counter++;
        comboNum.text = counter.ToString();
        comboNum.enabled = true;
        comboText.enabled = true;
        //�A�j���[�V�����̃��Z�b�g
        timer = 0.0f;
    }

    public void ResetComboText() 
    {
        //�R���{�I���F�e�L�X�g��\��
        comboNum.enabled = false;
        comboText.enabled = false;
        counter = 0;
    }

    /*
    [SerializeField] string comboWord;
    [SerializeField] GameObject comboObj;
    [SerializeField] AnimationCurve scaleCurve;
    [SerializeField, Range(0.2f, 5.0f)] float initEffectDuration = 0.2f; //�������o����
    [SerializeField, Range(0.01f, 5.0f)] float durationIncrement = 0.03f; //��������
    [SerializeField, Range(0.4f, 5.0f)] float maxEffectDuration = 1.0f; //�ő剉�o����
    [SerializeField, Range(10.0f, 15.0f)] float initMaxScale = 10.0f; //�����ő�X�P�[��
    [SerializeField, Range(0.2f, 1f)] float scaleIncrement = 0.6f; //�X�P�[��������
    [SerializeField, Range(10, 15)] float maxScale = 10f; //�ő�X�P�[��
    [SerializeField, Range(0, 0.2f)] float basicScaleIncrement = 0.09f; //���o��̕����̑傫���̑�����

    Text comboText;
    RectTransform comboRectTrans;

    int counter = 0;
    bool playingEffect = false;
    float scale;
    float basicScale;
    float effectDuration;
    float timer = 0f;
    Coroutine effectCol;
    Queue<int> comboOrder = new Queue<int>();

    void Awake()
    {
        comboText = comboObj.GetComponent<Text>();
        comboRectTrans = comboObj.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (comboOrder.Count == 0) return;

        timer += Time.deltaTime;
        var tempRate = Mathf.Clamp((1f - counter / 10f), 0.3f, 0.5f); //�R���{�����傫���قǒZ���ԂŎ��̕\��
        if (timer > effectDuration * tempRate)
        {
            timer = 0;
            UpdateCombo(comboOrder.Dequeue());
        }
    }

    public void IncreaseCombo()
    {
        counter++;
        comboOrder.Enqueue(counter);
        if (counter == 1) //����̂�
            UpdateCombo(comboOrder.Dequeue());
    }

    /// <summary>
    /// �R���{�̍X�V.
    /// </summary>
    /// <param name="comboCount">�R���{��</param>
    void UpdateCombo(int comboCount)
    {
        Show();
        comboText.text = comboCount + comboWord;

        comboRectTrans.localRotation = Quaternion.Euler(0, 0, Random.Range(-15f, 15f));

        if (playingEffect)
        { //�O�̃R���{���o���I�����ĂȂ��ꍇ
            StopCoroutine(effectCol);
            if (effectDuration < maxEffectDuration)
            {
                effectDuration += durationIncrement;
            }
            if (scale < maxScale)
            {
                scale += scaleIncrement;
            }
            if (counter < 7)
            { //7�R���{�܂ł͏����X�P�[����傫������
                basicScale += basicScaleIncrement;
            }
        }
        else
        {
            scale = initMaxScale;
            basicScale = 1;
            effectDuration = initEffectDuration;
        }

        effectCol = StartCoroutine(PlayEffect(effectDuration));
    }

    void Show()
    {
        comboText.enabled = true;
    }

    void Hide()
    {
        comboText.enabled = false;
    }

    void Clear()
    {
        counter = 0;
        comboOrder.Clear();
        Hide();
    }

    /// <summary>
    /// ���o.�e�L�X�g�̑傫����偨���ɂ���.
    /// </summary>
    /// <param name="duration">����(sec)</param>
    IEnumerator PlayEffect(float duration)
    {
        var timer = 0f;
        var rate = 0f;
        var startScale = new Vector3(scale, scale, 1);
        var endScale = new Vector3(basicScale, basicScale, 1);

        playingEffect = true;
        while (rate < 1)
        {
            timer += Time.deltaTime;
            rate = Mathf.Clamp01(timer / duration);
            var curvePos = scaleCurve.Evaluate(rate);
            comboRectTrans.localScale = Vector3.Lerp(startScale, endScale, curvePos);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        Clear();

        playingEffect = false;
    }
    */
}