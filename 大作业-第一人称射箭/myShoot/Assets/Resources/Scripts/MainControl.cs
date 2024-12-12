using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class MainControl : MonoBehaviour
{
    // 私有变量，存储 UI 元素
     private GameObject canvasPrefab;      // Canvas 预制体
     private GameObject environmentPrefab; // Environment 预制体
     private GameObject playerPrefab;      // Player 预制体
     private GameObject eventSystemPrefab; // EventSystem 预制体
    private GameObject mainMenu;     // 主菜单
    private GameObject gameUI;       // 游戏UI
    private GameObject pauseMenu;    // 暂停菜单





    private bool isPaused = false;   // 是否暂停游戏
    private bool isStart=false;

    void Awake()
    {
        Initialize();
        // 自动获取 UI 元素
        mainMenu = GameObject.Find("MainMenuUI"); // 查找 MainMenu 对象
        gameUI = GameObject.Find("GameUI");     // 查找 GameUI 对象
        pauseMenu = GameObject.Find("PauseMenuUI"); // 查找 PauseMenu 对象

        // 获取组件
        // volumeSlider = GameObject.Find("VolumeSlider").GetComponent<Slider>();
        // scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        // powerBar = GameObject.Find("PowerBar").GetComponent<Slider>();

        // 确保 UI 元素初始化状态
        mainMenu.SetActive(true);
        gameUI.SetActive(false);
        pauseMenu.SetActive(false);

        environmentPrefab.SetActive(true);
        playerPrefab.SetActive(true);

  
    }

    // 游戏开始
    public void StartGame()
    {
        isStart=true;
        mainMenu.SetActive(false);  // 隐藏主菜单
        gameUI.SetActive(true);     // 显示游戏UI
        pauseMenu.SetActive(false); // 隐藏暂停菜单

    }

    // 暂停游戏
    public void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            pauseMenu.SetActive(true);   // 显示暂停菜单
            Time.timeScale = 0f;         // 停止游戏
        }
        else
        {
            pauseMenu.SetActive(false);  // 隐藏暂停菜单
            Time.timeScale = 1f;         // 恢复游戏
        }
    }

    // 继续游戏
    public void ContinueGame()
    {
        isPaused = false;
        pauseMenu.SetActive(false);  // 隐藏暂停菜单
        Time.timeScale = 1f;         // 恢复游戏
    }

    // 返回主菜单
    public void ReturnToMainMenu()
    {
        Debug.Log("1");
        Time.timeScale = 1f;         // 确保游戏恢复正常
        SceneManager.LoadScene("myScenes");  // 假设你有一个名为 "MainMenu" 的场景
    }

    // 音量调节
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;  // 设置全局音量
    }

    
    public bool getIsStart(){
        return isStart;
    }

    public bool getIsPaused(){
        return isPaused;
    }
   
    private void Initialize(){

        // 加载 Canvas 预制体
        canvasPrefab = Resources.Load<GameObject>("Prefabs/Canvas");
        if (canvasPrefab == null) Debug.LogError("Canvas 预制体未找到！");

        // 加载 Environment 预制体
        environmentPrefab = Resources.Load<GameObject>("Prefabs/Environment");
        if (environmentPrefab == null) Debug.LogError("Environment 预制体未找到！");

        // 加载 Player 预制体
        playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
        if (playerPrefab == null) Debug.LogError("Player 预制体未找到！");

        // 加载 EventSystem 预制体
        eventSystemPrefab = Resources.Load<GameObject>("Prefabs/EventSystem");
        if (eventSystemPrefab == null) Debug.LogError("EventSystem 预制体未找到！");
        // 实例化 Canvas
        if (canvasPrefab != null)
        {
            Instantiate(canvasPrefab, Vector3.zero, Quaternion.identity);
            Debug.Log("Canvas 已加载并实例化");
        }

        // 实例化 Environment
        if (environmentPrefab != null)
        {
            Instantiate(environmentPrefab, Vector3.zero, Quaternion.identity);
            Debug.Log("Environment 已加载并实例化");
        }

        // 实例化 Player
        if (playerPrefab != null)
        {
            Instantiate(playerPrefab, new Vector3(16,4,31), Quaternion.identity);
            Debug.Log("Player 已加载并实例化");
        }

        // 实例化 EventSystem
        if (eventSystemPrefab != null)
        {
            Instantiate(eventSystemPrefab, Vector3.zero, Quaternion.identity);
            Debug.Log("EventSystem 已加载并实例化");
        }
    }
    
}
