using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Linq;


public class InventoryHandler : MonoBehaviour
{

    public static InventoryHandler instance;

    [Header("Main")]

    [SerializeField] private Transform _content;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private GameObject _coinIcon;
    [SerializeField] private BoolReference _isShopOpened;
    [SerializeField] private FloatReference _playerCoins;
    [SerializeField] private TMP_Text _coinsText;
    [SerializeField] private TMP_Text _sellButtonText;
    [SerializeField] private Button _sellButton;
    [SerializeField] private ToggleGroup _toggleGroup;
    [SerializeField] private ShopHandler _shopItems;
    [SerializeField] private List<Item> _items = new List<Item>();


    [Space(20)]
    [Header("Clothing Interactions Vars")]

    [SerializeField] private SpriteRenderer _playerClothes;
    [SerializeField] private Sprite _nakedSprite;
    [SerializeField] private Sprite _defaultClothes;

    public UnityEvent _playerIsNaked;
    public UnityEvent _playerNotNakedAnymore;
    public UnityEvent _playerWearsBougthClothing;

    #region UNITY_METHODS

    private void Awake()
	{
        //Singleton pattern
		if(instance == null) instance = this;
		else { Destroy(this.gameObject); }

        UpdateItems();
    }

    #endregion

    #region PUBLIC_METHODS

    public void Add(Item item)
    {
        _items.Add(item);
        UpdateItems();
    }

    public void Remove(Item item)
    {
        _items.Remove(item);
        UpdateItems();

        if(_items.Count == 0)
		{
            //For a fun interaction, if the player were to sell the default clothing
            _playerClothes.sprite = _nakedSprite;

            // So we others listen to the event and we can change dialogue modularly
            _playerIsNaked.Invoke();
		}
    }

    public void Sell()
    {
        Toggle toggleSelected = _toggleGroup.ActiveToggles().FirstOrDefault();
        ItemInstance itemSelected = toggleSelected.GetComponent<ItemInstance>();
        
        Remove(itemSelected._item);
        _shopItems.Add(itemSelected._item);
        _playerCoins.value += itemSelected._item.cost;
        _coinsText.text = "X " + _playerCoins.value.ToString();

        // If the player sells his current clothing call the naked event
        if (itemSelected._item.clothing == _playerClothes.sprite)
		{
            _playerClothes.sprite = _nakedSprite;
            _playerIsNaked.Invoke();
		}
    }

    public void SetUpInventory()
	{
        // Deselect all toggles
        _toggleGroup.SetAllTogglesOff();

        // Set the sell button when the inventory is opened
        if (_isShopOpened.toggle)
		{
            _sellButtonText.transform.parent.gameObject.SetActive(true);
		}
		else
		{
            _sellButtonText.transform.parent.gameObject.SetActive(false);
        }       
    }

    #endregion

    #region PRIVATE_METHODS

    private void UpdateItems()
    {
        // Clear all existing items on the hierarchy
        foreach (Transform item in _content)
        {
            Destroy(item.gameObject);
        }

        foreach (Item _it in _items)
        {
            GameObject obj = Instantiate(_itemPrefab, _content);

            Toggle objToggle = obj.GetComponent<Toggle>();
            objToggle.group = _toggleGroup;
            objToggle.onValueChanged.AddListener(delegate { UpdateSellButtonCost(objToggle); });
            obj.GetComponent<ItemInstance>()._item = _it;

            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            itemIcon.sprite = _it.icon;

            var WearButton = obj.transform.Find("WearButton").GetComponent<Button>();
            WearButton.onClick.AddListener(delegate { ChangePlayerClothes(_it.clothing); });

        }

        // Do only when shopping
        if (!_isShopOpened.toggle) return;

        _sellButtonText.text = "SELL";
        _sellButtonText.GetComponentInParent<Button>().interactable = false;
        _coinIcon.SetActive(false);
    }

    private void UpdateSellButtonCost(Toggle toggle)
    {
        if (toggle.isOn)
        {
            _sellButton.interactable = true;
            _coinIcon.SetActive(true);
            ItemInstance itemSelected = toggle.GetComponent<ItemInstance>();
            _sellButtonText.text = "SELL " + itemSelected._item.cost;
        }
        else
        {
            _sellButtonText.text = "SELL";
            _sellButton.interactable = false;
            _coinIcon.SetActive(false);
        }
    }

    private void ChangePlayerClothes(Sprite clothes)
	{
        // The shop owner will say something about the player's new clothing
        if (clothes != _defaultClothes)
            _playerWearsBougthClothing.Invoke();

        // The shop owner will express relief that the player is now wearing something
        if(_playerClothes.sprite == _nakedSprite)
            _playerNotNakedAnymore.Invoke();

        _playerClothes.sprite = clothes;
	}

    #endregion

}
