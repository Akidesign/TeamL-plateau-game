using UnityEngine;
using System.Collections;

public class LocationUpdater : MonoBehaviour
{
    public float IntervalSeconds = 5.0f;  // 更新間隔
    public LocationServiceStatus Status;  // 位置情報の状態
    public LocationInfo Location;  // 位置情報
    public float PlaneX, PlaneY;  // 平面直交座標

    // 変換用の基準緯度経度
    private const double originLatitude = 36.0;
    private const double originLongitude = 139.83333333333;

    IEnumerator Start()
    {
        // 位置情報サービスの初期化
        while (true)
        {
            this.Status = Input.location.status;

            // 位置情報サービスが有効かチェック
            if (Input.location.isEnabledByUser)
            {
                switch (this.Status)
                {
                    case LocationServiceStatus.Stopped:
                        Input.location.Start();
                        break;

                    case LocationServiceStatus.Running:
                        this.Location = Input.location.lastData;

                        // 緯度経度から平面直交座標系に変換
                        (double x, double y) = CoordinateUtil.JGD2011ToPlaneRectCoord(
                            this.Location.latitude, this.Location.longitude, originLatitude, originLongitude);

                        // 座標を保持
                        this.PlaneX = (float)x;
                        this.PlaneY = (float)y;
                        break;

                    default:
                        break;
                }
            }
            else
            {
                Debug.Log("Location service is disabled by user.");
            }

            // 指定した間隔で更新を繰り返す
            yield return new WaitForSeconds(IntervalSeconds);
        }
    }
}
