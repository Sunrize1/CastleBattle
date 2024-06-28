using UnityEngine;

public class DualCameraController : MonoBehaviour
{
    public Camera verticalCamera;  // Первая камера (вертикальная)
    public Camera horizontalCamera;  // Вторая камера (горизонтальная)
    public float rotationSpeed = 50f;  // Скорость вращения

    void Update()
    {
        if (verticalCamera != null)
        {
            // Управление вертикальной камерой
            float verticalRotation = Input.GetAxis("Vertical") * rotationSpeed * Time.deltaTime;
            verticalCamera.transform.Rotate(verticalRotation, 0, 0);

            // Ограничение вращения по горизонтали
            Vector3 euler = verticalCamera.transform.eulerAngles;
            euler.y = 0;  // Ограничиваем вращение по оси Y
            verticalCamera.transform.eulerAngles = euler;
        }

        if (horizontalCamera != null)
        {
            // Управление горизонтальной камерой
            float horizontalRotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
            horizontalCamera.transform.Rotate(0, horizontalRotation, 0);

            // Ограничение вращения по вертикали
            Vector3 euler = horizontalCamera.transform.eulerAngles;
            euler.x = 0;  // Ограничиваем вращение по оси X
            horizontalCamera.transform.eulerAngles = euler;
        }
    }
}
