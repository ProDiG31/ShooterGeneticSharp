using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Chromosomes;
using System.Threading;
using UnityEngine;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System;
using System.Linq;


namespace GeneticSharp.Runner.UnityApp.Shooter
{


    public class ShooterFitness : IFitness
    {
        public ShooterFitness()
        {
            ChromosomesToBeginEvaluation = new BlockingCollection<ShooterChromosome>();
            ChromosomesToEndEvaluation = new BlockingCollection<ShooterChromosome>();
        }

        //Liste des chromosome pas encore evalué
        public BlockingCollection<ShooterChromosome> ChromosomesToBeginEvaluation { get; private set; }
        //liste des chromosome deja evalué 
        public BlockingCollection<ShooterChromosome> ChromosomesToEndEvaluation { get; private set; }

        public double Evaluate(IChromosome chromosome)
        {
            var c = chromosome as ShooterChromosome;

            ChromosomesToBeginEvaluation.Add(c);

            do
            {
                Thread.Sleep(100);
                c.Fitness = c.MinDistanceFromTarget;
                //Debug.Log("Begin Evaluation : " + c.ID + " : " + c.Fitness);
            } while (!c.Evaluated);

            ChromosomesToEndEvaluation.Add(c);

            do
            {
                Thread.Sleep(100);
                //Debug.Log("End Evaluation : " + c.ID + " : " + c.Fitness);
            } while (!c.Evaluated);

            return c.MinDistanceFromTarget;
        }
    }
}

