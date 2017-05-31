//------------------------------------------------------------------------------
// <auto-generated>
//     Il codice è stato generato da uno strumento.
//     Versione runtime:4.0.30319.42000
//
//     Le modifiche apportate a questo file possono provocare un comportamento non corretto e andranno perse se
//     il codice viene rigenerato.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GadgeteerApp1 {
    using Gadgeteer;
    using GTM = Gadgeteer.Modules;
    
    
    public partial class Program : Gadgeteer.Program {
        
        /// <summary>The USB Client EDP module using socket 1 of the mainboard.</summary>
        private Gadgeteer.Modules.GHIElectronics.USBClientEDP usbClientEDP;
        
        /// <summary>The Camera module using socket 3 of the mainboard.</summary>
        private Gadgeteer.Modules.GHIElectronics.Camera camera2;
        
        /// <summary>The Multicolor LED module using socket 6 of the mainboard.</summary>
        private Gadgeteer.Modules.GHIElectronics.MulticolorLED multicolorLED;
        
        /// <summary>The Multicolor LED module using socket 5 of the mainboard.</summary>
        private Gadgeteer.Modules.GHIElectronics.MulticolorLED multicolorLED2;
        
        /// <summary>The WiFi RS21 module using socket 9 of the mainboard.</summary>
        private Gadgeteer.Modules.GHIElectronics.WiFiRS21 wifiRS21;
        
        /// <summary>The Motor Driver L298 module using socket 11 of the mainboard.</summary>
        private Gadgeteer.Modules.GHIElectronics.MotorDriverL298 motorDriverL298;
        
        /// <summary>The Joystick module using socket 10 of the mainboard.</summary>
        private Gadgeteer.Modules.GHIElectronics.Joystick joystick;


        /// <summary>This property provides access to the Mainboard API. This is normally not necessary for an end user program.</summary>
        protected new static GHIElectronics.Gadgeteer.FEZSpider Mainboard
        {
            get
            {
                return ((GHIElectronics.Gadgeteer.FEZSpider)(Gadgeteer.Program.Mainboard));
            }
            set
            {
                Gadgeteer.Program.Mainboard = value;
            }
        }

        /// <summary>This method runs automatically when the device is powered, and calls ProgramStarted.</summary>
        public static void Main()
        {
            // Important to initialize the Mainboard first
            Program.Mainboard = new GHIElectronics.Gadgeteer.FEZSpider();
            Program p = new Program();
            p.InitializeModules();
            p.ProgramStarted();
            // Starts Dispatcher
            p.Run();
        }

        private void InitializeModules() {
            this.usbClientEDP = new GTM.GHIElectronics.USBClientEDP(1);
            this.camera2 = new GTM.GHIElectronics.Camera(3);
            this.multicolorLED = new GTM.GHIElectronics.MulticolorLED(6);
            this.multicolorLED2 = new GTM.GHIElectronics.MulticolorLED(5);
            this.wifiRS21 = new GTM.GHIElectronics.WiFiRS21(9);
            this.motorDriverL298 = new GTM.GHIElectronics.MotorDriverL298(11);
            this.joystick = new GTM.GHIElectronics.Joystick(10);
        }
    }
}
