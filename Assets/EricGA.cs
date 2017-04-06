using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class EricGA : MonoBehaviour
{
    static public void swap<T>(int index1, int index2, List<T> myList)
    {
        var temp = myList[index1];
        myList[index1] = myList[index2];
        myList[index2] = temp;
    }

    public class Chromosome
    {
        public List<Spot> spots;

        public Chromosome(Chromosome chromosome)
        {
            this.spots = new List<Spot>(chromosome.spots);
        }

        public Chromosome(List<Spot> _spots){
            spots = new List<Spot>(_spots);
            
            for (int i = 1; i < spots.Count; ++i)
            {
                var target = Random.Range(i, spots.Count);
                swap<Spot>(target, i, spots);
            }
        }

        public float fitnessValue()
        {
            float sumOfDistance = 0.0f;
            for (int i = 0; i < spots.Count; ++i)
            {
                sumOfDistance += (spots[i].transform.position - spots[(i + 1) % spots.Count].transform.position).magnitude;
            }
            return 100.0f / (sumOfDistance + 1);
        }
    }

    public EricLineHandler lineHandler;
    public GameObject spotsParrent;
    public Spot spot;
    public List<Spot> spots = new List<Spot>();
    public List<Chromosome> chromosomes = new List<Chromosome>();

    float CrossoverRate = 0.8f, MutationRate = 0.2f;
    int countOfGeneration = 1000, countOfChromosome = 100;

    void Start () {
        lineHandler = Instantiate(lineHandler.gameObject).gameObject.GetComponent<EricLineHandler>();
    }

    public void addSpot()
    {
        lineHandler.clearLines(EricLineHandler.LINE_TYPE.STATIC);
        Spot sp = Instantiate(spot);
        sp.spotID = spots.Count;
        sp.transform.position = new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f), 0);
        sp.transform.SetParent(spotsParrent.transform);
        spots.Add(sp);
    }

    public void algorithm() {
        initChromosome(countOfChromosome);

        for(int i = 0; i < countOfGeneration; i++)
        {
            //printGeneration(chromosomes, i);
            List<Chromosome> newGeneration = new List<Chromosome>();
            for (int j = 0; j < chromosomes.Count; j+=2)
            {
                Chromosome target1 = new Chromosome(chromosomes[selection()]);
                Chromosome target2 = new Chromosome(chromosomes[selection()]);

                if (CrossoverRate > Random.Range(0.0f, 1.0f))
                {
                    Crossover(target1, target2);
                }
                if (MutationRate > Random.Range(0.0f, 1.0f))
                {
                    Mutation(target1);
                }
                if (MutationRate > Random.Range(0.0f, 1.0f))
                {
                    Mutation(target2);
                }
                newGeneration.Add(target1);
                newGeneration.Add(target2);
            }
            chromosomes = newGeneration;
        }

        lineHandler.clearLines(EricLineHandler.LINE_TYPE.STATIC);
        Chromosome chromosome = chromosomes[selection()];
        List<Vector3> line = new List<Vector3>();

        for (int i = 0; i < chromosome.spots.Count; i++)
        {
            line.Add(chromosome.spots[i].transform.position);
        }
        line.Add(chromosome.spots[0].transform.position);
        lineHandler.drawPointsList(line, EricLineHandler.LINE_TYPE.STATIC, lineHandler.getBallColor(0));
    }

    void initChromosome(int countOfChromosome) {
        chromosomes.Clear();
        for (int i = 0; i < countOfChromosome; i++)
            chromosomes.Add(new Chromosome(spots));
    }

    protected int selection()
    {
        float sum = 0.0f;
        List<float> wheel = new List<float>();
        wheel.Add(sum);//add 0.0f
        for (int i = 0; i < chromosomes.Count; ++i)
        {
            sum += chromosomes[i].fitnessValue();
            wheel.Add(sum);
        }
        float random = Random.Range(0.0f, sum);
        for (int index = 0; index < wheel.Count - 1; ++index)
        {
            if (random <= wheel[index + 1] && random > wheel[index])
                return index;
        }
        return 0;
    }

    public void Crossover(Chromosome item1, Chromosome item2)
    {
        int min = Random.Range(0, item1.spots.Count);
        int max = Random.Range(min, item1.spots.Count);

        List<Spot> rangeOfItem1 = item1.spots.GetRange(min, max - min + 1);
        List<Spot> rangeOfItem2 = item2.spots.GetRange(min, max - min + 1);
        
        foreach (Spot item in rangeOfItem2)
            item1.spots.Remove(item);

        foreach (Spot item in rangeOfItem1)
            item2.spots.Remove(item);
        
        item1.spots.InsertRange(min, rangeOfItem2);
        item2.spots.InsertRange(min, rangeOfItem1);
    }

    public void Mutation(Chromosome item)
    {
        int min = Random.Range(1, item.spots.Count);
        int max = Random.Range(min, item.spots.Count);

        swap<Spot>(min, max, item.spots);
    }

    public void printGeneration(List<Chromosome> generation, int generationIndex)
    {
        string debugMessage = "generation: " + generationIndex + " Total: " + generation.Count + "\n";
        for (int i = 0; i < generation.Count; i++)
        {
            Chromosome chromosome = generation[i];
            debugMessage += "chreomosome" + i + ": ";

            for(int j = 0; j < chromosome.spots.Count; j++)
            {
                debugMessage += chromosome.spots[j].spotID + " ";
            }
            debugMessage += ",fitnessValue: " + chromosome.fitnessValue() + "\n";
        }
        Debug.Log(debugMessage);
    }
}
