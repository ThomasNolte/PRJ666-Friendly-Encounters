using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour {

    [SerializeField]
    private string _cardName; //It will be used to check if two cards match

    private bool _isUpsideDown = false; //To know if card was flipped

    private Sprite _backSideCardSprite; //Sprite that represents back of the card

    [SerializeField]
    private Sprite _frontSideCardSprite; //Sprite that represents front of the card

    private SpriteRenderer _spriteRenderer;
    private MatchingCardManager _gameManager;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _gameManager = FindObjectOfType<MatchingCardManager>(); //Finds GameManager

        _backSideCardSprite = _spriteRenderer.sprite;
    }
    public string cardName
    {
        get
        {
            return _cardName;
        }
        set
        {
            _cardName = value;
        }
    }
    private void OnMouseDown() //This function is called each time player clicks on GameObject
    {
        if (!MyGameManager.pause)
        {
            if (!_isUpsideDown && _gameManager.canFlip)
            {
                _gameManager.AddCard(gameObject); //Adds this card to it
                ChangeSide();
            }
        }
    }
    public void ChangeSide()
    {
        if (!_isUpsideDown)
        {
            _spriteRenderer.sprite = _frontSideCardSprite;
            _isUpsideDown = true;
        }
        else
        {
            _spriteRenderer.sprite = _backSideCardSprite;
            _isUpsideDown = false;
        }
    }
}
