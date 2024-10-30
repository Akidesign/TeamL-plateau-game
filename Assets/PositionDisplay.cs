using UnityEngine;
using UnityEngine.UI;

public class PositionDisplay : MonoBehaviour
{
    public Position position; // Positionスクリプトへの参照
    public Text positionText; // UI Textオブジェクトへの参照

    private void Update()
    {
        if (position != null)
        {
            positionText.text = $"緯度: {position.Latitude:F2}\n" +
                                $"経度: {position.Longitude:F2}\n" +
                                $"高度: {position.Altitude:F2}";
        }
        else
        {
            positionText.text = "位置情報を取得できません。";
        }
    }
}
