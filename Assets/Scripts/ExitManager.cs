using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitManager : MonoBehaviour
{
    [SerializeField] private GameObject exitPanel;

    // Start is called before the first frame update
    void Start()
    { 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            exitPanel.SetActive(true);

    }

    public void Yes()
    {
        Application.Quit();
    }

    public void No()
    {
        exitPanel.SetActive(false);
    }
}
