using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{        
    [SerializeField] private int amountToPool = 30;       

    [SerializeField] private GameObject objectToPool;    
    
    private List<GameObject> _pooledObjects;        

    public int amount
    {
        get
        {
            return amountToPool;
        }
    }

    private void Awake()
    {
        _pooledObjects = new List<GameObject>();
        // Loop through list of pooled objects,deactivating them and adding them to the list         
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = (GameObject)Instantiate(objectToPool);
            obj.SetActive(false);
            _pooledObjects.Add(obj);
            obj.transform.SetParent(this.transform);
        }
    }
    
    public GameObject GetPooledObject()
    {        
        for (int i = 0; i < _pooledObjects.Count; i++)
        {
            // if the pooled objects is NOT active, return that object 
            if (!_pooledObjects[i].activeInHierarchy)
            {
                _pooledObjects[i].SetActive(true);                
                return _pooledObjects[i];
            }
        }                
        return null;
    }    
}
