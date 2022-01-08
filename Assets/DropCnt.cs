using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class DropCnt : MonoBehaviour
{
    //DropCntのインスペクターウィンドウにドロップテクスチャ用のspフィールドを追加
    [SerializeField] Sprite[] sp;
    //ドロップ入れ替え用IDの変更プロパティの宣言
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
    //画面タッチフラグ変数の宣言(true,false)
    bool touchFlag;
    //ドロップ移動用の座標変数(Vector2型)の宣言
    Vector2 m;
    //GUI用Transformの変数(RectTransform型)の宣言
    RectTransform r, r2;
    //ドロップ初期位置保存用変数の宣言
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

        //GUI用Transform(RectTransform型)を取得
        //※r = transform as RectTransform;の方が処理が早いらしい(GetComponentが遅いらしい)
        r = GetComponent<RectTransform>();
        //親のGUI用Transform(RectTransform型)を取得
        r2 = transform.parent.GetComponent<RectTransform>();
        //
        d = GameObject.Find("D").GetComponent<Director>();
    }

    // Update is called once per frame
    void Update()
    {
        //ドロップタッチ中ならループ(touchFlagがtrueならループ)
        if(touchFlag)
        {
            //posにVector2(0, 0)を格納
            var pos = Vector2.zero;
            //画面タッチした座標を取得(カメラ(Camera.main)基準のスクリーン空間座標)
            m = Input.mousePosition;
            //カメラ(Camera.main)基準のスクリーン空間座標(m)をGUI用Transform(RectTransform型)の座標に変換し、posにアウトプット
            RectTransformUtility.ScreenPointToLocalPointInRectangle
                (r2, m, Camera.main, out pos);
            //親のTransformら見た相対的な位置をpos画面をタッチした位置に変更
            r.localPosition = pos;
        }

        //ドロップ初期位置が未保存の場合
        if(P1.x == 0)
        {
            //RectTransformのpositionを取得
            P1 = GetComponent<RectTransform>().position;
            //RectTransformをワールド座標に変更する
            P2 = RectTransformUtility.WorldToScreenPoint(Camera.main, P1);
        }
        else
        {
            //ドロップをタッチしていない(離した)場合
            if(!touchFlag)
            {
                //初期位置へ戻す
                GetComponent<RectTransform>().position = P1;
            }
        }
    }

    public void Set(int n)
    {
        //コンポーネント(SpriteRenderer)を取得し、spriteにランダム値をもとにspからテクスチャを設定する
        GetComponent<SpriteRenderer>().sprite = sp[n];
    }

    //dropオブジェクトのイベントトリガーで判定
    //ドロップをタッチしている場合
    public void GetDrop()
    {
        touchFlag = true;
    }
    //ドロップを離した場合
    public void SetDrop()
    {
        touchFlag = false;
        //Delete関数をコール
        Delete();
    }
    //ドロップが何かと衝突した場合の処理
    private void OnCollisionStay2D(Collision2D collision)
    {
        //ドロップタッチ中の場合
        if(touchFlag)
        {
            //タッチ中のドロップと衝突したドロップが近い場合
            if(d.CheckPos(m, collision.gameObject.GetComponent<DropCnt>().P2))
            {
                //ドロップを入れ替える
                d.ChangePos(gameObject, collision.gameObject);
            }
        }
    }
    //Delete処理(非同期)
    private async void Delete()
    {
        while (true)
        {
            //盤面ロック
            GameObject.Find("PuzllBlock").GetComponent<Image>().enabled = true;
            GameObject.Find("PuzllBlock").GetComponent<GraphicRaycaster>().enabled = true;
            //ドロップ削除処理をコール
            d.DeleteDrop();
            //盤面に削除テクスチャが無ければループを抜ける
            if (d.Check())
            {
                GameObject.Find("D").GetComponent<ComboSystem>().ResetComboText();
                int ComboCount = FlagManager.stageComboCount;
                FlagManager.ClearCHK(this);
                FlagManager.ComboReset();
                
                if (!FlagManager.stageClearFlag)
                {
                    //ダメージ処理用時間調整
                    if (ComboCount > 0)
                        await Task.Delay(1500);
                    //盤面ロック解除
                    GameObject.Find("PuzllBlock").GetComponent<GraphicRaycaster>().enabled = false;
                    GameObject.Find("PuzllBlock").GetComponent<Image>().enabled = false;
                }
                break;
            }
            //コンボが終わるまで待機
            int wait = d.Wait();
            //Debug.Log(wait);
            await Task.Delay(wait);
            //ドロップ落下処理をコール
            d.DownDrop();
            await Task.Delay(500);
            //ドロップ追加処理をコール
            d.ResetDrop();
            await Task.Delay(500);
            
        }
    }
}

