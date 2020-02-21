using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GeneticSharp.Runner.UnityApp.Shooter
{
    [CreateAssetMenu(menuName = "GeneticSharp/Shooter/ShootSampleConfig", order = 1)]
    public class ShooterSampleConfig : ScriptableObject
    {
        [Header("Projectile")]
        [Range(0, 5)]
        public float ProjectileRadius = 1f;
        public float z_Angle = 0f;
        public float y_Angle = 0f;
        public float strengh = 10f;

        [Header("Target")]
        [Range(1, 10)]
        public float TargerRadius = 1f;
        public float xPos = 0;
        public float yPos = 0;

        [Range(1, 100)]
        public int iteration = 1;

        public float Max_VectorAngle = 254;
        public float MaxStrengh = 10;

        [Header("Evaluation")]
        public float WarmupTime = 1f;

        //public float StartDelay = .5f; 

        public float TestDuration = 5f;
        //public float MinVelocityCheckTime = 5f;
        //public float MinVelocity = 2f;


    }
}
