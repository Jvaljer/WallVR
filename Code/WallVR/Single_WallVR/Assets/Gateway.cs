using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gateway : MonoBehaviour {
    public static string[] arguments;
    //public StreamWriter writer;

    public void Awake(){
        //string path = "WallVR/Single_WallVR/Assets/Resources/PersonalLogs/my_log.txt";
        //writer = new StreamWriter(path, true);

#if UNITY_EDITOR
  #if UNITY_EDITOR_WIN
        Debug.LogError("Windows Editor -> VR Part by default");
        //must initialize all args
        SceneManager.LoadScene("VR");

  #elif UNITY_EDITOR_LINUX
        Debug.LogError("Linux Editor -> Operator by default");
        //must initialize all args
        SceneManager.LoadScene("Wall");

  #endif
#elif UNITY_STANDALONE
        arguments = System.Environment.GetCommandLineArgs();
  #if UNITY_STANDALONE_WIN 
        Debug.LogError("Windows Standalone -> must parse arguments, VR authorized");
        bool vr_scene = false;
        //WriteLog("Windows Standalone -> must parse arguments, VR authorized");
        for(int i=0; i<arguments.Length; i++){
            if(arguments[i]=="-vr"){
                if(int.Parse(arguments[i+1])==1){
                    vr_scene = true;
                } else {
                    vr_scene = false;
                }
            }
        }
        if(vr_scene){
            Debug.LogError("Loading VR Scene");
            //WriteLog("Loading VR Scene");
            SceneManager.Load("VR");
        } else {
            Debug.LogError("Loading Wall Scene");
            //WriteLog("Loading Wall Scene");
            SceneManager.Load("Wall");
        }
  #else 
        Debug.LogError("Linux Standalone -> must parse argument, VR prohibited");
        SceneManager.LoadScene("Wall"):
  #endif
#else
        Debug.Log("ELSE -=> NOTHING");
        Debug.LogError("Couldn't Identify the executive source");
#endif
    }

    /*public void WriteLog(string str){
        //writing some text
        writer.WriteLine(str);
        writer.Flush();
    }*/
}
