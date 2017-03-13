using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Matrix;

namespace Guppy_Simple
{
    public partial class Form1 : Form
    {
        Random rnd = new Random();

        Guppy[] fish = new Guppy[100];
        Food[] food = new Food[1];
        Bomb[] bomb = new Bomb[1];

        Bitmap scrn = new Bitmap(1382, 784);
        Graphics graphicObj;
        Pen penObj = new Pen(Color.White, 2);

        int countdown = 0;
        int generation = 1;

        int selectedGuppy = -1;
        int selectedMother = -1;
        int selectedFather = -1;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            graphicObj = Graphics.FromImage(scrn);
            graphicObj.Clear(Color.White);

            Random rnd = new Random();

            for (int i = 0; i < fish.Length; i++)
            {
                fish[i] = new Guppy(800, 425);
            }

            for (int i = 0; i < food.Length; i++)
			{
                food[i] = new Food(rnd.Next(Constants.tankLeft, Constants.tankRight), rnd.Next(Constants.tankBottom, Constants.tankTop));
			}

            for (int i = 0; i < bomb.Length; i++)
            {
                bomb[i] = new Bomb(rnd.Next(Constants.tankLeft, Constants.tankRight), rnd.Next(Constants.tankBottom, Constants.tankTop));
            }

            this.WindowState = FormWindowState.Maximized;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            graphicObj.Clear(Color.Black);

            for (int i = 0; i < fish.Length; i++)
            {
                fish[i].draw(graphicObj, penObj, selectedGuppy, i, chkRelatives.Checked, selectedFather, selectedMother);
            }

            for (int i = 0; i < food.Length; i++)
            {
                food[i].draw(graphicObj, penObj);
            }

            
            for (int i = 0; i < bomb.Length; i++)
            {
                //bomb[i].draw(graphicObj, penObj);
            }
            

            e.Graphics.DrawImage(scrn, 140, 0);
        }

        private void tmrTick_Tick(object sender, EventArgs e)
        {
            //Step all guppies forward by one step
            for (int i = 0; i < fish.Length; i++)
            {
                fish[i].nerveImpulse(ref food[0], ref bomb[0]);
            }
            
            //Countdown until next generation
            if (countdown < numUDGenTime.Value)
            {
                countdown++; //Countdown (or up to Generation Time)
            }
            else //Once time has run out
            {
                fish = reproduce(fish); //Reproduce the fish

                //Reset all the selection
                selectedMother = -1;
                selectedGuppy = -1;
                selectedFather = -1;

                countdown = 0; //Reset countdown timers
                generation++; //Increment the generation counter
            }

            Invalidate(); //Draw everything (Form1_Paint)

            #region Display Info

            lblInfo.Text = "Generation: " + generation + "\nTime left: " + (numUDGenTime.Value - countdown);

            if (selectedGuppy != -1)
            {
                lblGupInfo.Text = "Guppy Number: " + selectedGuppy + "\nNumber of pellets eaten: " + fish[selectedGuppy].numEaten + "\nX position: " + fish[selectedGuppy].posX + "\nY position: " + fish[selectedGuppy].posY;

                StringBuilder strBrainBuilder = new StringBuilder("Brain Coefficients:\r\n");

                for (int i = 0; i < Constants.numHidden; i++)
                {
                    strBrainBuilder.Append("HB " + i + ": " + Math.Round(fish[selectedGuppy].brain.bHid[i, 0],7) + "\r\n");
                }
                for (int i = 0; i < Constants.numOutput; i++)
                {
                    strBrainBuilder.Append("OB " + i + ": " + Math.Round(fish[selectedGuppy].brain.bOut[i, 0],7) + "\r\n");
                }
                for (int i = 0; i < Constants.numInput; i++)
                {
                    for (int j = 0; j < Constants.numHidden; j++)
                    {
                        strBrainBuilder.Append("IHW " + i + "-" + j + ": " + Math.Round(fish[selectedGuppy].brain.wHid[j, i],5) + "\r\n");
                    }
                }
                for (int i = 0; i < Constants.numOutput; i++)
                {
                    for (int j = 0; j < Constants.numHidden; j++)
                    {
                        strBrainBuilder.Append("IHW " + j + "-" + i + ": " + Math.Round(fish[selectedGuppy].brain.wHid[j, i], 5) + "\r\n");
                    }
                }

                lblBrainInfo.Text = strBrainBuilder.ToString();
            }

            #endregion
        }

        private static Guppy[] reproduce(Guppy[] parents)
        {
            Guppy[] children = new Guppy[parents.Length];
            Random rnd = new Random();

            for (int i = 0; i < children.Length; i++)
            {
                children[i] = new Guppy(200, 200);
            }

            //Sort parents by numEaten
            Array.Sort(parents,
                delegate(Guppy par1, Guppy par2) { return par1.numEaten.CompareTo(par2.numEaten); });

            List<int> roulette = new List<int>();

            //Keep 20% of the best guppies
            for (int i = children.Length - 1; i > children.Length - children.Length * .2 - 1; i--)
            {
                children[i] = parents[i];

                for (int j = 0; j < parents[i].numEaten; j++)
                {
                    roulette.Add(i);
                }
            }
            
            //For the other 80%, choose two of the best 20% and make a child
            for (int i = 0; i < children.Length - children.Length * .2; i++)
            {
                //Randomly choose parents from the top 20%
                //int randFatherIndex = rnd.Next(Convert.ToInt32(children.Length - children.Length * .2), Convert.ToInt32(children.Length));
                //int randMotherIndex = rnd.Next(Convert.ToInt32(children.Length - children.Length * .2), Convert.ToInt32(children.Length));

                int randFatherIndex = roulette.ElementAt(rnd.Next(0, roulette.Count));
                int randMotherIndex = roulette.ElementAt(rnd.Next(0, roulette.Count));

                //Save the mother and father indexes
                children[i].fatherIndex = randFatherIndex;
                children[i].motherIndex = randMotherIndex;

                //Create a genome for the child from the genomes of the parents
                Genome childGenome = parents[randFatherIndex]
                    .DNA.offspring(parents[randMotherIndex].DNA, .25, .05);

                //Create a brain for the child from its genome
                Network brain = new Network(Constants.numInput, Constants.numHidden, Constants.numOutput, childGenome);

                children[i].DNA = childGenome;
                children[i].brain = brain;
            }

            //Reset all positions and numEatens

            for (int i = 0; i < children.Length; i++)
            {
                children[i].numEaten = 0;
                //children[i].posX = rnd.Next(Constants.tankLeft, Constants.tankRight);
                //children[i].posY = rnd.Next(Constants.tankBottom, Constants.tankTop);

                children[i].posX = 800;
                children[i].posY = 400;
            }


            return children;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < fish.Length; i++)
            {
                //If fish is clicked
                if ((fish[i].posX - e.X + 140) * (fish[i].posX - e.X + 140) + (fish[i].posY - e.Y) * (fish[i].posY - e.Y) <= Guppy.radius * Guppy.radius)
                {
                    lblGupInfo.Text = "Guppy Number: " + i + "\nNumber of pellets eaten: " + fish[i].numEaten + "\nX position: " + fish[i].posX + "\nY position: " + fish[i].posY;
                    selectedGuppy = i;
                    selectedFather = fish[i].fatherIndex;
                    selectedMother = fish[i].motherIndex;
                    break;
                }
                //If blank space is clicked (Deselect)
                else
                {
                    lblGupInfo.Text = "Guppy Number: " + "\nNumber of pellets eaten: " + "\nX position: " + "\nY position: ";
                    lblBrainInfo.Text = "Brain Coefficients: ";
                    selectedGuppy = -1;
                    selectedMother = -2;
                    selectedFather = -2;
                }
            }
        }

        private void btnTogglePause_Click(object sender, EventArgs e)
        {
            tmrTick.Enabled = true;
            btnTogglePause.Text = "pause";
        }
    }

    class Guppy
    {
        #region Movement Properties

        public double posX { get; set; }
        public double posY { get; set; }

        public const double velMax = 5;
        public const double radius = 10;

        public int fatherIndex { get; set; }
        public int motherIndex { get; set; }

        public int numEaten { get; set; }

        #endregion

        //Inputs: x dist from food, y dist from food
        //Outputs: x vel, y vel

        public Genome DNA { get; set; }

        public Network brain { get; set; }

        /// <summary>
        /// Advances the guppy forward in time by dt, given input acceleration
        /// </summary>
        /// <param name="dt">Change in time</param>
        /// <param name="accel">Linear Acceleartion</param>
        /// <param name="alpha">Rotational acceleration</param>
        public void step(double dt, double velX, double velY)
        {
            velX = 20 * (velX - .5); //VelX is -1 - 1, normalize VelX
            velY = 20 * (velY - .5); //Normalize VelY

            if (posX + velX * dt < Constants.tankRight  && posX + velX * dt > Constants.tankLeft) //If guppy is not at side of tank
            {
                posX += velX * dt; //Change X position
            }

            if (posY + velY * dt < Constants.tankTop && posY + velY * dt > Constants.tankBottom) //If guppy is not at side of tank
            {
                posY += velY * dt; //Change Y position
            }
        }

        public void nerveImpulse(ref Food food, ref Bomb bomb)
        {
            double[,] input = new double[Constants.numInput, 1];

            Random rnd = new Random();

            //Input is difference between guppy position and food position
            input[0, 0] = -posX + food.posX;
            input[1, 0] = -posY + food.posY;
            input[2, 0] = (-posY + bomb.posY) / 10;
            input[3, 0] = (-posY + bomb.posY) / 10;

            brain.feedforward(input); //NerveImpulse, take inputs and give outputs

            step(1, brain.output[0, 0], brain.output[1, 0]); //Step forward by dt, with output speeds from the brain

            //If the guppy is touching the food
            if ((posX - food.posX) * (posX - food.posX) + (posY - food.posY) * (posY - food.posY) <= (food.radius + Guppy.radius) * (food.radius + Guppy.radius))
            {
                //Put food in a new random place
                food.posX = Convert.ToDouble(rnd.Next(1, Constants.tankRight));
                food.posY = Convert.ToDouble(rnd.Next(1, Constants.tankTop));

                //Increment numEaten for this guppy by one
                numEaten++;
            }

            /*
            if ((posX - bomb.posX) * (posX - bomb.posX) + (posY - bomb.posY) * (posY - bomb.posY) <= (bomb.radius + Guppy.radius) * (bomb.radius + Guppy.radius))
            {
                bomb.posX = Convert.ToDouble(rnd.Next(1, Constants.tankRight));
                bomb.posY = Convert.ToDouble(rnd.Next(1, Constants.tankTop));

                numEaten--;
            }
            */
        }

        public Guppy(double x, double y)
        {
            //Set starting position
            posX = x;
            posY = y;

            //Make a new genome for the guppy
            DNA = new Genome((Constants.numHidden * (Constants.numInput + Constants.numOutput + 1) + Constants.numOutput + 1) * 16); //273 * 16

            //Make a new brain for the guppy using the genome
            brain = new Network(Constants.numInput, Constants.numHidden, Constants.numOutput, DNA);

            numEaten = 0; //Set number of foods eaten counter to 0

            //Start off by setting with no parents
            fatherIndex = -1;
            motherIndex = -1;
        }

        /// <summary>
        /// Draws the guppy given a graphics object and a pen
        /// </summary>
        /// <param name="graphicsObj"></param>
        /// <param name="pen"></param>
        public void draw(Graphics graphicsObj, Pen pen, int selectedGuppy, int guppyIndex, bool showRelative, int inputFatherIndex, int inputMotherIndex)
        {
            //If guppy is the one selected then color it orange
            if (selectedGuppy != guppyIndex) //For all others, color it based on number of food eaten
            {
                //Color is greener the more the guppy has eaten
                pen.Color = Color.FromArgb(Math.Max(255 - Math.Max(numEaten, 0) * 51, 0), 255, Math.Max(255 - Math.Max(numEaten, 0) * 51, 0));
                pen.Width = 2;
            }

            //If show relatives are true
            if (showRelative == true)
            {
                //Show fathers
                if (fatherIndex == inputFatherIndex && inputFatherIndex >= 0)
                {
                    pen.Color = Color.DeepSkyBlue;
                    pen.Width = 4;
                }
                //Show mothers
                if (motherIndex == inputMotherIndex && inputMotherIndex >= 0)
                {
                    pen.Color = Color.HotPink;
                    pen.Width = 4;
                }
            }

            //If this guppy is selected, color it orange
            if (selectedGuppy == guppyIndex)
            {
                pen.Color = Color.Orange;
                pen.Width = 4;
            }

            //Draw body
            graphicsObj.DrawEllipse(pen, Convert.ToInt16(posX - radius),
                Convert.ToInt16(posY - radius),
                Convert.ToInt16(2 * radius),
                Convert.ToInt16(2 * radius));
        }
    }

    class Network
    {
        #region Properties

        /// <summary>
        /// Column matrix of values for inputs neurons
        /// </summary>
        public double[,] input { get; set; }
        /// <summary>
        /// Column matrix of values for hidden neurons
        /// </summary>
        public double[,] hidden { get; set; }
        /// <summary>
        /// Column matrix of values for output neurons
        /// </summary>
        public double[,] output { get; set; }

        /// <summary>
        /// Z values - before sigma - of hidden layer
        /// </summary>
        public double[,] zHid { get; set; }
        /// <summary>
        /// Z values - before sigma - of output layer
        /// </summary>
        public double[,] zOut { get; set; }

        /// <summary>
        /// Matrix of Weights from input to hidden layer.
        /// Format: [Hidden layer neuron, Input layer neuron]
        /// </summary>
        public double[,] wHid { get; set; } //[Hidden, Input]
        /// <summary>
        /// Matrix of Weights from hidden to output layer. 
        /// Format: [Output layer neuron, Hidden layer neuron]
        /// </summary>
        public double[,] wOut { get; set; } //[Output, Hidden]

        /// <summary>
        /// Column matrix of biases for hidden layer
        /// </summary>
        public double[,] bHid { get; set; } //[Hidden, Input]
        /// <summary>
        /// Column matrix of biases for the output layer
        /// </summary>
        public double[,] bOut { get; set; } //[Output, Hidden]

        #endregion

        /// <summary>
        /// Initializes a network with a specified number of neurons for each layer
        /// with random weights and biases
        /// </summary>
        /// <param name="numInp">Number of input neurons</param>
        /// <param name="numHid">Number of hidden neurons</param>
        /// <param name="numOut">Number of output neurons</param>
        public Network(int numInp, int numHid, int numOut, Genome chromosome)
        {
            #region Initializers
            // Set the number of neurons at each level to what the user specifies
            input = new double[numInp, 1];
            hidden = new double[numHid, 1];
            output = new double[numOut, 1];

            //Create biases for the hidden and output layer
            zHid = new double[numHid, 1];
            zOut = new double[numOut, 1];

            //Create weights between every nueron
            wHid = new double[numHid, numInp];
            wOut = new double[numOut, numHid];

            //Create biases for the hidden and output layer
            bHid = new double[numHid, 1];
            bOut = new double[numOut, 1];
            #endregion

            #region Random Initializers

            /*
            System.Security.Cryptography.RNGCryptoServiceProvider prov = new System.Security.Cryptography.RNGCryptoServiceProvider();
            byte[] rand = new byte[1];

            prov.GetBytes(rand);

            Random rnd = new Random(Convert.ToInt16(rand[0]));

            //Four loops below set all weights and biases to a random number between -2.5 and 2.5

            for (int i = 0; i < numHid; i++)
            {
                for (int j = 0; j < numInp; j++)
                {
                    wHid[i, j] = Convert.ToDouble(rnd.Next(-2500, 2500)) / 10000;
                }
            }

            for (int i = 0; i < numOut; i++)
            {
                for (int j = 0; j < numHid; j++)
                {
                    wOut[i, j] = Convert.ToDouble(rnd.Next(-2500, 2500)) / 10000;
                }
            }

            for (int i = 0; i < numHid; i++)
            {
                bHid[i, 0] = Convert.ToDouble(rnd.Next(-2500, 2500)) / 10000;
            }

            for (int i = 0; i < numOut; i++)
            {
                bOut[i, 0] = Convert.ToDouble(rnd.Next(-2500, 2500)) / 10000;
            }


            */
            #endregion

            //Transcribing the genome into the coefficients for the neural net
            #region Transcription

            for (int i = 0; i < bHid.Length; i++)
            {
                bHid[i, 0] = Convert.ToDouble(Convert.ToInt16(chromosome.genes.Substring(i * 16, 16), 2)) / 32768 / 4;
            }

            for (int i = 0; i < bOut.Length; i++)
            {
                bOut[i, 0] = Convert.ToDouble(Convert.ToInt16(chromosome.genes.Substring(i * 16 + 16 * (bHid.Length + 1), 16), 2)) / 32768 / 4;
            }

            for (int i = 0; i < wHid.GetLength(0); i++)
            {
                for (int j = 0; j < wHid.GetLength(1); j++)
                {
                    wHid[i, j] = Convert.ToDouble(Convert.ToInt16(
                        chromosome.genes.Substring((i * wHid.GetLength(1) + j) * 16 + 16 * (bHid.Length + bOut.Length + 1), 16)
                        , 2)) / 32768 / 4;
                }
            }

            for (int i = 0; i < wOut.GetLength(0); i++)
            {
                for (int j = 0; j < wOut.GetLength(1); j++)
                {
                    wOut[i, j] = Convert.ToDouble(Convert.ToInt16(
                        chromosome.genes.Substring((i * wOut.GetLength(1) + j) * 16 + 16 * (bHid.Length + bOut.Length + wHid.GetLength(0) * wHid.GetLength(1) + 1), 16)
                        , 2)) / 32768 / 4;
                }
            }

            #endregion
        }

        /// <summary>
        /// Runs calculations for the network given an input. Fills the hidden layer and output layer.
        /// </summary>
        /// <param name="input">Array of inputs to be put into the input layer</param>
        public void feedforward(double[,] input)
        {
            MatrixOp ops = new MatrixOp();

            #region Number Crunching
            //This loop cycles through each hidden neuron and sums up the input activations going to it
            //multiplied by a weight and then added to a bias -> Sigma (w*a) + b
            for (int cntrHid = 0; cntrHid < this.hidden.Length; cntrHid++) //For each hidden neuron
            {
                double sum = new double(); //Sum of input weights and activations, plus bias

                for (int cntrInp = 0; cntrInp < this.input.Length; cntrInp++) //For each input neuron
                {
                    sum += input[cntrInp, 0] * wHid[cntrHid, cntrInp]; //Add the weights * activations
                }

                zHid[cntrHid, 0] = sum; //Put sum in to Z values for each hidden neuron

                sum += bHid[cntrHid, 0]; //Add the bias of the neuron

                hidden[cntrHid, 0] = 1 / (1 + Math.Pow(2.718281828, -sum)); //Apply the sigma function and put into the hidden neuron layer
            }



            //This loop is the same as the one above, but cycles through the outputs and takes the sum of
            //the hidden neuron activations
            for (int cntrOut = 0; cntrOut < this.output.Length; cntrOut++) //For each output neuron
            {
                double sum = new double();

                for (int cntrHid = 0; cntrHid < this.hidden.Length; cntrHid++)// For each hidden neuron
                {
                    sum += hidden[cntrHid, 0] * wOut[cntrOut, cntrHid]; //Add the weights * activations
                }

                zOut[cntrOut, 0] = sum; //Put sum in to Z values for each output neuron

                sum += bOut[cntrOut, 0]; //Add the bias of the neuron

                output[cntrOut, 0] = 1 / (1 + Math.Pow(2.718281828, -sum)); //Apply the sigma function
            }
            #endregion
        }
    }

    class Genome
    {
        public string genes { get; set; }

        /// <summary>
        /// Create a new random Genome with a ceratin number of base pairs.
        /// </summary>
        /// <param name="numBases"></param>
        public Genome(int numBases)
        {
            #region Create Random Genes

            string strGenes = null;

            System.Security.Cryptography.RNGCryptoServiceProvider prov = new System.Security.Cryptography.RNGCryptoServiceProvider();
            byte[] rand = new byte[1];

            prov.GetBytes(rand);

            Random rnd = new Random(Convert.ToInt32(rand[0]));

            for (int i = 0; i < numBases; i++)
            {
                strGenes += rnd.Next(0, 2);
            }

            genes = strGenes;

            #endregion
        }

        /// <summary>
        /// Create a genome using an existing string of genes
        /// </summary>
        /// <param name="inputGenes"></param>
        public Genome(string inputGenes)
        {
            //Set genes in genome equal to input string
            genes = inputGenes;
        }

        /// <summary>
        /// Given a mate, creates genes (string) for an offspring after a double crossover.
        /// </summary>
        /// <param name="mate"></param>
        /// <param name="start">Start of crossover</param>
        /// <param name="length">Length of crossover</param>
        /// <returns></returns>
        public string dblCross(Genome mate, int start, int length)
        {
            string offspring = null;

            offspring += this.genes.Substring(0, start - 1);
            offspring += mate.genes.Substring(start - 1, length);
            offspring += this.genes.Substring(start + length - 1, genes.Length - start - length + 1);

            return offspring;
        }

        /// <summary>
        /// Given a mate, returns genes (string) of an offspring after a single crossover
        /// </summary>
        /// <param name="mate"></param>
        /// <param name="start">Start of crossover</param>
        /// <returns></returns>
        public string sngCross(Genome mate, int start)
        {
            string offspring = null;

            offspring += this.genes.Substring(0, start - 1);
            offspring += mate.genes.Substring(start - 1, genes.Length - start + 1);

            return offspring;
        }

        /// <summary>
        /// Returns the mutated genes (string) of this Genome's genes
        /// </summary>
        /// <param name="rate">Mutation rate, between 0 and 1</param>
        /// <returns></returns>
        public string mutate(double rate)
        {
            Random rnd = new Random();

            StringBuilder offspring = new StringBuilder(genes);

            for (int i = 0; i < genes.Length; i++)
            {
                if (rnd.Next(0, 10001) <= rate * 10000)
                {
                    if (genes.ElementAt(i) == 49) //Char 49 = '1'
                    {
                        offspring.Insert(i, "0");
                    }
                    else
                    {
                        offspring.Insert(i, "1");
                    }

                    offspring.Remove(i + 1, 1);
                }
            }

            return offspring.ToString();
        }

        /// <summary>
        /// Creates an offspring between another mate
        /// </summary>
        /// <param name="mate"></param>
        /// <param name="dblRate">Rate of double crossover, between 0 and 1 (rate of single crossover = 1 - dblRate)</param>
        /// <param name="mutRate">Rate of mutations, between 0 and 1</param>
        /// <returns></returns>
        public Genome offspring(Genome mate, double dblRate, double mutRate)
        {
            Random rnd = new Random();

            string childsGenes = "";
            childsGenes = genes;

            
            if (rnd.Next(1, 10001) < dblRate * 10000)
            {
                int start = rnd.Next(1, genes.Length);
                childsGenes = dblCross(mate, start, rnd.Next(1, genes.Length - start));
            }
            else
            {
                childsGenes = sngCross(mate, rnd.Next(1, genes.Length));
            }
            

            Genome child = new Genome(childsGenes);

            child.genes = child.mutate(mutRate);

            return child;
        }
    }

    class Food
    {
        public double posX { get; set; }
        public double posY { get; set; }
        public double radius = 5;

        public Food(double x, double y)
        {
            posX = x;
            posY = y;
        }

        public void draw(Graphics graphicsObj, Pen pen)
        {
            pen.Color = Color.Gold;

            //Draw body
            graphicsObj.DrawEllipse(pen, Convert.ToInt16(posX - radius),
                Convert.ToInt16(posY - radius),
                Convert.ToInt16(2 * radius),
                Convert.ToInt16(2 * radius));
        }
    }

    class Bomb
    {
        public double posX { get; set; }
        public double posY { get; set; }

        public double velX { get; set; }
        public double velY { get; set; }

        public const double velMax = 3;
        
        public double radius = 150;

        public Bomb(double x, double y)
        {
            posX = x;
            posY = y;

            System.Security.Cryptography.RNGCryptoServiceProvider prov = new System.Security.Cryptography.RNGCryptoServiceProvider();
            byte[] rand = new byte[1];

            prov.GetBytes(rand);

            Random rnd = new Random(Convert.ToInt32(rand[0]));

            velX = rnd.Next(0, Convert.ToInt32(velMax * 2 + 1)) - velMax;
            velY = rnd.Next(0, Convert.ToInt32(velMax * 2 + 1)) - velMax;
        }

        public void draw(Graphics graphicsObj, Pen pen)
        {
            pen.Color = Color.Red;

            //Draw body
            graphicsObj.DrawEllipse(pen, Convert.ToInt16(posX - radius),
                Convert.ToInt16(posY - radius),
                Convert.ToInt16(2 * radius),
                Convert.ToInt16(2 * radius));
        }
    }

    class Constants
    {
        public const int tankTop = 732;
        public const int tankRight = 1215;
        public const int tankBottom = 9;
        public const int tankLeft = 9;

        public const int numInput = 4;
        public const int numHidden = 30;
        public const int numOutput = 2;
    }
}
