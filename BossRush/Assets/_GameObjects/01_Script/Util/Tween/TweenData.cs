using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public enum TweenType
{
    move, rotate, scale, punch, shake, path, spriterenderercolor, spriterendererfade, materialcolor, materialfade
}

public enum TweenEvents
{
    enable = 0,
    disable = 1,
    custom = 2
}

[System.Serializable]
public class TweenData
{
    [Tooltip("Tween Name")]
    public string tweenName;

    [Tooltip("Type of tween that will play")]
    public TweenType tweenType;

    [Tooltip("Only works for - move, rotate, scale, spriterenderercolor, spriterendererfade")]
    public bool useTweenInitialValue;

    [SerializeField]
    public TDMove tdMove;

    [SerializeField]
    public TDRotate tdRotate;

    [SerializeField]
    public TDScale tdScale;

    [SerializeField]
    public TDPunch tdPunch;

    [SerializeField]
    public TDShake tdShake;

    [SerializeField]
    public TDPath tdPath;

    [SerializeField]
    public TDSpriteRendererColor tdSpriteRendererColor;

    [SerializeField]
    public TDSpriteRendererFade tdSpriteRendererFade;

    [SerializeField]
    public TDMaterialColor tdMaterialColor;

    [SerializeField]
    public TDMaterialFade tdMaterialFade;
    
    [Space]
    [Tooltip("Event on which this tween will play")]
    public TweenEvents tweenEvent;

    [Space]
    public UnityEvent OnStart;

    [Space]
    public UnityEvent OnComplete;
}

[System.Serializable]
public class TDMove
{
    [Tooltip("Tween space")]
    public bool isLocalSpace;
    [Tooltip("Tween starting value")]
    public Vector3 tweenStartValue;
    [Tooltip("Tween ending value")]
    public Vector3 tweenEndValue;
    [Tooltip("Tween duration")]
    public float tweenDuration;
    [Tooltip("Tween Dealy")]
    public float tweenDelay;
    [Tooltip("Tween snaps to ints")]
    public bool isSnapping;
    [Tooltip("Number of jumps")]
    public int numOfJumps;
    [Tooltip("Jump Power")]
    public float jumpPower;
    [Tooltip("Ease Type")]
    public Ease easeType;
}

[System.Serializable]
public class TDRotate
{
    [Tooltip("Tween space")]
    public bool isLocalSpace;
    [Tooltip("Tween starting value - degree")]
    public Vector3 tweenStartValue;
    [Tooltip("Tween ending value - degree")]
    public Vector3 tweenEndValue;
    [Tooltip("Tween duration")]
    public float tweenDuration;
    [Tooltip("Tween Dealy")]
    public float tweenDelay;
    [Tooltip("Rotation Modes")]
    public RotateMode rotateMode;
    [Tooltip("Ease Type")]
    public Ease easeType;
}

[System.Serializable]
public class TDScale
{
    [Tooltip("Tween starting value")]
    public Vector3 tweenStartValue;
    [Tooltip("Tween ending value")]
    public Vector3 tweenEndValue;
    [Tooltip("Tween duration")]
    public float tweenDuration;
    [Tooltip("Tween Dealy")]
    public float tweenDelay;
    [Tooltip("Ease Type")]
    public Ease easeType;
}

[System.Serializable]
public class TDPunch
{
    [Tooltip("0 = position, 1 = rotation, 2 = scale, all in local space")]
    public int type;
    [Tooltip("Punch direction and strength")]
    public Vector3 puch;
    [Tooltip("Tween duration")]
    public float tweenDuration;
    [Tooltip("Tween Dealy")]
    public float tweenDelay;
    [Tooltip("Punch vibrate amount")]
    public int vibrato;
    [Tooltip("0-1 amount is to go beyond points")]
    [Range(0.0f, 1.0f)]
    public float elasticity;
    [Tooltip("Tween snaps to ints")]
    public bool isSnapping;
    [Tooltip("Ease Type")]
    public Ease easeType;
}

[System.Serializable]
public class TDShake
{
    [Tooltip("0 = position, 1 = rotation, 2 = scale, all in local space")]
    public int type;
    [Tooltip("Shake direction and strength")]
    public Vector3 strength;
    [Tooltip("Tween duration")]
    public float tweenDuration;
    [Tooltip("Tween Dealy")]
    public float tweenDelay;
    [Tooltip("Punch vibrate amount")]
    public int vibrato;
    [Tooltip("Randomness in shake 0-180")]
    [Range(0.0f, 180.0f)]
    public float randomness;
    [Tooltip("Tween snaps to ints")]
    public bool isSnapping;
    [Tooltip("true - Fade out smoothly")]
    public bool fadeOut;
    [Tooltip("Type of randomness applied")]
    public ShakeRandomnessMode shakeRandomnessMode;
    [Tooltip("Ease Type")]
    public Ease easeType;
}

[System.Serializable]
public class TDPath
{
    [Tooltip("Tween space")]
    public bool isLocalSpace;
    public Vector3[] waypoints;
    [Tooltip("Tween duration")]
    public float tweenDuration;
    [Tooltip("Tween Dealy")]
    public float tweenDelay;
    [Tooltip("Tween path type")]
    public PathType pathType;
    [Tooltip("Tween path mode")]
    public PathMode pathMode;
    [Tooltip("Tween path resolution - more means better curve")]
    public int resolution;
    [Tooltip("Tween path gizmo color in scene window")]
    public Color gizmoColor;
    [Tooltip("Ease Type")]
    public Ease easeType;
}

[System.Serializable]
public class TDSpriteRendererColor
{
    [Tooltip("Tween starting value - color")]
    public Color tweenStartValue;
    [Tooltip("Tween end value - color")]
    public Color tweenEndValue;
    [Tooltip("Tween duration")]
    public float tweenDuration;
    [Tooltip("Tween Dealy")]
    public float tweenDelay;
    [Tooltip("Ease Type")]
    public Ease easeType;
}

[System.Serializable]
public class TDSpriteRendererFade
{
    [Tooltip("Tween starting value - fade int")]
    [Range(0.0f, 1.0f)]
    public float tweenStartValue;
    [Tooltip("Tween end value - fade int")]
    [Range(0.0f, 1.0f)]
    public float tweenEndValue;
    [Tooltip("Tween duration")]
    public float tweenDuration;
    [Tooltip("Tween Dealy")]
    public float tweenDelay;
    [Tooltip("Ease Type")]
    public Ease easeType;
}

[System.Serializable]
public class TDMaterialColor
{
    [Tooltip("Tween end value - color")]
    public Color tweenEndValue;
    [Tooltip("Tween duration")]
    public float tweenDuration;
    [Tooltip("Tween Dealy")]
    public float tweenDelay;
    [Tooltip("Ease Type")]
    public Ease easeType;
}

[System.Serializable]
public class TDMaterialFade
{
    [Tooltip("Tween end value - fade int")]
    [Range(0.0f, 1.0f)]
    public float tweenEndValue;
    [Tooltip("Tween duration")]
    public float tweenDuration;
    [Tooltip("Tween Dealy")]
    public float tweenDelay;
    [Tooltip("Ease Type")]
    public Ease easeType;
}