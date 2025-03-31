using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadlySurface : MonoBehaviour
{
    public float restartDelay = 1f; // Задержка перед перезапуском
    
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Проверяем, что столкнулись с игроком
        if (collision.gameObject.CompareTag("Player"))
        {
            
            
            // Запускаем перезапуск с задержкой
            Invoke("RestartLevel", restartDelay);
        }
    }

    private void RestartLevel()
    {
        // Перезагружаем текущую сцену
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}