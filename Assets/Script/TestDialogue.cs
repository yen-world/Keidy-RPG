using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDialogue : MonoBehaviour
{
    [SerializeField]
    public Dialogue dialogue;

    DialogueManager theDM;
    // Start is called before the first frame update
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            theDM.ShowDialogue(dialogue);
        }
    }
}
