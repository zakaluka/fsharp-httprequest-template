namespace Function

open Giraffe
open Microsoft.AspNetCore.Http
open System

module Say =
  let hello (names: string seq) : HttpHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
      let namesSplit =
        names
        |> Seq.collect (fun e -> e.Split('/'))
        |> Seq.filter (String.IsNullOrWhiteSpace >> not)
      (match Seq.length namesSplit with
      | 0 -> text "Howdy stranger!"
      | 1 -> text $"Hello, my one and only friend %s{Seq.head namesSplit}"
      | _ -> String.Join(", ", namesSplit) |> sprintf "Hello all my friends %s!" |> text
      ) next ctx 
