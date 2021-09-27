using NonsensicalKit;
using UnityEngine;
using UnityEngine.Networking;

namespace NonsensicalKit
{
    public class TestWebErrorHandle : IHandleWebError
    {
        public void OnConnectionError(UnityWebRequest unityWebRequest)
        {
            Debug.Log(unityWebRequest.downloadHandler.text);
        }

        public void OnDataProcessingError(UnityWebRequest unityWebRequest)
        {
            Debug.Log(unityWebRequest.downloadHandler.text);
        }

        public void OnUnknowError(UnityWebRequest unityWebRequest)
        {
            Debug.Log(unityWebRequest.downloadHandler.text);
        }
    }

}