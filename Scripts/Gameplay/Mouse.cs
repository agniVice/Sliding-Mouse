using DG.Tweening;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    [SerializeField] private GameObject _particlePrefab;

    [SerializeField] private Transform _leftPoint;
    [SerializeField] private Transform _rightPoint;
    [SerializeField] private float _speed = 5f;

    private Rigidbody2D _rigidBody;
    private SpriteRenderer _spriteRenderer;

    private bool _isMovingRight = false;

    private void OnEnable()
    {
        PlayerInput.Instance.PlayerMouseDown += OnPlayerMouseDown;
        GameState.Instance.ScoreAdded += ChangeMySpeed;
    }
    private void OnDisable()
    {
        PlayerInput.Instance.PlayerMouseDown -= OnPlayerMouseDown;
        GameState.Instance.ScoreAdded -= ChangeMySpeed;
    }
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        ChangeMySpeed();
    }
    private void FixedUpdate()
    {
        if (GameState.Instance.CurrentState != GameState.State.InGame)
        { 
            _rigidBody.velocity = Vector3.zero;
            return;
        }

        if (_isMovingRight)
        {
            MoveTowards(_rightPoint.position);
            if (Vector2.Distance(transform.position, _rightPoint.position) < 0.1f)
                ChangeDirection();
        }
        else
        {
            MoveTowards(_leftPoint.position);
            if (Vector2.Distance(transform.position, _leftPoint.position) < 0.1f)
                ChangeDirection();
        }
    }
    private void MoveTowards(Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        _rigidBody.velocity = direction * _speed;
    }
    private void ChangeDirection()
    {
        _isMovingRight = !_isMovingRight;
        _spriteRenderer.flipX = !_spriteRenderer.flipX;

        AudioVibrationManager.Instance.PlaySound(AudioVibrationManager.Instance.Swap, 1f);
    }
    private void OnPlayerMouseDown()
    {
        ChangeDirection();
    }
    private void SpawnParticle()
    {
        var particle = Instantiate(_particlePrefab).GetComponent<ParticleSystem>();

        particle.transform.position = new Vector2(transform.position.x, transform.position.y + 0.2f);
        particle.Play();

        Destroy(particle.gameObject, 2f);
    }
    private void ChangeMySpeed()
    {
        if (PlayerScore.Instance.Score > 50)
            _speed = Random.Range(2.5f, 3f);
        if (PlayerScore.Instance.Score > 30)
            _speed = Random.Range(2.2f, 2.5f);
        if (PlayerScore.Instance.Score > 20)
            _speed = Random.Range(2.1f, 2.2f);
        if (PlayerScore.Instance.Score > 10)
            _speed = Random.Range(1.7f, 1.9f);
        if (PlayerScore.Instance.Score < 5)
            _speed = Random.Range(1.5f, 1.7f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Object>() != null)
        {
            if (collision.gameObject.GetComponent<Object>().GetObjectType() == ObjectType.Cheese)
            {
                AudioVibrationManager.Instance.PlaySound(AudioVibrationManager.Instance.Eat, 1f);
                AudioVibrationManager.Instance.PlaySound(AudioVibrationManager.Instance.ScoreAdd, 1f);
                PlayerScore.Instance.AddScore();

                Camera.main.DOShakePosition(0.1f, 0.1f, fadeOut: true).SetUpdate(true).SetLink(Camera.main.gameObject);
                Camera.main.DOShakeRotation(0.1f, 0.1f, fadeOut: true).SetUpdate(true).SetLink(Camera.main.gameObject);

                collision.gameObject.GetComponent<Object>().DestroyMe();
            }
            else
            {
                SpawnParticle();
                
                AudioVibrationManager.Instance.PlaySound(AudioVibrationManager.Instance.Win, 1f);
                AudioVibrationManager.Instance.PlaySound(AudioVibrationManager.Instance.Burst, 1f);
                GameState.Instance.FinishGame();
            }
        }
    }
}