using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ProiectSincreticM42
{
    public partial class MainWindow : Window
    {
        private static readonly HttpClient client = new HttpClient();

        private bool liftInInitialPosition = true;
        private bool emergencyStopActivated = false;
        private int currentLevel = 0; 

        public MainWindow()
        {
            InitializeComponent();         
            LiftCanvas.Loaded += (s, e) =>
            {
                SetInitialLiftPosition();
                CenterLift();
                UpdateLights(currentLevel);
            };          
            this.SizeChanged += (s, e) => CenterLift();
        }

        private void SetInitialLiftPosition()
        {
            Canvas.SetTop(Lift, LiftCanvas.ActualHeight - Lift.ActualHeight);
        }

        private void CenterLift()
        {
            double canvasWidth = LiftCanvas.ActualWidth;
            double liftWidth = Lift.ActualWidth;
            double leftPosition = (canvasWidth - liftWidth) / 2;
            Canvas.SetLeft(Lift, leftPosition);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string buttonName = (sender as Button).Name;
            int command = int.Parse(buttonName.Substring(1));
            ProcessCommand(command);
        }

        private void ProcessCommand(int command)
        {
            switch (command)
            {
                case 0:
                                   
                    break;
                case 1:
                    if (liftInInitialPosition && !emergencyStopActivated)
                    {

                        MoveLift(command);
                        liftInInitialPosition = false;
                        SendLiftData(command, $"Deplasare la nivel {command}");
                    }
                    break;
                case 2:
                    if (liftInInitialPosition && !emergencyStopActivated)
                    {

                        MoveLift(command);
                        liftInInitialPosition = false;
                        SendLiftData(command, $"Deplasare la nivel {command}");
                    }
                    break;
                case 3:
                    if (liftInInitialPosition && !emergencyStopActivated)
                    {

                        MoveLift(command);
                        liftInInitialPosition = false;
                        SendLiftData(command, $"Deplasare la nivel {command}");
                    }
                    break;
                case 4:
                    if (liftInInitialPosition && !emergencyStopActivated)
                    {
                        
                        MoveLift(command);
                        liftInInitialPosition = false;
                        SendLiftData(command, $"Deplasare la nivel {command}");
                    }
                    break;
                case 5:
                    if (!liftInInitialPosition)
                    {
                        liftInInitialPosition = true;
                        emergencyStopActivated = false;                       
                        MoveLift(0);
                        SendLiftData(0, "Intoarcere pozitie initiala");
                    }
                    break;
                case 10:
                    liftInInitialPosition = true;
                    SendLiftData(0, "Intoarcere pozitie initiala");
                    break;
            }
        }

        private void UpdateLights(int level)
        {           
            Light0.Fill = System.Windows.Media.Brushes.Red;
            Light1.Fill = System.Windows.Media.Brushes.Red;
            Light2.Fill = System.Windows.Media.Brushes.Red;
            Light3.Fill = System.Windows.Media.Brushes.Red;
            Light4.Fill = System.Windows.Media.Brushes.Red;          
            switch (level)
            {
                case 0:
                    Light0.Fill = System.Windows.Media.Brushes.Green;
                    break;
                case 1:
                    Light1.Fill = System.Windows.Media.Brushes.Green;
                    break;
                case 2:
                    Light2.Fill = System.Windows.Media.Brushes.Green;
                    break;
                case 3:
                    Light3.Fill = System.Windows.Media.Brushes.Green;
                    break;
                case 4:
                    Light4.Fill = System.Windows.Media.Brushes.Green;
                    break;
            }
        }

        private void MoveLift(int level)
        {         
            UpdateLights(-1);
            double liftCanvasHeight = LiftCanvas.ActualHeight;
            double liftHeight = Lift.ActualHeight;
            double levelHeight = liftCanvasHeight / 5;
            double offset = liftCanvasHeight - (level * levelHeight) - liftHeight - 17;
            var animation = new DoubleAnimation
            {
                To = offset,
                Duration = TimeSpan.FromSeconds(2)
            };
            animation.Completed += (s, e) =>
            {
                currentLevel = level;
                UpdateLights(level);
            };

            Storyboard.SetTarget(animation, Lift);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(Canvas.Top)"));
            var storyboard = new Storyboard();
            storyboard.Children.Add(animation);
            storyboard.Begin();
        }

        private async void SendLiftData(int level, string status)
        {
            var liftData = new { Level = level, Status = status };
            var json = JsonSerializer.Serialize(liftData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var response = await client.PostAsync("http://45.55.200.160:5000/api/lift", content); // Asigura-te ca portul este corect
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
            }
        }
    }
}