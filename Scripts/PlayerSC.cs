using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Tốc độ di chuyển
    private Animator animator; // Tham chiếu đến Animator

    void Start()
    {
        animator = GetComponent<Animator>(); // Lấy tham chiếu đến Animator
    }

    void Update()
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
        }
        else
        {
            animator.SetInteger("State", 0); // Dừng lại
        }

        // Gọi phương thức di chuyển trong LateUpdate
        MoveCharacter(movement);

        if (Input.GetMouseButtonDown(0))
        {
            RotateCharacterToMouse();
        }
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
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 60f); // Tốc độ xoay
        }
    }
    void LateUpdate()
    {
        // Gọi phương thức xoay trong LateUpdate
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