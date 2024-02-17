using DG.Tweening;
using System;
using UnityEngine;

public class Object : MonoBehaviour
{
    private ObjectType _currentType;

    private float _speed;

    private Rigidbody2D _rigidBody;
    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer;

    private GameObject _particlePrefab;

    private Vector2 _direction;
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Initialize(ObjectType type, float speed, Sprite sprite, GameObject particlePrefab)
    {
        _currentType = type;
        _speed = speed;
        _spriteRenderer.sprite = sprite;
        _particlePrefab = particlePrefab;

        _direction = (FindObjectOfType<Mouse>().transform.position - new Vector3(_rigidBody.position.x, _rigidBody.position.y, 0)).normalized;
    }
    private void FixedUpdate()
    {
        if (GameState.Instance.CurrentState != GameState.State.InGame)
        {
            _rigidBody.velocity = Vector3.zero;
            return;
        }

        _rigidBody.velocity = _direction * _speed * 10 * Time.fixedDeltaTime;
    }
    private void SpawnParticle()
    {
        var particle = Instantiate(_particlePrefab).GetComponent<ParticleSystem>();

        particle.transform.position = new Vector2(transform.position.x, transform.position.y + 0.2f);
        particle.Play();

        Destroy(particle.gameObject, 2f);
    }
    public void DestroyMe()
    {
        _collider.enabled = false;
        transform.DOScale(0, 0.2f).SetLink(gameObject);
        
        SpawnParticle();

        Destroy(gameObject, 0.3f);
    }
    public ObjectType GetObjectType()
    {
        return _currentType;
    }
}
