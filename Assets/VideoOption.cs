using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoOption : MonoBehaviour
{
    public Dropdown resolutionDropdown;
   
    List<Resolution>resolutions = new List<Resolution>();
    // Start is called before the first frame update
    void Start()
    {
        InitUI();
    }

    // Update is called once per frame
    void InitUI(){
        resolutions.AddRange(Screen.resolutions);
        resolutionDropdown.options.Clear();

        foreach(Resolution item in resolutions){
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = item.width + "x" + item.height + " " + item.refreshRate + "hz";
            resolutionDropdown.options.Add(option);
        }
        resolutionDropdown.RefreshShownValue();
    }
    void Update()
    {
        
    }
}
