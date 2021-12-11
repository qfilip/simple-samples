module AppSql

    open Dapper  // adds QueryAsync and ExecuteAsync
    open System.Data.SqlClient
    open Microsoft.Data.Sqlite
    open QueryHelpers

    let query<'T> connectString q =
        async {
            //use connection = new SqlConnection(connectString)
            use connection = new SqliteConnection(connectString)
            do! connection.OpenAsync() |> Async.AwaitTask
            let! result =
                connection.QueryAsync<'T>(q.Query, dict q.Parameters)
                    |> Async.AwaitTask
            return Array.ofSeq result
        }


    let writeBatch connectString writes =
        async {
            use connection = new SqlConnection(connectString)
            do! connection.OpenAsync() |> Async.AwaitTask
            use transaction = connection.BeginTransaction()
            for write in writes do
                do!
                    connection.ExecuteAsync
                        (write.Query, dict write.Parameters, transaction)
                        |> Async.AwaitTask
                        |> Async.Ignore
            transaction.Commit()
        }


    let r =
        let conn = new Microsoft.Data.Sqlite.SqliteConnection("")
        0