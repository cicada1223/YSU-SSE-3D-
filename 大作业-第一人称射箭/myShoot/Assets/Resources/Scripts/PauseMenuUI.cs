using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    private MainControl mainControl;
    public Button continueButton;  // 继续游戏按钮
    public Button mainMenuButton;  // 返回主菜单按钮

    private void Start()
    {
         mainControl = FindObjectOfType<MainControl>();
        // 为按钮添加点击事件
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(OnContinueButtonClicked);
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
        }
    }

    // 继续游戏按钮点击时调用
    private void OnContinueButtonClicked()
    {
        mainControl.ContinueGame();
    }

    // 返回主菜单按钮点击时调用
    private void OnMainMenuButtonClicked()
    {
        mainControl.ReturnToMainMenu();
    }


}
