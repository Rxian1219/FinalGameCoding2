using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{


    // ===================== 人物移动相关 =====================
    [Header("Movement")]
    public float walkSpeed = 5f;                  // 正常行走速度
    public float runMultiplier = 1.5f;            // 跑步时速度倍率（行走速度 × 跑步倍率）
    public float crouchSpeedMultiplier = 0.5f;    // 蹲下时速度倍率（行走速度 × 蹲下倍率）
    public float jumpForce = 3.5f;                // 跳跃的力度（数值越大跳得越高）
    public float gravity = -20f;                  // 重力（必须是负数，越大下落越快）
    public float mouseSensitivity = 2f;           // 鼠标灵敏度（视角旋转速度）

    // ===================== 耐力系统 =====================
    [Header("Stamina")]
    public float stamina = 10f;                   // 当前耐力值
    public float maxStamina = 10f;                // 最大耐力值
    public float runStaminaCostPerHalfSecond = 1f; // 跑步每0.5秒消耗的耐力
    public float jumpStaminaCost = 1f;            // 跳跃每次消耗的耐力
    public float staminaRegenDelay = 1.5f;        // 停止跑步多久后开始回血
    public float staminaRegenRate = 2f;           // 每秒恢复多少耐力
    public Image staminaBar;                      // 用于展示耐力条的 UI Image

    // ===================== 蹲下系统 =====================
    [Header("Crouch")]
    public float crouchScale = 0.66f;             // 蹲下时人物缩小的高度比例

    // ===================== 跳跃缓冲（按早了/晚了都能跳） =====================
    [Header("Jump Buffer")]
    public float jumpGraceTime = 0.2f;            // 缓冲时间窗口（单位：秒）

    // ========== 私有变量 ==========
    private CharacterController controller;       // Unity内建角色控制器（处理移动、碰撞）
    private Transform cameraTransform;            // 主摄像机（处理上下视角）
    private float yRotation = 0f;                 // 当前摄像机的上下旋转角度
    private float originalYScale;                 // 角色初始高度（用于恢复）
    private float staminaUseTimer = 0f;           // 用于计时跑步耐力消耗
    private float staminaRegenTimer = 0f;         // 用于计时耐力恢复
    private float lastGroundedTime;               // 最近一次在地面的时间
    private float lastJumpPressedTime;            // 最近一次按下跳跃键的时间
    private bool isRunning = false;               // 当前是否处于跑步状态
    private Vector3 velocity;                     // 当前垂直速度（用于重力和跳跃）

    void Start()
    {
        controller = GetComponent<CharacterController>();        // 获取角色控制器
        cameraTransform = Camera.main.transform;                 // 获取主摄像机
        Cursor.lockState = CursorLockMode.Locked;                // 锁定鼠标在屏幕中间
        Cursor.visible = false;                                  // 隐藏鼠标
        originalYScale = transform.localScale.y;                 // 记录初始身高
    }

    void Update()
    {
        HandleLook();         // 处理鼠标控制视角
        HandleMovement();     // 处理移动、跑步、蹲下、耐力
        HandleJump();         // 处理跳跃、重力、跳跃耗耐力
        UpdateStaminaBar();   // 更新 UI 耐力条
    }

    // ========== 鼠标控制视角 ==========
    void HandleLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX); // 控制左右旋转（角色整体）

        yRotation -= mouseY; // 控制上下旋转（摄像机）
        yRotation = Mathf.Clamp(yRotation, -90f, 90f); // 限制视角范围
        cameraTransform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
    }

    // ========== 玩家移动、跑步、蹲下、耐力恢复 ==========
    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        bool isCrouching = Input.GetKey(KeyCode.C);
        bool wantsToRun = Input.GetKey(KeyCode.LeftShift);
        bool isMoving = move.magnitude > 0.1f;

        float speed = walkSpeed;

        // 蹲下：速度变慢
        if (isCrouching)
        {
            speed *= crouchSpeedMultiplier;
        }

        // 是否允许跑步（有按键 + 不蹲下 + 正在移动 + 有耐力）
        isRunning = wantsToRun && !isCrouching && isMoving && stamina > 0f;

        if (isRunning)
        {
            speed *= runMultiplier;
        }

        // 实际移动
        controller.Move(move * speed * Time.deltaTime);

        // ================= 耐力逻辑 =================

        // 跑步时每0.5秒扣1点耐力
        if (isRunning)
        {
            staminaUseTimer += Time.deltaTime;
            if (staminaUseTimer >= 0.5f)
            {
                stamina = Mathf.Max(0f, stamina - runStaminaCostPerHalfSecond);
                staminaUseTimer = 0f;
            }
            staminaRegenTimer = 0f; // 正在跑步时不恢复
        }
        else
        {
            // 停止跑步开始恢复耐力
            staminaRegenTimer += Time.deltaTime;
            if (staminaRegenTimer >= staminaRegenDelay)
            {
                stamina += staminaRegenRate * Time.deltaTime;
                stamina = Mathf.Min(stamina, maxStamina);
            }
        }

        // 蹲下缩小身高
        if (isCrouching)
        {
            transform.localScale = new Vector3(1f, originalYScale * crouchScale, 1f);
        }
        else
        {
            transform.localScale = new Vector3(1f, originalYScale, 1f);
        }
    }

    // ========== 跳跃处理、缓冲、耐力消耗 ==========
    void HandleJump()
    {
        // 防止角色贴地时不断往下加速度（跳跃前稳定在地面）
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // 记录最后一次站在地面时间
        if (controller.isGrounded)
            lastGroundedTime = Time.time;

        // 记录最后一次按跳跃键的时间
        if (Input.GetKeyDown(KeyCode.Space))
            lastJumpPressedTime = Time.time;

        // 缓冲跳跃：地面+空格都在缓冲范围内 + 有耐力，才跳
        if (Time.time - lastGroundedTime <= jumpGraceTime &&
            Time.time - lastJumpPressedTime <= jumpGraceTime &&
            stamina >= jumpStaminaCost)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity); // 应用跳跃力度
            stamina -= jumpStaminaCost; // 扣除跳跃耐力

            // 防止多次跳跃
            lastGroundedTime = -999f;
            lastJumpPressedTime = -999f;
        }

        // 重力应用 + 下落加速
        if (!controller.isGrounded && velocity.y < 0)
        {
            velocity.y += gravity * 1.5f * Time.deltaTime; // 下落时更快
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    // ========== 更新耐力条UI显示 ==========
    void UpdateStaminaBar()
    {
        if (staminaBar != null)
        {
            staminaBar.fillAmount = stamina / maxStamina;
        }
    }

}
