using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Football.Outfit
{
    public class OutfitHandler : MonoBehaviour
    {
        public OutfitContainer outfits;

        public OutfitData currentOutfit;

        [Header("References")]
        public SpriteRenderer HairRenderer;
        public SpriteRenderer MouthRenderer;
        public SpriteRenderer EyeRenderer;
        public SpriteRenderer ShortsRenderer;
        public SpriteRenderer ShirtRenderer;

        public void UpdateOutfit()
        {
            HairRenderer.sprite = outfits.hair[Mathf.Clamp(currentOutfit.hair, 0, outfits.hair.Count - 1)];
            MouthRenderer.sprite = outfits.mouth[Mathf.Clamp(currentOutfit.mouth, 0, outfits.mouth.Count - 1)];
            EyeRenderer.sprite = outfits.eye[Mathf.Clamp(currentOutfit.eye, 0, outfits.eye.Count - 1)];
            ShortsRenderer.sprite = outfits.shorts[Mathf.Clamp(currentOutfit.shorts, 0, outfits.shorts.Count - 1)];
            ShirtRenderer.sprite = outfits.shirt[Mathf.Clamp(currentOutfit.shirt, 0, outfits.shirt.Count - 1)];
        }

        public void GenerateRandomOutfit()
        {
            currentOutfit.hair = Random.Range(0, outfits.hair.Count);
            currentOutfit.mouth = Random.Range(0, outfits.mouth.Count);
            currentOutfit.eye = Random.Range(0, outfits.eye.Count);
        }

        private void Awake()
        {
            GenerateRandomOutfit();
            UpdateOutfit();
        }
    }
}
