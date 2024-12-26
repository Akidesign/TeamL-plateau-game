using UnityEngine;
using TMPro;

public class LocationRenderer : MonoBehaviour
{
    public LocationUpdater updater;  // LocationUpdater への参照
    public TMP_Text text;  // 表示用のテキストUI

    void Update()
    {
        // 位置情報の状態と、変換された平面直交座標を表示
        text.text = updater.Status.ToString()
                  //+ "\n" + "lat: " + updater.Location.latitude.ToString("F6")
                  //+ "\n" + "lng: " + updater.Location.longitude.ToString("F6")
                  + "\n" + "Latitude: " + updater.PlaneX.ToString("F2")  // 平面座標X
                  + "\n" + "Longitude: " + updater.PlaneY.ToString("F2")  // 平面座標Y
                  + "\n" + "Altitude: " + updater.PlaneZ.ToString("F2"); //平面座標Z
    }
}

