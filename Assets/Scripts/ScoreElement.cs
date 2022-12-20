using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreElement : MonoBehaviour
{

    public TMP_Text usernameText;
    public TMP_Text _numberText;
    public TMP_Text _scoreText;

    public void NewScoreElement (string _username, int _number, int _score)
    {
        usernameText.text = _username;
        _numberText.text = _number.ToString();
        _scoreText.text = _score.ToString();
    }

}
