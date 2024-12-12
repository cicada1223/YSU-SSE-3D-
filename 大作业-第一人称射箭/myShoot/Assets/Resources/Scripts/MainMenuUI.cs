using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainMenuUI : MonoBehaviour

{
   
    public Slider volumeSlider;  // 音量滑动条
    public Button startButton;   // 开始游戏按钮
    private MainControl mainControl;

    private void Start()
    {
         mainControl = FindObjectOfType<MainControl>();

        // 初始化音量滑动条
        if (volumeSlider != null)
        {
            volumeSlider.value = AudioListener.volume;
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

        }

        // 为开始游戏按钮添加点击事件
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
        }
    }

    // 音量滑动条值变化时调用
   private void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;  // 设置系统音量
    }

    // 开始游戏按钮点击时调用
    private void OnStartButtonClicked()
    {
        // 切换到游戏界面或加载游戏场景
        Debug.Log("开始游戏");
        mainControl.StartGame();


    }
}
