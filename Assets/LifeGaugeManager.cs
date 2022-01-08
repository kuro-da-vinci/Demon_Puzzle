using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeGaugeManager : MonoBehaviour
{
    //HP
    [SerializeField]
    private int hp;

    //　ライフゲージプレハブ
    [SerializeField]
    private GameObject lifeObj;

    void Start()
    {
        //　体力の初期化
        hp = 5;
        //　体力ゲージに反映
        SetLifeGauge(hp);
    }

    // Update is called once per frame
    void Update()
    {
        //　キャラクター操作処理
    }

    //　ダメージ処理メソッド（全削除＆HP分作成）
    public void Damage(int damage)
    {
        hp -= damage;
        //　0より下の数値にならないようにする
        //hp = Mathf.Max(0, hp);

        if (hp >= 0)
        {
            SetLifeGauge(hp);
        }
    }
    //　ダメージ処理メソッド（ダメージ数分だけアイコンを削除）
    public void Damage2(int damage)
    {
        hp -= damage;
        if (hp < 0)
        {
            //　ダメージ調整
            damage = Mathf.Abs(hp + damage);
            hp = 0;
        }
        if (damage > 0)
        {
            SetLifeGauge2(damage);
        }
    }





    //　ライフゲージ全削除＆HP分作成
    public void SetLifeGauge(int life)
    {
        //　体力を一旦全削除
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        //　現在の体力数分のライフゲージを作成
        for (int i = 0; i < life; i++)
        {
            Instantiate<GameObject>(lifeObj, transform);
        }
    }
    //　ダメージ分だけ削除
    public void SetLifeGauge2(int damage)
    {
        for (int i = 0; i < damage; i++)
        {
            //　最後のライフゲージを削除
            Destroy(transform.GetChild(i).gameObject);
            //Destroy(transform.GetChild(transform.childCount - 1 - i).gameObject);
        }
    }
}
