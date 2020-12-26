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
    g.FillRectangle(new SolidBrush(Color.White), 0, 0, size, size)
    g.DrawRectangle(new Pen(Color.Black, 1.f), 0, 0, size - 1, size - 1)

    let drawLeaf() = ()
    let drawTrunk (startX, startY) direction length =
        let x = length * Math.Sin(direction)
        let y = length * Math.Cos(direction)
        let nextX, nextY = startX + x, startY + y
        let pen = new Pen(Color.Black, float32 length / 15.f)
        g.DrawLine(pen, int startX, int startY, int nextX, int nextY)
        (nextX, nextY)

    let largeAngle = Math.PI / 2.4
    let smallAngle = 0.2
    let factor n = 0.6 

    let rec innerDrawTree n location direction  length =
        if n > 0 then
            let location = drawTrunk location direction length
            let op1, op2 = if n % 2 = 0 then (-), (+) else (+), (-)  
            innerDrawTree (n - 1) location (op1 direction largeAngle) (length * factor n)
            innerDrawTree (n - 1) location (op2 direction smallAngle) (length * factor n)
        else
            drawLeaf()

    innerDrawTree 10 ((float size) / 2., (float size) - 50.) (Math.PI * 0.98) 300.

    img.Save(file)

