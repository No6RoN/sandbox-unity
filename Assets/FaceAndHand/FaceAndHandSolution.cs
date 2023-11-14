// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity.FaceAndHand
{
  public class FaceAndHandSolution : ImageSourceSolution<FaceAndHandGraph>
  {
    [SerializeField] private DetectionListAnnotationController _faceDetectionsAnnotationController;
    [SerializeField] private MultiFaceLandmarkListAnnotationController _multiFaceLandmarksAnnotationController;
    [SerializeField] private NormalizedRectListAnnotationController _faceRectsFromLandmarksAnnotationController;
    [SerializeField] private NormalizedRectListAnnotationController _faceRectsFromDetectionsAnnotationController;
    
    [SerializeField] private DetectionListAnnotationController _palmDetectionsAnnotationController;
    [SerializeField] private NormalizedRectListAnnotationController _handRectsFromPalmDetectionsAnnotationController;
    [SerializeField] private MultiHandLandmarkListAnnotationController _handLandmarksAnnotationController;
    [SerializeField] private NormalizedRectListAnnotationController _handRectsFromLandmarksAnnotationController;

    //[SerializeField] private Test test;

    public int maxNumFaces
    {
      get => graphRunner.maxNumFaces;
      set => graphRunner.maxNumFaces = value;
    }

    public bool refineLandmarks
    {
      get => graphRunner.refineLandmarks;
      set => graphRunner.refineLandmarks = value;
    }
    
    public FaceAndHandGraph.ModelComplexity modelComplexity
    {
      get => graphRunner.modelComplexity;
      set => graphRunner.modelComplexity = value;
    }

    public int maxNumHands
    {
      get => graphRunner.maxNumHands;
      set => graphRunner.maxNumHands = value;
    }

    public float minDetectionConfidence
    {
      get => graphRunner.minDetectionConfidence;
      set => graphRunner.minDetectionConfidence = value;
    }

    public float minTrackingConfidence
    {
      get => graphRunner.minTrackingConfidence;
      set => graphRunner.minTrackingConfidence = value;
    }

    protected override void OnStartRun()
    {
      if (!runningMode.IsSynchronous())
      {
        graphRunner.OnFaceDetectionsOutput += OnFaceDetectionsOutput;
        graphRunner.OnMultiFaceLandmarksOutput += OnMultiFaceLandmarksOutput;
        graphRunner.OnFaceRectsFromLandmarksOutput += OnFaceRectsFromLandmarksOutput;
        graphRunner.OnFaceRectsFromDetectionsOutput += OnFaceRectsFromDetectionsOutput;
        
        graphRunner.OnPalmDetectectionsOutput += OnPalmDetectionsOutput;
        graphRunner.OnHandRectsFromPalmDetectionsOutput += OnHandRectsFromPalmDetectionsOutput;
        graphRunner.OnHandLandmarksOutput += OnHandLandmarksOutput;
        // TODO: render HandWorldLandmarks annotations
        graphRunner.OnHandWorldLandmarksOutput += OnHandWorldLandmarksOutput;
        graphRunner.OnHandRectsFromLandmarksOutput += OnHandRectsFromLandmarksOutput;
        graphRunner.OnHandednessOutput += OnHandednessOutput;
      }

      var imageSource = ImageSourceProvider.ImageSource;
      
      SetupAnnotationController(_faceDetectionsAnnotationController, imageSource);
      SetupAnnotationController(_faceRectsFromLandmarksAnnotationController, imageSource);
      SetupAnnotationController(_multiFaceLandmarksAnnotationController, imageSource);
      SetupAnnotationController(_faceRectsFromDetectionsAnnotationController, imageSource);
      
      SetupAnnotationController(_palmDetectionsAnnotationController, imageSource, true);
      SetupAnnotationController(_handRectsFromPalmDetectionsAnnotationController, imageSource, true);
      SetupAnnotationController(_handLandmarksAnnotationController, imageSource, true);
      SetupAnnotationController(_handRectsFromLandmarksAnnotationController, imageSource, true);
    }

    protected override void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      graphRunner.AddTextureFrameToInputStream(textureFrame);
    }

    protected override IEnumerator WaitForNextValue()
    {
      List<Detection> faceDetections = null;
      List<NormalizedLandmarkList> multiFaceLandmarks = null;
      List<NormalizedRect> faceRectsFromLandmarks = null;
      List<NormalizedRect> faceRectsFromDetections = null;
      
      List<Detection> palmDetections = null;
      List<NormalizedRect> handRectsFromPalmDetections = null;
      List<NormalizedLandmarkList> handLandmarks = null;
      List<LandmarkList> handWorldLandmarks = null;
      List<NormalizedRect> handRectsFromLandmarks = null;
      List<ClassificationList> handedness = null;

      if (runningMode == RunningMode.Sync)
      {
        var _ = graphRunner.TryGetNext(out faceDetections, out multiFaceLandmarks, out faceRectsFromLandmarks, out faceRectsFromDetections, out palmDetections, out handRectsFromPalmDetections, out handLandmarks, out handWorldLandmarks, out handRectsFromLandmarks, out handedness, true);
      }
      else if (runningMode == RunningMode.NonBlockingSync)
      {
        yield return new WaitUntil(() => graphRunner.TryGetNext(out faceDetections, out multiFaceLandmarks, out faceRectsFromLandmarks, out faceRectsFromDetections, out palmDetections, out handRectsFromPalmDetections, out handLandmarks, out handWorldLandmarks, out handRectsFromLandmarks, out handedness, false));
      }

      _faceDetectionsAnnotationController.DrawNow(faceDetections);
      _multiFaceLandmarksAnnotationController.DrawNow(multiFaceLandmarks);
      _faceRectsFromLandmarksAnnotationController.DrawNow(faceRectsFromLandmarks);
      _faceRectsFromDetectionsAnnotationController.DrawNow(faceRectsFromDetections);
      
      _palmDetectionsAnnotationController.DrawNow(palmDetections);
      _handRectsFromPalmDetectionsAnnotationController.DrawNow(handRectsFromPalmDetections);
      _handLandmarksAnnotationController.DrawNow(handLandmarks, handedness);
      // TODO: render HandWorldLandmarks annotations
      _handRectsFromLandmarksAnnotationController.DrawNow(handRectsFromLandmarks);
    }

    private void OnFaceDetectionsOutput(object stream, OutputEventArgs<List<Detection>> eventArgs)
    {
      _faceDetectionsAnnotationController.DrawLater(eventArgs.value);
    }

    private void OnMultiFaceLandmarksOutput(object stream, OutputEventArgs<List<NormalizedLandmarkList>> eventArgs)
    {
      _multiFaceLandmarksAnnotationController.DrawLater(eventArgs.value);
    }

    private void OnFaceRectsFromLandmarksOutput(object stream, OutputEventArgs<List<NormalizedRect>> eventArgs)
    {
      _faceRectsFromLandmarksAnnotationController.DrawLater(eventArgs.value);
    }

    private void OnFaceRectsFromDetectionsOutput(object stream, OutputEventArgs<List<NormalizedRect>> eventArgs)
    {
      _faceRectsFromDetectionsAnnotationController.DrawLater(eventArgs.value);
    }
    
    private void OnPalmDetectionsOutput(object stream, OutputEventArgs<List<Detection>> eventArgs)
    {
      _palmDetectionsAnnotationController.DrawLater(eventArgs.value);
    }

    private void OnHandRectsFromPalmDetectionsOutput(object stream, OutputEventArgs<List<NormalizedRect>> eventArgs)
    {
      _handRectsFromPalmDetectionsAnnotationController.DrawLater(eventArgs.value);
    }

    private void OnHandLandmarksOutput(object stream, OutputEventArgs<List<NormalizedLandmarkList>> eventArgs)
    {
      _handLandmarksAnnotationController.DrawLater(eventArgs.value);
    }

    private void OnHandWorldLandmarksOutput(object stream, OutputEventArgs<List<LandmarkList>> eventArgs)
    {
      //test.UpdateLandmarks(eventArgs.value);
    }

    private void OnHandRectsFromLandmarksOutput(object stream, OutputEventArgs<List<NormalizedRect>> eventArgs)
    {
      _handRectsFromLandmarksAnnotationController.DrawLater(eventArgs.value);
    }

    private void OnHandednessOutput(object stream, OutputEventArgs<List<ClassificationList>> eventArgs)
    {
      _handLandmarksAnnotationController.DrawLater(eventArgs.value);
    }
  }
}
