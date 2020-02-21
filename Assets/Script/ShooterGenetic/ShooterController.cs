using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace GeneticSharp.Runner.UnityApp.Shooter
{
    public class ShooterController : MonoBehaviour
    {
        public Rigidbody rb;

        public ShooterChromosome Chromosome { get; private set; }

        private ShooterSampleConfig m_config;

        private FollowChromosomeCam m_cam;
        private TextMesh m_fitnessText;

        //public float Distance { get; private set; }
        private float m_startTime;
        private float m_currentVelocity;

        private float Y, Z, Strengh;

        public GameObject Target;
        private bool IsThrow = false;

        private void Awake()
        {
            m_fitnessText = GetComponentInChildren<TextMesh>();
        }

        private void Start()
        {
            m_cam = GameObject.Find("SimulationGrid")
               .GetComponent<SimulationGrid>()
               .AddChromosome(gameObject);

            rb.isKinematic = false;

        }



        void OnDestroy()
        {
            StopEvaluation();
        }

        private void OnCollisionEnter(Collision collision)
        {
            var other = collision.gameObject;

            if (other.gameObject.Equals(Target))
                Chromosome.MinDistanceFromTarget = DistanceFromTarget();
            else
                Chromosome.MinDistanceFromTarget = 100;

            Debug.Log("Stop Evaluation Collision");
            
            //UpdateFitnessText();

            StopEvaluation();
        }


        private float DistanceFromTarget()
        {
            return Vector3.Distance(transform.position, Target.transform.position);
        }

        public void SetChromosome(ShooterChromosome chromosome, ShooterSampleConfig config)
        {
            Chromosome = chromosome;
            //Debug.Log("Setting Chromosome : " + Chromosome);

            Chromosome.MinDistanceFromTarget = DistanceFromTarget();
            chromosome.Evaluated = false;

            m_startTime = Time.time;

            transform.rotation = Quaternion.identity;
            m_config = config;

            var phenotypes = chromosome.GetPhenotypes();
            var str =  phenotypes.Select(p => p.ThrowingVector).ToArray();
            Y = str[0].y;
            Z = str[0].z;
            Strengh = str[0].x;

            if (m_cam != null)
            {
                m_cam.StartFollowing(gameObject);
            }

            SetBallColor(Color.green);
            

            //Appliquer la force
            StartCoroutine(Throwing());

            //Calculer Fin de la Mesure
            StartCoroutine(CheckTimeout());
        }


        public void StopEvaluation()
        {
            SetBallColor(Color.red);
            Chromosome.MinDistanceFromTarget = DistanceFromTarget();
            rb.Sleep();
            //rb.isKinematic = true;
            Chromosome.Evaluated = true;
        }


        private IEnumerator Throwing()
        {
            //yield return new WaitForSeconds(m_config.StartDelay);
            yield return new WaitForSeconds(0);
            rb.isKinematic = false;

            Vector3 forcesDirectionApplies = new Vector3(0, Y, Z);

            Debug.Log("Throwing : " + forcesDirectionApplies + " / " + Strengh + "/ Pos : " + transform.position);

            rb.AddForce(forcesDirectionApplies * Strengh, ForceMode.Impulse);
            IsThrow = true;
        }


        private IEnumerator CheckTimeout()
        {
            yield return new WaitForSeconds(m_config.WarmupTime);

            if (!IsThrow) Throwing();
            else if (!Chromosome.Evaluated || (Time.time > (m_startTime + m_config.TestDuration)))
            {
                Debug.Log("Stop Evaluation Time");
                StopEvaluation();
            }
        }

        private void SetBallColor(Color c)
        {
            GetComponent<MeshRenderer>().material.color = c;
        }
    }
}