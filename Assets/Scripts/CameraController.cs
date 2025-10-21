using UnityEngine;

public class CameraController : MonoBehaviour

{
    //��]�ݒ�
    public float mouseSensitivity = 3.0f; // �}�E�X���x
    public float minVerticalAngle = -15.0f; // ����������
    public float maxVerticalAngle = 15.0f;  // ���������
                                            //�Ǐ]�ݒ�
    Transform player; // �v���C���[�Q��
    public Vector3 offset = new Vector3(0f, 2f, -5f); // �v���C���[����̑��Έʒu
    public float followSpeed = 16f; // �Ǐ]�X�s�[�h
    float verticalRotation = 0f; // �J�����̏㉺�p
    float currentYaw = 0f;       // �v���C���[�̍��E��]�i���R�j

    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
            else
            {
                enabled = false;
                return;
            }
        }

        // �J�[�\�����b�N
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // �����p�x�ݒ�
        Vector3 angles = transform.eulerAngles;
        currentYaw = angles.y;
        verticalRotation = angles.x;
    }

    void LateUpdate()
    {
        if (GameManager.gameState != GameState.playing) return;

        // �}�E�X����
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // ����]�i�����Ȃ��j
        currentYaw += mouseX;

        // �c��]�i��������j
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, minVerticalAngle, maxVerticalAngle);

        // �J������]��K�p
        Quaternion rotation = Quaternion.Euler(verticalRotation, currentYaw, 0f);

        // �v���C���[�̌���Ɉʒu����悤�ɃJ�������W���v�Z
        Vector3 desiredPosition = player.position + rotation * offset;

        // �X���[�Y�ɒǏ]
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // �J�������v���C���[�Ɍ�����
        transform.LookAt(player.position + Vector3.up * 2f);

        // �v���C���[�̌������J������Y��]�ɍ��킹��i�㉺�͖����j
        player.rotation = Quaternion.Euler(0f, currentYaw, 0f);
    }
}