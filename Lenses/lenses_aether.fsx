// https://bugsquash.blogspot.com/2011/11/lenses-in-f.html
// https://xyncro.tech/aether/guides/lenses.html
#r "nuget: Aether"
open Aether

type Lens<'a,'b> = ('a -> 'b) * ('b -> 'a -> 'a)
let composeLens (g1, s1) (g2, s2) =
    (fun x -> x |> g1 |> g2), (fun y x -> s1 (g1 x |> s2 y) x)

let setWithLens (a:'a) (b:'b) ((_,setter): Lens<'a,'b>) =
    setter b a


type Pilot = {
    Name: string
    YrsOfExp: int
}


type Plane = {
    Vendor: string
    Pilot: Pilot
}
with 
    static member PilotLens: Lens<Plane, Pilot> =
        (
            (fun p -> p.Pilot),
            (fun x p -> { p with Pilot = x })
        )


type Hangar = {
    Number: int
    Plane: Plane
}
with
    static member PlaneLens: Lens<Hangar, Plane> =
        (
            (fun h -> h.Plane),
            (fun x h -> { h with Plane = x })
        )

    static member PilotLens: Lens<Hangar, Pilot> =
        composeLens (Hangar.PlaneLens) (Plane.PilotLens)



let pilot = { Name = "Red Barron"; YrsOfExp = 26 }
let plane = { Vendor = "Learjet"; Pilot = pilot }
let hangar = { Number = 2; Plane = plane }

let newPilot = { Name = "Blue Barry"; YrsOfExp = 23 }
let newHangar = setWithLens hangar newPilot Hangar.PilotLens


