using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

// Main client controller which communicates to the server, updates the UI and generally holds
// all logic pertaining to client processing.
public class ClientController : MonoBehaviour
{
    // Constants
    // Maximum amount of characters in the scolling window before truncating takes place
    internal const int OUTPUTGRID_THRESHOLD = 10000;
    // Maximum number of commands recorded
    internal const int INPUTHISTORY_BUFFER_CAPACITY = 50;

    // References
    public InputField mClientInputField;
    public GameObject mScrollOutputGrid;

    // Statics
    // Network elements
    static NetworkStream sNetworkStream;
    static System.IO.StreamReader sStreamReader;
    static System.IO.StreamWriter sStreamWriter;
    static Queue<String> sServerResponses;

    // MT Locks
    static object sQueueLock;
    static object sNetworkLock;

    // Privates
    // Command history buffered to use with the up and down arrow keys
    private List<String> mInputHistory;

    // Current index of where we are in the command history buffer
    private int mInputHistoryIndex;

    // Use this for initialization
    void Awake()
    {
        Text outputText = mScrollOutputGrid.GetComponent<Text>();
        sServerResponses = new Queue<String>();
        sQueueLock = new object();
        sNetworkLock = new object();
        mInputHistory = new List<String>();
        mInputHistoryIndex = 0;

        // Only try to initialize the network elements if we are connected to the server
        if (GlobalData.sSocketForServer != null)
        {
            sNetworkStream = GlobalData.sSocketForServer.GetStream();
            outputText.text = "Connected to " + GlobalData.sServerIPAddr + "::" + 8888 + "\n";
            sStreamReader = new System.IO.StreamReader(sNetworkStream);
            sStreamWriter = new System.IO.StreamWriter(sNetworkStream);

            Thread readerThread = new Thread(ServerListener);
            readerThread.Start();
        }

        mClientInputField.text = "";
        mClientInputField.placeholder.GetComponent<Text>().text = "";
    }// Awake

    // Main thread to listen to the server for responses
    static void ServerListener()
    {
        String outputString = "";
        
        try
        {
            while (true)
            {
                outputString = sStreamReader.ReadLine();

                // Unity thread can also access this, it needs to be protected
                lock (sQueueLock)
                {
                    sServerResponses.Enqueue(outputString + "\n");
                    outputString = "";
                }
            }
        }
        catch
        {
            CloseNetworkResources();
        }
    }// ServerReader

    // Process commands inputted by the player.
    public void ClientInputEntered()
    {
        // Make sure the inputfield is selected after commands are entered so the player can input
        // another command immediately afterwords without having to select it again manually.
        mClientInputField.Select();
        mClientInputField.ActivateInputField();
        
        try
        {
            // Clear out the last element in the input history if we are about to go over its capacity
            if (mInputHistory.Count >= INPUTHISTORY_BUFFER_CAPACITY)
                mInputHistory.RemoveAt(mInputHistory.Count - 1);

            mInputHistory.Insert(0, mClientInputField.text);
            mInputHistoryIndex = 0;

            // Send the command to the server
            sStreamWriter.WriteLine(mClientInputField.text);
            sStreamWriter.Flush();

            // Reset the inputfield after a command is entered
            mClientInputField.text = ""; 
        }
        catch
        {
            lock (sQueueLock)
            {
                SafeQuit();
            }
        }
    }// clientInputEntered

    // Mainly used to empty the server responses and update the UI. Also process other things like keypresses.
    public void Update()
    {
        ProcessInputHistory();

        try
        {   
            ProcessServerResponses();
        }
        catch
        {
            SafeQuit();
        }
    }// Update

    // Cycles through a history of commands executed by the player as a shortcut using the up and down arrow keys
    private void ProcessInputHistory()
    {
        // Only traverse the buffer in the forward direction when the down arrow key is pressed and
        // we are and will be within the bounds of the buffer.
        if (Input.GetKeyDown(KeyCode.UpArrow) &&
            mInputHistoryIndex <= INPUTHISTORY_BUFFER_CAPACITY - 1 && 
            mInputHistory.Count > mInputHistoryIndex)
        {   
            mClientInputField.text = mInputHistory[mInputHistoryIndex++];
        }

        // Only traverse the buffer in the backwards direction when the up arrow key is pressed and
        // we are and will be within the bounds of the buffer.
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (mInputHistoryIndex > 0 && mInputHistory.Count - mInputHistoryIndex >= 0)
                mClientInputField.text = mInputHistory[--mInputHistoryIndex];
            else if (mInputHistoryIndex <= 0)
            {
                // If they press down and are at the start of their command history, just blank the inputfield.
                mClientInputField.text = "";
                mInputHistoryIndex = 0;
            }

            mClientInputField.caretPosition = mClientInputField.text.Length;
        }
    }// processmInputHistory

    // Drain server responses from the queue and update the UI
    private void ProcessServerResponses()
    {
        String outputString = "";
        Text outputText = mScrollOutputGrid.GetComponent<Text>();

        // Queue can be accessed by the server at anytime, it needs to be protected
        lock (sQueueLock)
        {
            while (sServerResponses.Count > 0)
                outputString += sServerResponses.Dequeue();

            if (!String.IsNullOrEmpty(outputString))
            {
                int outputTextLen = outputText.text.Length;

                // The scrolling textbox can only hold 65000 verticies, this amounts to about 15000 or so characters.
                // Just to be on the safe side, when we start reaching this number, just cut the buffer in half
                // so we still have some but recycle the older ones we don't need.
                if ((outputTextLen + outputString.Length) > OUTPUTGRID_THRESHOLD)
                    outputText.text = outputString + "\n" + outputText.text.Substring(outputTextLen / 2);

                // Display server responses on the UI
                outputText.text += outputString + "\n";
                outputText.text += "----------------------------------------------------------------\n";
            }
        }
    }// ProcessServerResponses

    public void OnApplicationQuit()
    {
        SafeQuit();
    }// OnApplicationQuit

    // Safely close all networking elements
    public static void CloseNetworkResources()
    {
        lock (sNetworkLock)
        {
            if (sStreamReader != null)
                sStreamReader.Close();
            if (sStreamWriter != null)
                sStreamWriter.Close();
            if (sNetworkStream != null)
                sNetworkStream.Close();
            if (GlobalData.sSocketForServer != null)
                GlobalData.sSocketForServer.Close();
        }
    }// CloseNetworkResources

    // Safely leave unity play mode and the application itself
    public void SafeQuit()
    {
        // Don't forget to close the connections
        CloseNetworkResources();

        // If we are in play mode, exit, if we are in an actual build, exit the application.
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }// SafeQuit

}// class ClientController
