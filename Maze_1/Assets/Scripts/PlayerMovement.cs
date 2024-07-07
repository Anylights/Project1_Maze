using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 添加对UI命名空间的引用

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;
    public ParticleSystem teleportEffect; // 添加对ParticleSystem的引用
    public Text errorMessage; // 添加对Text的引用

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;
    float fixedYPosition = 0f; // 初始化为0
    bool errorMessageDisplayed = false; // 用于跟踪错误信息是否已显示

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        errorMessage.gameObject.SetActive(false); // 初始化时隐藏错误消息
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);

        // 播放或停止脚步声
        if (isWalking && !isPlayingFootstep)
        {
            PlayFootstepSound();
        }
        else if (!isWalking && isPlayingFootstep)
        {
            StopFootstepSound();
        }

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);
    }

    void OnAnimatorMove()
    {
        // 在应用移动和旋转时，保持y坐标不变
        Vector3 newPosition = m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude;
        newPosition.y = fixedYPosition;

        m_Rigidbody.MovePosition(newPosition);
        m_Rigidbody.MoveRotation(m_Rotation);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (IsPlayerStationary())
            {
                StartCoroutine(ChangeYPosition());
                errorMessageDisplayed = false; // 重置错误信息显示状态
            }
            else if (!errorMessageDisplayed)
            {
                StartCoroutine(ShowErrorMessage("Can't transform when walking!\n\n")); // 显示错误消息
            }
        }
    }

    bool IsPlayerStationary()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        return Mathf.Approximately(horizontal, 0f) && Mathf.Approximately(vertical, 0f);
    }

    IEnumerator ChangeYPosition()
    {
        yield return new WaitForSeconds(0f);

        // 计算传送后的新位置
        Vector3 newPosition;
        if (Mathf.Approximately(fixedYPosition, 0f))
        {
            newPosition = new Vector3(m_Rigidbody.position.x, 56.52612f, m_Rigidbody.position.z);
            fixedYPosition = 56.52612f;
        }
        else if (Mathf.Approximately(fixedYPosition, 56.52612f))
        {
            newPosition = new Vector3(m_Rigidbody.position.x, 0f, m_Rigidbody.position.z);
            fixedYPosition = 0f;
        }
        else
        {
            newPosition = m_Rigidbody.position; // 默认情况下保持当前位置
        }

        // 在新位置播放特效
        if (teleportEffect != null)
        {
            Instantiate(teleportEffect, newPosition, Quaternion.identity);
            teleportEffect.Play();
        }

        // 更新刚体的位置
        m_Rigidbody.position = newPosition;
    }

    IEnumerator ShowErrorMessage(string message)
    {
        errorMessage.text = message;
        errorMessage.gameObject.SetActive(true);
        errorMessageDisplayed = true; // 设置错误信息显示状态
        yield return new WaitForSeconds(2f); // 显示2秒钟
        errorMessage.gameObject.SetActive(false);
    }

    void PlayFootstepSound()
    {
        if (Mathf.Approximately(fixedYPosition, 0f))
        {
            m_AudioSource.clip = footstepSound1;
        }
        else if (Mathf.Approximately(fixedYPosition, 56.52612f))
        {
            m_AudioSource.clip = footstepSound2;
        }

        if (m_AudioSource.clip != null)
        {
            m_AudioSource.loop = true;
            m_AudioSource.Play();
            isPlayingFootstep = true;
        }
    }

    void StopFootstepSound()
    {
        if (isPlayingFootstep)
        {
            m_AudioSource.Stop();
            isPlayingFootstep = false;
        }
    }
}
