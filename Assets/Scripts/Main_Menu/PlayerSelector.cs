using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelector : MonoBehaviour
{
    public GameObject[] players;
    public Transform selectionFrame;

    private int currentIndex = 0;
    private Animator currentAnimator;

    void Start()
    {
        UpdateSelection();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            currentIndex = (currentIndex + 1) % players.Length;
            UpdateSelection();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            currentIndex--;
            if (currentIndex < 0)
                currentIndex = players.Length - 1;
            UpdateSelection();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerSelectionData.SelectedPlayerIndex = currentIndex;
            SceneManager.LoadScene(1);
        }
    }

    void UpdateSelection()
    {
        selectionFrame.position = players[currentIndex].transform.position;

        if (currentAnimator != null)
            currentAnimator.SetBool("Walking", false);

        currentAnimator = players[currentIndex].GetComponent<Animator>();
        if (currentAnimator != null)
            currentAnimator.SetBool("Walking", true);
    }
}