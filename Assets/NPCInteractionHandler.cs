using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using DG.Tweening;
using TMPro;

public class NPCInteractionHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
	// Using some of the pointer interfaces, we'll handle interaction with the cursor as in stardew valley

	#region VARIABLES

	[Header("Main")]
	[SerializeField] private Sprite _defaultSprite;
	[SerializeField] private Sprite _spriteOnHover;
	[SerializeField] private GameObject _textBubble;
	[SerializeField] private GameObject[] _buttons;
	[SerializeField] private TMP_Text _npcDialogue;
	[TextArea]
	[SerializeField] private string _openingDialogue;
	[TextArea]
	[SerializeField] private string _closingDialogue;
	[SerializeField] private BoolReference _isInteracting;

	[Space(20)]
	[Header("Tweening Customization")]

	[SerializeField] private float _punch = .1f;
	[SerializeField] private float _duration = 1f;
	[SerializeField] private float _elasticity = 1f;
	[SerializeField] private float _bubbleDuration = 5f;
	[SerializeField] private int _vibrato = 5;

	private SpriteRenderer _renderer;

	#endregion

	#region UNITY_METHODS

	private void Awake()
	{
		_renderer = GetComponent<SpriteRenderer>();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		StartCoroutine(Interact());
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!_isInteracting)
			_renderer.sprite = _spriteOnHover;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		_renderer.sprite = _defaultSprite;
	}

	#endregion

	#region PRIVATE_METHODS
	private IEnumerator Interact()
	{
		// Only one coroutine will have effect, even if the player clicks multiple times
		if (!_isInteracting.toggle)
		{
			_isInteracting.toggle = true;
			_textBubble.SetActive(true);
			_renderer.sprite = _defaultSprite;
			_npcDialogue.text = _openingDialogue;

			// We scale it to vector.one quickly and then apply the tweening "PUNCH" so it creates a nice visual "bounce" effect
			_textBubble.transform.localScale = Vector3.zero;
			_textBubble.transform.DOScale(1f, .25f).SetEase(Ease.InOutSine);
			yield return new WaitForSeconds(.25f);
			_textBubble.transform.DOPunchScale(new Vector3(_punch, _punch, 0f), _duration, _vibrato, _elasticity);
			yield return new WaitForSeconds(_duration);
			foreach(GameObject button in _buttons)
			{
				button.SetActive(true);
			}
			yield return new WaitForSeconds(_bubbleDuration);
		}
	}

	private IEnumerator CloseDialogue()
	{
		foreach (GameObject button in _buttons)
		{
			button.SetActive(false);
		}
		_npcDialogue.text = _closingDialogue;

		yield return new WaitForSeconds(2f);

		// Scale to zero after bubble's duration and enable interaction for next coroutine
		_textBubble.transform.DOScale(0f, 1f).SetEase(Ease.InOutSine);
		yield return new WaitForSeconds(1f);
		_textBubble.SetActive(false);
		_isInteracting.toggle = false;
	}

	#endregion

	#region PUBLIC_METHODS

	public void EndInteraction()
	{
		StartCoroutine(CloseDialogue());
	}

	#endregion


}
