using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SkeletonRenderer : MonoBehaviour
{
    private long _lastFrameIndex = -1;

    private Astra.Body[] _bodies;
    private Dictionary<int, GameObject[]> _bodySkeletons;

    private readonly Vector3 NormalPoseScale = new Vector3(1, 1, 1);
    private readonly Vector3 GripPoseScale = new Vector3(0.5f, 0.5f, 0.5f);

    public GameObject JointPrefab;
    public Transform JointRoot;

    public Toggle ToggleSeg = null;
    public Toggle ToggleSegBody = null;
    public Toggle ToggleSegBodyHand = null;

    public Toggle ToggleProfileFull = null;
    public Toggle ToggleProfileBasic = null;

    private Astra.BodyTrackingFeatures _previousTargetFeatures = Astra.BodyTrackingFeatures.HandPose;
    private Astra.SkeletonProfile _previousSkeletonProfile = Astra.SkeletonProfile.Full;

    void Start()
    {
        _bodySkeletons = new Dictionary<int, GameObject[]>();
        _bodies = new Astra.Body[Astra.BodyFrame.MaxBodies];
    }

    public void OnNewFrame(Astra.BodyStream bodyStream, Astra.BodyFrame frame)
    {
        if (frame.Width == 0 ||
            frame.Height == 0)
        {
            return;
        }

        if (_lastFrameIndex == frame.FrameIndex)
        {
            return;
        }

        _lastFrameIndex = frame.FrameIndex;

        frame.CopyBodyData(ref _bodies);
        UpdateSkeletonsFromBodies(_bodies);
        UpdateBodyFeatures(bodyStream, _bodies);
        UpdateSkeletonProfile(bodyStream);
    }

    void UpdateSkeletonsFromBodies(Astra.Body[] bodies)
    {
        foreach (var body in bodies)
        {
            if (body.Status == Astra.BodyStatus.NotTracking)
            {
                continue;
            }

            GameObject[] joints;
            if (!_bodySkeletons.ContainsKey(body.Id))
            {
                joints = new GameObject[body.Joints.Length];

                for (int i = 0; i < joints.Length; i++)
                {
                    joints[i] = (GameObject)Instantiate(JointPrefab, Vector3.zero, Quaternion.identity);
                    joints[i].transform.SetParent(JointRoot);
                }

                _bodySkeletons.Add(body.Id, joints);
            }
            else
            {
                joints = _bodySkeletons[body.Id];
            }

            for (int i = 0; i < body.Joints.Length; i++)
            {
                var skeletonJoint = joints[i];
                var bodyJoint = body.Joints[i];

                if (bodyJoint.Status != Astra.JointStatus.NotTracked)
                {
                    if (!skeletonJoint.activeSelf)
                    {
                        skeletonJoint.SetActive(true);
                    }

                    skeletonJoint.transform.localPosition =
                        new Vector3(bodyJoint.WorldPosition.X / 1000f,
                                    bodyJoint.WorldPosition.Y / 1000f,
                                    bodyJoint.WorldPosition.Z / 1000f);

                    //skel.Joints[i].Orient.Matrix:
                    // 0, 			1,	 		2,
                    // 3, 			4, 			5,
                    // 6, 			7, 			8
                    // -------
                    // right(X),	up(Y), 		forward(Z)

                    //Vector3 jointRight = new Vector3(
                    //    bodyJoint.Orientation.M00,
                    //    bodyJoint.Orientation.M10,
                    //    bodyJoint.Orientation.M20);

                    Vector3 jointUp = new Vector3(
                        bodyJoint.Orientation.M01,
                        bodyJoint.Orientation.M11,
                        bodyJoint.Orientation.M21);

                    Vector3 jointForward = new Vector3(
                        bodyJoint.Orientation.M02,
                        bodyJoint.Orientation.M12,
                        bodyJoint.Orientation.M22);

                    skeletonJoint.transform.rotation =
                        Quaternion.LookRotation(jointForward, jointUp);

                    if (bodyJoint.Type == Astra.JointType.LeftHand)
                    {
                        UpdateHandPoseVisual(skeletonJoint, body.HandPoseInfo.LeftHand);
                    }
                    else if (bodyJoint.Type == Astra.JointType.RightHand)
                    {
                        UpdateHandPoseVisual(skeletonJoint, body.HandPoseInfo.RightHand);
                    }
                }
                else
                {
                   if (skeletonJoint.activeSelf) skeletonJoint.SetActive(false);
                }
            }
        }
    }

    private void UpdateHandPoseVisual(GameObject skeletonJoint, Astra.HandPose pose)
    {
        Vector3 targetScale = NormalPoseScale;
        if (pose == Astra.HandPose.Grip)
        {
            targetScale = GripPoseScale;
        }
        skeletonJoint.transform.localScale = targetScale;
    }

    private void UpdateBodyFeatures(Astra.BodyStream bodyStream, Astra.Body[] bodies)
    {
        if (ToggleSeg != null &&
            ToggleSegBody != null &&
            ToggleSegBodyHand != null)
        {
            Astra.BodyTrackingFeatures targetFeatures = Astra.BodyTrackingFeatures.Segmentation;
            if (ToggleSegBodyHand.isOn)
            {
                targetFeatures = Astra.BodyTrackingFeatures.HandPose;
            }
            else if (ToggleSegBody.isOn)
            {
                targetFeatures = Astra.BodyTrackingFeatures.Skeleton;
            }

            if (targetFeatures != _previousTargetFeatures)
            {
                _previousTargetFeatures = targetFeatures;
                foreach (var body in bodies)
                {
                    if (body.Status != Astra.BodyStatus.NotTracking)
                    {
                        bodyStream.SetBodyFeatures(body.Id, targetFeatures);
                    }
                }
                bodyStream.SetDefaultBodyFeatures(targetFeatures);
            }
        }
    }

    private void UpdateSkeletonProfile(Astra.BodyStream bodyStream)
    {
        if (ToggleProfileFull != null &&
            ToggleProfileBasic != null)
        {
            Astra.SkeletonProfile targetSkeletonProfile = Astra.SkeletonProfile.Full;
            if (ToggleProfileFull.isOn)
            {
                targetSkeletonProfile = Astra.SkeletonProfile.Full;
            }
            else if (ToggleProfileBasic.isOn)
            {
                targetSkeletonProfile = Astra.SkeletonProfile.Basic;
            }

            if (targetSkeletonProfile != _previousSkeletonProfile)
            {
                _previousSkeletonProfile = targetSkeletonProfile;
                bodyStream.SetSkeletonProfile(targetSkeletonProfile);
            }
        }
    }
}
