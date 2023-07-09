namespace Function

open Microsoft.AspNetCore.Http
open FSharp.Data
open Giraffe

module parseurl =
  let run (names: string seq) : HttpHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
      task {
        let urlOpt = ctx.TryGetQueryStringValue "url" |> Option.defaultValue ""

        let results: HtmlDocument = HtmlDocument.Load(urlOpt)

        let openGraphProperties =
          results.Descendants [ "meta" ]
          |> Seq.choose (fun x ->
            x.TryGetAttribute("property")
            |> Option.map (fun a -> a.Value(), x.AttributeValue("content")))
          |> Seq.map (fun (a, b) -> a.Replace(":", ""), b)

        let authorAppValues =
          Seq.map
            ((fun (a: string, b) -> a.Replace(":", ""), b)
             >> (fun (a, b) -> a.Replace("-", ""), b))
            (results.Descendants [ "meta" ]
             |> Seq.choose (fun x ->
               x.TryGetAttribute("name")
               |> Option.map (fun a -> a.Value(), x.AttributeValue("content"))))

        let fullDict = Seq.append openGraphProperties authorAppValues |> dict
        return! json fullDict next ctx
      }
