# dotnet producer client

## How to use

### build

Change directiry to dotnet_producer (where dotnet_producer.csproj exists).

And do.
```
$ dotnet run
Error: arguments config_file_path, topic_name are required.
Usage: dotnet_producer.exe ... <options> ...
Options:
 configFilePath=<config_file_path> [required]
 topicName=<topic_name> [required]
 dataLength=<data_length> [default=1024 int32]
 numberOfRecords=<number_of_records> [default=1000 int32]
 outputFilePath=<output_file_path> [default=./output]
 verbose=<verbosity> [default=0]
```

### Run program

After building executable will live under ./bin folder.

Fill in cluster credencial in sample.config.

```
# Required connection configs for Kafka producer, consumer, and admin
bootstrap.servers=
security.protocol=SASL_SSL
sasl.mechanisms=PLAIN
sasl.username=
sasl.password=
```


**Sample command**

Minimum
```
$ ..\dotnet_producer\bin\Debug\net6.0\dotnet_producer.exe \
 configFilePath="sample.config" \ 
 topicName=test
```

Sending 1000 records with 1024bytes each with verbose output 
```
$ ..\dotnet_producer\bin\Debug\net6.0\dotnet_producer.exe \
 configFilePath="sample.config" \ 
 topicName=test dataLength=1024 numberOfRecords=10 verbose=1 
```

#### What Happens after running the executable.

log of produce will be written in ./output directiry in csv file format.

`<dataLength>-<timestamp in yyyyMMdd-hhmmss format>.csv`

This file only has millisecond valur of each produce. Not separeted by partitions.


Also some stat will appear after all records are sent to Topic.

**Output Sample**

Target topic has 6 partitions.

Stats givin in #2 Record produce time per partitions is sample of record excluding initial record send.
This is because initial produce tends to be much slower due to metadata exchange and etc.

```
! For record by record output set verbose=1 in command argument

--------------------- RESULT ------------------------

#1 Initial Record produced to Partitions. (Display in recieving order)
3 : 287
2 : 143
5 : 132
1 : 154
0 : 172
4 : 89

#2 Record produce time per partitions

--------------------- Partition: [3] ------------------------
MIN: 6, Median:7, MAX: 28
50%: 7
75%: 7
90%: 7
99%: 28
99.9%: 28

--------------------- Partition: [2] ------------------------
MIN: 6, Median:7, MAX: 8
50%: 7
75%: 7
90%: 8
99%: 8
99.9%: 8

--------------------- Partition: [5] ------------------------
MIN: 7, Median:7, MAX: 11
50%: 7
75%: 8
90%: 9
99%: 11
99.9%: 11

--------------------- Partition: [1] ------------------------
MIN: 6, Median:7, MAX: 14
50%: 7
75%: 7
90%: 8
99%: 14
99.9%: 14

--------------------- Partition: [0] ------------------------
MIN: 6, Median:7, MAX: 25
50%: 7
75%: 7
90%: 7
99%: 25
99.9%: 25

--------------------- Partition: [4] ------------------------
MIN: 7, Median:7, MAX: 9
50%: 7
75%: 8
90%: 8
99%: 9
99.9%: 9
```