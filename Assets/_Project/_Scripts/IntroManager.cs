using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project._Scripts
{
    public class IntroManager : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
