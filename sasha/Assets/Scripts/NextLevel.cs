using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndTrigger : MonoBehaviour
{
    public string nextLevelName; // Имя следующей сцены
    
    public float transitionDelay = 1f; // Задержка перед переходом
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Проверяем, что столкнулись с игроком
        if (collision.CompareTag("Player"))
        {
            
            

            // Запускаем переход с задержкой
            Invoke("LoadNextLevel", transitionDelay);
        }
    }

    private void LoadNextLevel()
    {
        // Загружаем следующую сцену
        SceneManager.LoadScene(nextLevelName);
    }
}