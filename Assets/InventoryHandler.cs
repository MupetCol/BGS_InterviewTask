using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class InventoryHandler : MonoBehaviour
{
    public List<Item> _items = new List<Item>();

    public Transform _content;
    public GameObject _itemPrefab;

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
        _items.Add(item);
    }

    public void Remove(Item item)
    {
        _items.Remove(item);
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

            var ItemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            ItemIcon.sprite = _it.icon;
        }
    }

    #endregion

}
