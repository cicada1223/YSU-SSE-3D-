using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;

public class ShootControl : MonoBehaviour
{
  

    private MainControl mainControl;
    private GameUI gameUI;
    public GameObject arrowPrefab; // 箭的预制体
    public Transform arrowSpawnPoint; // 箭的发射位置

    public float maxSpeed;
    private Animator animator; // Animator组件的引用
    private AudioSource[] audioSources; 
   

    // Start is called before the first frame update
    void Start()
    {
        mainControl=FindObjectOfType<MainControl>();
     
        // 打印出 Canvas 下的所有子对象名称
        Transform canvasTransform =FindObjectOfType<Canvas>().transform;

        foreach (Transform child in canvasTransform)
        {
            Debug.Log("Canvas child: " + child.name);  // 打印 Canvas 下所有子对象的名称
        }

        // 查找名为 "GameUI" 的子对象
        
        Transform transform_gameui= canvasTransform.Find("GameUI");
        if(transform_gameui==null){
            Debug.Log("no gameui_transform");
        }
        gameUI=transform_gameui.GetComponent<GameUI>();
        animator = GetComponent<Animator>();
        audioSources=GetComponents<AudioSource>();

    }

    void Update()
    {   

        if((!mainControl.getIsStart())||mainControl.getIsPaused())return;
        // 获取蓄力值
        float power = animator.GetFloat("Power");
        float ipower=animator.GetFloat("IPower");
        power-=ipower;
        if(power<0)power=0;
        gameUI.UpdatePowerBar(power);
         
        // 检测鼠标左键是否被按下
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetBool("Fire", true);
            if(!audioSources[0].isPlaying){
                audioSources[0].Play();
            }
        }
        // 检测鼠标左键是否被释放
        if (Input.GetMouseButtonUp(0)&&animator.GetBool("Fire"))
        {
            animator.SetBool("Fire", false);

            if(audioSources[0].isPlaying){
                audioSources[0].Stop();
            }
            if(!audioSources[1].isPlaying){
                audioSources[1].Play();
            }
            // 检查预制体和发射点是否已经设置
            if (arrowPrefab != null && arrowSpawnPoint != null)
            {   
                

                // 输出Power参数的值到控制台
                Debug.Log("Power: " + power);

                // 实例化箭的预制体
                GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);

                // 给箭添加速度，速度根据蓄力值来设置
                if (arrow.TryGetComponent<Rigidbody>(out Rigidbody rb))
                {
                    rb.velocity = arrowSpawnPoint.forward * power * maxSpeed; 
                }
            }
        }

        // // 每一帧都输出Power参数的值，以检查其变化
        // if (animator != null)
        // {
        //     float power = animator.GetFloat("Power");
        //     Debug.Log("Current Power: " + power);
        // }
    }
}