module Tree
open System
open System.Drawing
open System.Drawing.Drawing2D

let size = 1000

type TreePoint = float * float

type Tree =
    | Root of TreePoint * TreePoint * Tree * Tree
    | Branch of TreePoint * Tree * Tree
    | Leaf of TreePoint

let drawTree file = 
    let rnd = new Random()
    let img = new Bitmap(size, size)
    use g = Graphics.FromImage(img)
    g.SmoothingMode <- SmoothingMode.HighQuality;
    g.FillRectangle(new SolidBrush(Color.White), 0, 0, size, size)
    g.DrawRectangle(new Pen(Color.Black, 1.f), 0, 0, size - 1, size - 1)

    let largeAngle = Math.PI / 2.4
    let smallAngle = 0.2
    let factor = 0.6 

    let rec makeTreeStructure n startX startY direction length isRoot =
        let x = length * Math.Sin(direction)
        let y = length * Math.Cos(direction)
        let nextX, nextY = startX + x, startY + y
        let width = length / 15. 
        let widthX = width * Math.Cos(direction)
        let widthY = width * Math.Sin(direction)
        match n with
        | _ when n = 1 -> 
            Leaf (nextX, nextY)
        | _ -> 
            let ang1, ang2 = if n % 2 = 0 then smallAngle, largeAngle else largeAngle, smallAngle  
            let left = makeTreeStructure (n - 1) nextX nextY (direction - ang1) (length * factor) false
            let right = makeTreeStructure (n - 1) nextX nextY (direction + ang2) (length * factor) false
            if isRoot then
                Root((startX, startY), (nextX, nextY), left, right)
            else
                Branch((nextX, nextY), left, right)
        //if n > 0 then

        //    let acc = new Point(int (startX - widthX), int (startY - widthY)) :: acc

        //    new Point(int (startX + widthX), int (startY + widthY)) :: acc
        //else
        //    acc

    let rec walkTree tree acc =
        match tree with
        | Root ((startX, startY), (nextX, nextY), left, right) ->
            let acc = new Point(int startX, int startY) :: acc
            let acc = walkTree left acc
            let acc = walkTree right acc
            new Point(int nextX, int nextY) :: acc
        | Branch ((nextX, nextY), left, right) -> 
            let acc = walkTree left acc
            let acc = walkTree right acc
            new Point(int nextX, int nextY) :: acc
        | Leaf (nextX, nextY) -> new Point(int nextX, int nextY) :: acc

    let startAngle = Math.PI // Math.PI * 0.98
    let startX, startY = ((float size) / 2.), ((float size) - 50.)
    let treeStructure = makeTreeStructure 10 startX startY startAngle 300. true

    let points = 
        walkTree treeStructure []
        |> List.toArray

    printfn "%A" points

    g.FillClosedCurve(new SolidBrush(Color.Black), points)


    //let drawFont = new Font(FontFamily.GenericMonospace, 16.f);
    //let drawBrush = new SolidBrush(Color.Red);

    //points
    //|> Seq.iteri (fun i p -> g.DrawString(string i, drawFont, drawBrush, float32 p.X, float32 p.Y))

    img.Save(file)

