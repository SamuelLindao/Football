using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Football.Outfit;

namespace Football
{
    public class OutfitControl : MonoBehaviour
    {
        public static OutfitControl instance;
        public OutfitContainer container;
        public int HairOption;
        public int MouthOption;
        public int EyeOption;
        public int ShortOption;
        public int ShirtOption;

        [Space]
        public SpriteRenderer Hair;
        public SpriteRenderer Mouth;
        public SpriteRenderer Eye;
        public SpriteRenderer Short;
        public SpriteRenderer Shirt;

        private void Awake()
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        private void Start()
        {
            UpdatePreview();
        }
        public void Next()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay");
        }
        public void UpdatePreview()
        {
            Hair.sprite = container.hair[HairOption];
            Mouth.sprite = container.mouth[MouthOption];
            Eye.sprite = container.eye[EyeOption];
            Short.sprite = container.shorts[ShortOption];
            Shirt.sprite = container.shirt[ShirtOption];
        }

        public void ModifyHair(int value)
        {
            HairOption = Mathf.Clamp(HairOption + value, 0, container.hair.Count -1 );
            UpdatePreview();
        }
        public void ModifyMouth(int value)
        {
            MouthOption = Mathf.Clamp(MouthOption + value, 0, container.mouth.Count -1 );

            UpdatePreview();

        }
        public void ModifyEye(int value)
        {
            EyeOption = Mathf.Clamp(EyeOption + value, 0, container.eye.Count - 1 );

            UpdatePreview();

        }
        public void ModifyShort(int value)
        {
            ShortOption = Mathf.Clamp(ShortOption + value, 0, container.shorts.Count - 1);

            UpdatePreview();

        }
        public void ModifyShirt(int value)
        {
            ShirtOption = Mathf.Clamp(ShirtOption + value, 0, container.shirt.Count - 1);

            UpdatePreview();

        }
    }
}
