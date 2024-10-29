using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class DevilController : MonoBehaviour,IPointerClickHandler {
    public GameObject devilPrefab; // 魔鬼预制件
    public Vector3 spawnPosition = new Vector3(0, 0, 0);
    public float moveDistance = 2f; // 每次点击后魔鬼移动的距离
    private BoxCollider boxCollider; // 
    private GameObject spawnedDevil; // 存储生成的魔鬼实例

    void Start() {
        SpawnDevil();
        boxCollider = GetComponent<BoxCollider>();
         if (boxCollider == null) {
            boxCollider = gameObject.AddComponent<BoxCollider>();
        }
    }

      public void OnPointerClick(PointerEventData eventData)
    {   
        Debug.Log("Hit!!!!");
        MoveDevil();
    }

    void Update() {
        // 检测鼠标点击事件
        // if (Input.GetMouseButtonDown(0)) {
        //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //     RaycastHit hit;

        //     // 如果点击到了魔鬼对象
        //     if (Physics.Raycast(ray, out hit)) {
        //         	
        //         if (hit.transform.gameObject == spawnedDevil) {
        //             MoveDevil();
        //         }
        //     }
        // }
    }

    // 生成魔鬼的方法
    void SpawnDevil() {
        // 在指定位置生成魔鬼，可以根据需要调整位置

        spawnedDevil = Instantiate(devilPrefab, spawnPosition, Quaternion.identity);
        UpdateBoxColliderPosition();
        //Debug.Log(boxCollider.transform);
    }

    // 魔鬼移动的方法
    void MoveDevil() {
        // 向魔鬼的前方移动 moveDistance 距离
        spawnedDevil.transform.Translate(moveDistance,0,0);
        UpdateBoxColliderPosition();
        
    }
    private void UpdateBoxColliderPosition() {
        // 将 BoxCollider 的中心与魔鬼的当前位置保持一致
        if (boxCollider != null) {
            boxCollider.center = transform.position - transform.position;
        }else{
            Debug.Log("error:not found boxCollider");
        }
    }

  
}

