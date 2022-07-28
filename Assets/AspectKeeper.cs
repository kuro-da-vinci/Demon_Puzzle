using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class AspectKeeper : MonoBehaviour
{
    //参考資料：https://3dunity.org/game-create-lesson/clicker-game/mobile-adjustment/
    [SerializeField]
    private Camera targetCamera; //対象とするカメラ

    [SerializeField]
    private Vector2 aspectVec; //目的解像度

    private float posRange = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var screenAspect = Screen.width / (float)Screen.height; //画面のアスペクト比
        var targetAspect = aspectVec.x / aspectVec.y; //目的のアスペクト比

        var magRate = targetAspect / screenAspect; //目的アスペクト比にするための倍率

        var viewportRect = new Rect(0, 0, 1, 1); //Viewport初期値でRectを作成

        posRange = (aspectVec.x * (Screen.height / aspectVec.y)) / 12; //ドロップ交換距離の測定

        /*
        Debug.Log("�@全体縦サイズ:" + Screen.height);
        Debug.Log("�A目標縦サイズ:" + aspectVec.y);
        Debug.Log("�B比率:" + Screen.height / aspectVec.y);
        Debug.Log("�C横比:" + aspectVec.x * (Screen.height / aspectVec.y));
        Debug.Log("�D12分割後:" + (aspectVec.x * (Screen.height / aspectVec.y)) / 12);
        Debug.Log("�E画面横:" + Screen.width / 12);
        Debug.Log("�Fpos:" + posRange);
        */

        if (magRate < 1)
        {
            viewportRect.width = magRate; //使用する横幅を変更
            viewportRect.x = 0.5f - viewportRect.width * 0.5f;//中央寄せ
        }
        else
        {
            viewportRect.height = 1 / magRate; //使用する縦幅を変更
            viewportRect.y = 0.5f - viewportRect.height * 0.5f;//中央寄せ
        }
        
        targetCamera.rect = viewportRect; //カメラのViewportに適用
    }

    public float GetPosRange
    {
        
        get { return this.posRange; }  //取得用
        private set { this.posRange = value; } //値入力用
        
    }

}
