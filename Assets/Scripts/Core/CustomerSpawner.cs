using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private Customer _customerPrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _spawnInterval = 5f;
    [SerializeField] private int _maxCustomers = 10;

    private readonly List<GameObject> _spawnedCustomers = new();

    private float _spawnTimer;

    private void Update()
    {
        _spawnTimer += Time.deltaTime;

        if (_spawnTimer >= _spawnInterval && _spawnedCustomers.Count < _maxCustomers)
        {
            SpawnCustomer();
            _spawnTimer = 0f;
        }
    }

    //TODO: Use object pooling
    private void SpawnCustomer()
    {
        var customerObject = Instantiate(_customerPrefab, _spawnPoint.position, Quaternion.identity);

        var hungerStat = customerObject.StatCollection.GetStat<Hunger>();
        hungerStat.Value = Random.Range(hungerStat.MinValue, hungerStat.MaxValue);
        var desireForSaltyStat = customerObject.StatCollection.GetStat<DesireForSalty>();
        desireForSaltyStat.Value = Random.Range(desireForSaltyStat.MinValue, desireForSaltyStat.MaxValue);
        var desireForSweetStat = customerObject.StatCollection.GetStat<DesireForSweets>();
        desireForSweetStat.Value = Random.Range(desireForSweetStat.MinValue, desireForSweetStat.MaxValue);
        var desireForSourStat = customerObject.StatCollection.GetStat<DesireToCheckout>();
        desireForSourStat.Value = Random.Range(desireForSourStat.MinValue, desireForSourStat.MaxValue);

        customerObject.gameObject.SetActive(true);
    }
}