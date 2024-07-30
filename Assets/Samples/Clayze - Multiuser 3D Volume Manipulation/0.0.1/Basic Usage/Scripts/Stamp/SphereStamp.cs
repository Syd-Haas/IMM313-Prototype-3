using System;
using Clayze.Marching.Operations;
using UnityEngine;
using Leap;
using LeapInternal;
using Hand = Leap.Hand;

namespace Clayze.Marching
{
    public class SphereStamp : MonoBehaviour
    {
        public float radius;
        private float _distanceFromCamera = 5;
        public float maxSphereDistanceFromCamera;
        public float maxSphereSize;
        private OperationType _opType = OperationType.Add;
        [SerializeField] private OperationCollection opCollection;
        [SerializeField] private LeapProvider leapProvider;
        public PinchDetector pinchDetector;
        
        private void OnUpdateHand(Hand _hand)
        {
            Finger _indexFinger = _hand.fingers[1];
            transform.localPosition = _indexFinger.TipPosition;
            
        }

        private void OnEnable()
        {
            leapProvider.OnUpdateFrame += LeapProviderOnOnUpdateFrame;
        }

        private void OnDisable()
        {
            leapProvider.OnUpdateFrame -= LeapProviderOnOnUpdateFrame;
        }

        private void LeapProviderOnOnUpdateFrame(Frame frame)
        {
            var hands = frame.Hands;
            if (hands.Count == 0)
            {
                return;
            }
            
            if (hands.Count == 1)
            {
                OnUpdateHand(hands[0]);
            }else if (hands.Count == 2)
            {
                Hand right = frame.GetHand(Chirality.Right);
                if (right != null)
                {
                    OnUpdateHand(right);
                    return;
                }
                OnUpdateHand(hands[0]);
            }
            else
            {
                OnUpdateHand(hands[0]);
            }
            
            if (pinchDetector.IsPinching)
            {
                GetComponent<PinchDetector>();
                Debug.Log("I AM PINCHING");
                _opType = OperationType.Add;
                Stamp();
                Debug.Log("stamp");
            }
            
        }

        private void Update() {
            
            
            
            
            
            // if (// it doesnt exist //)
            // {
            //     if (Input.GetMouseButton(1))
            //     {
            //         radius += Input.mouseScrollDelta.y;
            //         radius = Mathf.Clamp(radius, 0, maxSphereSize);
            //     }
            //     else
            //     {
            //         _distanceFromCamera += Input.mouseScrollDelta.y;
            //         _distanceFromCamera = Mathf.Clamp(_distanceFromCamera, 1, maxSphereDistanceFromCamera);
            //     }
            // }
			         //
            // if (Input.GetMouseButtonDown(0))
            // {
            //     if (Input.GetMouseButton(1))
            //     {
            //         _opType = OperationType.Remove;
            //     }
            //     else
            //     {
            //         _opType = OperationType.Add;
            //     }
				        //
            //     Stamp();
            // }
			
        }

        [ContextMenu("Stamp")]
        private void Stamp()
        {
            SphereOp op = new SphereOp(transform.position,radius, _opType);
            opCollection.Add(op);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position,radius);
        }
    }
}