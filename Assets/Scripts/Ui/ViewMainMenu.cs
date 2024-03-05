using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewMainMenu : UiView {
    public void OnBeginButton() {
        SceneManager.LoadScene("GameScene");
    }

    public void OnExitButton() {
        Application.Quit(0);
    }
}
