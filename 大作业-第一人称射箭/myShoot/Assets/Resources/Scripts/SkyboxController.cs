using UnityEngine;

public class SkyboxController : MonoBehaviour
{
    // 可序列化的变量，用于在Inspector中拖拽设置
    [SerializeField] private Material sunnySkybox;  // 阳光明媚的天空盒
    [SerializeField] private Material rainySkybox;  // 雨天的天空盒
    [SerializeField] private Material snowySkybox;  // 雪天的天空盒
    [SerializeField] private Material cloudySkybox; // 多云的天空盒

    [SerializeField] private KeyCode switchKey = KeyCode.T; // 切换天空盒的按键

    private int currentSkyboxIndex = 0; // 当前天空盒索引

    private Material[] skyboxes; // 存储所有天气对应的天空盒

    private void Start()
    {
        // 初始化天空盒数组
        skyboxes = new Material[] { sunnySkybox, rainySkybox, snowySkybox, cloudySkybox };

        // 设置初始天空盒
        UpdateSkybox();
    }

    private void Update()
    {
        // 按下切换键切换天空盒
        if (Input.GetKeyDown(switchKey))
        {
            SwitchSkybox();
        }
    }

    private void SwitchSkybox()
    {
        // 循环切换天空盒
        currentSkyboxIndex = (currentSkyboxIndex + 1) % skyboxes.Length;
        UpdateSkybox();
    }

    private void UpdateSkybox()
    {
        // 设置场景的天空盒为当前选中的天空盒
        if (skyboxes.Length > 0)
        {
            RenderSettings.skybox = skyboxes[currentSkyboxIndex];
            AdjustLightingBasedOnWeather();
            Debug.Log($"Switched to {skyboxes[currentSkyboxIndex].name} skybox.");
        }
    }

    private void AdjustLightingBasedOnWeather()
    {
        // 根据不同天气调整光源的强度和颜色
        switch (currentSkyboxIndex)
        {
            case 0: // Sunny
                RenderSettings.skybox = sunnySkybox;
                GetComponent<Light>().intensity = 1.0f;
                GetComponent<Light>().color = Color.white; // 白色光
                break;

            case 1: // Rainy
                RenderSettings.skybox = rainySkybox;
                GetComponent<Light>().intensity = 0.6f;
                GetComponent<Light>().color = new Color(0.7f, 0.7f, 0.7f); // 灰色光，模拟阴天
                break;

            case 2: // Snowy
                RenderSettings.skybox = snowySkybox;
                GetComponent<Light>().intensity = 0.8f;
                GetComponent<Light>().color = new Color(0.9f, 0.9f, 0.9f); // 亮白光
                break;

            case 3: // Cloudy
                RenderSettings.skybox = cloudySkybox;
                GetComponent<Light>().intensity = 0.7f;
                GetComponent<Light>().color = new Color(0.8f, 0.8f, 0.8f); // 朦胧白光
                break;
        }
    }
}
