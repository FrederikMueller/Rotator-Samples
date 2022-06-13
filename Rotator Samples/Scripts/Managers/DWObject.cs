using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DWObject : MonoBehaviour
{
    [SerializeField] public List<Text> texts = new List<Text>();

    private void Awake() => DW.dWObject = this;
}