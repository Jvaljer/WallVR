using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gateway : MonoBehaviour {
    public static string[] arguments;

    public void Awake(){
#if UNITY_EDITOR
  #if UNITY_EDITOR_WIN
        Debug.LogError("Windows Editor -> VR Part by default");
        //must initialize all args
        SceneManager.LoadScene("VR");

  #elif UNITY_EDITOR_LIN
        Debug.LogError("Linux Editor -> Operator by default");
        //must initialize all args
        SceneManager.LoadScene("VR");

  #endif
#elif UNITY_STANDALONE
        arguments = System.Environment.GetCommandLineArgs();
  #if UNITY_STANDALONE_WIN 
        Debug.LogError("Windows Standalone -> must parse arguments, VR authorized");
        for(int i=0; i<arguments.Length; i++){
            if(arguments[i]=="-vr"){
                if(int.Parse(arguments[i+1])==1){
                    Debug.LogError("Loading VR Scene");
                    SceneManager.LoadScene("VR");
                } else {
                    Debug.LogError("Loading Wall Scene");
                    SceneManager.LoadScene("Wall");
                }
            }
        }
        SceneManager.LoadScene("Wall");
  #elif UNITY_STANDALONE_LIN
        Debug.LogError("Linux Standalone -> must parse argument, VR prohibited");
        SceneManager.LoadScene("Wall"):
  #endif
#else
        Debug.Log("ELSE -=> NOTHING");
        Debug.LogError("Couldn't Identify the executive source");
#endif
    }
}
