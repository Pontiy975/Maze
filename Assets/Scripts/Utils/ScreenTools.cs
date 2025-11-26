using UnityEngine;

namespace Utils
{
    public class ScreenTools
    {
        public static Vector2 AdaptedScreenSize(Vector2 designedSize)
        {
            float multiplier = designedSize.x / Screen.width;

            return new Vector2(designedSize.x, Screen.height * multiplier);
        }

        public static Vector3 CanvasToWorldPosition(Camera cam, Vector2 canvasPosition, RectTransform canvasRectTransform, float distanceFromCamera)
        {
            Vector2 sizeDelta = canvasRectTransform.sizeDelta;

            canvasPosition += sizeDelta * 0.5f;

            Vector3 coinScreenPos = new Vector3(
                canvasPosition.x / sizeDelta.x * Screen.width,
                canvasPosition.y / sizeDelta.y * Screen.height,
                distanceFromCamera
            );

            Vector3 worldPos = cam.ScreenToWorldPoint(coinScreenPos);

            return worldPos;
        }

        public static Vector2 WorldToCanvasPosition(Camera cam, Vector3 worldPos, RectTransform canvasRectTransform, bool isCanvasOverlay = true)
        {
            /*
            * https://forum.unity.com/threads/camera-worldtoscreenpoint-bug.85311/
            * If world position behind the camera WorldToScreenPoint returns incorrect value
            */

            //Vector2 screenPos = cam.WorldToScreenPoint(worldPos);
            //Vector2 screenPos = WorldToScreenPointProjected(cam, worldPos);
            Vector2 screenPos = CalculateWorldPosition(worldPos, cam);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPos, isCanvasOverlay ? null : cam, out Vector2 canvasPos);

            /*
             * ScreenPointToLocalPointInRectangle can be changed buy this code
             */
            /*Vector2 canvasPos = new Vector2(
                screenPos.x / cam.pixelWidth * canvasRectTransform.sizeDelta.x,
                screenPos.y / cam.pixelHeight * canvasRectTransform.sizeDelta.y
                );

            canvasPos -= canvasRectTransform.sizeDelta * 0.5f;*/

            return canvasPos;
        }

        /*
         * https://forum.unity.com/threads/camera-worldtoscreenpoint-bug.85311/
         * If world position behind the camera WorldToScreenPoint returns incorrect value
         * Project position in front of camera
         */
        private static Vector2 WorldToScreenPointProjected(Camera camera, Vector3 worldPos)
        {
            Vector3 camNormal = camera.transform.forward;
            Vector3 vectorFromCam = worldPos - camera.transform.position;
            float camNormDot = Vector3.Dot(camNormal, vectorFromCam);
            if (camNormDot <= 0)
            {
                // we are behind the camera forward facing plane, project the position in front of the plane
                Vector3 proj = (camNormal * camNormDot * 1.01f);
                worldPos = camera.transform.position + (vectorFromCam - proj);
            }

            return RectTransformUtility.WorldToScreenPoint(camera, worldPos);
        }

        /*
         * https://forum.unity.com/threads/camera-worldtoscreenpoint-bug.85311/
         * If world position behind the camera WorldToScreenPoint returns incorrect value
         * Project position in front of camera
         */
        private static Vector2 CalculateWorldPosition(Vector3 position, Camera camera)
        {
            //if the point is behind the camera then project it onto the camera plane
            Vector3 camNormal = camera.transform.forward;
            Vector3 vectorFromCam = position - camera.transform.position;
            float camNormDot = Vector3.Dot(camNormal, vectorFromCam.normalized);
            if (camNormDot <= 0f)
            {
                //we are beind the camera, project the position on the camera plane
                float camDot = Vector3.Dot(camNormal, vectorFromCam);
                Vector3 proj = (camNormal * camDot * 1.01f);   //small epsilon to keep the position infront of the camera
                position = camera.transform.position + (vectorFromCam - proj);
            }

            return RectTransformUtility.WorldToScreenPoint(camera, position);
        }
    }
}