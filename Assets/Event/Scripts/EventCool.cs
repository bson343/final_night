using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EventCool : MonoBehaviour
{
    public Button button;
   // public GameObject bt;
    
    
    
    

    // Start is called before the first frame update
    void Start()
    {
        //bt.SetActive(false);
        button.interactable = false;
        Debug.Log("1");
        Invoke("cool", 5);
        Debug.Log("2");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void cool()
    {
        //bt.SetActive(true);
        button.interactable = true;
        Debug.Log("fdfdf");
    }
}
