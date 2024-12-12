using System.Collections;
using UnityEngine;

public class SpiderController : MonoBehaviour
{
    public GameObject explosionEffect;  // 爆炸效果的预制体
    public float moveSpeed = 2f;        // 蜘蛛的移动速度
    public float moveTime = 3f;         // 每次移动持续的时间
    public float moveRange = 5f;        // 蜘蛛在碰撞盒内的最大移动范围

    private GameUI gameUI;
    private Animator animator; // Animator组件的引用
    private Rigidbody rb;
    private AudioSource audioSource;
    private Vector3 targetPosition;     // 目标位置
    private float moveTimer;            // 移动计时器
    private bool isWalking = false;     // 是否正在移动
    private float rotationSpeed = 5f;   // 蜘蛛的旋转速度

    private void Start()
    {
        // 初始化蜘蛛的目标位置
        SetRandomTargetPosition();
        animator=GetComponent<Animator>();
        rb=GetComponent<Rigidbody>();
        audioSource=GetComponent<AudioSource>();
        Transform canvasTransform =FindObjectOfType<Canvas>().transform;

        // 查找名为 "GameUI" 的子对象
        Transform transform_gameui= canvasTransform.Find("GameUI");
        if(transform_gameui==null){
            Debug.Log("no gameui_transform");
        }
        gameUI=transform_gameui.GetComponent<GameUI>();
    }

    private void Update()
    {
        // 移动蜘蛛
        MoveSpider();

        // 更新移动计时器
        moveTimer += Time.deltaTime;
        if (moveTimer >= moveTime)
        {
            // 每次完成一段移动后，重新设定一个随机的目标位置
            SetRandomTargetPosition();
            moveTimer = 0f;
        }

        // 根据是否移动来控制动画状态机中的 SpiderIsWalking 参数
        animator.SetBool("SpiderIsWalking", isWalking);

    }

    private void MoveSpider()
    {
        // 计算蜘蛛与目标位置的方向
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0;  // 使其在Y轴方向不旋转，保持水平移动

        // 平滑旋转蜘蛛朝向目标
        if (direction.magnitude > 0.1f)
        {
            isWalking = true;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // 使用 Rigidbody 的 MovePosition 方法来移动蜘蛛
            Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            rb.MovePosition(newPosition);  // 使用 Rigidbody 进行物理移动
            
        }
        else
        {
            isWalking = false;
        }
    }

    private void SetRandomTargetPosition()
    {
        // 生成一个随机位置，限制在碰撞盒的范围内
        Vector3 randomDirection = new Vector3(Random.Range(-moveRange, moveRange), 100, Random.Range(-moveRange, moveRange));
        targetPosition = transform.position + randomDirection;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 检测到箭矢碰撞
        if (collision.gameObject.CompareTag("Arrow"))
        {
            audioSource.Play();
            // 播放爆炸效果
            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
            }
            gameUI.UpdateScore(50);
            // 销毁蜘蛛
            StartCoroutine(DestroySpiderAfterDelay(1f)); // 延迟1秒销毁蜘蛛
            
        }
    }
    IEnumerator DestroySpiderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 等待指定的时间
        Destroy(gameObject); // 销毁蜘蛛对象
    }
 
}
