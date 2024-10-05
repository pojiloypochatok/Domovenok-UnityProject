using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class buttonActivated : MonoBehaviour
{

    public Button buttonOpen;
    public Button buttonClose;
    public GameObject collectionContainer;

    [SerializeField]
    private GameObject iventoryView;

    [SerializeField]
    private Button buttonCloseInventory;

    void Start()
    {
        collectionContainer.SetActive(false);
        buttonClose.gameObject.SetActive(false);
    }

    public void OpenCollection() {
        if (collectionContainer.activeSelf) {
            CloseCollection();
        } else {
            if (iventoryView.activeSelf){
                iventoryView.SetActive(false);
                buttonCloseInventory.gameObject.SetActive(false);
                
                collectionContainer.SetActive(true);
                buttonClose.gameObject.SetActive(true);
            } else {
                collectionContainer.SetActive(true);
                buttonClose.gameObject.SetActive(true);
            }
        }
    }

    public void CloseCollection() {
        collectionContainer.SetActive(false);
        buttonClose.gameObject.SetActive(false);
        
    }
}
