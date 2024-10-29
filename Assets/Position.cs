using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Position : MonoBehaviour
{

    void Start()
    {
        // GPSの初期化
        if (!Input.location.isEnabledByUser)
        {
            alertText.text = "位置情報の使用を許可してください。";
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
            float latitude = Input.location.lastData.latitude;
            float longitude = Input.location.lastData.longitude;
            float altitude = Input.location.lastData.altitude;

            // 緯度、経度、高度を平面直交座標に変換
            var (x, y) = CoordinateUtil.JGD2011ToPlaneRectCoord(latitude, longitude, 36d, 139.83333333333d);
            x = x + 25425.3497;
            y = y - 3200.4710;
            float z = altitude; // 高度はそのまま使用

            // 結果を表示
            Debug.Log($"緯度: {latitude}, 経度: {longitude}, 高度: {altitude}");
            Debug.Log($"平面直交座標: x = {x}, y = {y}, z = {z}");

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

