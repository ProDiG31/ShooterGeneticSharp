using GeneticSharp.Domain.Chromosomes;

namespace GeneticSharp.Runner.UnityApp.Shooter
{
    public class ShooterChromosome : BitStringChromosome<ShootVectorPhenotypeEntity>
    {

        private ShooterSampleConfig m_config;

        public ShooterChromosome(ShooterSampleConfig config)
        {
            m_config = config;

            var phenotypeEntities = new ShootVectorPhenotypeEntity[config.iteration];

            for (int i = 0; i < phenotypeEntities.Length; i++)
            {
                phenotypeEntities[i] = new ShootVectorPhenotypeEntity(config, i);
            }

            SetPhenotypes(phenotypeEntities);
            CreateGenes();
        }

        public string ID { get; } = System.Guid.NewGuid().ToString();

        public bool Evaluated { get; set; }
        public float MinDistanceFromTarget { get; set; }

        //Chromosome Function 

        public override IChromosome CreateNew()
        {
            return new ShooterChromosome(m_config);
        }

    }
}
