using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Unity.VisualScripting.FullSerializer;

public class GameData
{
    public int HP { get; set; }
    public int Gold { get; set; }
    public List<int> CardDeckIndex { get; set; }
    public List<int> HeroCardDeckIndex { get; set; }
    public string Map { get; set; }
}
