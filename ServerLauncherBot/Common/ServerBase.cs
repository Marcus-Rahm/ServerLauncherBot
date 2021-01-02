using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ServerLauncherBot.Common
{
    public abstract class ServerBase
    {
        private Process _process;
        private string _serverFile;
        private string _arguments;
        public ServerBase(string serverFile, string arguments)
        {
            _serverFile = serverFile;
            _arguments = arguments;
        }

        public void Run(Process process = null)
        {
            if(process == null)
            {
                var startInfo = CreateProcessInfo();
                process = Process.Start(startInfo);
            }

            process.OutputDataReceived += OutputHandler;
            process.ErrorDataReceived += ErrorHandler;

            // Start the asynchronous read of the standard output stream.
            process.BeginOutputReadLine();

            // Start the asynchronous read of the standard error stream.
            process.BeginErrorReadLine();

            process.WaitForExit();
        }

        private ProcessStartInfo CreateProcessInfo()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(_serverFile, _arguments);
            startInfo.ErrorDialog = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;

            return startInfo;
        }

        private void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {

        }

        private void ErrorHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {

        }
    }
}
