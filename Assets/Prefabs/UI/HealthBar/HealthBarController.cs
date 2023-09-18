using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    
    [SerializeField] private GameObject _heartPrefab;
    [SerializeField] private int _startHealth;
    [SerializeField] private Side _side;


    private int _health;


    private void Awake()
    {
        _health = _startHealth;
        updateHealthBar();
    }
    
    
    public void addHealth(int health)
    {
        _health += health;
        updateHealthBar();
    }

    public void removeHealth(int health)
    {
        _health -= health;
        updateHealthBar();
    }

    public void setHealth(int health)
    {
        _health = health;
        updateHealthBar();
    }

    private void updateHealthBar() {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < _health; i++)
        {
            if (_side == Side.Left) {
                GameObject heart = Instantiate(_heartPrefab, transform);
                heart.transform.parent = transform;
                heart.transform.localPosition += new Vector3(i * heart.GetComponent<RectTransform>().rect.width + 5, 0, 0);
            }
            else if (_side == Side.Right) {
                GameObject heart = Instantiate(_heartPrefab, transform);
                heart.transform.parent = transform;
                heart.transform.localPosition += new Vector3(-i * heart.GetComponent<RectTransform>().rect.width - 5, 0, 0);
            }
            
        }
    }

}


public enum Side {
    Left,
    Right
}

