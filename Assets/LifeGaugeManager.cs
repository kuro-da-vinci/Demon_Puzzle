using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeGaugeManager : MonoBehaviour
{
    //HP
    [SerializeField]
    private int hp;

    //�@���C�t�Q�[�W�v���n�u
    [SerializeField]
    private GameObject lifeObj;

    void Start()
    {
        //�@�̗͂̏�����
        hp = 5;
        //�@�̗̓Q�[�W�ɔ��f
        SetLifeGauge(hp);
    }

    // Update is called once per frame
    void Update()
    {
        //�@�L�����N�^�[���쏈��
    }

    //�@�_���[�W�������\�b�h�i�S�폜��HP���쐬�j
    public void Damage(int damage)
    {
        hp -= damage;
        //�@0��艺�̐��l�ɂȂ�Ȃ��悤�ɂ���
        //hp = Mathf.Max(0, hp);

        if (hp >= 0)
        {
            SetLifeGauge(hp);
        }
    }
    //�@�_���[�W�������\�b�h�i�_���[�W���������A�C�R�����폜�j
    public void Damage2(int damage)
    {
        hp -= damage;
        if (hp < 0)
        {
            //�@�_���[�W����
            damage = Mathf.Abs(hp + damage);
            hp = 0;
        }
        if (damage > 0)
        {
            SetLifeGauge2(damage);
        }
    }





    //�@���C�t�Q�[�W�S�폜��HP���쐬
    public void SetLifeGauge(int life)
    {
        //�@�̗͂���U�S�폜
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        //�@���݂̗̑͐����̃��C�t�Q�[�W���쐬
        for (int i = 0; i < life; i++)
        {
            Instantiate<GameObject>(lifeObj, transform);
        }
    }
    //�@�_���[�W�������폜
    public void SetLifeGauge2(int damage)
    {
        for (int i = 0; i < damage; i++)
        {
            //�@�Ō�̃��C�t�Q�[�W���폜
            Destroy(transform.GetChild(i).gameObject);
            //Destroy(transform.GetChild(transform.childCount - 1 - i).gameObject);
        }
    }
}
