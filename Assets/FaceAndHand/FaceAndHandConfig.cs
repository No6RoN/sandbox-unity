// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using Mediapipe.Unity.Holistic;
using UnityEngine;
using UnityEngine.UI;
using Mediapipe.Unity.UI;

namespace Mediapipe.Unity.FaceAndHand.UI
{
  public class FaceAndHandConfig : ModalContents
  {
    private const string _MaxNumFacesPath = "Scroll View/Viewport/Contents/Max Num Faces/InputField";
    private const string _RefineLandmarksPath = "Scroll View/Viewport/Contents/Refine Landmarks/Toggle";
    
    private const string _ModelComplexityPath = "Scroll View/Viewport/Contents/Model Complexity/Dropdown";
    private const string _MaxNumHandsPath = "Scroll View/Viewport/Contents/Max Num Hands/InputField";
    
    private const string _MinDetectionConfidencePath = "Scroll View/Viewport/Contents/Min Detection Confidence/InputField";
    private const string _MinTrackingConfidencePath = "Scroll View/Viewport/Contents/Min Tracking Confidence/InputField";
    private const string _RunningModePath = "Scroll View/Viewport/Contents/Running Mode/Dropdown";
    private const string _TimeoutMillisecPath = "Scroll View/Viewport/Contents/Timeout Millisec/InputField";

    private FaceAndHandSolution _solution;
    
    private InputField _maxNumFacesInput;
    private Toggle _refineLandmarksInput;
    
    private Dropdown _modelComplexityInput;
    private InputField _maxNumHandsInput;
    
    private InputField _minDetectionConfidenceInput;
    private InputField _minTrackingConfidenceInput;
    private Dropdown _runningModeInput;
    private InputField _timeoutMillisecInput;

    private bool _isChanged;

    private void Start()
    {
      _solution = GameObject.Find("Solution").GetComponent<FaceAndHandSolution>();
      InitializeContents();
    }

    public override void Exit()
    {
      GetModal().CloseAndResume(_isChanged);
    }

    public void UpdateMaxNumFaces()
    {
      if (int.TryParse(_maxNumFacesInput.text, out var value))
      {
        _solution.maxNumFaces = Mathf.Max(0, value);
        _isChanged = true;
      }
    }

    public void ToggleRefineLandmarks()
    {
      _solution.refineLandmarks = _refineLandmarksInput.isOn;
      _isChanged = true;
    }
    
    public void SwitchModelComplexity()
    {
      _solution.modelComplexity = (FaceAndHandGraph.ModelComplexity)_modelComplexityInput.value;
      _isChanged = true;
    }

    public void UpdateMaxNumHands()
    {
      if (int.TryParse(_maxNumHandsInput.text, out var value))
      {
        _solution.maxNumHands = Mathf.Max(0, value);
        _isChanged = true;
      }
    }

    public void SetMinDetectionConfidence()
    {
      if (float.TryParse(_minDetectionConfidenceInput.text, out var value))
      {
        _solution.minDetectionConfidence = value;
        _isChanged = true;
      }
    }

    public void SetMinTrackingConfidence()
    {
      if (float.TryParse(_minTrackingConfidenceInput.text, out var value))
      {
        _solution.minTrackingConfidence = value;
        _isChanged = true;
      }
    }

    public void SwitchRunningMode()
    {
      _solution.runningMode = (RunningMode)_runningModeInput.value;
      _isChanged = true;
    }

    public void SetTimeoutMillisec()
    {
      if (int.TryParse(_timeoutMillisecInput.text, out var value))
      {
        _solution.timeoutMillisec = value;
        _isChanged = true;
      }
    }

    private void InitializeContents()
    {
      InitializeMaxNumFaces();
      InitializeRefineLandmarks();
      
      InitializeModelComplexity();
      InitializeMaxNumHands();
      
      InitializeMinDetectionConfidence();
      InitializeMinTrackingConfidence();
      InitializeRunningMode();
      InitializeTimeoutMillisec();
    }

    private void InitializeMaxNumFaces()
    {
      _maxNumFacesInput = gameObject.transform.Find(_MaxNumFacesPath).gameObject.GetComponent<InputField>();
      _maxNumFacesInput.text = _solution.maxNumFaces.ToString();
      _maxNumFacesInput.onEndEdit.AddListener(delegate { UpdateMaxNumFaces(); });
    }

    private void InitializeRefineLandmarks()
    {
      _refineLandmarksInput = gameObject.transform.Find(_RefineLandmarksPath).gameObject.GetComponent<Toggle>();
      _refineLandmarksInput.isOn = _solution.refineLandmarks;
      _refineLandmarksInput.onValueChanged.AddListener(delegate { ToggleRefineLandmarks(); });
    }
    
    private void InitializeModelComplexity()
    {
      _modelComplexityInput = gameObject.transform.Find(_ModelComplexityPath).gameObject.GetComponent<Dropdown>();
      _modelComplexityInput.ClearOptions();

      var options = new List<string>(Enum.GetNames(typeof(FaceAndHandGraph.ModelComplexity)));
      _modelComplexityInput.AddOptions(options);

      var currentModelComplexity = _solution.modelComplexity;
      var defaultValue = options.FindIndex(option => option == currentModelComplexity.ToString());

      if (defaultValue >= 0)
      {
        _modelComplexityInput.value = defaultValue;
      }

      _modelComplexityInput.onValueChanged.AddListener(delegate { SwitchModelComplexity(); });
    }

    private void InitializeMaxNumHands()
    {
      _maxNumHandsInput = gameObject.transform.Find(_MaxNumHandsPath).gameObject.GetComponent<InputField>();
      _maxNumHandsInput.text = _solution.maxNumHands.ToString();
      _maxNumHandsInput.onEndEdit.AddListener(delegate { UpdateMaxNumHands(); });
    }

    private void InitializeMinDetectionConfidence()
    {
      _minDetectionConfidenceInput = gameObject.transform.Find(_MinDetectionConfidencePath).gameObject.GetComponent<InputField>();
      _minDetectionConfidenceInput.text = _solution.minDetectionConfidence.ToString();
      _minDetectionConfidenceInput.onValueChanged.AddListener(delegate { SetMinDetectionConfidence(); });
    }

    private void InitializeMinTrackingConfidence()
    {
      _minTrackingConfidenceInput = gameObject.transform.Find(_MinTrackingConfidencePath).gameObject.GetComponent<InputField>();
      _minTrackingConfidenceInput.text = _solution.minTrackingConfidence.ToString();
      _minTrackingConfidenceInput.onValueChanged.AddListener(delegate { SetMinTrackingConfidence(); });
    }

    private void InitializeRunningMode()
    {
      _runningModeInput = gameObject.transform.Find(_RunningModePath).gameObject.GetComponent<Dropdown>();
      _runningModeInput.ClearOptions();

      var options = new List<string>(Enum.GetNames(typeof(RunningMode)));
      _runningModeInput.AddOptions(options);

      var currentRunningMode = _solution.runningMode;
      var defaultValue = options.FindIndex(option => option == currentRunningMode.ToString());

      if (defaultValue >= 0)
      {
        _runningModeInput.value = defaultValue;
      }

      _runningModeInput.onValueChanged.AddListener(delegate { SwitchRunningMode(); });
    }

    private void InitializeTimeoutMillisec()
    {
      _timeoutMillisecInput = gameObject.transform.Find(_TimeoutMillisecPath).gameObject.GetComponent<InputField>();
      _timeoutMillisecInput.text = _solution.timeoutMillisec.ToString();
      _timeoutMillisecInput.onValueChanged.AddListener(delegate { SetTimeoutMillisec(); });
    }
  }
}
