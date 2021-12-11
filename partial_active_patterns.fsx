type Position = Cook | Bartender | Waiter
type Worker = { Salary: int; Position: Position }
type Staff =
    | Front of Worker
    | Back of Worker

// if you uncomment this, function 'getCook' goes bonkers
// let (|Cook|_|) worker = if worker.Position = Cook then Some worker else None

// thus, just give it a different name
let (|IsCook|_|) worker = if worker.Position = Cook then Some worker else None
let (|IsBartender|_|) worker = if worker.Position = Bartender then Some worker else None
let (|IsWaiter|_|) worker = if worker.Position = Waiter then Some worker else None


let isBackStaff salaryConstraint =
    let matcherFn worker =
        match worker with
        | (IsCook worker | IsBartender worker) when (salaryConstraint worker) -> true
        | _ -> false
    
    matcherFn


let isFrontStaff salaryConstraint =
    let matcherFn worker =
        match worker with
        | (IsBartender worker | IsWaiter worker) when (salaryConstraint worker) -> true
        | _ -> false
    
    matcherFn


let isHighlyPaidBackStaff worker =
    let filter = isBackStaff (fun x -> x.Salary >= 1000)
    filter worker


let isLowlyPaidFrontStaff worker =
    let filter = isFrontStaff (fun x -> x.Salary >= 300 && x.Salary < 500)
    filter worker