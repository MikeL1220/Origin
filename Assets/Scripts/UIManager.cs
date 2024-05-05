using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private Player _player;

    [SerializeField]
    private int _score;
    [SerializeField]
    private int _scoreIncrease;


    [SerializeField] 
    private TMP_Text _scoreText;

    [SerializeField]
    private Sprite[] _livesDisplay;
    [SerializeField]
    private Sprite _currentLife;

    [SerializeField]
    private TMP_Text _gameOverText;
    [SerializeField]
    private GameObject _gameOverDisplay;
    [SerializeField]
    private bool _gameIsOver; 


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _scoreText.text = "Score: " + _score;

        _currentLife = GameObject.Find("Life_Display").GetComponent<SpriteRenderer>().sprite = _livesDisplay[0];
        _gameOverDisplay.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        _scoreText.text = "Score: " + _score;
        RestartGame();

    }
    public void UpdateScore()
    {
         _score += _scoreIncrease;
        
               
    }
    public void UpdateLifeDisplay(int currentLife)
    {
        
        switch (currentLife)
        {
            case 0:
                 _currentLife = GameObject.Find("Life_Display").GetComponent<SpriteRenderer>().sprite = _livesDisplay[3];
                break;
            case 1:
                _currentLife = GameObject.Find("Life_Display").GetComponent<SpriteRenderer>().sprite = _livesDisplay[2]; 
                break;
            case 2:
                _currentLife = GameObject.Find("Life_Display").GetComponent<SpriteRenderer>().sprite = _livesDisplay[1];
                break;
            case 3:
                _currentLife = GameObject.Find("Life_Display").GetComponent<SpriteRenderer>().sprite = _livesDisplay[0];
                break;
        }
    }

    public IEnumerator GameOverFlicker()
    {
       while(true)
        {
            _gameIsOver = true;
            _gameOverDisplay.SetActive(true);
            yield return new WaitForSeconds(1);
            _gameOverDisplay.SetActive(false);
            yield return new WaitForSeconds(1);

        }
        
       


    }
    private void RestartGame()
    {  
        if(_gameIsOver == true && Input.GetKeyDown(KeyCode.R))
        {
           
                SceneManager.LoadScene(1);
                _gameIsOver = false;
           
        }
          
    }
}
