using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class InventoryHandler : MonoBehaviour
{
    public List<Item> _items = new List<Item>();
    public static InventoryHandler instance;

    public Transform _content;
    public GameObject _itemPrefab;
    [SerializeField] private ToggleGroup _toggleGroup;
    [SerializeField] private ShopHandler _shopItems;
    [SerializeField] private BoolReference _isShopOpened;
    [SerializeField] private GameObject _coinIcon;
    [SerializeField] private TMP_Text _coinsText;
    [SerializeField] private TMP_Text _sellButtonText;
    [SerializeField] private FloatReference _playerCoins;

	#region UNITY_METHODS

	private void Awake()
	{
        //Singleton pattern
		if(instance == null) instance = this;
		else { Destroy(this.gameObject); }
	}

	void Start()
    {
        UpdateItems();
    }


    void Update()
    {

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
    }

    public void UpdateSellButtonCost(Toggle toggle)
	{        
		if (toggle.isOn)
		{
            _sellButtonText.GetComponentInParent<Button>().interactable = true;
            _coinIcon.SetActive(true);
            ItemInstance itemSelected = toggle.GetComponent<ItemInstance>();
            _sellButtonText.text = "SELL " + itemSelected._item.cost;
		}
		else
		{
            _sellButtonText.text = "SELL";
            _sellButtonText.GetComponentInParent<Button>().interactable = false;
            _coinIcon.SetActive(false);
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
    }


    #endregion

    #region PRIVATE_METHODS

    public void UpdateItems()
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


        }

        _sellButtonText.text = "SELL";
        _sellButtonText.GetComponentInParent<Button>().interactable = false;
        _coinIcon.SetActive(false);
    }

    #endregion

}
