using GeneticSharp.Domain;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Infrastructure.Framework.Threading;
using UnityEngine;

namespace GeneticSharp.Runner.UnityApp.Shooter
{
    public class ShooterSampleController : SampleControllerBase
    {
        private static ShooterSampleConfig s_config;
        private int NumberOfSimultaneousEvaluations;
        public Vector2Int SimulationsGrid = new Vector2Int(5, 5);
        public Vector3 OffsetBetweenEvaluationInstance = new Vector3(10, 0, 0);

        public GameObject EvaluationPrefab;
        public ShooterSampleConfig Config;

        private ShooterFitness m_fitness;

        private Vector3 m_DistanceToTarget;
        private PrefabPool m_evaluationPool;
        private Vector3 EvalutionInstancePosition;

        //private PrefabPool

        public static void SetConfig(ShooterSampleConfig config)
        {
            s_config = config;
        }

        private void Awake()
        {
            if (s_config != null)
            {
                Config = s_config;
            }

            NumberOfSimultaneousEvaluations = SimulationsGrid.x * SimulationsGrid.y;
        }

        protected override GeneticAlgorithm CreateGA()
        {
            m_fitness = new ShooterFitness();
            var chromosome = new ShooterChromosome(Config);
            var crossover = new UniformCrossover();
            var mutation = new FlipBitMutation();
            var selection = new EliteSelection();
            var population = new Population(NumberOfSimultaneousEvaluations, NumberOfSimultaneousEvaluations, chromosome)
            {
                GenerationStrategy = new PerformanceGenerationStrategy()
            };

            GeneticAlgorithm ga = new GeneticAlgorithm(population, m_fitness, selection, crossover, mutation)
            {
                Termination = new ShooterTermination(),
                TaskExecutor = new ParallelTaskExecutor
                {
                    MinThreads = population.MinSize,
                    MaxThreads = population.MaxSize * 2
                }
            };

            ga.GenerationRan += delegate
            {
                EvalutionInstancePosition = Vector3.zero;
                m_evaluationPool.ReleaseAll();
            };

            ga.MutationProbability = .1f;

            return ga;
        }


        protected override void StartSample()
        {
            ChromosomesCleanupEnabled = false;
            EvalutionInstancePosition = Vector3.zero ;
            m_evaluationPool = new PrefabPool(EvaluationPrefab);
        }

        protected override void UpdateSample()
        {

            //Debug.Log("Controller update , Pop Begin " + m_fitness.ChromosomesToBeginEvaluation.Count + " pop End :  " + m_fitness.ChromosomesToEndEvaluation.Count);
            //Debug.Log("Fitness " + m_fitness.);
             
            // end evaluation.
            while (m_fitness.ChromosomesToEndEvaluation.Count > 0)
            {
                ShooterChromosome c;
                m_fitness.ChromosomesToEndEvaluation.TryTake(out c);
                Debug.Log("End Evaluation " + c.MinDistanceFromTarget + " pop :  " + m_fitness.ChromosomesToEndEvaluation.Count );
                c.Fitness = c.MinDistanceFromTarget;
                c.Evaluated = true;
                Debug.Log("Chromosome Take : [" + c.ID + "] : Fitness " + c.Fitness);
            }


            // in evaluation.
            while (m_fitness.ChromosomesToBeginEvaluation.Count > 0)
            {
                //Debug.Log("In Evaluation");

                ShooterChromosome c;
                m_fitness.ChromosomesToBeginEvaluation.TryTake(out c);
                c.Evaluated = false;
                c.MinDistanceFromTarget = float.PositiveInfinity;

                var evaluation = m_evaluationPool.Get(EvalutionInstancePosition);
                evaluation.name = c.ID;
 
                var Shooter = evaluation.GetComponentInChildren<ShooterController>();
                //Shooter.transform.position = EvalutionInstancePosition;
                Shooter.SetChromosome(c, Config);
                Debug.Log("Chromosome Evaluated: [" + c.ID + "] : Fitness " + c.Fitness);

                EvalutionInstancePosition += OffsetBetweenEvaluationInstance;
            }
        }
    }
}
