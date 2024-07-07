using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyUI : MonoBehaviour
{
    [SerializeField] Animator m_canvasGroup;
    [SerializeField] bool pannelActive;
    public void ClosePannel()
    {
        Debug.Log("ButtonDown");
        m_canvasGroup.Play("FadeOut");
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
#else
        Application.Quit();
#endif
    }
    private void ESC()
    {
        if (pannelActive)
            m_canvasGroup.Play("FadeIn");
        else
            m_canvasGroup.Play("FadeOut");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pannelActive = !pannelActive;
            ESC();
        }
    }
}
