module Tree
open System
open System.Drawing
open System.Drawing.Drawing2D

let size = 1000

let drawTree file = 
    let rnd = new Random()
    let img = new Bitmap(size, size)
    use g = Graphics.FromImage(img)
    g.SmoothingMode <- SmoothingMode.HighQuality;
    let pen = new Pen(Color.Black, 1.f)
    g.FillRectangle(new SolidBrush(Color.White), 0, 0, size, size)
    g.DrawRectangle(new Pen(Color.Black, 1.f), 0, 0, size - 1, size - 1)

    let drawLeaf() = ()
    let drawTrunk (startX, startY) direction length =
        let x = length * Math.Sin(direction)
        let y = length * Math.Cos(direction)
        let nextX, nextY = startX + x, startY + y
        g.DrawLine(pen, int startX, int startY, int nextX, int nextY)
        (nextX, nextY)

    let randomAngle() = rnd.NextDouble() * 0.6
    let factor n = 0.6 

    let rec innerDrawTree n location direction  length =
        if n > 0 then
            let location = drawTrunk location direction length
            innerDrawTree (n - 1) location (direction + randomAngle()) (length * factor n)
            innerDrawTree (n - 1) location (direction - randomAngle()) (length * factor n)
        else
            drawLeaf()

    innerDrawTree 10 ((float size) / 2., (float size) - 50.) Math.PI 300.

    img.Save(file)

