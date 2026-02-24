using UnityEngine;

namespace Assets.Scripts.Utilities
{
    public class ConsoleInfo : MonoBehaviour
    {
        private void Update()
        {
            PrintConsole();
        }

        private void PrintConsole()
        {
            Debug.Log("Hello World!");
        }
    }
}