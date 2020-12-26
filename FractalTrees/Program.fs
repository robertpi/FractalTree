open System.Diagnostics

[<EntryPoint>]
let main argv =
    printfn "Drawing a tree"
    
    let outPath = @"C:\code\FractalTrees\FractalTrees\tree.jpg"
    
    Tree.drawTree outPath
    
    Process.Start(ProcessStartInfo(outPath, UseShellExecute = true)) 
    |> ignore
    
    0 // return an integer exit code
