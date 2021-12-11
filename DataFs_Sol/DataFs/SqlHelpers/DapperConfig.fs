module DapperConfig

open System
open Dapper
open QueryHelpers

let RegisterOptionTypes () =
    SqlMapper.AddTypeHandler(new OptionHandler<bool>())
    SqlMapper.AddTypeHandler(new OptionHandler<int>())
    SqlMapper.AddTypeHandler(new OptionHandler<int64>())
    SqlMapper.AddTypeHandler(new OptionHandler<string>())
    SqlMapper.AddTypeHandler(new OptionHandler<Guid>())
    SqlMapper.AddTypeHandler(new OptionHandler<DateTime>())
    SqlMapper.AddTypeHandler(new OptionHandler<single>())
    SqlMapper.AddTypeHandler(new OptionHandler<double>())
    SqlMapper.AddTypeHandler(new OptionHandler<decimal>())