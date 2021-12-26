using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGeneratorManagerScript : MonoBehaviour
{
    public int seed = 0;

    public Transform[] startLocations;

    private List<Transform> pieces = new List<Transform>();

    [Tooltip("Pieces that change gravity.")]
    public Transform[] upRotatorPieces;
    public Transform[] downRotatorPieces;
    private int sameRotatorCount = 0;
    [Tooltip("0 == Down, 1 == Up")]
    private int savedRotator;
    public Transform[] trackPieces;
    public Transform[] turnPieces;
    private int sameTurnCount = 0;
    private int savedTurn = 0;
    public Transform checkpointPiece;
    public Transform finishLinePiece;

    private int lastPieceId = -1;
    private Transform nextPieceSpawnLoc;

    private bool canUseRotator = true;
    private int piecesSinceRotator = 0;

    [Header("Level Gen Attributes")]
    [Range(0, 100)] public int totalPieces = 10;
    private int curPiece = 0;

    public int numOfCheckpoints = 3;
    private int[] checkpointLoc;
    public int totalRotators = 100;
    private int numOfRotators = 0;
    public int piecesBetweenRotators = 2;
    [Range(0, 100)] public int chanceForRotator = 20;

    public Material[] pieceMaterials;
    private int setMaterial = 0;

    private void Start()
    {
        if (seed == 0)
            seed = Random.Range(0, 99999);
        Random.InitState(seed);

        GenerateLevel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void GenerateLevel()
    {
        int _startLoc = Random.Range(0, startLocations.Length);
        nextPieceSpawnLoc = startLocations[_startLoc];

        //Accounting for the Respawn Point.
        totalPieces += numOfCheckpoints;
        FindCheckpointLocations();

        setMaterial = Random.Range(0, pieceMaterials.Length);

        for (int i = 0; i < totalPieces; i++)
        {
            AddPiece();
        }

        AddFinishLinePiece();

        Debug.Log("Rotators: " + numOfRotators + " / " + totalRotators);
    }

    void FindCheckpointLocations()
    {
        checkpointLoc = new int[numOfCheckpoints];
        int _dividedPieces = Mathf.FloorToInt(totalPieces / numOfCheckpoints);
        for (int i = 0; i < checkpointLoc.Length; i++)
        {
            if (i == 0)
                checkpointLoc[i] = 999999999;
            else
                checkpointLoc[i] = _dividedPieces * i;
        }
    }

    void AddPiece()
    {
        //Use a rotator piece.
        bool useRotator = false;
        if (canUseRotator && numOfRotators < totalRotators)
        {
            int _rand = Random.Range(0, 100);
            if (_rand < chanceForRotator)
            {
                useRotator = true;
            }
        }

        if (numOfRotators >= totalRotators)
            Debug.Log(curPiece);

        //Use a turn piece.
        int _nextPieceId = 0;
        Transform _pieceInstance;
        bool isCheckpoint = false;
        for (int i = 0; i < checkpointLoc.Length; i++)
        {
            if (curPiece == checkpointLoc[i])
            {
                isCheckpoint = true;
                break;
            }
        }
        if (isCheckpoint)
        {
            _pieceInstance = Instantiate(checkpointPiece, nextPieceSpawnLoc.position, nextPieceSpawnLoc.rotation);
            nextPieceSpawnLoc = _pieceInstance.Find("End");
            piecesSinceRotator++;
        }
        else if (Random.Range(0, 5) == 0)
        {
            if (sameTurnCount > 1)
            {
                _nextPieceId = 1 - savedTurn;
                sameTurnCount--;
                if (sameTurnCount < 0)
                {
                    sameTurnCount = 0;
                    savedTurn = -1;
                }
            }
            else
            {
                _nextPieceId = Random.Range(0, turnPieces.Length);
                if (_nextPieceId == lastPieceId)
                    _nextPieceId++;
                if (_nextPieceId >= turnPieces.Length)
                    _nextPieceId = 0;
                if (_nextPieceId == savedTurn)
                    sameTurnCount++;
                if (savedTurn == -1)
                    savedTurn = _nextPieceId;
            }
            _pieceInstance = Instantiate(turnPieces[_nextPieceId], nextPieceSpawnLoc.position, nextPieceSpawnLoc.rotation);
            piecesSinceRotator++;
        }
        else
        {
            //Use a track piece.
            if (useRotator)
            {
                if (sameRotatorCount > 1)
                {
                    if (savedRotator == 0)
                    {
                        //Place Up Rotator
                        _nextPieceId = Random.Range(0, upRotatorPieces.Length);
                        _pieceInstance = Instantiate(upRotatorPieces[_nextPieceId], nextPieceSpawnLoc.position, nextPieceSpawnLoc.rotation);
                    }
                    else
                    {
                        //Place Down Rotator
                        _nextPieceId = Random.Range(0, downRotatorPieces.Length);
                        _pieceInstance = Instantiate(downRotatorPieces[_nextPieceId], nextPieceSpawnLoc.position, nextPieceSpawnLoc.rotation);
                    }
                    sameRotatorCount--;
                }
                else
                {
                    int _rand = Random.Range(0, 2);
                    if (_rand == 0)
                    {
                        //Place Up Rotator
                        _nextPieceId = Random.Range(0, upRotatorPieces.Length);
                        _pieceInstance = Instantiate(upRotatorPieces[_nextPieceId], nextPieceSpawnLoc.position, nextPieceSpawnLoc.rotation);
                        if (sameRotatorCount == 0)
                            savedRotator = 0;
                        if (savedRotator == 0)
                            sameRotatorCount++;
                    }
                    else
                    {
                        //Place Down Rotator
                        _nextPieceId = Random.Range(0, downRotatorPieces.Length);
                        _pieceInstance = Instantiate(downRotatorPieces[_nextPieceId], nextPieceSpawnLoc.position, nextPieceSpawnLoc.rotation);
                        if (sameRotatorCount == 1)
                            savedRotator = 1;
                        if (savedRotator == 1)
                            sameRotatorCount++;
                    }
                }
                canUseRotator = false;
                piecesSinceRotator = 0;
                numOfRotators++;
            }
            else
            {
                _nextPieceId = Random.Range(0, trackPieces.Length);
                if (_nextPieceId == lastPieceId)
                    _nextPieceId++;
                if (_nextPieceId >= trackPieces.Length)
                    _nextPieceId = 0;
                _pieceInstance = Instantiate(trackPieces[_nextPieceId], nextPieceSpawnLoc.position, nextPieceSpawnLoc.rotation);
                piecesSinceRotator++;
            }

        }

        if (piecesSinceRotator >= piecesBetweenRotators && numOfRotators <= totalRotators)
            canUseRotator = true;

        Renderer[] _renderers = _pieceInstance.GetComponentsInChildren<Renderer>();
        foreach (var _r in _renderers)
        {
            if (_r.tag == "Piece")
                _r.material = pieceMaterials[setMaterial];
        }

        nextPieceSpawnLoc = _pieceInstance.Find("End");
        lastPieceId = _nextPieceId;
        pieces.Add(_pieceInstance);

        curPiece++;
    }

    void AddFinishLinePiece()
    {
        Transform _finishPiece = Instantiate(finishLinePiece, nextPieceSpawnLoc.position, nextPieceSpawnLoc.rotation);
        pieces.Add(_finishPiece);
    }
}