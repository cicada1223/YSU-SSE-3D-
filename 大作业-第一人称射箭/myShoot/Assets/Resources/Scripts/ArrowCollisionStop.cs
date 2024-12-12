using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowCollisionStop : MonoBehaviour
{
    private Rigidbody rb;
    private AudioSource audioSource;
    private Collider arrowCollider;

    void Start()
    {
        // 获取箭的Rigidbody组件
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        arrowCollider=GetComponent<BoxCollider>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // 如果碰撞的物体标签是 "Player"，则不处理
        if (collision.gameObject.CompareTag("Player"))
        {
            return; // 直接返回，不执行后续代码
        }

        // 当箭发生碰撞时，执行以下操作
        AttachArrowToCollisionObject(collision);
    }

    void AttachArrowToCollisionObject(Collision collision)
    {
        // 播放音效
        audioSource.Play();

        // 禁用重力，防止重力影响箭的位置
        rb.useGravity = false;

        // 将Rigidbody设置为isKinematic，使其不再参与物理模拟
        rb.isKinematic = true;
         // 禁用箭矢的 Collider
        
        
            arrowCollider.enabled = false; // 禁用箭矢的 Collider，停止碰撞
        
        

        // 将箭设置为碰撞物体的子物体
        transform.SetParent(collision.transform);
    }
}
