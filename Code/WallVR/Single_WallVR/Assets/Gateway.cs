using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gateway : MonoBehaviour {
    public void Awake(){
#if UNITY_EDITOR
  #if UNITY_EDITOR_WIN
        Debug.Log("Windows Editor -> VR Part by default");
        ArgsTemp.arguments = new string[1];
        //must initialize all args
        SceneManager.LoadScene("VR");

  #elif UNITY_EDITOR_LIN
        Debug.Log("Linux Editor -> Operator by default");
        ArgsTemp.args = new string[1];
        //must initialize all args
        SceneManager.LoadScene("VR");

  #endif
#elif UNITY_STANDALONE
        string[] args = System.Environment.GetCommandLineArgs();
        ArgsTemp.arguments = args;
  #if UNITY_STANDALONE_WIN 
        Debug.Log("Windows Standalone -> must parse arguments, VR authorized");
        for(int i=0; i<args.length; i++){
            if(args[i]=="-vr"){
                if(int.Parse(args[i+1])==1){
                    SceneManager.LoadScene("VR");
                } else {
                    SceneManager.LoadScene("Wall");
                }
            }
        }
        SceneManager.LoadScene("Wall");
  #elif UNITY_STANDALONE_LIN
        Debug.Log("Linux Standalone -> must parse argument, VR prohibited");
        SceneManager.LoadScene("Wall"):
  #endif
#else
        Debug.Log("ELSE -=> NOTHING");
        Debug.LogError("Couldn't Identify the executive source");
#endif
    }
}
