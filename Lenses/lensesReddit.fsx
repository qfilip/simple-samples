// A Lens is a pair of a getter and a setter. Typically the 'a is an "outer" type and the 'b is an "inner" type.
/// Getter: takes an 'a and returns its inner 'b.
/// Setter: takes a 'b and an outer 'a, and returns an 'a with its 'b value equal to the supplied 'b.
/// Note: We (aka the Aether devs) use tuples so we can easily deconstruct during pattern  matching.
type Lens<'a,'b> = ('a -> 'b) * ('b -> 'a -> 'a)

/// Compose two lenses. 
/// If we have a lens from 'a to its inner 'b, and a lens from 'b to its inner 'c,
/// compose returns the lens from 'a to its (inner) inner 'c.
let composeLens ((g1,s1):Lens<'a,'b>) ((g2,s2):Lens<'b,'c>) : Lens<'a,'c> =
        (fun a -> g2 (g1 a)), (fun c a -> s1 (s2 c (g1 a)) a)

let setWithLens (a:'a) (b:'b) ((_,setter): Lens<'a,'b>) =
    setter b a

let getWithLens (a:'a) ((getter,_): Lens<'a,'b>) =
    getter a

let mapWithLens ((getter,setter): Lens<'a,'b>) (f: 'b -> 'b) (a:'a) =
    setter (f (getter a)) a


type XAxis = int
type YAxis = int

/// Examples
type Position = {
    x : XAxis
    y : YAxis
}
with

    // At this point these lenses are overkill, but they'll be useful later.
    static member XAxisLens : Lens<Position,XAxis> =
        ((fun p -> p.x), (fun x p -> {p with x = x}))

    static member YAxisLens : Lens<Position,YAxis> =
        ((fun p -> p.y), (fun y p -> {p with y = y}))


type Entity = {
    position: Position
    mass : float
}
with
    static member PositionLens : Lens<Entity,Position> =
        (
            (fun entity -> entity.position),
            (fun position entity -> {entity with position = entity.position})
        )

    static member XAxisLens : Lens<Entity, XAxis> = 
        composeLens (Entity.PositionLens) (Position.XAxisLens)

    static member YAxisLens : Lens<Entity,YAxis> =
        composeLens (Entity.PositionLens) (Position.YAxisLens)

// The below pairs of functions demonstrate that lenses
// 1) Incur an overhead in cost in terms of carrying around the actual lens and having to 
// reason about its use 
// 2) In the long term save cognitive/LoC effort due to requiring the same
// complexity for arbitrarily nested records

let getPosition entity = 
    getWithLens entity Entity.PositionLens

// This is cleaner and clearer than using a lens.
let getPositionNested entity = 
    entity.position 

let getX entity = 
    getWithLens entity Entity.XAxisLens
// Being nested is not a big deal for record getters.
let getXNested entity = 
    entity.position.x 

let setPosition entity position = 
    setWithLens entity position Entity.PositionLens
let setPositionNested entity position = 
    // This is newer F# syntax which makes working with records somewhat easier
    // Again I think this is cleaner 
    {entity with position = position} 

let setX entity x = setWithLens entity x Entity.XAxisLens // constant size at the cost
                                                        // of some overhead in the type def 
let setXNested entity x : Entity = 
    // not that bad yet, but starting to get hairy
    {entity with position = {entity.position with x=x}}

type GameMonster = {
    entity : Entity
    health : int
    name : string
}
with

    // this boilerplate does get a bit long and the
    // number of static Lens members you need to add is 
    // ~ quadratic in the nesting depth :(

    static member EntityLens : Lens<GameMonster,Entity> = (
        (fun m -> m.entity), (fun e m -> {m with entity = e})
    )

    static member PositionLens : Lens<GameMonster,Position> =
        composeLens GameMonster.EntityLens Entity.PositionLens

    static member XAxisLens : Lens<GameMonster, XAxis> =
        composeLens GameMonster.PositionLens Position.XAxisLens
        
    // Unless you screwed something up, the specific lenses to compose shouldn't matter
    static member YAxisLens : Lens<GameMonster, YAxis> =
        composeLens GameMonster.EntityLens Entity.YAxisLens

let getMonsterX monster = 
    getWithLens monster GameMonster.XAxisLens

let getMonsterXNested monster = 
    monster.entity.position.x

let setMonsterX monster x = 
    // Note that this is still only four terms, just like
    // setting any other property, regardless of nesting level
    setWithLens monster x GameMonster.XAxisLens

let setMonsterXNested monster x = 
    // yuuuuck
    {monster with entity = 
        {monster.entity with position = 
            {monster.entity.position with x = x}}}


