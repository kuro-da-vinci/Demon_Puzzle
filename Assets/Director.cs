using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class Director : MonoBehaviour
{
    //全ドロップ管理用配列を宣言
    GameObject[,] o = new GameObject[5, 6];
    public GameObject[,] Obj
    {
        get { return o; }
        set { o = value; }
    }
    //盤面情報管理用(ドロップテクスチャ) 配列の宣言
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

    //ドロップ間の距離を図り、ドロップ入れ替え可能距離を判定する
    public bool CheckPos(Vector2 p1,Vector2 p2)
    {
        //p1とp2のドロップ距離を求める(ピタゴラスの定理(a×a+b×b=c×c))
        //p1とp2のx座標の距離(a)
        float x = p1.x - p2.x;
        //p1とp2のy座標の距離(b)
        float y = p1.y - p2.y;
        //Mathf.Sqrtで平方根を求める(2乗の逆)
        float r = Mathf.Sqrt(x * x + y * y);
        
        float PosRange = aspectKeeper.GetPosRange;


        //ドロップ同士の距離が近いか
        if (r < PosRange)
        {
            //Debug.Log("比率" + aspectKeeper.GetMagRate);
            //Debug.Log("距離1：" + MagRate + " 距離2" + r);
            return true;
        }
        return false;
    }

    //ドロップを入れ替え処理(obj1:タッチ中のドロップ、obj2:衝突したドロップ)
    public void ChangePos(GameObject obj1,GameObject obj2)
    {
        //DropCntを入れる変数を用意し、obj1,2のコンポーネント(DropCnt)を取得
        DropCnt d1 = obj1.GetComponent<DropCnt>();
        DropCnt d2 = obj2.GetComponent<DropCnt>();
        //ドロップ本体入れ替え用変数
        GameObject tempObj;
        //
        Vector2 tempPos;
        int temp;

        //ドロップ本体の入れ替え処理
        tempObj = Obj[d1.ID1, d1.ID2];
        Obj[d1.ID1, d1.ID2] = Obj[d2.ID1, d2.ID2];
        Obj[d2.ID1, d2.ID2] = tempObj;

        //ドロップテクスチャ情報の入れ替え処理
        temp = Field[d1.ID1, d1.ID2];
        Field[d1.ID1, d1.ID2] = Field[d2.ID1, d2.ID2];
        Field[d2.ID1, d2.ID2] = temp;

        //ドロップ座標(初期位置)の入れ替え処理
        tempPos = d1.P1;
        d1.P1 = d2.P1;
        d2.P1 = tempPos;
        tempPos = d1.P2;
        d1.P2 = d2.P2;
        d2.P2 = tempPos;

        //ドロップIDの入れ替え処理
        temp = d1.ID1;
        d1.ID1 = d2.ID1;
        d2.ID1 = temp;
        temp = d1.ID2;
        d1.ID2 = d2.ID2;
        d2.ID2 = temp;
    }

    //ドロップ削除処理
    private int[,] temp3 = new int[7, 8];
    private int[,] temp4 = new int[5, 6];
    int combCnt = 0;

    private void fill(int x, int y, int cl)
    { /* 塗りつぶし */
        temp4[x - 1, y - 1] = combCnt;
        temp3[x, y] = -1; /* ー１を置く */
        if (temp3[x, y - 1] == cl) /* 上に自分と同じ色があるか */
            fill(x, y - 1, cl); /* あればその座標で再帰呼び出し */

        if (temp3[x + 1, y] == cl) /* 右 */
            fill(x + 1, y, cl);

        if (temp3[x, y + 1] == cl) /* 下 */
            fill(x, y + 1, cl);

        if (temp3[x - 1, y] == cl) /* 左 */
            fill(x - 1, y, cl);
    }

    public IEnumerator DeleteDrop()
    {
        //隣接ドロップカウント変数(C)、ドロップテクスチャ情報変数(t)の宣言
        int c = 0, t = 0;
        //横列のドロップ削除情報管理用配列の宣言
        int[,] temp = new int[5, 6];
        int[,] temp2 = new int[5, 6];
        combCnt = 0;

        //配列の初期化
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

        //削除ドロップの選別
        //横列CHKループ
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                //比較ドロップ情報の設定(初期値代入)
                if (j == 0)
                {
                    c = 1;
                    t = Field[i,j];
                    continue;
                }
                //一つとなりのドロップが同一テクスチャの場合
                if(t == Field[i,j])
                {
                    //カウントアップ
                    c++;
                    //同一テクスチャのドロップが3つ以上隣接している場合
                    if(c>=3)
                    {
                        //削除ドロップ情報を代入
                        temp[i, j] = c;
                    }
                    //@ドロップが4以上隣接している場合、古い削除ドロップ情報の削除
                    temp[i, j - 1] = 0;
                }
                //一つとなりのドロップが同一テクスチャではない場合
                else
                {
                    //比較ドロップ情報を更新
                    c = 1;
                    t = Field[i,j];
                }
            }
        }
        //縦列CHKループ
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
                    //@ドロップが4以上隣接している場合、古い削除ドロップ情報の削除
                    temp2[i - 1, j] = 0;
                }
                else
                {
                    c = 1;
                    t = Field[i,j];
                }
            }
        }
        //@削除ドロップシート作成
        for(int i = 0; i < 5; i++)
        {
            for(int j = 0; j < 6; j++)
            {
                //横列で3つ以上同一テクスチャのドロップが隣接している場合
                if (temp[i, j] >= 3)
                {
                    //コンボデータ更新(横列)
                    if (temp[i,j] == 6)
                        FlagManager.RowComboCHK(Field[i,j]);

                    //隣接しているドロップ数分ループ
                    int temp_bk = temp[i, j];
                    for(int k = j; temp_bk > 0; k--, temp_bk--)
                    {
                        //ドロップテクスチャ情報を追加
                        temp3[i+1, k+1] = Field[i, j];
                        //Debug.Log("i:" + i + ",j;" + k + " " + temp3[i, k]);
                    }
                }
                //縦列で3つ以上同一テクスチャのドロップが隣接している場合
                if (temp2[i, j] >= 3)
                {
                    //コンボデータ更新(縦列)
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

        //@コンボシートを作成
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                //コンボが成立している場合
                if (temp3[i, j] >= 0 )
                {
                    //コンボ情報更新(コンボ数)
                    FlagManager.MaxComboCHK(temp3[i, j]);

                    //塗りつぶし処理をコール
                    //Debug.Log("塗りつぶし開始");
                    combCnt += 1;
                    fill(i, j, temp3[i, j]);
                    //Debug.Log("塗りつぶし終了");
                }
            }
        }
        /*for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 6; j++)
            { Debug.Log("i:" + i + ",j;" + j + " " + temp4[i, j]); }
        }*/

        
        //コンボシートをもとにドロップテクスチャ変更処理
        for (int n = 1; n <= combCnt; n++)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    //cコンボ目なら
                    if (temp4[i, j] == n)
                    {
                        //ドロップテクスチャ情報に削除テクスチャ情報を代入
                        Field[i, j] = 6;
                        //GameObjectのDropCnt内のSetをコールし削除テクスチャに変更
                        Obj[i, j].GetComponent<DropCnt>().Set(6);
                    }
                }
            }
            //Debug.Log(n+"コンボ目");
            this.GetComponent<ComboSystem>().IncreaseCombo();
            //await Task.Delay(400);
            yield return new WaitForSeconds(0.4f);
        }
        //Debug.Log("conbCnt"+combCnt);
    }

    //ドロップ落下処理
    public void DownDrop()
    {
        //Debug.Log("落下処理開始");
        //縦列ループ
        for (int j = 0; j < 6; j++)
        {
            for(int i = 0; i < 5; i++)
            {
                //削除テクスチャの場合
                if (Field[i,j] == 6)
                {
                    //上にドロップがある場合ループ
                    for(int k = i; k > 0; k--)
                    {
                        //上にあるドロップが削除テクスチャ以外の場合
                        if(Field[k - 1, j] != 6)
                        {
                            //上にあるドロップと入れ替える
                            ChangePos(Obj[k, j], Obj[k - 1, j]);
                        }
                    }
                }
            }
        }
    }
    //ドロップ削除後のドロップ追加処理
    public void ResetDrop()
    {
        for (int i =0 ; i < 5; i++)
        {
            for(int j = 0; j < 6; j++)
            {
                //削除テクスチャの場合
                if(Field[i,j] == 6)
                {
                    //ランダムでドロップテクスチャを選択
                    int type = Random.Range(0, 6);
                    //ドロップテクスチャ情報を更新
                    Field[i, j] = type;
                    //GameObjectのDropCnt内のSetをコールしテクスチャを更新
                    Obj[i, j].GetComponent<DropCnt>().Set(type);
                }
            }
        }
    }
    //削除テクスチャ有無チェック処理
    public bool Check()
    {
        for(int i = 0; i < 5; i++)
        {
            for(int j = 0; j < 6; j++)
            {
                //盤面に削除テクスチャがある場合
                if(Field[i,j] == 6)
                {
                    return false;
                }
            }
        }
        return true;
    }
    //待機処理
    public float Wait()
    {
        //int waitTime = combCnt * 400+500;
        float waitTime = combCnt * 0.4f + 0.5f;
        return waitTime;
    }
}


