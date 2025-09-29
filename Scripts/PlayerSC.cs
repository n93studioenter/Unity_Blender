using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Tốc độ di chuyển
    private Animator animator; // Tham chiếu đến Animator
    private bool isAttack = false;

    private int comboStep = 0; // Bước combo hiện tại
    private float comboResetTime = 1.5f; // Thời gian giữa các đòn tấn công
    private float lastAttackTime = 0; // Thời gian tấn công lần cuối
    private float timeCbb1 = 0.3f;
    private float timeCbb2 = 0.3f;
    private float timeCbb3 = 0.5f;
    void Start()
    {
        animator = GetComponent<Animator>(); // Lấy tham chiếu đến Animator
    }

    void Update()
    {
        if (isAttack == false)
        {
            // Lấy input di chuyển từ bàn phím
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            // Tính toán vectơ di chuyển
            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

            // Kiểm tra trạng thái di chuyển
            if (movement.magnitude > 0)
            {
                animator.SetInteger("State", 1); // Di chuyển
                MoveCharacter(movement);
            }
            else
            {
                animator.SetInteger("State", 0); // Dừng lại
            }

            // Gọi phương thức di chuyển trong LateUpdate


            if (Input.GetMouseButtonDown(0))
            { 
                RotateCharacterToMouse();
                StartCoroutine(Attack());
            }
            if (Time.time - lastAttackTime > comboResetTime)
            {
                comboStep = 0; // Reset combo nếu quá thời gian
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(Dash());
            }
           
        }
        
    }
    private IEnumerator Dash()
    {
        // Đặt trạng thái dừng lại
        animator.SetInteger("State", 5);

        // Tính toán hướng di chuyển
        Vector3 dashDirection = transform.forward; // Hướng đi về phía trước
        float dashSpeed = 2f; // Tốc độ dash
        float dashDuration = 0.5f; // Thời gian dash
        float elapsedTime = 0f;

        // Di chuyển nhân vật trong khi dash
        while (elapsedTime < dashDuration)
        {
            transform.Translate(dashDirection * dashSpeed * Time.deltaTime, Space.World);
            elapsedTime += Time.deltaTime;
            yield return null; // Đợi đến frame tiếp theo
        }

        // Trở về trạng thái dừng lại
        animator.SetInteger("State", 0);
    }
    private IEnumerator Attack()
    {
        lastAttackTime = Time.time; // Cập nhật thời gian tấn công lần cuối

        comboStep++;
        if (comboStep > 3) comboStep = 1; // Reset combo nếu vượt quá số lượng đòn tấn công
         
        isAttack = true;
        animator.SetInteger("State", comboStep+1); // Chuyển sang trạng thái tấn công

        // Đợi 2 giây
        if(comboStep == 1)
            yield return new WaitForSeconds(timeCbb1);
        else if (comboStep == 2)
            yield return new WaitForSeconds(timeCbb2);
        else if (comboStep == 3)
            yield return new WaitForSeconds(timeCbb3);
        yield return new WaitForSeconds(0.3f);

        // Trở về trạng thái cũ (dừng lại hoặc di chuyển)
        isAttack=false;
        animator.SetInteger("State", 0); // Dừng lại
    }
    private void RotateCharacterToMouse()
    {
        // Lấy vị trí chuột trên màn hình
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Tính toán hướng xoay đến vị trí chuột
            Vector3 targetPosition = hit.point;
            targetPosition.y = transform.position.y; // Giữ nguyên chiều cao

            // Lưu góc xoay mục tiêu
            Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 30f); // Tốc độ xoay
        }
    }
    void LateUpdate()
    {
        // Gọi phương thức xoay trong LateUpdate
        if(isAttack==false)
            RotateCharacter();
    }

    private void MoveCharacter(Vector3 movement)
    {
        // Di chuyển nhân vật
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
    }

    private void RotateCharacter()
    {
        // Lấy hướng di chuyển từ input
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        if (movement.magnitude > 0)
        {
            // Tính toán góc xoay dựa trên hướng di chuyển
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 20f); // Tốc độ xoay
        }
    }
}