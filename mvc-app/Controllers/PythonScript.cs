// using Python.Runtime;
// using System;
// using System.Diagnostics;

// namespace mvc_app.Controllers
// {
//     // public class PythonScript
//     // {
//     //     Runtime.PythonDLL = ""
//     // }

//     private void RunCMD(string cmd, string args)
//     {
//         var start = new ProcessStartInfo();
//         start.FileName = "C:/Users/PhuTuan/anaconda3/envs/pythonProject/python.exe";
//         start.Arguments = string.Format("{0} {1} {2}", cmd, args);
//         start.UseShellExecute = false;
//         start.RedirectStandardOutput = true;
//         using(Process process = Process.Start(start))
//         {
//             using(StreamReader reader = process.StandardOutput)
//             {
//                 string result = reader.ReadToEnd();
//                 Console.Write(result);
//             }
//         }
//     }
// }