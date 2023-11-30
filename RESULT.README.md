



## benchmark Azure Tokyo region

NO Error observied

### Attemp 1

**Default 30000**

socket.timeout.ms=50


**Deafult 300000**

message.timeout.ms=500


**Default 30000**

request.timeout.ms=50


### Attempt 2

**Default 30000**

socket.timeout.ms=50


**Deafult 300000**

message.timeout.ms=250


**Default 30000**

request.timeout.ms=30



## In flightを起こす設定


**Default 30000**

socket.timeout.ms=10


**Deafult 300000**

message.timeout.ms=500


**Default 30000**

request.timeout.ms=50


**Error Message**

At 10ms

```
sending message: 13149609 : y59?!Na)2k
%5|1700443783.462|REQTMOUT|rdkafka#producer-1| [thrd:sasl_ssl://b3-pkc-22z82.japaneast.azure.confluent.cloud:9092/3]: sasl_ssl://b3-pkc-22z82.japaneast.azure.confluent.cloud:9092/3: Timed out ProduceRequest in flight (after 46ms, timeout #0)
%4|1700443783.471|REQTMOUT|rdkafka#producer-1| [thrd:sasl_ssl://b3-pkc-22z82.japaneast.azure.confluent.cloud:9092/3]: sasl_ssl://b3-pkc-22z82.japaneast.azure.confluent.cloud:9092/3: Timed out 1 in-flight, 0 retry-queued, 0 out-queue, 0 partially-sent requests
%3|1700443783.471|FAIL|rdkafka#producer-1| [thrd:sasl_ssl://b3-pkc-22z82.japaneast.azure.confluent.cloud:9092/3]: sasl_ssl://b3-pkc-22z82.japaneast.azure.confluent.cloud:9092/3: 1 request(s) timed out: disconnect (after 1018ms in state UP)
Time took to fail: 00:00:00.0655689
%3|1700443783.471|ERROR|rdkafka#producer-1| [thrd:app]: rdkafka#producer-1: sasl_ssl://b3-pkc-22z82.japaneast.azure.confluent.cloud:9092/3: 1 request(s) timed out: disconnect (after 1018ms in state UP)
failed to deliver message: Local: Message timed out [Local_MsgTimedOut]
failed message: 13149609 : y59?!Na)2k
```

At 20ms

```
sending message: 59130840 : jO"[cE2CZQ
%5|1700444438.354|REQTMOUT|rdkafka#producer-1| [thrd:sasl_ssl://b3-pkc-22z82.japaneast.azure.confluent.cloud:9092/3]: sasl_ssl://b3-pkc-22z82.japaneast.azure.confluent.cloud:9092/3: Timed out ProduceRequest in flight (after 47ms, timeout #0)
%4|1700444438.354|REQTMOUT|rdkafka#producer-1| [thrd:sasl_ssl://b3-pkc-22z82.japaneast.azure.confluent.cloud:9092/3]: sasl_ssl://b3-pkc-22z82.japaneast.azure.confluent.cloud:9092/3: Timed out 1 in-flight, 0 retry-queued, 0 out-queue, 0 partially-sent requests
%3|1700444438.354|FAIL|rdkafka#producer-1| [thrd:sasl_ssl://b3-pkc-22z82.japaneast.azure.confluent.cloud:9092/3]: sasl_ssl://b3-pkc-22z82.japaneast.azure.confluent.cloud:9092/3: 1 request(s) timed out: disconnect (after 7042ms in state UP)
%3|1700444438.354|ERROR|rdkafka#producer-1| [thrd:app]: rdkafka#producer-1: sasl_ssl://b3-pkc-22z82.japaneast.azure.confluent.cloud:9092/3: 1 request(s) timed out: disconnect (after 7042ms in state UP)
Time took to fail: 00:00:00.0578965
failed to deliver message: Local: Message timed out [Local_MsgTimedOut]
failed message: 59130840 : jO"[cE2CZQ
```


At 30ms

```
sending message: 42390889 : zY)9Y.Ilqy
%5|1700444487.805|REQTMOUT|rdkafka#producer-1| [thrd:sasl_ssl://b5-pkc-22z82.japaneast.azure.confluent.cloud:9092/5]: sasl_ssl://b5-pkc-22z82.japaneast.azure.confluent.cloud:9092/5: Timed out ProduceRequest in flight (after 31ms, timeout #0)
%4|1700444487.805|REQTMOUT|rdkafka#producer-1| [thrd:sasl_ssl://b5-pkc-22z82.japaneast.azure.confluent.cloud:9092/5]: sasl_ssl://b5-pkc-22z82.japaneast.azure.confluent.cloud:9092/5: Timed out 1 in-flight, 0 retry-queued, 0 out-queue, 0 partially-sent requests
%3|1700444487.805|FAIL|rdkafka#producer-1| [thrd:sasl_ssl://b5-pkc-22z82.japaneast.azure.confluent.cloud:9092/5]: sasl_ssl://b5-pkc-22z82.japaneast.azure.confluent.cloud:9092/5: 1 request(s) timed out: disconnect (after 4033ms in state UP)
%3|1700444487.805|ERROR|rdkafka#producer-1| [thrd:app]: rdkafka#producer-1: sasl_ssl://b5-pkc-22z82.japaneast.azure.confluent.cloud:9092/5: 1 request(s) timed out: disconnect (after 4033ms in state UP)
Time took to fail: 00:00:00.0427745
failed to deliver message: Local: Message timed out [Local_MsgTimedOut]
failed message: 42390889 : zY)9Y.Ilqy
```


At 40ms

```
sending message: 17954799 : 9Fn\u{S5*]
%5|1700444539.019|REQTMOUT|rdkafka#producer-1| [thrd:sasl_ssl://b4-pkc-22z82.japaneast.azure.confluent.cloud:9092/4]: sasl_ssl://b4-pkc-22z82.japaneast.azure.confluent.cloud:9092/4: Timed out ProduceRequest in flight (after 47ms, timeout #0)
%4|1700444539.019|REQTMOUT|rdkafka#producer-1| [thrd:sasl_ssl://b4-pkc-22z82.japaneast.azure.confluent.cloud:9092/4]: sasl_ssl://b4-pkc-22z82.japaneast.azure.confluent.cloud:9092/4: Timed out 1 in-flight, 0 retry-queued, 0 out-queue, 0 partially-sent requests
%3|1700444539.019|FAIL|rdkafka#producer-1| [thrd:sasl_ssl://b4-pkc-22z82.japaneast.azure.confluent.cloud:9092/4]: sasl_ssl://b4-pkc-22z82.japaneast.azure.confluent.cloud:9092/4: 1 request(s) timed out: disconnect (after 12105ms in state UP)
%3|1700444539.019|ERROR|rdkafka#producer-1| [thrd:app]: rdkafka#producer-1: sasl_ssl://b4-pkc-22z82.japaneast.azure.confluent.cloud:9092/4: 1 request(s) timed out: disconnect (after 12105ms in state UP)
Time took to fail: 00:00:00.0568333
failed to deliver message: Local: Message timed out [Local_MsgTimedOut]
failed message: 17954799 : 9Fn\u{S5*]

```

At 50ms

no error message




## Request timeoutを起こす設定


**Default 30000**

socket.timeout.ms=50


**Deafult 300000**

message.timeout.ms=500


**Default 30000**

request.timeout.ms=10


**Error Message**

At 10ms

```
sending message: 75416075 : }<@03FujB|
Time took to fail: 00:00:00.0684048
failed to deliver message: Broker: Request timed out [RequestTimedOut]
failed message: 75416075 : }<@03FujB|
```

AT 20 ms

```
sending message: 30385596 : R[R0d4MJz>
Time took to fail: 00:00:00.0967994
failed to deliver message: Broker: Request timed out [RequestTimedOut]
failed message: 30385596 : R[R0d4MJz>
```

AT 30ms

NO error message



## Local timeout を起こす方法


**Default 30000**

socket.timeout.ms=50


**Deafult 300000**

message.timeout.ms=100


**Default 30000**

request.timeout.ms=0


AT 100ms

```
sending message: 78161341 : N=kj)ThYKR
Time took to fail: 00:00:00.1639608
failed to deliver message: Local: Message timed out [Local_MsgTimedOut]
failed message: 78161341 : N=kj)ThYKR
```

AT 150ms
```
sending message: 27688878 : 8)|(`_8bcB
Time took to fail: 00:00:00.1609661
failed to deliver message: Local: Message timed out [Local_MsgTimedOut]
failed message: 27688878 : 8)|(`_8bcB
```

At 200ms
```
sending message: 47817993 : &cT}5d7A7&
Time took to fail: 00:00:00.2151822
failed to deliver message: Local: Message timed out [Local_MsgTimedOut]
failed message: 47817993 : &cT}5d7A7&
```


At 250ms

No error message




## 考察

**request.timeout.ms**
5-10msなどでもデータを送信できる。
これはBrokerがackを返すまでの時間に関するtimeout値。
このパラメタでわかることはBroker間の通信状態の良さ。


**socket.timeout.ms**


以下の設定を変更することで、Cloudへのデータproduceも10msでデータをbrokerに送ることができる。

### SocketNagleDisable
#### Default : false
socket.nagle.disable=true

### LingerMs
#### Default : 5 (ms)
linger.ms=0



