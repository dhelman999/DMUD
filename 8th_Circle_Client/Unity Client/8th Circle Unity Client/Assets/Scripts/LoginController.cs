using System;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Lightweight login controller to handle the logic to login to the server
public class LoginController : MonoBehaviour
{
    // Constants
    internal const int MUD_SERVER_PORT = 8888;

    // References
    public Text mServerIpText;
    public Text mStatusText;

    public enum Scenes
    {
        MAIN_MENU,
        MAIN_CLIENT
    }// Scenes

    // Load a scene via build index
    public void LoadByIndex(int sceneIndex)
    {
        if ((Scenes)sceneIndex == Scenes.MAIN_MENU)
            return;
        else if ((Scenes)sceneIndex == Scenes.MAIN_CLIENT)
            SceneManager.LoadScene(sceneIndex);
    }// LoadByIndex

    // Attempt to connect to the server and load the main client scene
    public void LoadMainClientScene(bool useLocalHost)
    {
        GlobalData.sServerIPAddr = mServerIpText.text;
        String serverAddr = mServerIpText.text;

        if (useLocalHost)
            serverAddr = "127.0.0.1";

        try
        {
            GlobalData.sSocketForServer = new TcpClient(serverAddr, MUD_SERVER_PORT);
        }
        catch
        {
            mStatusText.text = "Failed to connect to " + serverAddr + "::" + MUD_SERVER_PORT;
            return;
        }

        LoadByIndex((int)Scenes.MAIN_CLIENT);
    }// LoadMainClientScene

    // Leave Unity play mode or the application
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }// Quit

}// LoginController
