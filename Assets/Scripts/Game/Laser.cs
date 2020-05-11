using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;

    [SerializeField]
    private int _laserID;

    // Update is called once per frame
    void Update()
    {
        switch (_laserID)
        {
            case 0:
                MoveUp();
                break;
            case 1:
                MoveDown();
                break;
        }
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y >= 8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void AssignEnemyLaser(int id)
    {
        _laserID = id;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_laserID == 1 && other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            player.Damage(1);
        }
    }

    public int GetLaser()
    {
        return _laserID;
    }
}
