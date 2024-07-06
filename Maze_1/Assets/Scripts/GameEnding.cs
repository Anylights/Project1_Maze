using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class GameEnding : MonoBehaviour
{
    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
    public GameObject player;
    public CanvasGroup exitBackgroundImageCanvasGroup;
    public VideoPlayer endVideoPlayer; // 新增VideoPlayer引用
    public GameObject videoCanvas; // 用于显示VideoPlayer的UI画布
    public AudioSource Back_audioSource;

    bool m_IsPlayerAtExit;
    float m_Timer;
    float m_Timer_1;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            m_IsPlayerAtExit = true;
        }
    }

    void Update()
    {
        if (m_IsPlayerAtExit)
        {
            EndLevel();
        }

        // 检测按下Esc键
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void EndLevel()
    {
        m_Timer += Time.deltaTime;

        Back_audioSource.volume = 1 - m_Timer / fadeDuration;
        exitBackgroundImageCanvasGroup.alpha = m_Timer / fadeDuration;

        if (m_Timer > fadeDuration + displayImageDuration)
        {
            StartEndVideo(); // 开始播放结束动画
            if (m_Timer > 15f)
                Application.Quit();
        }
    }

    void StartEndVideo()
    {
        m_Timer_1 += Time.deltaTime;
        // 显示视频画布
        videoCanvas.SetActive(true);

        // 播放视频
        endVideoPlayer.Play();
        exitBackgroundImageCanvasGroup.alpha = 1 - m_Timer_1 / 3f;
        // 注册视频播放结束事件


    }
}