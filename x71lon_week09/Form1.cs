using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using x71lon_week09.Entities;
using System.IO;

namespace x71lon_week09
{
    public partial class Form1 : Form
    {
        List<Person> Population = new List<Person>();
        List<BirthProbability> BirthProbabilities = new List<BirthProbability>();
        List<DeathProbability> DeathProbabilities = new List<DeathProbability>();
        Random rng = new Random(1234);
        int[] numberOfMales;
        int[] numberOfFemales;
        public Form1()
        {
            InitializeComponent();
            Population = GetPopulation(@"C:\Users\DavidLazar\Downloads\nép.csv");
            BirthProbabilities = GetBirthProbabilities(@"C:\Users\DavidLazar\Downloads\születés.csv");
            DeathProbabilities = GetDeathProbabilities(@"C:\Users\DavidLazar\Downloads\halál.csv");
        }
        private void SimStep(int year, Person person)
        {
            if (!person.IsAlive) return;
            byte age = (byte)(year - person.BirthYear);
            double pDeath = (from x in DeathProbabilities
                             where x.Gender == person.Gender && x.Age == age
                             select x.P).FirstOrDefault();
            if (rng.NextDouble() <= pDeath)
                person.IsAlive = false;
            if (person.IsAlive && person.Gender == Gender.Female)
            {
                double pBirth = (from x in BirthProbabilities
                                 where x.Age == age
                                 select x.P).FirstOrDefault();
                if (rng.NextDouble() <= pBirth)
                {
                    Person újszülött = new Person();
                    újszülött.BirthYear = year;
                    újszülött.NbrOfChildren = 0;
                    újszülött.Gender = (Gender)(rng.Next(1, 3));
                    Population.Add(újszülött);
                }
            }
        }
        public List<Person> GetPopulation(string csvpath)
        {
            List<Person> population = new List<Person>();
            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    population.Add(new Person()
                    {
                        BirthYear = int.Parse(line[0]),
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[1]),
                        NbrOfChildren = int.Parse(line[2])
                    });
                }
            }
            return population;
        }
        public List<BirthProbability> GetBirthProbabilities(string csvpath)
        {
            List<BirthProbability> birth = new List<BirthProbability>();
            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    birth.Add(new BirthProbability()
                    {
                        Age = int.Parse(line[0]),
                        NbrOfChildren = int.Parse(line[1]),
                        P = double.Parse(line[2])
                    });
                }
            }
            return birth;
        }
        public List<DeathProbability> GetDeathProbabilities(string csvpath)
        {
            List<DeathProbability> death = new List<DeathProbability>();
            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    death.Add(new DeathProbability()
                    {
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[0]),
                        Age = int.Parse(line[1]),
                        P = double.Parse(line[2])
                    });
                }
            }
            return death;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if(openFile.ShowDialog()== DialogResult.OK)
                textBox1.Text = openFile.FileName;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            Array.Clear(numberOfMales, 0, numberOfMales.Length);
            Array.Clear(numberOfFemales, 0, numberOfFemales.Length);
            Simulation();
            DisplayResults();
        }
        public void Simulation()
        {
            for (int year = 2005; year <= 2024; year++)
            {
                for (int i = 0; i < Population.Count; i++)
                {
                    SimStep(year, Population[i]);
                    numberOfMales[i] = (from x in Population
                                     where x.Gender == Gender.Male && x.IsAlive
                                     select x).Count();
                    numberOfFemales[i] = (from x in Population
                                       where x.Gender == Gender.Female && x.IsAlive
                                       select x).Count();
                }
                
                //Console.WriteLine(
                    //string.Format("Év:{0} Fiúk:{1} Lányok:{2}", year, nbrOfMales, nbrOfFemales));
            }
        }
        public void DisplayResults()
        {
            for (int year = 2005; year < numericUpDown1.Value; year++)
            {
                int i = 0;
                richTextBox1.Text += "Szimuálciós év:" + year + "\n\t Fiúk:" + numberOfMales[i] + "\n\t Lányok:" + numberOfFemales[i] + "\n\t";
                i++;
            }
        }
    }
}
