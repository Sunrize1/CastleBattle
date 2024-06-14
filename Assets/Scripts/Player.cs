using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Player : MonoBehaviour
{
    public List<PlayerUnit> playerUnits = new List<PlayerUnit>();
    public GameObject playerUnitPrefab;
    public Transform unitTarget;
    public TextMeshProUGUI moneyTextUI;
    public int money = 0;

   private void Start()
    {
        // ������ �������� ��� ���������� ������ ������ 5 ������
        StartCoroutine(AddUnitEveryFiveSeconds());
    }

    private void Update()
    {
        moneyTextUI.text = money.ToString();  
    }

    IEnumerator AddUnitEveryFiveSeconds()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            AddNewUnit();
        }
    }

    void AddNewUnit()
    {
        // �������� ������ ����� � ���������� ��� � ������ � �����
        GameObject newUnitObject = Instantiate(playerUnitPrefab, GetSpawnPosition(), Quaternion.identity);
        PlayerUnit newUnit = newUnitObject.GetComponent<PlayerUnit>();
        newUnit.SetMainTarget(unitTarget);
        newUnit.SetPlayer(this);
        playerUnits.Add(newUnit);
    }

    Vector3 GetSpawnPosition()
    {
        // ���������� ������� ����� � �������� Player
        float spawnRadius = 2f; // ������ ������ ������ ������� Player
        Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
        randomDirection.y = 0; // ��������� �� ��� �� ������
        return transform.position + randomDirection;
    }

    public void AddMoney(int amount)
    {
        money += amount;
        Debug.Log("Money added. Total money: " + money);
    }
}