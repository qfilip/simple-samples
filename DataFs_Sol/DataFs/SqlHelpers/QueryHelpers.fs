module QueryHelpers

open Dapper
open System

    type SqlQuery =
        {
            Query : string
            Parameters : (string * obj) list
        }

    type OptionHandler<'T>() =
        inherit SqlMapper.TypeHandler<option<'T>>()
        
        override __.SetValue(param, value) = 
            let valueOrNull = 
                match value with
                | Some x -> box x
                | None -> null
        
            param.Value <- valueOrNull    
        
        override __.Parse value =
            if isNull value || value = box DBNull.Value then
                None
            else
                Some (value :?> 'T)

    let p name value =
        (name, box value)


    let sql query parameters =
        {
            Query = query
            Parameters = parameters
        }

