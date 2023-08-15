using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Football.Outfit
{
    [CreateAssetMenu]
    public class OutfitContainer : ScriptableObject
    {
        public List<Sprite> hair;
        public List<Sprite> mouth;
        public List<Sprite> eye;
        public List<Sprite> shorts;
        public List<Sprite> shirt;
    }
}
