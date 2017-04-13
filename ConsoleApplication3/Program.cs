using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyboardInput
{
    class Program
    {
        static void Main(string[] args)
        {
            MyDataGrid data = new MyDataGrid();
            data.ControlAdded += Data_ControlAdded;
            data.KeyDown += Data_KeyDown;
            data.Controls.Add(new Control());
            while (true) ;

        }

        private static void Data_ControlAdded(object sender, ControlEventArgs e)
        {
            Console.WriteLine("Control added");  
        }

        private static void Data_KeyDown (object sender, System.Windows.Forms.KeyEventArgs e)
        {
            KeysConverter conv = new KeysConverter();
            
            Keys key = e.KeyCode;
            switch (key)
            {
                case Keys.Down:
                    Console.WriteLine("Down Arrow Captured");
                    break;

                case Keys.Up:
                    Console.WriteLine("Up Arrow Captured");
                    break;

                case Keys.Left:
                    Console.WriteLine("Left Arrow Captured");
                    break;

                case Keys.Right:
                    Console.WriteLine("Right Arrow Captured");
                    break;

                case Keys.Tab:
                    Console.WriteLine("Tab Key Captured");
                    break;

                case Keys.Control | Keys.M:
                    Console.WriteLine("<CTRL> + m Captured");
                    break;

                case Keys.Alt | Keys.Z:
                    Console.WriteLine("<ALT> + z Captured");
                    break;
            }
        }
    }
}
