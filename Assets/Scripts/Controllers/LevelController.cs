using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{            
    [SerializeField] private float additionChance = 0.001f;

    [SerializeField] private PlayerController player;
    [SerializeField] private GameObject startObject;

    [SerializeField] private ObjectPooler planePooler;
    [SerializeField] private float planeLength;
    
    [SerializeField] private ObjectPooler obstaclePooler;


    [SerializeField] private float crystalAppearingProbability;
    [SerializeField] private GameObject cristalPrefab;

    private float _placeObstacleChance = 0.0f;

    private GameObject _prevPlane;
    private Vector3 _planeOffset;
    
    private List<GameObject> _placedPlanes;
    private List<GameObject> _placedObstacles;

    private void Awake()
    {        
        _planeOffset = new Vector3(0, 0, planeLength);
        _prevPlane = startObject;
        _placedPlanes = new List<GameObject>();
        _placedObstacles = new List<GameObject>();
    }    

    private void Start()
    {
        for (int i = 0; i < planePooler.amount; i++)
        {
            PlacePlane();
        }
    }

    private void Update()
    {       
        // If passed a certain plate, then make the first inactive.     
        if (player.transform.position.z >= _placedPlanes[2].transform.position.z)
        {                                   
            _placedPlanes[0].SetActive(false);              
            _placedPlanes.Remove(_placedPlanes[0]);   
                        
            PlacePlane();                         
        }        
    }

    private void PlacePlane()
    {                            
        GameObject plane = planePooler.GetPooledObject();
        if (plane != null)
        {                        
            // Arrange the panels sequentially.
            plane.transform.position = _prevPlane.transform.position + _planeOffset;
            _prevPlane = plane;
            _placedPlanes.Add(plane);
            
            // Increase the likelihood of placing an asteroid with each plate
            // if managed ti take asteroid.
            _placeObstacleChance += additionChance;
            GameObject obstacle = obstaclePooler.GetPooledObject();
            if (obstacle != null)
            {
                if (Random.value <= _placeObstacleChance)
                {
                    // Place on the current panel within the radius of the panel.
                    Vector2 posInCircle = Random.insideUnitCircle * planeLength / 2;
                    Vector3 pos = new Vector3(posInCircle.x + _prevPlane.transform.position.x, 1, posInCircle.y + _prevPlane.transform.position.z);
                    obstacle.transform.position = pos;
                    _placedObstacles.Add(obstacle);   
                }                
                else
                {
                    obstacle.SetActive(false);
                }                
            }   
            else
            {
                _placeObstacleChance -= additionChance;
                _placedObstacles[0].SetActive(false);  
                _placedObstacles.Remove(_placedObstacles[0]);
            }

            if (Random.value <= crystalAppearingProbability)
            {
                // Place on the current panel within the radius of the panel.
                Vector2 posInCircle = Random.insideUnitCircle * planeLength / 2;
                Vector3 pos = new Vector3(posInCircle.x + _prevPlane.transform.position.x, 1, posInCircle.y + _prevPlane.transform.position.z);
                var crist = Instantiate(cristalPrefab);
                crist.transform.position = pos;
                //_placedObstacles.Add(obstacle);
            }
            else
            {
                //obstacle.SetActive(false);
            }
        }                
    }    
}
