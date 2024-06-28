using UnityEngine;

public class DualCameraController : MonoBehaviour
{
    public Camera verticalCamera;  // ������ ������ (������������)
    public Camera horizontalCamera;  // ������ ������ (��������������)
    public float rotationSpeed = 50f;  // �������� ��������

    void Update()
    {
        if (verticalCamera != null)
        {
            // ���������� ������������ �������
            float verticalRotation = Input.GetAxis("Vertical") * rotationSpeed * Time.deltaTime;
            verticalCamera.transform.Rotate(verticalRotation, 0, 0);

            // ����������� �������� �� �����������
            Vector3 euler = verticalCamera.transform.eulerAngles;
            euler.y = 0;  // ������������ �������� �� ��� Y
            verticalCamera.transform.eulerAngles = euler;
        }

        if (horizontalCamera != null)
        {
            // ���������� �������������� �������
            float horizontalRotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
            horizontalCamera.transform.Rotate(0, horizontalRotation, 0);

            // ����������� �������� �� ���������
            Vector3 euler = horizontalCamera.transform.eulerAngles;
            euler.x = 0;  // ������������ �������� �� ��� X
            horizontalCamera.transform.eulerAngles = euler;
        }
    }
}
