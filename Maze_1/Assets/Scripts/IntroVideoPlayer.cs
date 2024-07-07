using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroVideoPlayer : MonoBehaviour
{

    public VideoClip introVideo; // 引用开场视频
    public string nextSceneName; // 要加载的场景名称

    private VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = gameObject.GetComponent<VideoPlayer>();
        videoPlayer.playOnAwake = false;
        videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
        videoPlayer.clip = introVideo;
        videoPlayer.loopPointReached += OnVideoEnd;

        PlayVideo();
    }

    void PlayVideo()
    {
        videoPlayer.Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        if (nextSceneName != null)
            SceneManager.LoadScene(nextSceneName);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            SceneManager.LoadScene(nextSceneName);

    }
}
