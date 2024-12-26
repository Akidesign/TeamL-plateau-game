using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // プレイヤーのTransform
    public Vector3 offset;    // カメラとプレイヤーの間のオフセット

    void Start()
    {
        // カメラとプレイヤーの初期オフセットを計算
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        // カメラがプレイヤーの位置 + オフセットの位置に追従する
        transform.position = player.position + offset;
    }
}