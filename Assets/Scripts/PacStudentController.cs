using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PacStudentController : MonoBehaviour
{
    public enum Direction { None, Up, Down, Left, Right }

    [SerializeField] private GameObject player;
    [SerializeField] private AudioSource walkAudio;
    [SerializeField] private Tilemap wallTilemap;
    [SerializeField] private Tilemap pelletTilemap;
    [SerializeField] private AudioSource palletEatAudio;
    [SerializeField] private ParticleSystem dustParticles;

    public Direction lastInput { get; private set; } = Direction.None;
    public Direction currentInput { get; private set; } = Direction.None;

    private Tweener tweener;
    private Animator playerAnimator;
    private Vector3Int currentCell;
    private Vector3Int pendingCell;
    private bool wasTweeningLastFrame = false;
    private Coroutine footstepCoroutine = null;

    [SerializeField] private float moveSpeed = 3f; // units/sec

    void Start()
    {
        if (player == null) player = gameObject;
        tweener = GetComponent<Tweener>();
        playerAnimator = player.GetComponent<Animator>();
        if (walkAudio == null) walkAudio = player.GetComponent<AudioSource>();
        if (palletEatAudio == null) palletEatAudio = player.GetComponent<AudioSource>();

        // start cell based on player's world position (tilemap cell coords)
        currentCell = wallTilemap.WorldToCell(player.transform.position);
        pendingCell = currentCell;
        player.transform.position = wallTilemap.GetCellCenterWorld(currentCell);

        if (playerAnimator != null) playerAnimator.SetTrigger("TrRight");
    }

    void Update()
    {
        // lastInput only on KeyDown (remember until changed)
        if (Input.GetKeyDown(KeyCode.W)) lastInput = Direction.Up;
        if (Input.GetKeyDown(KeyCode.S)) lastInput = Direction.Down;
        if (Input.GetKeyDown(KeyCode.A)) lastInput = Direction.Left;
        if (Input.GetKeyDown(KeyCode.D)) lastInput = Direction.Right;

        // currentInput is held key
        if (Input.GetKey(KeyCode.W)) currentInput = Direction.Up;
        else if (Input.GetKey(KeyCode.S)) currentInput = Direction.Down;
        else if (Input.GetKey(KeyCode.A)) currentInput = Direction.Left;
        else if (Input.GetKey(KeyCode.D)) currentInput = Direction.Right;
        else currentInput = Direction.None;

        bool isTweeningNow = (tweener != null) && tweener.isTweening();

        if (wasTweeningLastFrame && !isTweeningNow) FinishedMove();

        if (!isTweeningNow) TryStartMoveFromInputs();

        wasTweeningLastFrame = isTweeningNow;
    }

    private void TryStartMoveFromInputs()
    {
        // try lastInput first (remembered), if blocked try currentInput
        if (lastInput != Direction.None)
        {
            Vector3Int cand = currentCell + DirToCell(lastInput);
            if (!IsBlocked(cand))
            {
                StartMoveTo(cand, lastInput);
                return;
            }
        }

        if (currentInput != Direction.None)
        {
            Vector3Int cand = currentCell + DirToCell(currentInput);
            if (!IsBlocked(cand))
            {
                StartMoveTo(cand, currentInput);
                return;
            }
        }
    }

    private void StartMoveTo(Vector3Int targetCell, Direction dir)
    {
        Vector3 startWorld = player.transform.position;
        Vector3 endWorld = wallTilemap.GetCellCenterWorld(targetCell);

        float distance = Vector3.Distance(startWorld, endWorld);
        float duration = Mathf.Max(0.0001f, distance / Mathf.Max(0.0001f, moveSpeed));

        if (tweener != null)
            tweener.AddTween(player.transform, startWorld, endWorld, duration);
        else
            player.transform.position = endWorld;

        pendingCell = targetCell;

        // animation / dust / footsteps
        if (playerAnimator != null) PlayDirectionAnimation(dir);
        if (dustParticles != null && !dustParticles.isPlaying) dustParticles.Play();
        if (walkAudio != null)
        {
            if (footstepCoroutine != null) StopCoroutine(footstepCoroutine);
            footstepCoroutine = StartCoroutine(PlayFootsteps(duration));
        }

        // pellet check & eat (use tile coordinates)
        if (pelletTilemap != null)
        {
            TileBase pellet = pelletTilemap.GetTile(targetCell);
            if (pellet != null)
            {
                pelletTilemap.SetTile(targetCell, null);
                if (palletEatAudio != null) palletEatAudio.Play();
                // optional: instantiate particle at endWorld if you want visual pop
            }
        }
    }

    private void FinishedMove()
    {
        currentCell = pendingCell;

        if (dustParticles != null && dustParticles.isPlaying) dustParticles.Stop();
        if (footstepCoroutine != null) { StopCoroutine(footstepCoroutine); footstepCoroutine = null; }
        if (walkAudio != null && walkAudio.isPlaying) walkAudio.Stop();
    }

    private IEnumerator PlayFootsteps(float duration)
    {
        float elapsed = 0f;
        float interval = 0.47f;
        while (elapsed < duration)
        {
            if (walkAudio != null) walkAudio.Play();
            yield return new WaitForSeconds(interval);
            elapsed += interval;
        }
    }

    private Vector3Int DirToCell(Direction d)
    {
        switch (d)
        {
            case Direction.Up: return new Vector3Int(0, 1, 0);
            case Direction.Down: return new Vector3Int(0, -1, 0);
            case Direction.Left: return new Vector3Int(-1, 0, 0);
            case Direction.Right: return new Vector3Int(1, 0, 0);
            default: return Vector3Int.zero;
        }
    }

    private bool IsBlocked(Vector3Int cell)
    {
        if (wallTilemap == null) return false;
        return wallTilemap.GetTile(cell) != null;
    }

    private void PlayDirectionAnimation(Direction d)
    {
        switch (d)
        {
            case Direction.Up: playerAnimator.SetTrigger("TrUp"); break;
            case Direction.Down: playerAnimator.SetTrigger("TrDown"); break;
            case Direction.Left: playerAnimator.SetTrigger("TrLeft"); break;
            case Direction.Right: playerAnimator.SetTrigger("TrRight"); break;
        }
    }
}
