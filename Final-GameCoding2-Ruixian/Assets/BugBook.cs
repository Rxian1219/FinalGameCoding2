using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BugBook : MonoBehaviour
{

    public GameObject panel;
    public TextMeshProUGUI displayText;

    private bool isOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isOpen = !isOpen;
            panel.SetActive(isOpen);

            if (isOpen)
            {
                UpdateUI();
            }
        }
    }

    void UpdateUI()
    {
        Dictionary<string, int> bugCounts = BugTracker.Instance.GetAllBugCounts();

        string result = "";
        foreach (var pair in bugCounts)
        {
            result += pair.Key + " x " + pair.Value + "\n";
        }

        displayText.text = result;
    }

}
