using UnityEngine;

namespace Assets.SoccerGameEngine_Basic_.Scripts.Entities
{
    public class SupportSpot : MonoBehaviour
    {
        [SerializeField]
        bool _isPickedOut;

        public Player Owner;// { get; set; }

        public MeshRenderer MeshRenderer { get; set; }

        private void Awake()
        {
            MeshRenderer = GetComponent<MeshRenderer>();
        }

        private void Update()
        {
            if (MeshRenderer != null)
            {
                if (_isPickedOut)
                    MeshRenderer.material.color = Color.red;
                else
                    MeshRenderer.material.color = Color.green;
            }
        }

        public void SetIsNotPickedOut()
        {
            // set is picked out
            _isPickedOut = false;

            // set the owner
            Owner = null;
        }

        public void SetIsPickedOut(Player player)
        {
            // set is picked out
            _isPickedOut = true;

            // set the owner
            Owner = player;
        }

        public bool IsPickedOut(Player player)
        {
            // if there is no owner then I'm not picked out
            // if the owner is not equal to the testing player then I'm picked out
            if (Owner == player)
                return false;
            else
            {
                // return result
                return _isPickedOut;
            }
        }
    }
}
