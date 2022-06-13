namespace Function

open Giraffe
open Microsoft.AspNetCore.Http

module Say =
  let hello (name: string) : HttpHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
      (text $"Hey %s{name}, how's it going?") next ctx
