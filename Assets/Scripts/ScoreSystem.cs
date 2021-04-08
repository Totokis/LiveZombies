using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{

    [SerializeField] TMP_Text _scoreText;
    [SerializeField] TMP_Text _highScoreText;
    [SerializeField] TMP_Text _multiplierScoreText;
    [SerializeField] FloatingScoreText _floatingTextPrefab;
    [SerializeField] Canvas _floatingScoreCanvas;

    int _score;
    int _highScore;
    float _scoreMultiplierExpiration;
    int _killMultiplier;

    void Start()
    {
        Zombie.Died+= ZombieOnDied;
        _highScore = PlayerPrefs.GetInt("HighScore");
        _highScoreText.SetText("High Score: " + _highScore);

    }
    
    void OnDestroy()
    {
        Zombie.Died -= ZombieOnDied;
    }

    void ZombieOnDied(Zombie zombie)
    {
        UpdateKillMultiplier();

        _score += _killMultiplier;
        
        if (_score > _highScore)
        {
            _highScore = _score;
           
            PlayerPrefs.SetInt("HighScore",_highScore);
            _highScoreText.SetText("High Score: " + _highScore);

        }
        _scoreText.SetText(_score.ToString());

        var floatingText = Instantiate(
            _floatingTextPrefab,
            zombie.transform.position,
            _floatingScoreCanvas.transform.rotation,
            _floatingScoreCanvas.transform);
        
        floatingText.SetScoreValue(_killMultiplier);
    }

    void UpdateKillMultiplier()
    {
        if (Time.time < _scoreMultiplierExpiration)
        {
            _killMultiplier++;
        }
        else
        {
            _killMultiplier = 1;
        }

        _scoreMultiplierExpiration = Time.time + 1f;
        
        _multiplierScoreText.SetText("x " + _killMultiplier);
        
        if (_killMultiplier < 3)
            _multiplierScoreText.color = Color.white;
        else if(_killMultiplier < 10)
            _multiplierScoreText.color = Color.green;
        else if(_killMultiplier < 20)
            _multiplierScoreText.color = Color.yellow;
        else if(_killMultiplier < 30)
            _multiplierScoreText.color = Color.red;
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}