using System.Collections;
using UnityEngine;

public class Position : MonoBehaviour
{
    private float latitude;
    private float longitude;
    private float altitude;

    public float Latitude => latitude; // 緯度のプロパティ
    public float Longitude => longitude; // 経度のプロパティ
    public float Altitude => altitude; // 高度のプロパティ

    void Start()
    {
        // GPSの初期化
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("GPS not enabled");
        }
        else
        {
            Input.location.Start();
            StartCoroutine(GetLocation());
        }
    }

    private IEnumerator GetLocation()
    {
        // GPSの初期化待機
        yield return new WaitUntil(() => Input.location.status == LocationServiceStatus.Running);

        while (true)
        {
            // 緯度、経度、高度を取得
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;
            altitude = Input.location.lastData.altitude;

            // 緯度、経度、高度を平面直交座標に変換
            var (x, y) = CoordinateUtil.JGD2011ToPlaneRectCoord(latitude, longitude, 36d, 139.83333333333d);
            x += 25425.3497f;
            y -= 3200.4710f;

            // 結果を表示
            Debug.Log($"緯度: {latitude}, 経度: {longitude}, 高度: {altitude}");
            Debug.Log($"平面直交座標: x = {x}, y = {y}");

            // 5秒ごとに更新
            yield return new WaitForSeconds(5f);
        }
    }

    private void OnDestroy()
    {
        // GPSの停止
        Input.location.Stop();
        Debug.Log("位置情報の取得を終了しました。");
    }
}

