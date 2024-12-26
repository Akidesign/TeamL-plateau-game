using UnityEngine;
using System.Collections.Generic;

public class DistanceBasedVisibility : MonoBehaviour
{
    public Transform Player;              // プレイヤーのTransform
    public GameObject yokohamaCity;        // 親オブジェクト
    public float activationDistance = 10f; // オブジェクトが再表示される距離

    [Header("Debug Options")]
    public bool showDebugLines = false;   // デバッグ表示用
    public Color debugLineColor = Color.green;

    // オブジェクトごとの表示フラグとバウンディングボックスを管理
    private Dictionary<GameObject, bool> objectVisibility;
    private Dictionary<GameObject, Bounds> objectBounds;

    private bool IsTargetObj(Transform trans)
    {
        if (trans == null) return false;
        var objName = trans.gameObject.name;
        if (objName.StartsWith("dem_")) return false;
        if (objName.StartsWith("urf_")) return false;
        if (objName.StartsWith("tran_")) return false;
        if (objName.StartsWith("luse_")) return false;
        return true;
    }

    void Start()
    {
        objectVisibility = new Dictionary<GameObject, bool>();
        objectBounds = new Dictionary<GameObject, Bounds>();

        foreach (Transform child in yokohamaCity.transform)
        {
            if (!IsTargetObj(child)) continue;

            var meshRenderer = child.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                Bounds worldBounds = GetWorldBounds(meshRenderer);
                objectBounds[child.gameObject] = worldBounds;

                child.gameObject.SetActive(false);
                objectVisibility[child.gameObject] = false;

                Debug.Log($"Calculated bounds for {child.name}: Center={worldBounds.center}, Size={worldBounds.size}");
            }
        }
        Debug.Log($"Initialized {objectBounds.Count} objects with bounds calculations");
    }

    private Bounds GetWorldBounds(MeshRenderer renderer)
    {
        return renderer.bounds;
    }

    void Update()
    {
        Vector3 playerPos = Player.position;

        foreach (var kvp in objectBounds)
        {
            GameObject obj = kvp.Key;
            if (obj == null) continue;

            Bounds bounds = kvp.Value;

            // プレイヤーの位置をXZ平面上で考慮
            Vector3 adjustedPlayerPos = new Vector3(playerPos.x, bounds.center.y, playerPos.z);

            // バウンディングボックスへの最近接点を計算
            Vector3 closestPoint = GetClosestPointOnBounds(bounds, adjustedPlayerPos);

            // XZ平面での距離を計算
            float distanceXZ = Vector2.Distance(
                new Vector2(playerPos.x, playerPos.z),
                new Vector2(closestPoint.x, closestPoint.z)
            );

            if (showDebugLines)
            {
                Debug.DrawLine(playerPos, closestPoint, debugLineColor);
            }

            // 距離が指定値以下かつ、まだ表示されていない場合
            if (distanceXZ <= activationDistance && !objectVisibility[obj])
            {
                obj.SetActive(true);
                objectVisibility[obj] = true;
                Debug.Log($"Activated {obj.name} at XZ distance {distanceXZ:F2}");
            }
            else if (distanceXZ > activationDistance)
            {
                // デバッグ用の距離情報
                Debug.Log($"Object {obj.name} is at XZ distance {distanceXZ:F2} (activation distance: {activationDistance})");
            }
        }
    }

    // バウンディングボックスへの最近接点を取得
    private Vector3 GetClosestPointOnBounds(Bounds bounds, Vector3 point)
    {
        Vector3 closest = bounds.center;
        Vector3 halfExtents = bounds.extents;

        // X軸
        closest.x = Mathf.Clamp(point.x, bounds.center.x - halfExtents.x, bounds.center.x + halfExtents.x);
        // Y軸（今回は使用しないが、完全性のために含める）
        closest.y = Mathf.Clamp(point.y, bounds.center.y - halfExtents.y, bounds.center.y + halfExtents.y);
        // Z軸
        closest.z = Mathf.Clamp(point.z, bounds.center.z - halfExtents.z, bounds.center.z + halfExtents.z);

        return closest;
    }

    // デバッグ用：バウンディングボックスの可視化
    void OnDrawGizmos()
    {
        if (!Application.isPlaying || objectBounds == null || !showDebugLines) return;

        foreach (var kvp in objectBounds)
        {
            if (kvp.Key == null) continue;

            Bounds bounds = kvp.Value;

            // バウンディングボックスを表示
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(bounds.center, bounds.size);

            if (Player != null)
            {
                // プレイヤーの位置をXZ平面上で考慮した位置を表示
                Gizmos.color = Color.blue;
                Vector3 adjustedPlayerPos = new Vector3(Player.position.x, bounds.center.y, Player.position.z);
                Gizmos.DrawSphere(adjustedPlayerPos, 0.5f);

                // 最近接点を表示
                Gizmos.color = Color.red;
                Vector3 closestPoint = GetClosestPointOnBounds(bounds, adjustedPlayerPos);
                Gizmos.DrawSphere(closestPoint, 0.3f);
            }
        }
    }
}
