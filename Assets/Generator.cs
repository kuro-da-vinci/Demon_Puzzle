using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;

public class Generator : MonoBehaviour
{
    //GameObject格納変数の宣言 DにdropGameObject、LにGameObject(1)(水平レイアウト)
    public GameObject D, L;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(SceneManager.GetActiveScene().buildIndex);

        //ドロップテクスチャ設定用変数
        int type;
        //初期盤面コンボCHK用配列
        int[,] typeChk = new int[7, 8]
        {
            {-1,-1,-1,-1,-1,-1,-1,-1 },
            {-1,-1,-1,-1,-1,-1,-1,-1 },
            {-1,-1,-1,-1,-1,-1,-1,-1 },
            {-1,-1,-1,-1,-1,-1,-1,-1 },
            {-1,-1,-1,-1,-1,-1,-1,-1 },
            {-1,-1,-1,-1,-1,-1,-1,-1 },
            {-1,-1,-1,-1,-1,-1,-1,-1 },
        };
        //盤面ループ(縦5行分)
        for (int i = 0; i < 5; i++)
        {
            //GameObjectのLを新たなGameObjectとして複製
            GameObject l = Instantiate(L) as GameObject;
            //親を変更し、親のtransformを基準にする
            l.transform.SetParent(transform);
            //親のスケールを基準にスケール変更、Vector3.oneはVector3(1, 1, 1)と同義
            //※Vector3型は3つのfloat型の値をまとめて保持できる型
            l.transform.localScale = Vector3.one;
            //盤面ループ(横6列分)
            for (int j = 0; j < 6; j++)
            {
                //GameObjectのDを新たなGameObjectとして複製
                GameObject d = Instantiate(D) as GameObject;
                //親を変更し、親のtransformを基準にする
                d.transform.SetParent(l.transform);
                while (true)
                {
                    //0～5までの数字をランダムで宣言した変数へ格納(ドロップテクスチャの決定)
                    type = Random.Range(0, 6);
                    typeChk[i + 2, j + 2] = type;
                    //左2つor上2つのドロップがtypeと同じ場合、別の色に変更
                    if (type == typeChk[i + 2, j] && type == typeChk[i + 2, j + 1]
                        || type == typeChk[i, j + 2] && type == typeChk[i + 1, j + 2]) {
                        continue;
                    }
                    break;
                }
                //生成したGameObjectdのコンポーネント(DropCnt)を取得し、Set関数をコールしtype(ランダム値)を引き渡す
                d.GetComponent<DropCnt>().Set(type);
                //ドロップ入れ替え用のIDをセット
                d.GetComponent<DropCnt>().ID1 = i;
                d.GetComponent<DropCnt>().ID2 = j;
                //GameObject[D]を探し、コンポーネント(Director)を取得し、ドロップ(d)をObjに、ドロップテクスチャ情報(type)をFieldに格納
                GameObject.Find("D").GetComponent<Director>().Obj[i, j] = d;
                GameObject.Find("D").GetComponent<Director>().Field[i, j] = type;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
