using UnityEngine;

public class SoundActivatedLoweringPlatform : MonoBehaviour
{
    [Header("Microphone Settings")]
    public float volumeThreshold = 0.05f; // Минимальная громкость для реакции
    public float sensitivity = 2.0f;     // Усиление сигнала микрофона

    [Header("Platform Movement")]
    public float lowerSpeed = 3f;        // Скорость опускания
    public float riseSpeed = 2f;         // Скорость подъема
    public float maxHeight = 0f;         // Исходная высота (верхняя граница)
    public float minHeight = -3f;        // Минимальная высота (насколько низко уйдет)

    private AudioClip microphoneInput;
    private string selectedMicrophone;
    private bool isMicrophoneConnected;
    private Vector3 initialPosition;

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

        initialPosition = transform.position; // Запоминаем начальную позицию
    }

    void Update()
    {
        if (!isMicrophoneConnected) return;

        float currentVolume = GetMicrophoneVolume() * sensitivity;
        bool shouldLower = currentVolume > volumeThreshold;

        // Движение платформы вниз/вверх
        if (shouldLower && transform.position.y > initialPosition.y + minHeight)
        {
            // Опускаем вниз
            transform.Translate(Vector3.down * lowerSpeed * Time.deltaTime);
        }
        else if (!shouldLower && transform.position.y < initialPosition.y + maxHeight)
        {
            // Поднимаем обратно
            transform.Translate(Vector3.up * riseSpeed * Time.deltaTime);
        }

        // Ограничение высоты (чтобы не ушла за границы)
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