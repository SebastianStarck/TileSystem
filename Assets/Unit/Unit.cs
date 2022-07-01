using FormationSystem;
using UnityEngine;

namespace Unit
{
    public class Unit : MonoBehaviour
    {
        private FormationPosition _position;

        public FormationPosition Position
        {
            get => Position;
            set => _position = value;
        }
    }
}