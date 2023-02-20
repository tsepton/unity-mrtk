using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using UnityEngine;
using UnityEngine.Events;

public class MUIController : MonoBehaviour {
    
    [Header("Gesture Settings")] 
    public Handedness handedness = Handedness.Right; 
    public HandTrackingController.Gestures openEvent = HandTrackingController.Gestures.HandWideOpen;
    public HandTrackingController.Gestures closeEvent = HandTrackingController.Gestures.HandItalian;
    public HandTrackingController.Gestures fourthSelectionEvent = HandTrackingController.Gestures.HandPointing;

    [Header("Gesture registration")] 
    public UnityEvent open;
    public UnityEvent close;
    public HandTrackingController.GazeTargetEvent fourthSelection;

    [Header("Debugger Settings")] public bool useDebug;
    public GameObject debugUi;
    public HandTrackingController.Debugger.DebugMode debugMode = HandTrackingController.Debugger.DebugMode.Curl;

    // Children game objects
    private GameObject _constrainerGO;
    private GameObject _controllerGO;
    
    // Children components
    private HandTrackingController _controller; 
    private SolverHandler _solverHandler;
    private HandConstraintPalmUp _palmUp;
        
    void Start() {
        _constrainerGO = new GameObject("HandTrackingConstrainer");
        _constrainerGO.transform.parent = transform;
        _controllerGO = new GameObject("HandTrackingController");
        _controllerGO.transform.parent = transform;

        // Configure default constrainer (palms up)
        _constrainerGO.AddComponent<HandBounds>();
        _solverHandler = _constrainerGO.AddComponent<SolverHandler>();
        _solverHandler.TrackedTargetType = TrackedObjectType.HandJoint;
        _solverHandler.TrackedHandedness = handedness;
        _solverHandler.TrackedHandJoint = TrackedHandJoint.Palm;
        _palmUp = _constrainerGO.AddComponent<HandConstraintPalmUp>();
        _palmUp.OnHandActivate.AddListener(() => _controller.ConstrainerState = true);
        _palmUp.OnHandDeactivate.AddListener(() => _controller.ConstrainerState = false);
        _palmUp.FacingCameraTrackingThreshold = 50f;
        
        // Add user settings to controller script
        _controller = _controllerGO.AddComponent<HandTrackingController>();
        _controller.handedness = handedness;
        _controller.openEvent = openEvent;
        _controller.closeEvent = closeEvent;
        _controller.pointingEvent = fourthSelectionEvent;
        _controller.useDebug = useDebug;
        _controller.debugUi = debugUi;
        _controller.debugMode = debugMode;
        _controller.open = open;
        _controller.close = close;
        _controller.fourthSelection = fourthSelection;
    }
    
}
