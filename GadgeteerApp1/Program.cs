using Gadgeteer.Modules.GHIElectronics;
using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Presentation.Shapes;
using Microsoft.SPOT.Touch;

using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Microsoft.SPOT.Net.NetworkInformation;
using GHI.Networking;

using Microsoft.SPOT.Hardware;


namespace GadgeteerApp1
{
    public partial class Program
    {
        // Bitmap currentBitmapData;
        //GT.Picture currentPicture;
        // Font font = Resources.GetFont(Resources.FontResources.NinaB);
        // public delegate string Analysis(string str);
        //event Analysis imageAnalysis;
        //WebEvent pictureTaken;
        private MyServer movementserver;
        private Status newStatus, currentStatus;
        // This method is run when the mainboard is powered up or reset.   
        void ProgramStarted()
        {           
            // MyDataGrid key = new MyDataGrid();

            // pictureTaken = WebServer(8080);
            movementserver.DataReceived += Movementserver_DataReceived;
            wifiRS21.NetworkDown += new GTM.Module.NetworkModule.NetworkEventHandler(wifiNetworkDown);
            wifiRS21.NetworkUp += new GTM.Module.NetworkModule.NetworkEventHandler(wifiNetworkUp);
            NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAvailabilityChanged;
            NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;
            //pictureTaken.WebEventReceived += new WebEvent.ReceivedWebEventHandler(pictureTaken_WebEventReceived);
            camera2.CameraConnected += Camera2_CameraConnected;
            camera2.PictureCaptured += camera_Stream;

            init_Wifi();

            GT.Timer timer = new GT.Timer(500);
           // GT.Timer motor = new GT.Timer(200);
            timer.Tick += TakePhoto;
            timer.Tick += CheckPosition;
            displayTE35.BacklightEnabled = true;
            multicolorLED.TurnWhite();
            multicolorLED2.TurnWhite();
            timer.Start();
            //motor.Start();
            //displayTE35.SimpleGraphics.DisplayText("Hello World", font, Gadgeteer.Color.Orange, 50, 50);
            Debug.Print("Program Started");
        }

        private void Movementserver_DataReceived(object sender, DataReceivedEventArgs e)
        {
            string command = MyServer.BytesToString(e.Data);
            this.CheckCommand(command);
        }
        private enum Status
        {
            Idle, MoveForward, MoveBackward, TurnRight, TurnLeft, RotateLeft,RotateRight
        }
        private void CheckPosition(GT.Timer timer)
        {
            if (newStatus != currentStatus)
                Move();
        }

        private void CheckCommand(string command)
        {
            switch (command.ToString().ToUpper())
            {
                case "FW":
                    newStatus = Status.MoveForward;
                    break;

                case "BW":
                    newStatus = Status.MoveBackward;
                    break;

                case "ST":
                    newStatus = Status.Idle;
                    break;

                case "RL":
                    newStatus = Status.RotateLeft;
                    break;

                case "RR":
                    newStatus = Status.RotateRight;
                    break;

                case "TR":
                    newStatus = Status.TurnRight;
                    break;

                case "TL":
                    newStatus = Status.TurnLeft;
                    break;

                default:
                    break;
            }

        }

        private void Move(Joystick sender)
        {
            Joystick.Position pos = new Joystick.Position();
            pos = sender.GetPosition();
            //Debug.Print("x="+sender.GetPosition().X.ToString());
            //Debug.Print("y=" + sender.GetPosition().Y.ToString());
            if (pos.X > 0.7)            // go front
            {
                motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor1, -0.9);
                motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor2, -0.9);
                motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor1, -0.55);
                motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor2, -0.6);
            }
            else if (pos.X < -0.7)      // go back
            {
                motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor1, 0.9);
                motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor2, 0.9);
                motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor1, 0.55);
                motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor2, 0.6);
            }
            else if (pos.Y < -0.7)      // rotate right
            {
                motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor1, -0.9);
                motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor2, 0.9);
                motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor1, -0.55);
                motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor2, 0.6);
            }
            else if (pos.Y > 0.7)       // rotate left
            {
                motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor1, 0.9);
                motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor2, -0.9);
                motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor1, 0.55);
                motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor2, -0.6);
            }
           /* else if (pos.Y > 0.5 && pos.X > 0.2)    // turn right front
            {
                motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor2, 0.0);
                //motorDriverL298.StopAll();
                motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor1, 0.9);
                motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor1, 0.6);
            }
            else if (pos.Y > 0.5 && pos.X < -0.2)   // turn right back
            {
                motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor2, 0.0);
                //motorDriverL298.StopAll();
                motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor1, -0.9);
                motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor1, -0.6);
            }
            else if (pos.Y < -0.5 && pos.X > 0.2)    // turn left front
            {
                motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor1, 0.1);
                motorDriverL298.StopAll();
                motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor2, 0.9);
                motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor2, 0.7);
            }
            else if (pos.Y < -0.5 && pos.X < -0.2)     // turn left back
            {
                motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor1, 0.1);
                motorDriverL298.StopAll();
                motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor2, -0.9);
                motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor2, -0.7);
            }*/
            else
            {
                motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor1, 0.1);
                motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor2, 0.1);
                motorDriverL298.StopAll();
            }
        }
        private void Move()
        {

            //Debug.Print("x="+sender.GetPosition().X.ToString());
            //Debug.Print("y=" + sender.GetPosition().Y.ToString());
            switch (newStatus) {
                case Status.MoveForward:            // go front
            
                    motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor1, -0.9);
                    motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor2, -0.9);
                    motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor1, -0.55);
                    motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor2, -0.6);
                    break;

                case Status.MoveBackward:      // go back
            
                    motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor1, 0.9);
                    motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor2, 0.9);
                    motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor1, 0.55);
                    motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor2, 0.6);
                    break;

                case Status.RotateRight:      // rotate right
            
                    motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor1, -0.9);
                    motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor2, 0.9);
                    motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor1, -0.55);
                    motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor2, 0.6);
                    break;
                case Status.RotateLeft:           // rotate left
            
                    motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor1, 0.9);
                    motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor2, -0.9);
                    motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor1, 0.55);
                    motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor2, -0.6);
                    break;

                case Status.Idle:
                    motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor1, 0.1);
                    motorDriverL298.SetSpeed(MotorDriverL298.Motor.Motor2, 0.1);
                    motorDriverL298.StopAll();
                    break;

                default:
                    break;
            }
        }

        /* private void pictureTaken_WebEventReceived(string path, WebServer.HttpMethod method, Responder responder)
         {
             if (currentPicture != null)
                 responder.Respond(currentPicture);
         }*/

        private static void NetworkChange_NetworkAddressChanged(object sender, Microsoft.SPOT.EventArgs e)
        {
            Debug.Print("Network address changed");
        }

        private static void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            Debug.Print("Network availability: " + e.IsAvailable.ToString());
        }

        private void init_Wifi()
        {
            /* string NetworkName = "TISCALI";
             string key = "HCB2cbjytY" + '"' + "tu7bhjc:K";
             string IpAddress = "192.168.1.120";
             string gateway = "192.168.1.254";
             Debug.Print("Scan for wireless networks");
             GHI.Networking.WiFiRS9110.NetworkParameters info = new GHI.Networking.WiFiRS9110.NetworkParameters();
             info.Ssid = NetworkName;
             info.Key = key;
             info.SecurityMode = WiFiRS9110.SecurityMode.Wpa2;

             wifiRS21.NetworkInterface.Open();
             if (wifiRS21.NetworkInterface.IsDhcpEnabled)
                 wifiRS21.UseStaticIP(IpAddress,"255.255.255.0", gateway);
             wifiRS21.NetworkInterface.Join(NetworkName,key);*/
            string NetworkName = "FezNetwork";
            string key = "FezSpider";
            string IpAddress = "192.168.1.120";
            string gateway = "192.168.1.254";
            GHI.Networking.WiFiRS9110.NetworkParameters info = new GHI.Networking.WiFiRS9110.NetworkParameters();
            info.Channel = 2;
            info.Ssid = NetworkName;
            info.Key = key;
            info.SecurityMode = WiFiRS9110.SecurityMode.Open;
            info.NetworkType = WiFiRS9110.NetworkType.AdHoc;

            wifiRS21.NetworkInterface.EnableDhcp();
            wifiRS21.NetworkInterface.Open();
            //if (wifiRS21.NetworkInterface.IsDhcpEnabled)
              //  wifiRS21.UseStaticIP(IpAddress, "255.255.255.0", gateway);
            wifiRS21.NetworkInterface.StartAdHocNetwork(info);

            
            //GHI.Networking.WiFiRS9110.NetworkParameters network = new GHI.Networking.WiFiRS9110.NetworkParameters(); 
            //GHI.Networking.WiFiRS9110.NetworkParameters[] info = wifiRS21.NetworkInterface.Scan(NetworkName);

            //wifiRS21.NetworkInterface.StartAdHocNetwork(network);
        }

        private void TakePhoto(GT.Timer timer)
            {
                  if(camera2.CameraReady)
                    camera2.TakePicture();            
            }
        
        private void Camera2_CameraConnected(Camera sender, EventArgs e)
        {
            camera2.CurrentPictureResolution = Camera.PictureResolution.Resolution160x120;
            //camera2.StartStreaming();       
        }

       /* void camera_Stream(Camera sender, Bitmap bitmap)
          {          
            displayTE35.SimpleGraphics.DisplayImage(bitmap, 80, 60);
            currentBitmapData = bitmap;
            //WebServer.HttpMethod.POST;
            //currentBitmapData.GetBitmap();
            //image_Analysis();
          }*/

        void camera_Stream(Camera sender, Gadgeteer.Picture picture)
        {
            displayTE35.SimpleGraphics.DisplayImage(picture, 80, 60);
        }

        void wifiNetworkDown(GTM.Module.NetworkModule sender, GTM.Module.NetworkModule.NetworkState state)
        {
            Debug.Print("Network is down");
        }

        void wifiNetworkUp(GTM.Module.NetworkModule sender, GTM.Module.NetworkModule.NetworkState state)
        {
            Debug.Print("Network is up");
            movementserver.Start();
            // Debug.Print("IP Address: " + wifiRS21.NetworkSettings.IPAddress.ToString());
            //Debug.Print("Subnet Mask: " + wifiRS21.NetworkSettings.SubnetMask.ToString());
            //Debug.Print("Gateway: " + wifiRS21.NetworkSettings.GatewayAddress.ToString());
            //Debug.Print("DNS Server: " + wifiRS21.NetworkSettings.DnsAddresses[0].ToString());
            WebServer.StartLocalServer("192.168.1.115", 8081);          
        }
        
      /*  void image_Analysis()
        {
            byte[] bytes = new byte[currentBitmapData.Width * currentBitmapData.Height * 3 + 54];

        }*/       
    }
}
