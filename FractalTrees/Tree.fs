module Tree
open System
open System.Drawing
open System.Drawing.Drawing2D

let size = 1000

type TreePoint = float * float
type BranchVector = float * float

type Tree =
    | Root of TreePoint * TreePoint * BranchVector * Tree * Tree
    | Branch of TreePoint * BranchVector * Tree * Tree
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
        match n with
        | _ when n = 1 -> 
            Leaf (nextX, nextY)
        | _ -> 
            let ang1, ang2 = if n % 2 = 0 then largeAngle, smallAngle else smallAngle, largeAngle
            //let ang1, ang2 = Math.PI / 4., Math.PI / 4. 
            let left = makeTreeStructure (n - 1) nextX nextY (direction - ang1) (length * factor) false
            let right = makeTreeStructure (n - 1) nextX nextY (direction + ang2) (length * factor) false
            if isRoot then
                Root((startX, startY), (nextX, nextY), (length, direction), left, right)
            else
                Branch((nextX, nextY), (length, direction), left, right)

    let counter = ref 0

    let gdiPoint (x: float) (y: float) =
        counter := !counter + 1
        !counter, new Point(int x, int y)

    let rec walkTree tree acc =
        let midPointOffset length direction =
            let offset = length / 10. 
            let offsetX = offset * Math.Sin(direction)
            let offsetY = offset * Math.Cos(direction)
            offsetX, offsetY
        let calculateWidthOffset length direction =
            let direction = direction + (Math.PI / 2.)
            let width = length / 15. 
            let widthX = width * Math.Sin(direction)
            let widthY = width * Math.Cos(direction)
            widthX, widthY

        match tree with
        | Root ((startX, startY), (nextX, nextY), (length, direction), left, right) ->
            let widthX, widthY = calculateWidthOffset length direction
            let offsetX, offsetY = midPointOffset length direction
            let acc = gdiPoint (startX - widthX) (startY - widthY) :: acc
            let acc = gdiPoint (nextX - widthX) (nextY - widthY) :: acc
            let acc = walkTree left acc 
            let acc = gdiPoint (nextX + offsetX) (nextY + offsetY) :: acc
            let acc = walkTree right acc 
            let acc = gdiPoint (nextX + widthX) (nextY + widthY) :: acc
            gdiPoint (startX + widthX) (startY + widthY)  :: acc
        | Branch ((nextX, nextY), (length, direction), left, right) -> 
            let widthX, widthY = calculateWidthOffset length direction
            let offsetX, offsetY = midPointOffset length direction
            let acc = gdiPoint (nextX + widthX) (nextY + widthY) :: acc
            let acc = walkTree left acc
            let acc = gdiPoint (nextX + offsetX) (nextY + offsetY) :: acc
            let acc = walkTree right acc
            gdiPoint (nextX - widthX) (nextY - widthY) :: acc
        | Leaf (nextX, nextY) -> gdiPoint nextX nextY :: acc

    let startAngle = Math.PI * 0.98
    let startX, startY = ((float size) / 2.5), ((float size) - 150.)
    let treeStructure = makeTreeStructure 10 startX startY startAngle 300. true

    let points = walkTree treeStructure []

    //printfn "%A" points

    g.FillClosedCurve(new SolidBrush(Color.Black), points |> List.map snd |> List.toArray)

    //let drawFont = new Font(FontFamily.GenericMonospace, 16.f);
    //let drawBrush = new SolidBrush(Color.Red);

    //points
    //|> Seq.iter (fun (i, p) -> g.DrawString(string i, drawFont, drawBrush, float32 p.X, float32 p.Y))

    img.Save(file)

