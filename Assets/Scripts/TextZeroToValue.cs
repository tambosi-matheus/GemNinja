using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextZeroToValue : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    private int finalValue;
    public int value 
    {
        get { return finalValue; }
        set 
        {
            finalValue = value; 
            StartCoroutine(SetText());
        }
    }
    
    private IEnumerator SetText()
    {
        int startValue = 0;
        while(startValue < finalValue)
        {
            startValue += (int)((finalValue - startValue) * 0.1f);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
