using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapEvent : MonoBehaviour
{
    // Start is called before the first frame update
        public static int RandomInt;

    void Update()
    {
        RandomInt = Random.Range(0,2);
        
    }
    
    public void OnClickEvent()
    {
        Debug.Log(RandomInt);
        if (RandomInt == 0)
        {
            SceneManager.LoadScene("EventPage");
        }
        else if (RandomInt == 1)
        {
            SceneManager.LoadScene("EventPage2");
        }
    }
}
