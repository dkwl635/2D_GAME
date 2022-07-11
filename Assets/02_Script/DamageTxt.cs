using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DamageTxt : MonoBehaviour
{
   public  TextMeshPro textMeshPro;


    private void OnEnable()
    {
        textMeshPro.fontSize = 1;
    }

    private void Update()
    {
        if (textMeshPro.fontSize.Equals(0))
            OffDamageTxt();
    }

    public void SetDamageTxt(int value, Vector3 pos)
    {
        transform.position = pos;
        textMeshPro.text = value.ToString();

        gameObject.SetActive(true);
    }

    void OffDamageTxt()
    {
        gameObject.SetActive(false);
    }
}
