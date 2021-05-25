# Monads in C#

Implement a real world C# API via Monads.

To test this functional API with `curl`:

``` shell
curl -X GET -i
  -H "Accept: application/json"
   http://localhost:5000/accounts/669dceb5-107d-4701-ae9c-802d6963d081

curl -X POST -i
  -H "Accept: application/json"
  -H "Content-Type: application/json"
  -d "{\"Event\":\"CreatedAccount\",\"AccountId\":\"669dceb5-107d-4701-ae9c-802d6963d081\",\"Currency\":\"USD\"}"
  http://localhost:5000/accounts

curl -X POST -i
  -H "Accept: application/json"
  -H "Content-Type: application/json"
  -d "{\"Event\":\"DepositedCash\",\"AccountId\":\"669dceb5-107d-4701-ae9c-802d6963d081\",\"Amount\":1}"
  http://localhost:5000/accounts

curl -X POST -i
  -H "Accept: application/json"
  -H "Content-Type: application/json"
  -d "{\"Event\":\"DebitedFee\",\"AccountId\":\"669dceb5-107d-4701-ae9c-802d6963d081\",\"Amount\":1}"
  http://localhost:5000/accounts
```

To test the performance via Apache Benchmark and `dotnet-counters`:


```shell
ab -n 150 -c 4
  -p postfile
  -T "application/json"
  -H "Accept: application/json"
  http://localhost:5000/accounts

dotnet-counters monitor -p 11276
  --refresh-interval 3
  --counters System.Runtime,Microsoft.AspNetCore.Hosting
```
