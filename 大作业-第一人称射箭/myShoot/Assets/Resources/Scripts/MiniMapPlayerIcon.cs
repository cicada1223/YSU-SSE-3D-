using UnityEngine;
using UnityEngine.UI;

public class MiniMapPlayerIcon : MonoBehaviour
{
    public GameObject playerIcon;     // 小地图上的图标（UI Image）
    private Transform playerTransform; // 玩家对象的 Transform
    private RectTransform icon;       // 小地图图标的 RectTransform
    public Camera miniMapCamera;      // 小地图相机

    void Start()
    {
        // 获取玩家对象的 Transform
        playerTransform = GameObject.FindWithTag("Player").transform;

        // 获取玩家图标的 RectTransform
        icon = playerIcon.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (playerTransform == null || miniMapCamera == null)
            return;

        // 获取玩家在世界空间中的位置
        Vector3 playerPosition = playerTransform.position;

        // 将玩家位置从世界空间转换为小地图相机的视口坐标
        Vector3 miniMapPosition = miniMapCamera.WorldToViewportPoint(playerPosition);

        // 将小地图坐标限制在 0 到 1 之间，确保图标始终在小地图范围内
        miniMapPosition.x = Mathf.Clamp(miniMapPosition.x, 0f, 1f);
        miniMapPosition.y = Mathf.Clamp(miniMapPosition.y, 0f, 1f);

        // 更新小地图图标的位置（使用 RectTransform 以便 UI 元素移动）
        icon.anchorMin = miniMapPosition;
        icon.anchorMax = miniMapPosition;
    }
}
