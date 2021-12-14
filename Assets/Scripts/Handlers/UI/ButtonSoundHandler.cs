using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundHandler : MonoBehaviour
{
    public GameObject buttonSoundObj;

    private void Start()
    {
        PoolManager.CreatePool<ButtonSound>(buttonSoundObj, null, 3);

        Cex();
    }

    private void Cex()
    {
        Button[] buttons = GetComponentsInChildren<Button>();

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClick.AddListener(() => 
            {
                PoolManager.GetItem<ButtonSound>();
            });
        }
    }
}
