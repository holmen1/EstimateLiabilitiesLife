open System.Text.Json
open System.Threading.Tasks
open EstimateLiabilitiesLife;
open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Hosting






let builder = WebApplication.CreateBuilder()

let app = builder.Build()
app.UseHttpsRedirection() |> ignore
let options = JsonSerializerOptions(JsonSerializerDefaults.Web)


type RequestModel =
    { valueDate: DateTime
      contractNo: int
      birthDate: DateTime
      sex: string
      z: int
      guarantee: float
      payPeriod: int
      table: string }
    
    member this.Sex() =
        match this.sex with
        | "F" -> Mortality.Sex.F
        | "M" -> Mortality.Sex.M
        | _ -> failwith "non binary sex not supported"
        
    member this.Table() =
        match this.table with
        | "AP" -> Insurance.Table.AP
        | "APG" -> Insurance.Table.APG
        | _ -> failwith $"table {this.table} not supported"
       
        
    member this.ToContract() =
        { Insurance.contractNo = this.contractNo
          Insurance.birthDate = this.birthDate
          Insurance.sex = this.Sex()
          Insurance.z = this.z
          Insurance.guarantee = this.guarantee
          Insurance.payPeriod = this.payPeriod
          Insurance.table = this.Table() }

app.MapGet("/", Func<string>(fun () -> "Hello World!")) |> ignore

app.MapPost("/cashflows", Func<HttpContext, Task>(fun (context) -> 
    let jsonRequest = context.Request.Body
    let request = JsonSerializer.DeserializeAsync<RequestModel>(jsonRequest, options).Result
    let valueDate = request.valueDate
    let contract = request.ToContract()
    let cashflows = Reserving.projectCashflows valueDate contract
    let jsonResponse = JsonSerializer.Serialize(cashflows, options)
    context.Response.WriteAsync(jsonResponse)
)) |> ignore


app.Run()


