using UnityEngine;

public class SoundActivatedMovingPlatform : MonoBehaviour
{
    [Header("Microphone Settings")]
    public float volumeThreshold = 0.05f; // Минимальная громкость для реакции
    public float sensitivity = 2.0f;     // Чувствительность микрофона

    [Header("Platform Movement")]
    public float riseSpeed = 3f;         // Скорость подъема
    public float lowerSpeed = 2f;       // Скорость опускания
    public float maxHeight = 3f;         // Максимальная высота подъема
    public float minHeight = 0f;         // Исходная высота (может быть отрицательной)

    private AudioClip microphoneInput;
    private string selectedMicrophone;
    private bool isMicrophoneConnected;
    private Vector3 initialPosition;
    private bool isRising = false;

    void Start()
    {
        // Проверка микрофона
        if (Microphone.devices.Length > 0)
        {
            selectedMicrophone = Microphone.devices[0];
            microphoneInput = Microphone.Start(selectedMicrophone, true, 1, 44100);
            isMicrophoneConnected = true;
        }
        else
        {
            Debug.LogError("No microphone detected!");
            isMicrophoneConnected = false;
        }

        initialPosition = transform.position; // Запоминаем начальную позицию платформы
    }

    void Update()
    {
        if (!isMicrophoneConnected) return;

        float currentVolume = GetMicrophoneVolume() * sensitivity;
        bool shouldRise = currentVolume > volumeThreshold;

        // Плавное движение платформы вверх/вниз
        if (shouldRise && transform.position.y < initialPosition.y + maxHeight)
        {
            transform.Translate(Vector3.up * riseSpeed * Time.deltaTime);
            isRising = true;
        }
        else if (!shouldRise && transform.position.y > initialPosition.y + minHeight)
        {
            transform.Translate(Vector3.down * lowerSpeed * Time.deltaTime);
            isRising = false;
        }

        // Ограничение высоты (чтобы платформа не улетала)
        float clampedY = Mathf.Clamp(
            transform.position.y, 
            initialPosition.y + minHeight, 
            initialPosition.y + maxHeight
        );
        transform.position = new Vector3(
            transform.position.x, 
            clampedY, 
            transform.position.z
        );
    }

    float GetMicrophoneVolume()
    {
        int sampleSize = 128;
        float[] samples = new float[sampleSize];
        int microphonePosition = Microphone.GetPosition(selectedMicrophone) - (sampleSize + 1);

        if (microphonePosition < 0) return 0;

        microphoneInput.GetData(samples, microphonePosition);

        float sum = 0;
        for (int i = 0; i < sampleSize; i++)
        {
            sum += Mathf.Abs(samples[i]);
        }

        return sum / sampleSize;
    }

    void OnDestroy()
    {
        if (isMicrophoneConnected)
        {
            Microphone.End(selectedMicrophone);
        }
    }
}