using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

//参考資料
//https://nn-hokuson.hatenablog.com/entry/2019/02/28/205346
//https://3dunity.org/game-create-lesson/action-pazzle-game/touch-array-introduction/


public class oldCreateThread : MonoBehaviour
{

    public bool startFlg = false;
    public bool endFlg = false;

    [SerializeField]    private RawImage m_image = null;    //自動描画用イメージ
    [SerializeField]    private RawImage m_image2 = null;   //手動描画用イメージ
    [SerializeField]    GameObject image_Parent;            //親オブジェクトの取得

    private Texture2D m_texture = null;                     //自動描画用2Dテクスチャ
    private Texture2D m_texture2 = null;                    //手動描画用2Dテクスチャ

    [SerializeField]    private int m_width = 20;           //描画線の横の太さ
    [SerializeField]    private int m_height = 20;          //描画線の縦の太さ

    private Vector2 m_prePos, m_drawPos;        //自動描画用ポジション変数
    private Vector2 m_preTouchPos, m_TouchPos;  //手動描画用ポジション変数
    private Vector2 m_disImagePos;              //2Dテクスチャ起点ポジション変数

    private float m_drawTime, m_preDrawTime;    //自動描画用
    private float m_clickTime, m_preClickTime;  //手動描画用

    private Color setColor   = new Color32(255, 255, 255, 255); //自動描画用カラー(黒)
    private Color setColor2  = new Color32(125, 125, 125, 255); //手動描画用カラー(灰色)
    private Color colorClear = new Color32(0, 0, 0, 0);         //クリア用カラー(透明)


    void Start()
    {

        //Imageサイズの2Dテクスチャ作成(自動描画用、手動描画用) 
        var rect = m_image.gameObject.GetComponent<RectTransform>().rect;
        m_texture = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGBA32, false);
        m_texture2 = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGBA32, false);
        m_image.texture = m_texture;
        m_image2.texture = m_texture2;

        //Imageの起点を取得
        var m_imagePos = m_image.gameObject.GetComponent<RectTransform>().anchoredPosition;
        m_disImagePos = new Vector2(m_imagePos.x - rect.width / 2, m_imagePos.y - rect.height / 2);
        Debug.Log(m_disImagePos);

        //2Dテクスチャのピクセルデータを取得
        var pixelData = m_texture.GetPixelData<Color32>(0);
        var pixelData2 = m_texture2.GetPixelData<Color32>(0);

        //2Dテクスチャを透明に設定
        for (var i = 0; i < pixelData.Length; i++)
        {
            pixelData[i] = colorClear;
            pixelData2[i] = colorClear;
        }

        m_texture.Apply();
        m_texture2.Apply();
    }

    //隙の糸(自動描画)
    public void OnDrag(Vector3 pos, float time )
    {
        //https://light11.hatenadiary.com/entry/2019/04/16/003642
        //LineRendererの最新の描画時間を取得
        m_drawTime = time;
        Vector2 localPoint = pos;                   //ローカル座標をVector3からVector2へ変換
        m_drawPos = localPoint - m_disImagePos;     //ローカル座標をテクスチャ座標に合わせる

        float disTime = m_drawTime - m_preDrawTime; //前回のクリックイベントとの時差

        int width = m_width;  //ペンの太さ(ピクセル)
        int height = m_height; //ペンの太さ(ピクセル)

        var dir = m_prePos - m_drawPos; //直前のタッチ座標との差
        if (disTime > 0.01) dir = new Vector2(0, 0); //0.1秒以上間隔があいたらタッチ座標の差を0にする

        var dist = (int)dir.magnitude; //タッチ座標ベクトルの絶対値

        dir = dir.normalized; //正規化

        //指定のペンの太さ(ピクセル)で、前回のタッチ座標から今回のタッチ座標まで塗りつぶす
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
                if (y < 0 || y >= m_texture.height) continue; //タッチ座標がテクスチャの外の場合、描画処理を行わない
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

    //隙の糸クリア(手動描画用)
    public void ClearDrag(BaseEventData arg)
    {
        //自動描画終了したらtrue
        if (endFlg)
        {
         
            PointerEventData _event = arg as PointerEventData; //タッチの情報取得

            m_TouchPos = _event.position;   //現在のスクリーン座標
            m_clickTime = _event.clickTime; //最後にクリックイベントが送信された時間を取得

            //https://light11.hatenadiary.com/entry/2019/04/16/003642
            // クリック位置に対応するスクリーン座標をローカル座標に変換
            var localPoint = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(image_Parent.GetComponent<RectTransform>(), _event.position, Camera.main, out localPoint);
            m_TouchPos = localPoint - m_disImagePos;    //ローカル座標をテクスチャ座標に合わせる

            float disTime = m_clickTime - m_preClickTime; //前回のクリックイベントとの時差

            int width = m_width * 3;  //ペンの太さ(ピクセル)
            int height = m_height * 3; //ペンの太さ(ピクセル)

            var dir = m_preTouchPos - m_TouchPos; //直前のタッチ座標との差
            if (disTime > 0.01) dir = new Vector2(0, 0); //0.1秒以上間隔があいたらタッチ座標の差を0にする

            var dist = (int)dir.magnitude; //タッチ座標ベクトルの絶対値

            dir = dir.normalized; //正規化

            //指定のペンの太さ(ピクセル)で、前回のタッチ座標から今回のタッチ座標まで塗りつぶす
            var pixelData2 = m_texture2.GetPixelData<Color32>(0);
            for (int d = 0; d < dist; ++d)
            {
                var p_pos = m_TouchPos + dir * d; //paint position
                p_pos.y -= height/ 2.0f;
                p_pos.x -= width / 2.0f;

                for (int h = 0; h < height; ++h)
                {
                    int y = (int)(p_pos.y + h);
                    if (y < 0 || y >= m_texture2.height) continue; //タッチ座標がテクスチャの外の場合、描画処理を行わない
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

    //隙の糸の塗りつぶし量を比較(自動描画 vs 手動描画)
    public bool ColorChk()
    {
        var pixelData   = m_texture.GetPixelData<Color32>(0);   //自動描画
        var pixelData2  = m_texture2.GetPixelData<Color32>(0);  //手動描画
        int compareColorA = 0;                                  //比較元(自動描画)
        int compareColorB = 0;                                  //比較先(手動描画)

        //ピクセル分ループ
        for (var i = 0; i < pixelData.Length; i++)
        {
            if (pixelData[i] == setColor)                       //自動描画量を取得
                compareColorA++;
            if (pixelData2[i] == setColor2)                     //手動描画量を取得
                compareColorB++;
        }

        //2Dテクスチャを透明にリセット
        for (var i = 0; i < pixelData.Length; i++)
        {
            pixelData[i] = colorClear;
            pixelData2[i] = colorClear;
        }

        m_texture.Apply();
        m_texture2.Apply();

        Debug.Log("compareColorA:" + compareColorA + "/compareColorB:" + compareColorB);
        //自動描画を80％以上塗りつぶせていれば突破
        if (compareColorA*0.2 >= compareColorB )
        {
            Debug.Log("突破"+ compareColorA * 0.8);
            return true;
        }

        return false;
        
    }


        void Update()
    {
        if (startFlg)
        {
            //オブジェクトのローカルポジションを取得
            var rectpos = this.transform.localPosition;
            OnDrag(rectpos, Time.deltaTime);
        }
    }



    //隙の糸のルートをランダムで決定
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
            if (i == 0)                             //スタート位置
            {
                x = -900.0f;
                y = Random.Range(0.0f, 1000.0f);
            }
            else if (i == num - 1)                  //ゴール位置
            {
                x = 900.0f;
                y = Random.Range(0.0f, 1000.0f);
            }
            else                                    //中間位置
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

    //隙の糸終了
    public bool OpeningThreadEnd()
    {
        Debug.Log("隙の糸終了フラグ"+startFlg);
        endFlg = false;

        GameObject.Find("OpeningThreadPanel").GetComponent<Image>().enabled = false;
        GameObject.Find("OpeningThreadPanel").GetComponent<GraphicRaycaster>().enabled = false;

        return ColorChk();
        
    }

 
}
