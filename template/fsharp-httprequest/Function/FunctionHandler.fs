namespace Function

open Giraffe
open System
open Microsoft.AspNetCore.Http

module Say =
  let hello (names: string seq) : HttpHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
      let pathNames =
        names
        |> Seq.collect (fun e -> e.Split('/'))
        |> Seq.filter (String.IsNullOrWhiteSpace >> not)

      let pathNameMsg =
        match Seq.length pathNames with
        | 0 -> "Howdy path-based stranger!"
        | 1 -> $"Hello, my one and only friend %s{Seq.head pathNames}"
        | _ -> String.Join(", ", pathNames) |> sprintf "Hi path friends %s!"

      let queryNameMsg =
        match ctx.TryGetQueryStringValue "names" with
        | None -> "Hello query stranger!"
        | Some n -> $"Hi query friends %s{n}"

      text $"%s{pathNameMsg}\n%s{queryNameMsg}" next ctx
