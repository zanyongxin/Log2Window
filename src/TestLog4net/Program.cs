using System;

using log4net;


// Configure log4net using the .config file
[assembly: log4net.Config.XmlConfigurator(Watch = true)]


namespace Test
{
    using Company.Product.BusinessLogic;
    using Company.Product.ServiceTester;
    using log4net.Appender;
    using log4net.Repository.Hierarchy;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;

    class Program
    {
        // Create a logger for use in this class.
        static readonly ILog _log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo("Config/log4net.config"));

            ////if (!EventLog.SourceExists("log4net"))
            ////{
            ////    //An event log source should not be created and immediately used.
            ////    //There is a latency time to enable the source, it should be created
            ////    //prior to executing the application that uses the source.
            ////    //Execute this sample a second time to use the new source.
            ////    EventLog.CreateEventSource("log4net", "Application");
            ////}

            string cmd = Console.ReadLine();
            while ("" != cmd)
            { 
                DoLog(cmd); 

                DoWinDebug();
                cmd = Console.ReadLine();
            }
        }

        public static void FlushBuffers(ILog log)
        {
            var logger = log.Logger as Logger;
            if (logger != null)
            {
                foreach (IAppender appender in logger.Appenders)
                {
                    try
                    {
                        var buffered = appender as BufferingAppenderSkeleton;
                        if (buffered != null)
                        {
                            buffered.Flush();
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        static void DoWinDebug()
        {
            //Console.WriteLine("Begin Doing WinDebug!");
            //System.Diagnostics.Debug.WriteLine("This is a call to System.Diagnostics.Debug");
            //System.Diagnostics.Trace.WriteLine("This is a call to System.Diagnostics.Trace");
            //Console.WriteLine("End Doing WinDebug!");
        }

        static void DoLog(string cmd)
        {
            Console.WriteLine("\nBegin Doing Log!");
            Console.WriteLine(DateTime.Now);
            if (cmd.ToLower() == "b")
            {
                for (int i = 0; i < 1000000; i++)
                {
                    _log.Info(i);
                }
            }
            if (cmd.ToLower() == "b2")
            {
                string data = File.ReadAllText(@"d:\Projects\github\Log2Window\src\TestLog4net\testDatas.txt");
                for (int i = 0; i < 100; i++)
                {
                    _log.Info(i+" "+ data);
                }
            }
            else if (cmd.ToLower() == "t")
            {
                for (int i = 0; i < 10; i++)
                {
                    var log = LogManager.GetLogger("Test.Program.t" + i);
                    ThreadPool.QueueUserWorkItem(delegate (object ob)
                    {
                        for (int j = 0; j < 10000; j++)
                        {
                            (ob as ILog).Info(j);
                        }
                    }, log);
                }
            }
            else if (cmd[0] >= '1' && cmd[0] <= '9')
            {
                _log.Info(cmd);
            }
            else if (cmd.ToLower()  == "e")
            {
                try
                {
                    ThrowTestEx();
                    return;
                }
                catch (Exception ex)
                {
                    _log.Error("TestEx", ex);
                }
            }
            else if (cmd.ToLower()  == "c")
            {
                for (int i = 0; i < 10; i++)
                {
                    _log.Info("��������");
                }
            }
            else
            {
                if (_log.IsErrorEnabled)
                    _log.Error("This is an Error...");

                if (_log.IsDebugEnabled)
                    for (int i = 0; i < 10; i++)
                        _log.Debug("This is a simple log!");

                if (_log.IsErrorEnabled)
                    _log.Error("This is an Error...");

                if (_log.IsInfoEnabled)
                    _log.Info("This is an Info...");


                _log.Warn("This is a Warning...");
                _log.Fatal("This is a Fatal...");

                _log.Error("This is an error with an exception.", new Exception("The message exception here."));

                _log.Warn("This is a message on many lines...\nlines...\nlines...\nlines...");
                _log.Warn("This is a message on many lines...\r\nlines...\r\nlines...\r\nlines...");

                DummyManager dm = new DummyManager();
                dm.DoIt();

                DummyTester dt = new DummyTester();
                dt.DoIt();
            }

            Console.WriteLine("End Doing Log!");
        }

        private static void ThrowTestEx()
        {
            throw new Exception("TestEx");
        }
    }
}


namespace Company.Product.BusinessLogic
{

    public class DummyManager
    {
        public static readonly ILog _log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public DummyManager()
        {
            if (_log.IsInfoEnabled)
                _log.Info("Dummy Manager ctor");
        }

        public void DoIt()
        {
            if (_log.IsDebugEnabled)
                _log.Debug("DM: Do It, Do It Now!!");

            _log.Warn("This is a Warning from DM...");
            _log.Error("This is an Error from DM...");
            _log.Fatal("This is a Fatal from DM...");

            _log.Error("This is an error from DM with an exception.", new Exception("The message exception here."));
        }
    }
}

namespace Company.Product.ServiceTester
{

    public class DummyTester
    {
        public static readonly ILog _log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public DummyTester()
        {
            if (_log.IsInfoEnabled)
                _log.Info("Dummy Tester ctor");
        }

        public void DoIt()
        {
            if (_log.IsDebugEnabled)
                _log.Debug("DT: Do It, Do It Now!!");

            _log.Warn("This is a Warning from DT...");
            _log.Error("This is an Error from DT...");
            _log.Fatal("This is a Fatal from DT...");

            _log.Error("This is an error from DT with an exception.", new Exception("The message exception here."));
        }
    }
}


