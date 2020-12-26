module Star
open System
open System.Drawing


let drawSnowFlake file = 
    let rnd = new Random()
    let img = new Bitmap(500, 500)
    use g = Graphics.FromImage(img)
    let pen = new Pen(Color.Black, 1.f)
    g.DrawRectangle(pen, 1, 1, 498, 498)
    g.FillRectangle(new SolidBrush(Color.White), 0, 0, 500, 500)


    let drawLeaf() = ()
    let drawTrunk (startX, startY) direction length =
        let x = length * Math.Sin(direction)
        let y = length * Math.Cos(direction)
        let nextX, nextY = startX + x, startY + y
        g.DrawLine(pen, int startX, int startY, int nextX, int nextY)
        (nextX, nextY)

    let randomAngle() = rnd.NextDouble() * 0.6
    let factor n = 0.4 // 1. / (float n) 


    let rec innerDrawSnowFlake n  location direction  length =
        if n > 0 then
            for x in 1 .. 6 do
                let direction = (Math.PI / 3.) * float x
                let location = drawTrunk location direction length
                ()
            // innerDrawSnowFlake (n - 1) location direction (length * factor n)
        else
            drawLeaf()

    innerDrawSnowFlake 10 (250., 250.) (2. * Math.PI) 200.

    img.Save(file)


