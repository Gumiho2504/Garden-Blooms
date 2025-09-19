using System.Runtime.InteropServices;
using UnityEngine;

public class WebHook : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void Hello();

    [DllImport("__Internal")]
    private static extern void HelloString(string str);


    [DllImport("__Internal")]
    private static extern void SendMessageToFlutter(string str);

    public void Exit()
    {
        Hello();
        SendMessageToFlutter("exit");

    }
}
