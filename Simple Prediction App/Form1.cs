using System;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;


namespace Simple_Prediction_App
{
    public partial class Form1 : Form
    {
        private const string appName = "Simple Predictor";
        private readonly string predictionsFile = $"{Environment.CurrentDirectory}\\predictionConfig.json";
        private string[] predictions;
        private static Random rand = new Random();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            splitContainer1.IsSplitterFixed = true;
            this.Text = appName;

            try
            {
                var data = File.ReadAllText(predictionsFile, Encoding.UTF8);
                predictions = JsonConvert.DeserializeObject<string[]>(data);
                MessageBox.Show($"{predictions.Length}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (predictions == null)
                {
                    this.Close();
                }
                else if (predictions.Length == 0)
                {
                    MessageBox.Show("No Predictions");

                }
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            bPredict.Enabled = false;
            for (int i = 0; i <= progressBar1.Maximum; i++)
            {
                this.Invoke(new Action(() =>
                {
                    UpdateProgressBar(i);
                    this.Text = $"{i}%";
                }));
                await Task.Delay(20);
            }

            int index = RandomNPredictionNumber();

            string prediction = predictions[index];

            MessageBox.Show($"{prediction}!");

            this.Text = appName;
            progressBar1.Value = 0;
            bPredict.Enabled = true;
        }

        private int RandomNPredictionNumber()
        {
            return rand.Next(predictions.Length);
        }
        private void UpdateProgressBar(int i)
        {
            if (i == progressBar1.Maximum)
            {
                progressBar1.Maximum = i + 1;
                progressBar1.Value = i + 1;
                progressBar1.Maximum = i;
            }
            else { progressBar1.Value = i + 1; }
            progressBar1.Value = i;
        }
    }
}
