using UnityEngine;


namespace GeneticSharp.Runner.UnityApp.Shooter
{
    public class ShootVectorPhenotypeEntity : PhenotypeEntityBase
    {
        public const int Shoot_Y_VectorBits_Bits = 9;
        public const int Shoot_Z_VectorBits_Bits = 9;
        public const int Shoot_VectorStrengh_Bits = 9;

        public const int PhenotypeSize = Shoot_Y_VectorBits_Bits + Shoot_Z_VectorBits_Bits + Shoot_VectorStrengh_Bits;

        public ShootVectorPhenotypeEntity(ShooterSampleConfig config, int entityIndex)
        {
            Phenotypes = new IPhenotype[]
            {
                new Phenotype("Vector_Y_Shoot", Shoot_Y_VectorBits_Bits)
                {
                    MinValue = 0,
                    MaxValue = config.Max_VectorAngle
                },
                new Phenotype("Vector_Z_Shoot", Shoot_Z_VectorBits_Bits)
                {
                    MinValue = 0,
                    MaxValue = config.Max_VectorAngle
                },
                new Phenotype("Vector_Strengh", Shoot_VectorStrengh_Bits)
                {
                    MinValue = 0,
                    MaxValue = config.MaxStrengh
                }
            };
        }


        public Vector3 ThrowingVector
        {
            get
            {
                var y = (float)Phenotypes[0].Value;
                var z = (float)Phenotypes[1].Value;
                var str = (float)Phenotypes[2].Value;
                    
                return new Vector3(str , y , z);
            }
        }
    }
}
