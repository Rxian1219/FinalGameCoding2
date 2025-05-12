using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Dialogue : MonoBehaviour
{

    public TextMeshProUGUI dialogueText;
    public float typingSpeed = 0.05f;
    public string[] lines = {
        "You love catching bugs!",
        "Today, while your mom is out, you¡¯ve snuck out on your own for another bug-hunting adventure.",
        "I wonder what kind of bugs you¡¯ll find this time!",

        "Use WASD to move around.",
        "Hold Shift to run.",
        "Press C to toggle between walking and crouching.",
        "And press E to interact with anything you can¡ªlike catching bugs!",

        "Ants hide under rocks.",
        "Butterflies will fly away if you don¡¯t crouch and sneak up on them.",
        "Grasshoppers hides in the bushes and they run fast...you¡¯ll have to chase them down!",
        "There are all kinds of bugs waiting to be discovered, but you¡¯ll need some skill.",
        "It won¡¯t be easy!",
        
        "Press Tab to open your bug list and see how many you¡¯ve caught so far.",
        "Now then¡­ go explore, and happy bug hunting!"
    };


    private int index = 0;
    private bool isTyping = false;

// Start is called before the first frame update
void Start()
    {
        StartCoroutine(TypeLine());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = lines[index];
                isTyping = false;
            }
            else
            {
                index++;
                if (index < lines.Length)
                {
                    StartCoroutine(TypeLine());
                }
                else
                {
                    SceneManager.LoadScene("1"); 
                }
            }
        }
    }


    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char c in lines[index])
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

}
