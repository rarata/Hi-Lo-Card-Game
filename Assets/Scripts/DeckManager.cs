using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{
    public GameObject[] cardDeck;
    public GameObject slotOne;
    public GameObject slotTwo;
    public GameObject hiButton;
    public GameObject loButton;
    public GameObject newGameButton;
    public GameObject drawButton;
    public Text scoreText;
    Stack<GameObject> drawDeck = new Stack<GameObject>();
    GameObject slotOneCard;
    GameObject slotTwoCard;
    int score;

    void NewShuffle() {
        //Shuffles the whole deck into drawDeck
        drawDeck = new Stack<GameObject>();
        int deckSize = cardDeck.Length;

        // fill initial deck with ordered cards
        for (int i = 0; i < deckSize; i++) {
            drawDeck.Push(cardDeck[i]);
        }

        // randomly shuffle the deck a random number of times
        int numShuffles = UnityEngine.Random.Range(10,20);
        int minSplitSize = (int)System.Math.Round(deckSize / 4.0);
        int maxSplitSize = (int)System.Math.Round(3.0 * deckSize / 4.0);
        Stack<GameObject> leftHand = new Stack<GameObject>();
        Stack<GameObject> rightHand = new Stack<GameObject>();
        for (int i = 0; i < numShuffles; i++) {
            int splitNum = UnityEngine.Random.Range(minSplitSize, maxSplitSize);
            for (int j = 0; j < splitNum; j++) {
                leftHand.Push(drawDeck.Pop());
            }
            for (int j = 0; j < (deckSize - splitNum); j++) {
                rightHand.Push(drawDeck.Pop());
            }
            for (int j = 0; j < deckSize; j++) {
                // determine which hand next puts a card on the drawDeck
                int whichDeck = UnityEngine.Random.Range(0,2);
                if (leftHand.Count == 0) {
                    whichDeck = 1;
                }
                else if (rightHand.Count == 0) {
                    whichDeck = 0;
                }
                
                // put one card from the selected hand onto the drawDeck
                if (whichDeck == 0) {
                    drawDeck.Push(leftHand.Pop());
                }
                else {
                    drawDeck.Push(rightHand.Pop());
                }
            }
        }
    }

    void DrawCardOne() {
        // clear board, if cards are there
        if (slotOneCard != null) {
            slotOneCard.SetActive(false);
        }
        if (slotTwoCard != null) {
            slotTwoCard.SetActive(false);
            slotTwoCard = null;
        }
        // move new card to slot one and set it active
        slotOneCard = drawDeck.Pop();
        slotOneCard.transform.position = slotOne.transform.position;
        slotOneCard.SetActive(true);
    }

    void DrawCardTwo() {
        if (slotOneCard != null && slotTwoCard == null) {
            slotTwoCard = drawDeck.Pop();
            slotTwoCard.transform.position = slotTwo.transform.position;
            slotTwoCard.SetActive(true);
        }
    }

    public void NewGameButtonPress() {
        NewShuffle();
        score = 0;
        newGameButton.SetActive(false);
        drawButton.SetActive(true);
        int deckSize = cardDeck.Length;
        for (int i = 0; i < deckSize; i++) {
            cardDeck[i].SetActive(false);
        }
    }

    public void DrawButtonPress() {
        DrawCardOne();
        drawButton.SetActive(false);
        hiButton.SetActive(true);
        loButton.SetActive(true);
    }

    public void HiButtonPress() {
        DrawCardTwo();
        //TODO: update score
        hiButton.SetActive(false);
        loButton.SetActive(false);
        if (drawDeck.Count == 0) {
            newGameButton.SetActive(true);
        } else {
            drawButton.SetActive(true);
        }
        if (slotTwoCard.GetComponent<Card>().numericalValue > slotOneCard.GetComponent<Card>().numericalValue) {
            score = score + 1;
        } else if (slotTwoCard.GetComponent<Card>().numericalValue < slotOneCard.GetComponent<Card>().numericalValue) {
            score = score - 1;
        }
        UpdateScore();
    }

    public void LoButtonPress() {
        DrawCardTwo();
        //TODO: update score
        hiButton.SetActive(false);
        loButton.SetActive(false);
        if (drawDeck.Count == 0) {
            newGameButton.SetActive(true);
        } else {
            drawButton.SetActive(true);
        }
        if (slotTwoCard.GetComponent<Card>().numericalValue < slotOneCard.GetComponent<Card>().numericalValue) {
            score = score + 1;
        } else if (slotTwoCard.GetComponent<Card>().numericalValue > slotOneCard.GetComponent<Card>().numericalValue) {
            score = score - 1;
        }
        UpdateScore();
    }

    void UpdateScore() {
        scoreText.text = "SCORE: " + score.ToString();
    }
}
