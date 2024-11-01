using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 1f;

    [Header("Debug")]
    public bool showDebugInfo = false;

    private Vector3 movement;

    void Update()
    {
        // キーボード入力の取得
        float moveX = 0f;
        float moveZ = 0f;

        // 左右キーの入力
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            moveX = -1f;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            moveX = 1f;
        }

        // 上下キーの入力
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            moveZ = 1f;
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            moveZ = -1f;
        }

        // 移動ベクトルの作成
        movement = new Vector3(moveX, 0f, moveZ);

        // 斜め移動の場合に速度を正規化
        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }

        // 移動の適用
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

        // デバッグ情報の表示
        if (showDebugInfo)
        {
            Debug.Log($"Movement: X={moveX}, Z={moveZ}, Speed={moveSpeed}, Position={transform.position}");
            // 移動方向の可視化
            Debug.DrawRay(transform.position, movement * 2f, Color.blue);
        }
    }

    // Inspector上で現在位置を確認するための関数
    void OnDrawGizmos()
    {
        if (showDebugInfo)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 25f);
        }
    }
}
