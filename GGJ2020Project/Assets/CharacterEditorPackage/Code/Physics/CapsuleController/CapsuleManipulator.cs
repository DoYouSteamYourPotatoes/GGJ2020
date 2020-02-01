using UnityEngine;
using System.Collections;
//--------------------------------------------------------------------
//DEPRECATED
//All functionality now integrated in CapsuleTransform
//
//Class which checks and executes CapsuleTransform manipulations, such as resizing, rotating and moving.
//The check functions allow classes to check if a capsuletransform can move in a certain way, can exist somewhere without clipping or is currently clipping
//Could be placed in CapsuleTransform itself, but CapsuleManipulator was initially used on ControlledCapsuleCollider.
//--------------------------------------------------------------------