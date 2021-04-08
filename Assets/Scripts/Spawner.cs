using System.Collections;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;

public class Spawner : MonoBehaviour
{
    [SerializeField] float _spawnDelay = 12f;
    [SerializeField] Zombie[] _zombiePrefabs;
    [SerializeField] Transform _spawnPoint;
    
    float _nextSpwanTime;
    private int _spawnCount;

    void Update()
    {
        if (ReadyToSpawn())
            StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        float delay = _spawnDelay - _spawnCount;
        delay = Mathf.Max(1, delay);
        _nextSpwanTime = Time.time + _spawnDelay; 
        _spawnCount++;
        
        int randomIndex = UnityEngine.Random.Range(0,_zombiePrefabs.Length);
        var zombiePrefab = _zombiePrefabs[randomIndex];
        
        var zombie = Instantiate(zombiePrefab, _spawnPoint.transform.position,_spawnPoint.transform.rotation);
        GetComponent<Animator>().SetBool("Open",true);
        
        yield return new WaitForSeconds(1f);
        zombie.StartWalking();
        
        yield return new WaitForSeconds(3f);
        GetComponent<Animator>().SetBool("Open",false);
    }

    bool ReadyToSpawn()
    {
        return Time.time >= _nextSpwanTime;
    }
}
