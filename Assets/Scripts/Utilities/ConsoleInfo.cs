using UnityEngine;

public class ConsoleInfo: MonoBehaviour
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