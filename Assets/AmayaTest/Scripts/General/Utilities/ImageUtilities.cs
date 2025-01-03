using AmayaTest.Scripts.Data;
using UnityEngine;

namespace AmayaTest.Scripts.General.Utilities
{
    public static class ImageUtilities
    {
        public static void SetupSpriteRenderer(SpriteRenderer spriteRenderer, SpriteResource spriteResource)
        {
            spriteRenderer.flipX = spriteResource.FlipX;
            spriteRenderer.flipY = spriteResource.FlipY;

            spriteRenderer.transform.rotation = Quaternion.Euler(0,0, -(int)spriteResource.RotationToTheRight);
        }
    }
}