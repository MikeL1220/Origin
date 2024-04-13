using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    private int _lifeCount;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _scoreText.text = "Score: " + _score;

        _currentLife = GameObject.Find("Life_Display").GetComponent<SpriteRenderer>().sprite = _livesDisplay[0];

    }

    // Update is called once per frame
    void Update()
    {
        _scoreText.text = "Score: " + _score;
        UpdateLifeDisplay();
    }
    public void UpdateScore()
    {
         _score += _scoreIncrease;
               
    }
    public void UpdateLifeDisplay()
    {
        switch (_lifeCount)
        {
            case 0:
                 _currentLife = GameObject.Find("Life_Display").GetComponent<SpriteRenderer>().sprite = _livesDisplay[3];
                break;
            case 1:
                _currentLife = GameObject.Find("Life_Display").GetComponent<SpriteRenderer>().sprite = _livesDisplay[2]; ;
                break;
            case 2:
                _currentLife = GameObject.Find("Life_Display").GetComponent<SpriteRenderer>().sprite = _livesDisplay[1];
                break;
            case 3:
                _currentLife = GameObject.Find("Life_Display").GetComponent<SpriteRenderer>().sprite = _livesDisplay[0];
                break;
        }
    }
    public void UpdateLifeCount()
    {
        _lifeCount--; 
    }
}
