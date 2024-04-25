using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Introduce : MonoBehaviour
{
    public Text t;
    float timer;
    bool startTimer;
    string intro;
    private void Awake()
    {
        intro = t.text;
        t.text = null;
        startTimer = false;
    }
    public void ShowIntroduce()
    {
        startTimer = true;
        StartCoroutine(ShowText());
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopCoroutine(ShowText());
            FindObjectOfType<MainMenu>().StartGame();
            startTimer = false;
            timer = 0;
        }
        if (startTimer)
        {
            
            timer += Time.deltaTime;
            if (timer > 25)
            {
                StopCoroutine(ShowText());
                FindObjectOfType<MainMenu>().StartGame();
                startTimer = false;
                timer = 0;
            }
        }
    }
    IEnumerator ShowText()
    {
        for (int i = 0; i < intro.Length; i++)
        {
            t.text = intro.Substring(0, i);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
