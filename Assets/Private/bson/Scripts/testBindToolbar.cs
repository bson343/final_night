using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using TMPro;

public class testBindToolbar : MonoBehaviour
{
    private const string _nameGameBar = "GameBar";
    private const string _hp = "HP";
    private const string _sp = "SP";

    private GameObject _hud;
    private Slider _sliderHp;
    private Slider _sliderSp;
    private TMP_Text _textHP;
    private TMP_Text _textSP;
    
    private GameObject toolbar;

    private bool isbindGamebar = false;

    public bool IsbindGamebar
    {
        get => isbindGamebar;
    }

    void Start()
    {
        //NightSceneManager.Instance.UnloadScene();
        toolbar = GameObject.Find(_nameGameBar);
        Debug.Log(toolbar.name);

        Slider[] components = toolbar.GetComponentsInChildren<Slider>();
        
        Debug.Log($"components : {components.Length}");
        
        foreach (Slider t in components)
        {
            switch (t.name)
            {
                case _hp:
                    _sliderHp = t;
                    _textHP = t.GetComponentInChildren<TMP_Text>();
                    Assert.IsNotNull(_textHP);
                    break;
                case _sp:
                    _sliderSp = t;
                    _textSP = t.GetComponentInChildren<TMP_Text>();
                    Assert.IsNotNull(_textSP);
                    break;
                default:
                    Assert.IsTrue(true, "바인드 실패");
                    break;
            }
        }

        _sliderHp.interactable = false;
        _sliderHp.value = 0;
        _sliderSp.interactable = false;
        _sliderSp.value = 0;
        
        _textHP.text = "0";
        _textSP.text = "0";
    }

}
