using Confluent.Kafka;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

using Microsoft.Extensions.Configuration;


namespace Kumagai.Kafka.ProducerPerf
{

    class ArgumentException : Exception{
        public ArgumentException(string message): base(message){
        }
    }

    public class Program
    {
        static readonly Stopwatch s_timerProduce = new Stopwatch();
        static readonly Stopwatch s_timerApp = new Stopwatch();

        static Random s_rand = new Random();
        //static int[] s_outputSpacing;

        private static string GenerateText(int length = 10){
        	// string val = "";
            StringBuilder strBuild = new StringBuilder();  
        	for(int i = 0 ; i < length ; i++ ){
                strBuild.Append(Convert.ToChar(s_rand.Next(33, 126)));
        	}
        	return strBuild.ToString();
        }

        private static Dictionary<string, string> GenerateConfigFromArguments(string[] args){
            Dictionary<string, string> _settings = new Dictionary<string, string>(){
                {"configFilePath", null},
                {"topicName", null},
                {"dataLength", "1024"},
                {"numberOfRecords", "1000"},
                {"outputFilePath", "./output"},
                {"verbose", "0"}
            };

            if (args.Length < 2){
                throw new ArgumentException("arguments config_file_path, topic_name are required.");
            }

            foreach (string arg in args){
                string[] setting = arg.Split('=');
                _settings[setting[0]] = setting[1];
            }

            if(String.IsNullOrEmpty(_settings["configFilePath"]) || String.IsNullOrEmpty(_settings["topicName"])){
                throw new ArgumentException("arguments config_file_path, topic_name are required.");
            }

            return _settings; 
        }

        private static int[] TerminalOutputSpacingFormatter(string[] headers, int margin = 1){
            int[] _spacings = new int[headers.Length];

            for(int i = 0; i < headers.Length; i++){
                _spacings[i] = headers[i].Length + margin * 2;
            }
            return _spacings;
        }

        private static void TerminalOutput(string[] outputs, int[] format, int margin = 1){
            for(int i = 0; i < outputs.Length; i++){
                if (format[i] > (outputs[i].Length + margin * 2)){
                    outputs[i] = outputs[i].PadLeft(format[i] - margin, ' ').PadRight(format[i], ' ');
                } else {
                    outputs[i] = outputs[i].PadLeft(outputs[i].Length + margin, ' ').PadRight(outputs[i].Length + margin * 2, ' ');
                }
            }
            string _output = String.Join("|", outputs);
            Console.WriteLine(_output);
        }

        private static long SumOfListLong(List<long> values){
            long _sum = 0L;
            foreach(long val in values){
                _sum += val;
            }
            return _sum;
        }

        private static int ParcentilePosition(double parcentile, int listCount){
            int _pos = Convert.ToInt32(listCount * parcentile) - 1;
            return _pos;
        }

        public static async Task Main(string[] args)
        {
            Dictionary<string, string> settings;
            try {
                
                settings = GenerateConfigFromArguments(args);

            } catch  (ArgumentException e){
                Console.WriteLine($"Error: {e.Message}");
                Console.WriteLine("Usage: dotnet_producer.exe ... <options> ...");
                Console.WriteLine("Options:");
                Console.WriteLine(" configFilePath=<config_file_path> [required]");
                Console.WriteLine(" topicName=<topic_name> [required]");
                Console.WriteLine(" dataLength=<data_length> [default=1024 int32]");
                Console.WriteLine(" numberOfRecords=<number_of_records> [default=1000 int32]");
                Console.WriteLine(" outputFilePath=<output_file_path> [default=./output]");
                Console.WriteLine(" verbose=<verbosity> [default=0]");
                return;
            } 

            string[] headerTitle = {"Record No.","Partition","Offset","Record Key","Record Value","Produced Time","Time Stamp"};
            int[] outputFormat = TerminalOutputSpacingFormatter(headerTitle);
            string topicName = settings["topicName"];
            int dataLength = Int32.Parse(settings["dataLength"]);
            int numberOfRecords = Int32.Parse(settings["numberOfRecords"]);
            string benchmarkPath = settings["outputFilePath"];
            string bmFilename = Convert.ToString(dataLength) +"-"+ DateTime.Now.ToString(@"yyyyMMdd-hhmmss") + ".csv";
            string outputPath = Path.Join(benchmarkPath, bmFilename);

            Dictionary<string, long> initialProducedTimePerPartitions = new Dictionary<string, long>();
            Dictionary<string, List<long>> producedTimePerPartitions = new Dictionary<string, List<long>>();

            IConfiguration configuration = new ConfigurationBuilder()
                .AddIniFile(settings["configFilePath"])
                .Build();

            using (var producer = new ProducerBuilder<string, string>(configuration.AsEnumerable()).Build())
            using (StreamWriter sw = new StreamWriter(outputPath)){

                string failedKey = null;
                string failedValue = null;

                string key;
                string val;

                if(settings["verbose"] == "1"){
                    Console.WriteLine(" ");
                    TerminalOutput(headerTitle, outputFormat);
                } else {
                    Console.WriteLine(" ");
                    Console.WriteLine("! For record by record output set verbose=1 in command argument");
                }

                for (int i = 0; i < numberOfRecords ; i++){

                    if (failedKey != null || failedValue != null) {
                        key = failedKey + "-re";
                        val = failedValue;
                    } else {
                        key = "" + s_rand.Next(0, 100000000);
                        val = GenerateText(dataLength);
                    }

                    try {

                        // Note: Awaiting the asynchronous produce request below prevents flow of execution
                        // from proceeding until the acknowledgement from the broker is received (at the 
                        // expense of low throughput).

                        s_timerProduce.Start();

                        // this will wait for ack 
                        var deliveryReport = await producer.ProduceAsync(
                            topicName, 
                            new Message<string, string> { Key = "" + key, Value = val });

                        long elapsedTimeInMillisecond = s_timerProduce.ElapsedMilliseconds;
                        sw.WriteLine(elapsedTimeInMillisecond);
                        sw.Flush();

                        string[] _output = {
                                            Convert.ToString(i),
                                            Convert.ToString(deliveryReport.TopicPartitionOffset.Partition),
                                            Convert.ToString(deliveryReport.TopicPartitionOffset.Offset),
                                            deliveryReport.Message.Key,
                                            deliveryReport.Message.Value.Substring(0,10),
                                            elapsedTimeInMillisecond.ToString(),
                                            Convert.ToString(deliveryReport.Message.Timestamp.UtcDateTime)
                                            };
                        if(settings["verbose"] == "1"){
                            TerminalOutput(_output, outputFormat);
                        }
                        string _partition = Convert.ToString(deliveryReport.TopicPartitionOffset.Partition);
                        if(!initialProducedTimePerPartitions.ContainsKey(_partition)){
                            initialProducedTimePerPartitions.Add(_partition, elapsedTimeInMillisecond);
                            producedTimePerPartitions.Add(_partition, new List<long>{});
                        } else {
                            producedTimePerPartitions[_partition].Add(elapsedTimeInMillisecond);
                        }
                        failedKey = null;
                        failedValue = null;
                    }
                    catch (ProduceException<string, string> e)
                    {
                        string[] _output = {Convert.ToString(i),
                                            Convert.ToString(e.DeliveryResult.Partition),
                                            Convert.ToString(e.DeliveryResult.Offset),
                                            e.DeliveryResult.Key,
                                            e.DeliveryResult.Value.Substring(0,10),
                                            s_timerProduce.ElapsedMilliseconds.ToString(),
                                            Convert.ToString(e.DeliveryResult.Timestamp.UtcDateTime)
                                            };                    
                        TerminalOutput(_output, outputFormat);
                        failedKey = key;
                        failedValue = val;

                    } finally {
                        s_timerProduce.Reset();
                    }
            	}
            }

            Console.WriteLine(" ");
            Console.WriteLine("--------------------- RESULT ------------------------");
            Console.WriteLine(" ");
            Console.WriteLine("#1 Initial Record produced to Partitions. (Display in recieving order)");
            foreach(KeyValuePair<string, long> init in initialProducedTimePerPartitions){
                Console.WriteLine($"{init.Key} : {Convert.ToString(init.Value)}");
            }

            Console.WriteLine(" ");
            Console.WriteLine("#2 Record produce time per partitions");
            foreach(KeyValuePair<string, List<long>> partitionResult in producedTimePerPartitions){
                //Console.WriteLine($"[{partitionResult.Key}] : {String.Join(",",partitionResult.Value)}");
                List<long> _value = partitionResult.Value;
                int _listLength = _value.Count;
                _value.Sort();
                long _max = _value[_listLength - 1];
                long _min = _value[0];
                long _median = _value[(_listLength / 2) - 1];
                Console.WriteLine(" ");
                Console.WriteLine($"--------------------- Partition: [{partitionResult.Key}] ------------------------");
                Console.WriteLine($"MIN: {_min}, Median:{_median}, MAX: {_max}");
                Console.WriteLine($"50%: {_value[ParcentilePosition(0.5, _listLength)]}");
                Console.WriteLine($"75%: {_value[ParcentilePosition(0.75, _listLength)]}");
                Console.WriteLine($"90%: {_value[ParcentilePosition(0.9, _listLength)]}");
                Console.WriteLine($"99%: {_value[ParcentilePosition(0.99, _listLength)]}");
                Console.WriteLine($"99.9%: {_value[ParcentilePosition(0.999, _listLength)]}");

            }
            Console.WriteLine(" ");
        }
    }
}
