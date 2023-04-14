using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class ShopHandler : MonoBehaviour
{
    public List<Item> _items = new List<Item>();

    public Transform _shopContent;
    public GameObject _itemPrefab;

    [SerializeField] private ToggleGroup _toggleGroup;
    [SerializeField] private InventoryHandler _playerInventory;
    [SerializeField] private TMP_Text _coinsText;
	public FloatReference _playerCoins;

	#region UNITY_METHODS

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
        // For when the player sells to the shop
        _items.Add(item);
	}

    public void Remove(Item item)
	{
        // For when something is sold
        _items.Remove(item);
	}

    #endregion

    #region PRIVATE_METHODS

    private void UpdateItems()
	{
        // Clear all existing items on the hierarchy
		foreach (Transform item in _shopContent)
		{
            Destroy(item.gameObject);
		}

		foreach (Item _it in _items)
		{
            GameObject obj = Instantiate(_itemPrefab, _shopContent);
            obj.GetComponent<Toggle>().group = _toggleGroup;
            obj.GetComponent<ItemInstance>()._item = _it;

            var ItemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            var ItemCost = obj.transform.Find("ItemCost").GetComponent<TMP_Text>();

            ItemIcon.sprite = _it.icon;
            ItemCost.text = _it.cost.ToString();
		}
	}

    #endregion

    #region PUBLIC_VARIABLES


    public void Buy()
    {

        Toggle toggleSelected = _toggleGroup.ActiveToggles().FirstOrDefault();
        ItemInstance itemSelected = toggleSelected.GetComponent<ItemInstance>();


        if (_playerCoins.value >= itemSelected._item.cost)
        {
            Remove(itemSelected._item);
            _playerInventory.Add(itemSelected._item);
            _playerCoins.value -= itemSelected._item.cost;
            _coinsText.text = "X "+_playerCoins.value.ToString();


            UpdateItems();
            _playerInventory.UpdateItems();
        }
    }

    #endregion





}
