using UnityEngine;
using UnityEngine.UI;

public class testPaint : MonoBehaviour
{
    [SerializeField] private Vector3 _colorSeed;
    [SerializeField] private RawImage _rawImage;
    private Texture2D _texture;

    private void Update()
    {
        // テクスチャの色情報をNativeArrayで取得
        var pixelData = _texture.GetPixelData<Color32>(0);

        // 色を適当に変更する
        var seeds = _colorSeed + Vector3.one * Time.time;
        for (var i = 0; i < pixelData.Length; i++)
        {
            var p = (float)i / pixelData.Length;
            pixelData[i] = GetColor(p, seeds);
        }

        // GPUにアップロード
        _texture.Apply();
    }

    public void OnEnable()
    {
        var tex = new Texture2D(128, 128, TextureFormat.RGBA32, false);
        _rawImage.texture = tex;
        _texture = tex;
    }

    private void OnDisable()
    {
        Destroy(_texture);
    }

    // 適当に色を作って返すメソッド
    private static Color GetColor(float p, Vector3 s)
    {
        var r = 0.5f + 0.5f * Mathf.Cos(Mathf.PI * 2 * (p + s.x));
        var g = 0.5f + 0.5f * Mathf.Cos(Mathf.PI * 2 * (p + s.y));
        var b = 0.5f + 0.5f * Mathf.Cos(Mathf.PI * 2 * (p + s.z));
        return new Color(r, g, b);
    }
}