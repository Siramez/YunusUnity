using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimonSaysGame : MonoBehaviour
{
    public List<string> sequence;
    private int sequenceIndex;
    private bool playerTurn;
    private bool gameOver;

    public GameObject redCube;
    public GameObject blueCube;
    public GameObject greenCube;

    private Color originalColor;

    // Start is called before the first frame update
    void Start()
    {
        sequence = new List<string>();
        sequenceIndex = 0;
        playerTurn = false;
        gameOver = false;

        // Start the game with one color in the sequence
        AddRandomColorToSequence();
        StartCoroutine(PlaySequence());
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            if (playerTurn)
            {
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    CheckInput("RightArrow");
                    Debug.Log("Red button pressed.");
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    CheckInput("LeftArrow");
                    Debug.Log("Blue button pressed.");
                }
                else if (Input.GetKeyDown(KeyCode.G))
                {
                    CheckInput("G");
                    Debug.Log("Green button pressed.");
                }
            }
        }
    }

    void HighlightCube(string color)
    {
        redCube.GetComponent<Renderer>().material.color = Color.red;
        blueCube.GetComponent<Renderer>().material.color = Color.blue;
        greenCube.GetComponent<Renderer>().material.color = Color.green;

        switch (color)
        {
            case "RightArrow":
                StartCoroutine(ChangeColorTemporarily(redCube, Color.yellow, Color.red));
                break;
            case "LeftArrow":
                StartCoroutine(ChangeColorTemporarily(blueCube, Color.yellow, Color.blue));
                break;
            case "G":
                StartCoroutine(ChangeColorTemporarily(greenCube, Color.yellow, Color.green));
                break;
        }
    }

    IEnumerator ChangeColorTemporarily(GameObject cube, Color highlightColor, Color finalColor)
    {
        originalColor = cube.GetComponent<Renderer>().material.color;
        cube.GetComponent<Renderer>().material.color = highlightColor;

        yield return new WaitForSeconds(0.5f);

        cube.GetComponent<Renderer>().material.color = finalColor;
    }

    void AddRandomColorToSequence()
    {
        int randomIndex = Random.Range(0, 3);
        string randomColor = "";

        switch (randomIndex)
        {
            case 0:
                randomColor = "RightArrow";
                break;
            case 1:
                randomColor = "LeftArrow";
                break;
            case 2:
                randomColor = "G";
                break;
        }

        sequence.Add(randomColor);
    }

    IEnumerator PlaySequence()
    {
        while (!gameOver)
        {
            playerTurn = false;
            sequenceIndex = 0;

            foreach (string color in sequence)
            {
                HighlightCube(color);
                yield return new WaitForSeconds(1.0f);
                redCube.GetComponent<Renderer>().material.color = Color.red;
                blueCube.GetComponent<Renderer>().material.color = Color.blue;
                greenCube.GetComponent<Renderer>().material.color = Color.green;
                yield return new WaitForSeconds(0.5f);
            }

            playerTurn = true;
            yield return new WaitUntil(() => !playerTurn); // Wait for player's turn to complete

            // Increase the sequence length by adding one more color
            AddRandomColorToSequence();
        }
    }

    void CheckInput(string inputColor)
    {
        if (sequence[sequenceIndex] == inputColor)
        {
            sequenceIndex++;

            if (sequenceIndex == sequence.Count)
            {
                playerTurn = false;
                sequenceIndex = 0;
            }
        }
        else
        {
            Debug.Log("Game Over!");
            gameOver = true;
        }
    }
}

