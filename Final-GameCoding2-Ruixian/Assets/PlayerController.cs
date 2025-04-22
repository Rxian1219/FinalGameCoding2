using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{


    // ===================== �����ƶ���� =====================
    [Header("Movement")]
    public float walkSpeed = 5f;                  // ���������ٶ�
    public float runMultiplier = 1.5f;            // �ܲ�ʱ�ٶȱ��ʣ������ٶ� �� �ܲ����ʣ�
    public float crouchSpeedMultiplier = 0.5f;    // ����ʱ�ٶȱ��ʣ������ٶ� �� ���±��ʣ�
    public float jumpForce = 3.5f;                // ��Ծ�����ȣ���ֵԽ������Խ�ߣ�
    public float gravity = -20f;                  // �����������Ǹ�����Խ������Խ�죩
    public float mouseSensitivity = 2f;           // ��������ȣ��ӽ���ת�ٶȣ�

    // ===================== ����ϵͳ =====================
    [Header("Stamina")]
    public float stamina = 10f;                   // ��ǰ����ֵ
    public float maxStamina = 10f;                // �������ֵ
    public float runStaminaCostPerHalfSecond = 1f; // �ܲ�ÿ0.5�����ĵ�����
    public float jumpStaminaCost = 1f;            // ��Ծÿ�����ĵ�����
    public float staminaRegenDelay = 1.5f;        // ֹͣ�ܲ���ú�ʼ��Ѫ
    public float staminaRegenRate = 2f;           // ÿ��ָ���������
    public Image staminaBar;                      // ����չʾ�������� UI Image

    // ===================== ����ϵͳ =====================
    [Header("Crouch")]
    public float crouchScale = 0.66f;             // ����ʱ������С�ĸ߶ȱ���

    // ===================== ��Ծ���壨������/���˶������� =====================
    [Header("Jump Buffer")]
    public float jumpGraceTime = 0.2f;            // ����ʱ�䴰�ڣ���λ���룩

    // ========== ˽�б��� ==========
    private CharacterController controller;       // Unity�ڽ���ɫ�������������ƶ�����ײ��
    private Transform cameraTransform;            // ������������������ӽǣ�
    private float yRotation = 0f;                 // ��ǰ�������������ת�Ƕ�
    private float originalYScale;                 // ��ɫ��ʼ�߶ȣ����ڻָ���
    private float staminaUseTimer = 0f;           // ���ڼ�ʱ�ܲ���������
    private float staminaRegenTimer = 0f;         // ���ڼ�ʱ�����ָ�
    private float lastGroundedTime;               // ���һ���ڵ����ʱ��
    private float lastJumpPressedTime;            // ���һ�ΰ�����Ծ����ʱ��
    private bool isRunning = false;               // ��ǰ�Ƿ����ܲ�״̬
    private Vector3 velocity;                     // ��ǰ��ֱ�ٶȣ�������������Ծ��

    void Start()
    {
        controller = GetComponent<CharacterController>();        // ��ȡ��ɫ������
        cameraTransform = Camera.main.transform;                 // ��ȡ�������
        Cursor.lockState = CursorLockMode.Locked;                // �����������Ļ�м�
        Cursor.visible = false;                                  // �������
        originalYScale = transform.localScale.y;                 // ��¼��ʼ���
    }

    void Update()
    {
        HandleLook();         // �����������ӽ�
        HandleMovement();     // �����ƶ����ܲ������¡�����
        HandleJump();         // ������Ծ����������Ծ������
        UpdateStaminaBar();   // ���� UI ������
    }

    // ========== �������ӽ� ==========
    void HandleLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX); // ����������ת����ɫ���壩

        yRotation -= mouseY; // ����������ת���������
        yRotation = Mathf.Clamp(yRotation, -90f, 90f); // �����ӽǷ�Χ
        cameraTransform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
    }

    // ========== ����ƶ����ܲ������¡������ָ� ==========
    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        bool isCrouching = Input.GetKey(KeyCode.C);
        bool wantsToRun = Input.GetKey(KeyCode.LeftShift);
        bool isMoving = move.magnitude > 0.1f;

        float speed = walkSpeed;

        // ���£��ٶȱ���
        if (isCrouching)
        {
            speed *= crouchSpeedMultiplier;
        }

        // �Ƿ������ܲ����а��� + ������ + �����ƶ� + ��������
        isRunning = wantsToRun && !isCrouching && isMoving && stamina > 0f;

        if (isRunning)
        {
            speed *= runMultiplier;
        }

        // ʵ���ƶ�
        controller.Move(move * speed * Time.deltaTime);

        // ================= �����߼� =================

        // �ܲ�ʱÿ0.5���1������
        if (isRunning)
        {
            staminaUseTimer += Time.deltaTime;
            if (staminaUseTimer >= 0.5f)
            {
                stamina = Mathf.Max(0f, stamina - runStaminaCostPerHalfSecond);
                staminaUseTimer = 0f;
            }
            staminaRegenTimer = 0f; // �����ܲ�ʱ���ָ�
        }
        else
        {
            // ֹͣ�ܲ���ʼ�ָ�����
            staminaRegenTimer += Time.deltaTime;
            if (staminaRegenTimer >= staminaRegenDelay)
            {
                stamina += staminaRegenRate * Time.deltaTime;
                stamina = Mathf.Min(stamina, maxStamina);
            }
        }

        // ������С���
        if (isCrouching)
        {
            transform.localScale = new Vector3(1f, originalYScale * crouchScale, 1f);
        }
        else
        {
            transform.localScale = new Vector3(1f, originalYScale, 1f);
        }
    }

    // ========== ��Ծ�������塢�������� ==========
    void HandleJump()
    {
        // ��ֹ��ɫ����ʱ�������¼��ٶȣ���Ծǰ�ȶ��ڵ��棩
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // ��¼���һ��վ�ڵ���ʱ��
        if (controller.isGrounded)
            lastGroundedTime = Time.time;

        // ��¼���һ�ΰ���Ծ����ʱ��
        if (Input.GetKeyDown(KeyCode.Space))
            lastJumpPressedTime = Time.time;

        // ������Ծ������+�ո��ڻ��巶Χ�� + ������������
        if (Time.time - lastGroundedTime <= jumpGraceTime &&
            Time.time - lastJumpPressedTime <= jumpGraceTime &&
            stamina >= jumpStaminaCost)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity); // Ӧ����Ծ����
            stamina -= jumpStaminaCost; // �۳���Ծ����

            // ��ֹ�����Ծ
            lastGroundedTime = -999f;
            lastJumpPressedTime = -999f;
        }

        // ����Ӧ�� + �������
        if (!controller.isGrounded && velocity.y < 0)
        {
            velocity.y += gravity * 1.5f * Time.deltaTime; // ����ʱ����
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    // ========== ����������UI��ʾ ==========
    void UpdateStaminaBar()
    {
        if (staminaBar != null)
        {
            staminaBar.fillAmount = stamina / maxStamina;
        }
    }

}
