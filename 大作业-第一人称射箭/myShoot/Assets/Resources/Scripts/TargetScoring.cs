using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScoring : MonoBehaviour
{
    public float maxDistance = 100f; // 最大距离，超过此距离得分为最大值
    public int maxScore = 100; // 最大得分
    public int minScore = 0; // 最低得分

    private GameObject player; // 主角（弓箭手）的 Transform，用于计算与靶子的距离
    private GameUI gameUI;
    void Start(){
    
        player=GameObject.FindWithTag("Player");

        Transform canvasTransform =FindObjectOfType<Canvas>().transform;

        // 查找名为 "GameUI" 的子对象
        Transform transform_gameui= canvasTransform.Find("GameUI");
        if(transform_gameui==null){
            Debug.Log("no gameui_transform");
        }
        gameUI=transform_gameui.GetComponent<GameUI>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 检查是否与箭发生碰撞
        if (collision.gameObject.CompareTag("Arrow"))
        {
            // 计算靶子与主角的距离
            float distance = Vector3.Distance(transform.position, player.transform.position);

            // 根据距离计算得分
            int score = CalculateScore(distance);
            gameUI.UpdateScore(score);
            // 显示得分信息（可替换为实际的 UI 更新逻辑）
            Debug.Log($"Hit! Distance: {distance:F2}, Score: {score}");

            
        }
    }

    private int CalculateScore(float distance)
    {
        // 如果距离超过最大距离，返回最大得分
        if (distance >= maxDistance)
            return maxScore;

        // 如果距离小于等于 0，返回最低得分
        if (distance <= 10)
            return minScore;

        // 按距离线性插值计算得分
        float t = Mathf.Clamp01(distance / maxDistance);
        return Mathf.RoundToInt(Mathf.Lerp(minScore, maxScore, t));
    }
}
