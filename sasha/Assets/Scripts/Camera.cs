using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Игрок
    public float smoothSpeed = 0.125f; // Скорость сглаживания
    public Vector3 offset; // Смещение камеры относительно игрока

    public Vector2 minBounds; // Минимальные границы (X, Y)
    public Vector2 maxBounds; // Максимальные границы (X, Y)

    private Transform currentTarget; // Текущая цель камеры

    void Start()
    {
        // По умолчанию камера следует за первым игроком
        currentTarget = target;
    }

    void LateUpdate()
    {
        if (currentTarget != null)
        {
            // Вычисляем желаемую позицию камеры
            Vector3 desiredPosition = currentTarget.position + offset;

            // Ограничиваем позицию камеры в пределах границ
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minBounds.x, maxBounds.x);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minBounds.y, maxBounds.y);

            // Плавно перемещаем камеру к желаемой позиции
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            // Устанавливаем позицию камеры
            transform.position = smoothedPosition;
        }
    }

}