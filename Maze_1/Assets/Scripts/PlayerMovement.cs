using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;
    float fixedYPosition = 0f; // 初始化为0

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
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
            StartCoroutine(ChangeYPosition());
        }
    }

    IEnumerator ChangeYPosition()
    {
        yield return new WaitForSeconds(0f);

        if (Mathf.Approximately(fixedYPosition, 0f))
        {
            fixedYPosition = 56.5f;
        }
        else if (Mathf.Approximately(fixedYPosition, 56.5f))
        {
            fixedYPosition = 0f;
        }

        // 更新刚体的位置
        m_Rigidbody.position = new Vector3(m_Rigidbody.position.x, fixedYPosition, m_Rigidbody.position.z);
    }
}
