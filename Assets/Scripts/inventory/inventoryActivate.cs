using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inventoryActivate : MonoBehaviour
{
    public Button buttonOpen;
    public Button buttonClose;
    public GameObject inventoryView;

    void Start() {
        inventoryView.SetActive(false);
        buttonClose.gameObject.SetActive(false);
    }

    public void OpenInventory() {
        if (inventoryView.activeSelf) {
            CloseInventory();
        } else {
            inventoryView.SetActive(true);
            buttonClose.gameObject.SetActive(true); 
        }
    }

    public void CloseInventory() {
        inventoryView.SetActive(false);
        buttonClose.gameObject.SetActive(false);
    }
}