using Football.Gameplay;
using Football.Outfit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Football
{
    public class PlayerSelect : MonoBehaviour
    {
        private void Start()
        {
            OutfitHandler handler = GetComponent<OutfitHandler>();
            OutfitControl control = OutfitControl.instance;
            if(GetComponent<Player>().playerIsControllingThis)
            {
                handler.currentOutfit.hair = control.HairOption;
                handler.currentOutfit.mouth = control.MouthOption;
                handler.currentOutfit.eye = control.EyeOption;
                handler.currentOutfit.shirt = control.ShirtOption;
                handler.currentOutfit.shorts = control.ShortOption;
                handler.UpdateOutfit();
            }
        }
    }
}
