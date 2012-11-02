using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using FreePIE.Core.Contracts;

namespace FreePIE.Core.Plugins
{
    [LuaGlobalType(Type = typeof(DiyHtGlobal))]
    public class DiyHtPlugin : ComDevicePlugin
    {
        public override object CreateGlobal()
        {
            return new DiyHtGlobal(this);
        }

        protected override int DefaultBaudRate
        {
            get { return 57600; }
        }

        protected override void Init(SerialPort serialPort)
        {
            SendCommand(serialPort, "$PLST");
        }

        protected override void Stop(SerialPort serialPort)
        {            
            SendCommand(serialPort, "$PLEN");
        }

        private static void SendCommand(SerialPort serialPort, string command)
        {
            char[] buffer = command.ToCharArray(0, command.Length);
            serialPort.Write(buffer, 0, command.Length);
        }

        protected override void Read(SerialPort serialPort)
        {
            var line = serialPort.ReadLine();
            var data = Data;
            newData = true;
            var values = line.Split(',');
            if (values.Length != 3)
                return;

            data.Roll = ParseFloat(values[0]);
            data.Pitch = ParseFloat(values[1]);            
            data.Yaw = ParseFloat(values[2]);

            Data = data;
            newData = true;
        }

        private float ParseFloat(string value)
        {
            return float.Parse(value, CultureInfo.InvariantCulture);
        }

        protected override string BaudRateHelpText
        {
            get { return "Baud rate, default for the DiyHt library should be 57600"; }
        }

        public override string FriendlyName
        {
            get { return "DIY Headtracker"; }
        }
    }

    [LuaGlobal(Name = "diyHt")]
    public class DiyHtGlobal : DofGlobal<DiyHtPlugin>
    {
        public DiyHtGlobal(DiyHtPlugin plugin) : base(plugin) { }
    }
}
